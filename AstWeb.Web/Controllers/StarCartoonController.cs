using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AstWeb.Common.Models;
using AstWeb.Common.Services;
using AstWeb.Models;

namespace AstWeb.Controllers
{
    public class StarCartoonController : BaseController
    {
        //
        // GET: /StarCartoon/
        [AllowAnonymous]
        public ActionResult Index()
        {
            var keyword = "";
            if (Request.QueryString["keyword"] != null)
            {
                keyword = Request.QueryString["keyword"];
            }
            ViewBag.CartoonCategories = new CartoonCategoriesService().GetPageCartoonCategories(1, 15, keyword).Pages == null ?null : new CartoonCategoriesService().GetPageCartoonCategories(1, 15, keyword).Pages.Items;
            return View();
        }
        [AllowAnonymous]
        public ActionResult Contents(int pageIndex = 1, int id = 0)
        {
            var keyword = "";
            if (Request.QueryString["keyword"] != null)
            {
                keyword = Request.QueryString["keyword"];
            }
            if (id == 0)
            {
                ViewBag.Cartoon = new List<StarCartoon>();
            }
            else
            {
                ViewBag.Cartoon = new StarCartoonService().GetStarCartoon(id, pageIndex, 16,keyword);
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult Detail(int id)
        {
            var service = new StarCartoonService();
            var uparticleList = new StarCartoonService().GetPageArticleByOne(id, 0);
            var downarticleList = new StarCartoonService().GetPageArticleByOne(id, 1);
            var articles = service.GetArticleById(id);
            service.HitsPlusOne(articles);
            ViewBag.CurrentUserId = CurrentUserId;
            ViewBag.IsAdmin = IsAdmin;
            ViewBag.User = new UserService().GetUserInfo(CurrentUserId);
            ViewBag.articles = articles;
            ViewBag.uparticleList = uparticleList;
            ViewBag.downarticleList = downarticleList;
            return View();
        }
        [AllowAnonymous]
        public ActionResult XingLi()
        {
            return View();
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
            var entity = new CartoonComment
            {
                UserId = CurrentUserId,
                ArticleId = model.Id,
                IsPraise = 0,
                Content = model.Content,
                CreateTime = DateTime.Now,
                Ticks = DateTime.Now.Ticks
            };
            var response = new CartoonCommentService().AddComment(entity, CurrentUserId, names);
            return JsonResultForFly(response.IsSuccess ? 0 : 1, response.Message);
        }
        [AllowAnonymous]
        public PartialViewResult LoadComment(int id, int pageIndex = 1)
        {
            var comments = new CartoonCommentService().GetPageCommentsByArticleId(pageIndex, PageSize, id);
            return PartialView("_CartoonComment", comments);
        }
    }
}
