using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarMate.Models;

namespace CarMate.Controllers
{
    public class VendorController : Controller
    {
        //
        // GET: /Vendor/

        public ActionResult VendorbyPlacemark(List<int>lId)
        {
            VendorModel vmodel = new VendorModel();
            using (CarMateEntities context = new CarMateEntities())
            { 
                
            
            }
            return View();
        }

    }
}
