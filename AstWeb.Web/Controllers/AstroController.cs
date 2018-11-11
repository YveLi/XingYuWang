using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AstWeb.Common.Services;
using AstWeb.Common.Services.Messaging;
using AstWeb.Models;
using AstWeb.Common.Models;

namespace AstWeb.Controllers
{
    public class AstroController : BaseController
    {
        //
        // GET: /Astro/
        [AllowAnonymous]
        public ActionResult Index()
        {
            var TwelveAstro = new TwelveAstroService().GetTwelveAstroList();
            ViewBag.astro = TwelveAstro.Items;
            var ArticleType = new ArticleCategoryService().GetArticleCategories(0);
            ViewBag.ArticleType = ArticleType.Items;
            var ArticleTypes = new ArticleCategoryService().GetArticleCategories(1);
            ViewBag.ArticleTypes = ArticleTypes.Items;
            var bannerlist = new BannerService().GetBannerList(2);
            ViewBag.bannerlist = bannerlist.Items;
            return View();
        }
        [AllowAnonymous]
        public ActionResult AstroDetail(int astroid,int id=1)
        {
            var TwelveAstro = new TwelveAstroService().GetTwelveAstroList();
            ViewBag.astro = TwelveAstro.Items;
            ViewBag.queryId = id;
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult LoadArticle(string title, int CategoriesId = 0, int pageIndex = 1)
        {
            var request = new GetPagePostsRequest(pageIndex, PageSize)
            {
                CategoriesId = CategoriesId,
                Title = title
            };
            var ArticleList = new ArticlesService().GetPageArticlesByFilter(request);
            return PartialView("_ArticleModule", ArticleList);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetArticleTypeList(int CategoriesId)
        {
            var response = new ArticleCategoryService().GetArticleCategories(CategoriesId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        //详情
        [AllowAnonymous]
        public ActionResult Detail(int articleid, int pageIndex = 1)
        {
            var request = new GetPagePostsRequest(pageIndex, 1);
            var service = new ArticlesService();
            var uparticleList = new ArticlesService().GetPageArticleByOne(articleid, 0);
            var downarticleList = new ArticlesService().GetPageArticleByOne(articleid, 1);
            var articles = service.GetArticleById(articleid);
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
        public PartialViewResult LoadComment(int articleid, int pageIndex = 1)
        {
            var comments = new ArticleCommentService().GetPageCommentsByArticleId(pageIndex, PageSize, articleid);
            if (comments.IsSuccess && IsLogin)
            {
                var likeService = new ArticleLikesService();
                foreach (var item in comments.Pages.Items)
                {
                    item.IsLike = likeService.ZanIsExists(item.Id, CurrentUserId);
                }
            }
            return PartialView("_ArticleComment", comments);
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
            var entity = new ArticleComment
            {
                UserId = CurrentUserId,
                ArticleId = model.Id,
                IsPraise = 0,
                Content = model.Content,
                CreateTime = DateTime.Now,
                Ticks = DateTime.Now.Ticks
            };
            var response = new ArticleCommentService().AddComment(entity, CurrentUserId, names);
            return JsonResultForFly(response.IsSuccess ? 0 : 1, response.Message);
        }

        [AllowAnonymous]
        //最近热议的10篇文章
        public PartialViewResult LoadFunny()
        {
            var ArticleList = new ArticlesService().GetRandomArticles();
            return PartialView("_ArticleFunny", ArticleList);
        }

        public ActionResult Add()
        {
            return View();
        }

        //发贴
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddArticle(PostModel model)
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
            var service = new ArticlesService();
            var entity = new Articles
            {
                Title = model.Title,
                Content = model.Content,
                TUserId = CurrentUserId,
                ArticleCategoryId = 51,
                //ArticleStatus = ArticleStatus.Close,
                Sort = 0,
                AddTime = DateTime.Now,
                ModifyTime = DateTime.Now,
                IsShow = 0,
                Source = model.Source,
            };
            var response = service.AddArticle(entity);
            return JsonResultForFly(response.IsSuccess ? 0 : 1, response.Message, "/article/" + entity.Id);
        }

        [HttpPost]
        public ActionResult SeachPost(string title = "1=1", int pageIndex = 1)
        {
            var request = new GetPagePostsRequest(pageIndex, PageSize)
            {
                CategoriesId = 0,
                Title = title
            };
            var ArticleList = new ArticlesService().GetPageArticlesByFilter(request);
            return View(ArticleList);
        }
    }
}
