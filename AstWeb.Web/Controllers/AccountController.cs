using AstWeb.Common.Authentication;
using AstWeb.Common.Extension;
using AstWeb.Common.Models;
using AstWeb.Common.Services;
using AstWeb.Common.Services.Messaging;
using AstWeb.Models;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Astrology.Common;
using System.Collections.Generic;
using demo;

namespace AstWeb.Controllers
{
    public class ApiRoot
    {
        /// <summary>
        /// 状态编 0-失败 -1成功
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 返回消息备注
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回结果 用于存放序列化的实体
        /// </summary>
        public object Result { get; set; }
    }
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult GetSMSCode(string aid, string token, string appId, string mobile, string type)
        {
            ApiRoot _api = new ApiRoot();
            if (aid == SysConfig.SMS.AccountSid && token == SysConfig.SMS.Authtoken && appId == SysConfig.SMS.AppId && (mobile.Length == 11))
            {
                int T = DataConversion.StrToInt(type);
                switch (T)
                {
                    case 0:
                        if (new UserService().IsExist(mobile))
                        {
                            _api.Status = 0;
                            _api.Msg = "手机号已存在!";
                        }
                        else
                        {
                            string randNum = CommFun.GetRndNum(6);
                            _api.Status = 1;
                            _api.Msg = randNum;
                            Dictionary<string, object> dic = SMSHelper.SendSMS(mobile, SysConfig.SMS.TempleId0, new[] { randNum, "6" });
                            if (dic["resposeBody"].ToString().Contains("000000"))//有时间再优化
                            {
                                _api.Status = 1;
                                _api.Msg = SysConfig.IsOnLine
                                    ? "验证码已发送到您的手机 "
                                    : string.Format("当前验证码为：{0}", randNum);
                                //存储验证码
                                new SMSService().Add(new SMS
                                {
                                    mobile = mobile,
                                    code = randNum,
                                    AddTime = DateTime.Now,
                                    ModifyTime = DateTime.Now,
                                    type = T
                                });
                            }
                            else
                            {
                                _api.Status = 0;
                                _api.Msg = dic["resposeBody"].ToString();
                            }
                        }
                        break;
                    case 1:
                        if (new UserService().IsExist(mobile))
                        {
                            string randNum = CommFun.GetRndNum(5);
                            _api.Status = 1;
                            _api.Msg = randNum;
                            Dictionary<string, object> dic = SMSHelper.SendSMS(mobile, SysConfig.SMS.TempleId1, new[] { randNum, "5" });
                            if (dic["resposeBody"].ToString().Contains("000000"))//有时间再优化
                            {
                                _api.Status = 1;
                                _api.Msg = SysConfig.IsOnLine
                                    ? "验证码已发送到您的手机 "
                                    : string.Format("当前验证码为：{0}", randNum);
                                //存储验证码
                                new SMSService().Add(new SMS
                                {
                                    mobile = mobile,
                                    code = randNum,
                                    AddTime = DateTime.Now,
                                    ModifyTime = DateTime.Now,
                                    type = T
                                });
                            }
                            else
                            {
                                _api.Status = 0;
                                _api.Msg = dic["resposeBody"].ToString();
                            }
                        }
                        else
                        {
                            _api.Status = 0;
                            _api.Msg = "手机号不存在!";
                        }
                        break;
                }
            }
            return JsonResult(_api.Status==1?true:false, _api.Msg,null, JsonRequestBehavior.AllowGet);
        }
        // GET: Account
        [AllowAnonymous]
        public ActionResult Index(int userId)
        {
            var User = new UserService().GetUserInfo(userId);
            ViewBag.CurrentUserId = CurrentUserId;
            ViewBag.IsAdmin = IsAdmin;
            var MyVisitors = new VisitorService().GetTopTwelve(userId);
            var posts = new PostService().GetPagePostsByFilter(new GetPagePostsRequest(1, 30) { UserId = userId });
            if (posts.IsSuccess)
                ViewBag.MyPosts = posts.Pages.Items; //最近求解
            //var answers = new CommentService().GetPageCommentsByUserId(1, 30, userId);
            //if (answers.IsSuccess)
            //    ViewBag.MyAnswers = answers.Pages.Items;//最近回答 最新的30条
            if (CurrentUserId != -1 && userId != CurrentUserId)//证明有登录的 且不是自己访问自己则更新访客记录
            {
                new VisitorService().AddVisitor(new Visitor
                {
                    VisitorUserId = CurrentUserId,
                    UserId = userId,
                    LastTime = DateTime.Now
                });
            }
            var articleCount = new ArticlesService().GetArticlesByUserId(userId).Items==null ? 0 : new ArticlesService().GetArticlesByUserId(userId).Items.Count;
            ViewBag.articleCount = articleCount;
            var postCount = new PostService().GetPostCountByUserId(userId);
            ViewBag.postCount = postCount;
            ViewBag.User = User;
            ViewBag.MyVisitors = MyVisitors;
            ViewBag.IsGZ = new AsterLikesService().IsExistsGuanZhu(CurrentUserId, userId);
            ViewBag.LikeNum = new AsterLikesService().GetLikeNum(userId);
            ViewBag.FansNum = new AsterLikesService().GetFansNum(userId);
            ViewBag.MyFans = new AsterLikesService().GetMyFans(userId);
            ViewBag.Message = new MessageService().GetMsgCount(userId);
            return View();
        }
        [AllowAnonymous]
        public ActionResult MyHome(int userId)
        {
            var user = new UserService().GetUserInfo(userId);
            ViewBag.User = user;
            return View();
        }
        //登录
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.url = Request.QueryString["url"];
            //如果已经登录，直接跳转至主页
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            var cookie = Request.Cookies["Email"];
            if (cookie != null)
            {
                ViewBag.Email = cookie.Value;
            }
            return View();
        }
        //登录
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model)
        {
            if (model == null)
                return JsonResult(false, "请勿传递非法参数，请刷新重试！", JsonRequestBehavior.AllowGet);
            //验证验证码是否正确
            //var sessionVerifyCode = Session["VerifyCode"];
            //if (sessionVerifyCode == null || model.Vercode != sessionVerifyCode.ToString())
            //    return JsonResult(false, "验证码输入错误！");
            var userService = new UserService();
            var entity = userService.Login(model.Email);
            if (entity == null)
            {
                return JsonResult(false, "用户不存在！", JsonRequestBehavior.AllowGet);
            }
            if (entity.Password != model.Pass.ToMd5())
            {
                return JsonResult(false, "用户名或密码错误！", JsonRequestBehavior.AllowGet);
            }
            if (entity.IsDisabled == 1)
            {
                return JsonResult(false, "你的帐号已被封，请联系管理员。", JsonRequestBehavior.AllowGet);
            }
            //记录登录日志
            //new LoginLogService().Insert(new LoginLog
            //{
            //    UserId = entity.Id,
            //    Ip = WebHelper.GetIp(),
            //    CreateTime = DateTime.Now,
            //    UserAgent = Request.UserAgent
            //});
            //重置错误次数
            //Session["PwdErrorCount"] = null;
            //保存身份票据
            new AspFormsAuthentication().SetAuthenticationToken(entity.Email + "$" + entity.Id + "$" + entity.EmailConfirmed + "$" + entity.IsAdmin);
            //保存登录名
            //if (model.RememberMe)
            //{
            HttpCookie cookie = new HttpCookie("Email");
            cookie.Value = entity.Email;
            cookie.Expires = DateTime.Now.AddDays(5);
            Response.Cookies.Set(cookie);
            //}
            entity.Password = "";
            return JsonResult(true, "登录成功。", new { user = entity }, JsonRequestBehavior.AllowGet);
        }
        //注册
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        //注册
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterModel model)
        {
            if (model == null)  return JsonResult(false, "请勿传递非法参数，请刷新重试！");
            if( ! new SMSService().IsRight(model.PhoneNumber,model.SmsCode)) return JsonResult(false, "验证码错误");
            //var sessionVerifyCode = Session["VerifyCode"];
            //if (sessionVerifyCode == null || model.Vercode != sessionVerifyCode.ToString())
            //    return JsonResult(false, "验证码输入错误，请刷新重试！");

            var service = new UserService();
            if (service.CheckPhoneExists(model.PhoneNumber))
                return JsonResult(false, "该手机号已被注册！");
            if (service.CheckEmailIsExists(model.Email))
            return JsonResult(false, "该邮箱已被注册！");
            if (service.CheckNicknameIsExists(model.NickName))
            return JsonResult(false, "该昵称已被使用！");
            var entity = new W_Users
            {
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Password = model.Pass.ToMd5(),
                NickName = model.NickName,
                Gender = model.Gender,
                Birthday = DateTime.Now,
                IsDaylight = model.IsDaylight,
                Place1 = model.place1,
                Place2 = model.place2,
                AccessToken = model.AccessToken,
                HeadPortrait = model.HeadPortrait==null?GetRandomFileName():model.HeadPortrait,//头像
                VipLevel = -1,
                CreateTime = DateTime.Now,
                Integral = 200,//注册时送200积分
                EmailIsUpdate = 1,
                Title = "普通会员"
            };
            var response = service.Register(entity);
            return JsonResult(response.IsSuccess, response.Message);
        }
        //QQ第三方登录
        [AllowAnonymous]
        public ActionResult QQlogin(string NickName,string HeadPortraid,string AccessToken)
        {
            if (CurrentUserId > 0)
            {
                return JsonResult(false, "绑定成功！", null, JsonRequestBehavior.AllowGet);
            }
            var user = new UserService();
            //判断是否已经是本网站的QQ账户，如果是就直接登录，不是就跳转完善信息。
            var QQuser = user.CheckQQ(AccessToken);
            if (QQuser!=null && AccessToken!="" && AccessToken!=null)
            {
                var loginmodel = new LoginModel
                {
                    Email = QQuser.Email,
                    Pass = QQuser.Password,
                };
                //保存身份票据
                new AspFormsAuthentication().SetAuthenticationToken(QQuser.Email + "$" + QQuser.Id + "$" + QQuser.EmailConfirmed + "$" + QQuser.IsAdmin);
                HttpCookie cookie = new HttpCookie("Email");
                cookie.Value = QQuser.Email;
                cookie.Expires = DateTime.Now.AddDays(5);
                Response.Cookies.Set(cookie);
                QQuser.Password = "";
                return JsonResult(true, "登录成功。", new { user=QQuser}, JsonRequestBehavior.AllowGet);
            }
            return JsonResult(false, "需要完善信息", null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogOut()
        {
            new AspFormsAuthentication().SignOut();
            //return RedirectToAction("Index", "StarSocial", new { CategoriesId = 0, pageIndex = 1 });
            //return RedirectToAction("login", "user");
            return JsonResult(true, "登出！");
        }

        #region Private Methods
        /// <summary>
        /// 在指定文件夹内获取随机一个文件
        /// </summary>
        /// <returns></returns>
        private string GetRandomFileName()
        {
            var path = Server.MapPath("~/static/img/DefaultHeadPortraits/");//文件夹路径
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                System.IO.FileInfo[] fiList = dir.GetFiles();

                Random ran = new Random();
                int n = ran.Next(fiList.Length - 1);

                return "/static/img/DefaultHeadPortraits/" + fiList[n].Name;
            }
            return "";
        }
        #endregion

        [AllowAnonymous]
        public ActionResult MyPass(int userId)
        {
            var user = new UserService().GetUserInfo(userId);
            ViewBag.User = user;
            return View();
        }
        //修改密码
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePwd(RepassModel model)
        {
            if (model == null)
                return JsonResultForFly(0, "请勿传递非法参数，请刷新重试！");
            var service = new UserService();
            var user = service.GetUserInfo(CurrentUserId);
            if (user.Password != model.NowPass.ToMd5())
                return JsonResultForFly(0, "原密码输入错误，请重试！");
            user.Password = model.Pass.ToMd5();
            var response = service.UpdateUser(user);
            if (response.IsSuccess)
                return JsonResultForFly(1, "修改成功，下次登录生效！", "/user/");
            return JsonResultForFly(0, "修改失败，请重试！");
        }

        [AllowAnonymous]
        public ActionResult MyAvatar(int userId)
        {
            var user = new UserService().GetUserInfo(userId);
            ViewBag.User = user;
            return View();
        }

        /// <summary>
        /// 修改拖鞋
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase f)
        {
            try
            {
                var files = Request.Files;
                if (files.Count == 0)
                    return Json(new
                    {
                        status = 1,
                        msg = "请选择要上传的照片"
                    });
                var curFile = files[0];
                //if ((curFile.ContentLength / 1024) > 2048)
                //{
                //    return Json(new
                //    {
                //        status = 1,
                //        msg = "请选择小于2M的图片."
                //    });
                //}
                //获取保存路径
                var filesUrl = Server.MapPath("~/Uploads/HeadPortraits/");
                if (Directory.Exists(filesUrl) == false)//路径不存在则创建
                    Directory.CreateDirectory(filesUrl);
                var fileName = Path.GetFileName(curFile.FileName);
                //文件后缀名
                var filePostfixName = fileName.Substring(fileName.LastIndexOf('.'));
                //新文件名
                var newFileName = GetRandomFileName(filePostfixName);
                var path = Path.Combine(filesUrl, newFileName);
                //保存文件
                curFile.SaveAs(path);
                var newPath = "/Uploads/HeadPortraits/" + newFileName;
                new UserService().UpdateHeadPortrait(newPath, CurrentUserId);
                return Json(new
                {
                    status = 0,
                    msg = "上传成功",
                    url = newPath
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 1,
                    msg = "上传失败、错误信息：" + ex.Message
                });
            }
        }
        public static string GetRandomFileName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                System.Threading.Thread.Sleep(500);
                string fileType = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
                Random rand = new Random();
                DateTime now = DateTime.Now;
                string str = string.Empty;
                str += now.Year.ToString(CultureInfo.InvariantCulture);
                str += now.Month.ToString(CultureInfo.InvariantCulture);
                str += now.Day.ToString(CultureInfo.InvariantCulture);
                str += now.Hour.ToString(CultureInfo.InvariantCulture);
                str += now.Minute.ToString(CultureInfo.InvariantCulture);
                str += now.Second.ToString(CultureInfo.InvariantCulture);
                str += rand.Next(0, 1000);
                return str + fileType;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 我的帖子
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult MyPost(int userId, int pageIndex = 1)
        {
            var user = new UserService().GetUserInfo(userId);
            ViewBag.User = user;
            var posts = new PostService().GetPagePostsByFilter(new GetPagePostsRequest(pageIndex, PageSize) { UserId = userId });
            return View(posts);
        }

        /// <summary>
        /// 我的信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult MyMsg(int userId, int pageIndex = 1)
        {
            var user = new UserService().GetUserInfo(userId);
            ViewBag.User = user;
            var Msg = new MessageService().GetPageMessagesByFilter(new GetPageMessagesRequest(pageIndex, PageSize,userId) );
            return View(Msg);
        }

        /// <summary>
        /// 我的签名
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult MySign(int userId)
        {
            var user = new UserService().GetUserInfo(userId);
            ViewBag.Like = new AsterLikesService().GetMyLike(userId);
            ViewBag.User = user;
            return View();
        }
        [AllowAnonymous]
        public ActionResult MyFans(int userId)
        {
            var user = new UserService().GetUserInfo(userId);
            ViewBag.Like = new AsterLikesService().GetMyFans(userId);
            ViewBag.User = user;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateSign(string sign)
        {
            var service = new UserService();
            var user = service.GetUserInfo(CurrentUserId);
            user.Sign = sign;
            var response = service.UpdateUser(user);
            if (response.IsSuccess)
                return JsonResultForFly(1, "更新成功！", "/user/");
            return JsonResultForFly(0, "更新失败，请重试！");
        }
        public ActionResult UpdateInfo(W_Users model)
        {
            var service = new UserService();
            var user = service.GetUserInfo(CurrentUserId);
            user.Birthday = model.Birthday;
            user.Sign = model.Sign;
            user.Gender = model.Gender;
            var response = service.UpdateUser(user);
            if (response.IsSuccess)
                return JsonResultForFly(1, "更改成功！");
            return JsonResultForFly(0, "更改失败，请重试！");
        }
        public ActionResult UpdateToken(string AccessToken)
        {
            var service = new UserService();
            var user = service.GetUserInfo(CurrentUserId);
            user.AccessToken = AccessToken;
            var response = service.UpdateUser(user);
            if (response.IsSuccess)
                return JsonResult(true, "更改成功！");
            return JsonResult(false, "更改失败，请重试！");
        }
        public ActionResult Read(int id=0)
        {
            var msg = new MessageService();
            msg.SetRead(id);
            return JsonResultForFly(1, "成功！");
        }
        [AllowAnonymous]
        public ActionResult ResetPwd()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(RegisterModel email)
        {
            return JsonResultForFly(1, "发送成功！请稍等...");
        }
    }
}