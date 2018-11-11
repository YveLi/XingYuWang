using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AstWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region  ==用户相关==
            //登录
            routes.MapRoute("Home", "user/home/{userId}", new { controller = "Account", action = "Index" });
            routes.MapRoute("MyHome", "user/myhome/{userId}", new { controller = "Account", action = "MyHome" });
            routes.MapRoute("MyPass", "user/mypass/{userId}", new { controller = "Account", action = "MyPass" });
            routes.MapRoute("MyAvatar", "user/myavatar/{userId}", new { controller = "Account", action = "MyAvatar" });
            routes.MapRoute("MyPost", "user/mypost/{userId}/page_{pageIndex}", new { controller = "Account", action = "MyPost" });
            routes.MapRoute("MySign", "user/mysign/{userId}", new { controller = "Account", action = "MySign" });
            routes.MapRoute("MyFans", "user/myfans/{userId}", new { controller = "Account", action = "MyFans" });
            routes.MapRoute("MyMsg", "user/mymsg/{userId}", new { controller = "Account", action = "MyMsg" });
            routes.MapRoute("Login", "user/login/", new { controller = "Account", action = "Login" });
            //注册
            routes.MapRoute("Register", "user/register/", new { controller = "Account", action = "Register" });
            routes.MapRoute("Forget", "user/forget/", new { controller = "Account", action = "ResetPwd" });
            #endregion

            #region ==主页==
            routes.MapRoute("HomeIndex", "home/", new { controller = "Home", action = "Index" });
            #endregion

            #region  ==星座==
            routes.MapRoute("AstroIndex", "astro/home/", new { controller = "Astro", action = "Index" });
            routes.MapRoute("XzAstroDetail", "astro/{astroname}-{astroid}", new { controller = "Astro", action = "AstroDetail" });
            routes.MapRoute("AstroAdd", "astro/article/add", new { controller = "Astro", action = "Add" });
            routes.MapRoute("AstroDetail", "astro/detail/{articleid}", new { controller = "Astro", action = "Detail" });
            routes.MapRoute("HomeSeach", "home/seach/page_{pageIndex}", new { controller = "Home", action = "Seach" });
            #endregion

            #region ==星漫==
            routes.MapRoute("StarCartoonDetail", "StarCartoon/Detail/{id}", new { controller = "StarCartoon", action = "Detail" });
            routes.MapRoute("StarCartoonContents", "StarCartoon/Contents/{id}/{pageIndex}", new { controller = "StarCartoon", action = "Contents" });
            #endregion
            #region ==星测==
            routes.MapRoute("StarTest", "StarTest/Detail/{id}", new { controller = "StarTest", action = "Detail" });
            #endregion
            #region ==星盘==
            routes.MapRoute("Astrolabe", "astrolabe/", new { controller = "Astrolabe", action = "Index" });
            #endregion

            #region  ==社区主页==
            //社区首页
            routes.MapRoute("SocialIndex", "social/home/{CategoriesId}/page_{pageIndex}", new { controller = "StarSocial", action = "Index" });
            //社区搜索
            routes.MapRoute("SocialSeach", "social/seach/page_{pageIndex}", new { controller = "StarSocial", action = "Seach" });
            //发帖
            routes.MapRoute("SocialAdd", "social/post/add", new { controller = "StarSocial", action = "Add" });
            //详情
            routes.MapRoute("SocialDetail", "social/post/detail/{postid}page_{pageIndex}", new { controller = "StarSocial", action = "Detail" });
            //回帖
            routes.MapRoute("SocialReply", "social/reply", new { controller = "StarSocial", action = "Reply" });
            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
