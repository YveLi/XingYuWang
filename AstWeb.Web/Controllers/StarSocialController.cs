using AstWeb.Common.Models;
using AstWeb.Common.Services;
using AstWeb.Common.Services.Messaging;
using AstWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace AstWeb.Controllers
{
    public class StarSocialController : BaseController
    {
        //
        // GET: /StarSocial/
        [AllowAnonymous]
        public ActionResult Index(int pageIndex = 1, int CategoriesId = 0)
        {
            ViewBag.user = new UserService().GetUserInfo(CurrentUserId);
            var postCategoryResponse = new PostCategoryService().GetPostCategories();
            ViewBag.PostCategories = postCategoryResponse.Items;
            ViewBag.posttype = new PostCategoryService().GetPostType(CategoriesId);
            var request = new GetPagePostsRequest(pageIndex, PageSize)
            {
                CategoriesId = CategoriesId
            };
            var PostList = new PostService().GetPagePostsByFilter(request);
            var Postmodel = new SocialCount
            {
                allpostcount = new PostService().GetPostCountByUserId(0),
                catepostcount = new PostService().GetPostsCount(CategoriesId),
                usercount = new UserService().GetUserCount(),
                astercount = new UserService().GetAsterCount(),
            };
            ViewBag.Postmodel = Postmodel;
            //ViewBag.MyVisitors = new VisitorService().GetTopTwelve(CurrentUserId);
            return View(PostList);
        }
        [AllowAnonymous]
        public ActionResult Seach(int pageIndex = 1, string title = "1=1")
        {
            ViewBag.user = new UserService().GetUserInfo(CurrentUserId);
            var postCategoryResponse = new PostCategoryService().GetPostCategories();
            ViewBag.PostCategories = postCategoryResponse.Items;
            var request = new GetPagePostsRequest(pageIndex, PageSize)
            {
                CategoriesId = 0,
                Title = title,
            };
            var PostList = new PostService().GetPagePostsByFilter(request);
            var Postmodel = new SocialCount
            {
                allpostcount = new PostService().GetPostCountByUserId(0),
                catepostcount = new PostService().GetPostsCount(0),
                usercount = new UserService().GetUserCount()
            };
            ViewBag.Postmodel = Postmodel;
            return View(PostList);
        }

        public ActionResult Add()
        {
            var postCategoryResponse = new PostCategoryService().GetPostCategories();
            ViewBag.PostCategories = postCategoryResponse.Items;
            return View();
        }
        public ActionResult AddAdmin()
        {
            var postCategoryResponse = new PostCategoryService().GetPostCategories();
            ViewBag.PostCategories = postCategoryResponse.Items;
            return View();
            //发帖管理员超级模式
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddAdmin(AdminPost model)
        {
            var post = new PostService();
            var user = new UserService();
            //先建用户拿到用户ID再建贴子
            var userentity = new W_Users
            {
                NickName = model.NickName,
                Gender = model.Gender == 1 ? Gender.Man : Gender.Woman,
                Birthday = DateTime.Now,
                PhoneNumber = DateTime.Now.ToString("HHmmss") + new Random().Next(0, 9) * 10,
                Password = "xingzuoxingyu2008",
                Integral = 50,
                HeadPortrait = GetRandomFileName(),
            };
            user.Register(userentity);
            var userId = user.GetUserByNickname(model.NickName).Id;
            try
            {
                var postentity = new Posts
                {
                    Title = model.Title,
                    Content = model.Content,
                    TUserId = userId,
                    PostCategoryId = model.PostCateogryId,
                    Reward = model.Reward,
                    PostStatus = PostStatus.Open,
                    Sort = 0,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    Hits = 0,
                    Collection = 0,
                    IsShow = 1,
                };
                var response = post.SendPost(postentity);
                return JsonResultForFly(response.IsSuccess ? 0 : 1, response.Message);
            }
            catch
            {
                return JsonResultForFly(1, "失败，请检查内容格式");
            }
        }
        //发贴
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(PostModel model)
        {
            //if (!EmailConfirmed)
            //    return JsonResultForFly(1, "请先验证邮箱哦.");
            return AddOrEdit(model);
        }
        private ActionResult AddOrEdit(PostModel model)
        {
            if (model == null)
                return JsonResultForFly(1, "请勿传递非法参数，请刷新重试！");
            if (model.Title.Length < 5)
                return JsonResultForFly(1, "标题不能少于5个字符！");
            if (model.Content.Length < 20)
                return JsonResultForFly(1, "内容不能少于20个字符！");

            ////验证验证码是否正确
            //var sessionVerifyCode = Session["VerifyCode"];
            //if (sessionVerifyCode == null || model.Vercode != sessionVerifyCode.ToString())
            //    return JsonResultForFly(1, "验证码输入错误！");
            var service = new PostService();
            //编辑
            if (model.Id != 0)
            {
                var entity = service.GetPostById(model.Id);
                if (entity == null)
                    return JsonResultForFly(1, "请勿传递非法参数，请刷新重试！！");

                entity.Content = model.Content;
                entity.PostCategoryId = model.PostCateogryId;
                entity.Reward = model.Reward;
                var response = service.UpdatePost(entity);
                return JsonResultForFly(response.IsSuccess ? 0 : 1, response.Message, "/post/" + entity.Id);
            }
            else
            {
                var entity = new Posts
                {
                    Title = model.Title,
                    Content = model.Content,
                    TUserId = CurrentUserId,
                    PostCategoryId = model.PostCateogryId,
                    Reward = model.Reward,
                    PostStatus = PostStatus.Open,
                    Sort = 0,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    Hits = 0,
                    Collection = 0,
                    IsShow = 1,
                };
                var response = service.SendPost(entity);
                return JsonResultForFly(response.IsSuccess ? 0 : 1, response.Message, "/post/" + entity.Id);
            }
        }

        //详情
        [AllowAnonymous]
        public ActionResult Detail(int postid, int pageIndex = 1)
        {
            var service = new PostService();
            var post = service.GetPostById(postid);
            service.HitsPlusOne(post);
            ViewBag.CurrentUserId = CurrentUserId;
            ViewBag.IsAdmin = IsAdmin;
            var comments = new CommentService().GetPageCommentsByPostId(pageIndex, PageSize, postid);
            if (comments.IsSuccess && IsLogin)
            {
                var likeService = new ArticleLikesService();
                foreach (var item in comments.Pages.Items)
                {
                    item.IsLike = likeService.ZanIsExists(item.Id, CurrentUserId);
                }
            }
            ViewBag.Comments = comments;
            ViewBag.Posts = post;
            ViewBag.User = new UserService().GetUserInfo(CurrentUserId);
            var postCategoryResponse = new PostCategoryService().GetPostCategories();
            ViewBag.PostCategories = postCategoryResponse.Items;
            var Postmodel = new SocialCount
            {
                allpostcount = new PostService().GetPostCountByUserId(0),
                //catepostcount = new PostService().GetPostsCount(CategoriesId),
                usercount = new UserService().GetUserCount(),
                astercount = new UserService().GetAsterCount(),
            };
            ViewBag.Postmodel = Postmodel;
            return View(post);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        public ActionResult Reply(ReplyModel model)
        {
            if (!IsLogin)
                return JsonResultForFly(1, "你还没有登录呢，请登录后再来回复吧！");
            //if (!EmailConfirmed)
            //    return JsonResultForFly(1, "请先验证邮箱哦.");
            var names = new List<string>();
            var arrNames = model.Content.Split('@');
            foreach (var item in arrNames)
            {
                try
                {
                    var name = item.Length > 0 ? item.Trim() : item;
                    if (name.Length > 0)
                        names.Add(name);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            var entity = new Comment
            {
                UserId = CurrentUserId,
                PostId = model.Id,
                IsPraise = 0,
                Content = model.Content,
                CreateTime = DateTime.Now,
                Ticks = DateTime.Now.Ticks
            };
            //var post = new PostService().GetPostById(model.PostId);
            //post.CommentCount += 1;
            //new PostService().UpdatePost(post);
            var response = new CommentService().AddComment(entity, CurrentUserId, names);
            return JsonResultForFly(response.IsSuccess ? 0 : 1, response.Message);
        }

        //帖子采纳
        [HttpPost]
        public ActionResult Accept(int id)
        {
            var response = new CommentService().Accept(CurrentUserId, id);
            return JsonResultForFly(response.IsSuccess ? 0 : 1, response.Message);
        }

        [HttpPost]
        public ActionResult SeachPost(string title = "1=1", int pageIndex = 1)
        {
            var request = new GetPagePostsRequest(pageIndex, PageSize)
            {
                CategoriesId = 0,
                Title = title
            };
            var PostList = new PostService().GetPagePostsByFilter(request);
            return View(PostList);
        }
        public int Boutique(int id)
        {
            var response = new PostService().Boutique(id);
            return response;
        }
        public int Top(int id)
        {
            var response = new PostService().Top(id);
            return response;
        }
        public int Show(int id)
        {
            var response = new PostService().Show(id);
            return response;
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
    }
}
