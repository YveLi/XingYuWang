using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AstWeb.Controllers
{
    public class SalonController : BaseController
    {
        //
        // GET: /Salon/
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

    }
}
