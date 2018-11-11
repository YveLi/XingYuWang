using AstWeb.Common.Models;
using AstWeb.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AstWeb.Controllers
{
    public class AstrolabeController : BaseController
    {
        //
        // GET: /Astrolabe/
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.user = new UserService().GetUserInfo(CurrentUserId);
            return View();
        }

      
    }
}
