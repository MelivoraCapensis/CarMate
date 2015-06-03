using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarMate.Models;

namespace CarMate.Controllers
{
    public class RegionController : Controller
    {
        public ActionResult RegionByCountry(int id)
        {
            RegionModel model = new RegionModel();
            using (CarMateEntities context = new CarMateEntities())
            {
               
                var regions = context.Regions.Where(x => x.CountryId == id).ToList();
                model.RegionList = regions.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Region,
                        Value = x.Id.ToString(),
                    });
            }
            return PartialView(model);
        }

    }
}
