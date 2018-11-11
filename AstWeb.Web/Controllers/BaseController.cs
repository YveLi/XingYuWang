using AstWeb.Common.Helper;
using AstWeb.Common.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AstWeb.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        /// <summary>
        /// 域名
        /// </summary>
        protected string Domain
        {
            get
            {
                var domain = ConfigurationManager.AppSettings["Domain"];
                return domain == null ? "Domain" : domain;
            }
        }

        /// <summary>
        /// 每页显示的数据数
        /// </summary>
        protected int PageSize
        {
            get
            {
                var pageSize = ConfigurationManager.AppSettings["PageSize"];
                return pageSize == null ? 10 : Convert.ToInt32(pageSize);
            }
        }

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        protected bool IsLogin
        {
            get
            {
                return HttpContext.User.Identity.IsAuthenticated;
            }
        }
        //[AllowAnonymous]
        //public PartialViewResult LoadPCHeader()
        //{
        //    var user = new UserService().GetUserInfo(CurrentUserId);
        //    return PartialView("_PCHeader", user);
        //}
        //[AllowAnonymous]
        //public PartialViewResult LoadMHeader()
        //{
        //    var user = new UserService().GetUserInfo(CurrentUserId);
        //    return PartialView("_MHeader", user);
        //}
        [AllowAnonymous]
        public PartialViewResult LayoutHeader()
        {
            var user = new UserService().GetUserInfo(CurrentUserId);
            ViewBag.user = user;
            return PartialView("_LayoutHeader", user);
        }
        [AllowAnonymous]
        public PartialViewResult LayoutFooter()
        {
            var link = new LinksService().GetLinks();
            return PartialView("_LayoutFooter",link);
        }
        /// <summary>
        /// 当前登录的帐户
        /// </summary>
        protected string CurrentAccount
        {
            get
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                    return "";
                var name = HttpContext.User.Identity.Name;
                var email = name.Split('$')[0];
                return email;
            }
        }
        /// <summary>
        /// 当前登录的用户ID
        /// </summary>
        protected int CurrentUserId
        {
            get
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                    return -1;
                var name = HttpContext.User.Identity.Name;
                var userId = name.Split('$')[1];
                return Convert.ToInt32(userId);
            }
        }
        /// <summary>
        /// 邮箱是否已验证
        /// </summary>
        protected bool EmailConfirmed
        {
            get
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                    return false;
                var name = HttpContext.User.Identity.Name;
                var confirmed = name.Split('$')[2];
                return DataConversion.StrToBool(confirmed);
            }
        }
        /// <summary>
        /// 是否是管理员
        /// </summary>
        protected bool IsAdmin
        {
            get
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                    return false;
                var name = HttpContext.User.Identity.Name;
                var isAdmin = name.Split('$')[3];
                return DataConversion.StrToBool(isAdmin);
            }
        }
        ////登录后的头部信息
        //public PartialViewResult HeaderForLogIn()
        //{
        //    var user = new UserService().GetUserInfo(CurrentUserId);
        //    return PartialView("_HeaderForLogIn", user);
        //}
        ////最近热帖Top15
        //[AllowAnonymous]
        //public PartialViewResult LoadHotPostTopFifty()
        //{
        //    var response = new PostService().GetHotPostsTopFifteen();
        //    return PartialView("_HotPost", response);
        //}
        ////近期热议Top15
        //[AllowAnonymous]
        //public PartialViewResult LoadHotCommentTopFifty()
        //{
        //    var response = new PostService().GetHotCommentPostsTopFifteen();
        //    return PartialView("_HotComment", response);
        //}

        #region Return Data
        protected Dictionary<string, object> GetResult(bool success, string message, object data = null)
        {
            return DataHelper.GetResult(success, message, data);
        }
        protected ActionResult JsonResult(bool success, string message, object data = null)
        {
            return Json(GetResult(success, message, data));
        }
        protected ActionResult JsonResult(bool success, string message, JsonRequestBehavior behavior, object data = null)
        {
            return Json(GetResult(success, message), behavior);
        }
        protected ActionResult JsonResult(bool success, string message, object data, JsonRequestBehavior behavior)
        {
            return Json(GetResult(success, message, data), behavior);
        }
        protected ActionResult JsonResultForFly(int status, string msg)
        {
            return Json(DataHelper.GetResult(status, msg));
        }
        protected ActionResult JsonResultForFly(int status, string msg, string action)
        {
            return Json(DataHelper.GetResult(status, msg, action));
        }
        protected ActionResult JsonResultForFly(int status, string msg, object rows)
        {
            return Json(DataHelper.GetResult(status, msg, rows));
        }
        protected ActionResult JsonResultForFly(int status, string msg, object rows, string code)
        {
            return Json(DataHelper.GetResult(status, msg, rows, code));
        }
        protected ActionResult JsonResultForFly(int status, string msg, Dictionary<string, object> dict)
        {
            return Json(DataHelper.GetResult(status, msg, dict));
        }
        #endregion
    }
}