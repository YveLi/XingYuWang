using AstWeb.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AstWeb.Controllers
{
    public class StarTestController : BaseController
    {
        //
        // GET: /StarTest/
        [AllowAnonymous]
        public ActionResult Index()
        {
            var data = new StarTestService().GetTest();
            ViewBag.test = data;
            return View();
        }
        [AllowAnonymous]
        public ActionResult Detail(int id=0)
        {
            //var id = Request.QueryString["id"];
            ViewBag.user = new UserService().GetUserInfo(CurrentUserId);
            ViewBag.test = new StarTestService().GetTestById(id);
            return View();
        }
    }
}
