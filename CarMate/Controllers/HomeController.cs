using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarMate.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeCulture(string lang)
        {
            var cookie = new HttpCookie("lang", lang) { HttpOnly = true };
            Response.AppendCookie(cookie);
            return RedirectToAction("Index", "Home", new { culture = lang });
        }
    }
}
