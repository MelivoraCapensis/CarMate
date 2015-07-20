using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarMate.Controllers
{
    public class AdminController : BaseAdminController
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }



    }
}
