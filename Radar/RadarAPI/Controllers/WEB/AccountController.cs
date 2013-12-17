using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RadarAPI.Controllers.WEB
{
    public class AccountController : Controller
    {
        public ActionResult Login(string redirect)
        {
            return View();
        }

        public ActionResult Register(string redirect)
        {
            return View();
        }
    }
}
