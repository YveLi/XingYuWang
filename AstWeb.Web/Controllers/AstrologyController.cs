using AstWeb.Common.Infrastructure;
using AstWeb.Common.Models;
using AstWeb.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AstWeb.Controllers
{
    public class AstrologyController : BaseController
    {
        //
        // GET: /Astrology/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Aster(int userid)
        {
            ViewBag.user = new UserService().GetUserInfo(userid);
            ViewBag.CurrentUserId = CurrentUserId;
            ViewBag.aster = new UserService().GetAsterListToHome();
            if (CurrentUserId != -1 && userid != CurrentUserId)//证明有登录的 且不是自己访问自己则更新访客记录
            {
                new VisitorService().AddVisitor(new Visitor
                {
                    VisitorUserId = CurrentUserId,
                    UserId = userid,
                    LastTime = DateTime.Now
                });
            }
            ViewBag.IsGZ = new AsterLikesService().IsExistsGuanZhu(CurrentUserId, userid);
            return View();
        }

        /// <summary>
        /// 关注或取消
        /// </summary>
        /// <returns></returns>
        public ActionResult AddOrDelLike(int userid, int type)
        {
            var response = new Response();
            if (CurrentUserId != -1 && userid != CurrentUserId)//证明有登录的 且不是自己访问自己则更新访客记录
            {
                var service = new AsterLikesService();
                response = service.AddOrRemoveLike(CurrentUserId, type == 1 ? true : false, userid);
            }
            return JsonResultForFly(response.IsSuccess ? 0 : 1, response.Message);
        }
    }
}
