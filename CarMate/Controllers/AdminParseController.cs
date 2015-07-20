using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarMate.Classes;
using Hangfire;

namespace CarMate.Controllers
{
    public class AdminParseController : BaseAdminController
    {
        //
        // GET: /AdminParse/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ParseBrandsAndRegions()
        {
            RecurringJob.AddOrUpdate(() => ParsingJob.ParsePriceFromBrandsAndRegions(), Cron.Daily);
            ViewBag.BrandsAndRegions = "Готово";

            return View("Index");
        }

        public ActionResult ParseBrands()
        {
            RecurringJob.AddOrUpdate(() => ParsingJob.ParsePriceFromBrands(), Cron.Daily);
            ViewBag.Brands = "Готово";

            return View("Index");
            
        }

        public ActionResult Hangfire()
        {
            return Redirect("/hangfire");
        }

    }
}
