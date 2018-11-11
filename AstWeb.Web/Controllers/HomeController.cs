using AstWeb.Common.Models;
using AstWeb.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AstWeb.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.bannerlist = new BannerService().GetBannerList(0).Items;//index_banner
            ViewBag.postlist = new PostService().GetHotCommentPostsTopNine();
            ViewBag.astrolist = new ArticlesService().GetFastArticles();//星座快讯
            ViewBag.aster = new UserService().GetAsterListToHome();
            ViewBag.CartoonCategories = new CartoonCategoriesService().GetPageCartoonCategories(1, 15, "").Pages.Items;
            return View();
        }
        [AllowAnonymous]
        public ActionResult Seach(string title, int pageIndex = 1)
        {
            ViewBag.user = new UserService().GetUserInfo(CurrentUserId);
            return View();
        }
        /// <summary>
        /// 热门文章
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public PartialViewResult LoadHot()
        {
            var hotlist = new ArticlesService().GetHotArticlesTopTen();
            return PartialView("_HotArticle", hotlist);
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.articles= new ArticlesService().GetArticlesByUserId(-1).Items[0];
            return View();
        }
        [AllowAnonymous]
        public ActionResult getData(W_Users model)
        {
            model.NickName = model.NickName + new Random().Next(0, 99);
            model.Birthday = DateTime.Now;
            model.PhoneNumber = DateTime.Now.ToString("HHmmss") + new Random().Next(0,9)*10;
            model.Password = "123456";
            new UserService().Register(model);
            return JsonResult(true, "发送成功", 0, null);
        }
    }
}
