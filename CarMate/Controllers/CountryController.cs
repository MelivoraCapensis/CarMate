using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarMate.Models;

namespace CarMate.Controllers
{
    public class CountryController : Controller
    {
        //
        // GET: /Country/

        public ActionResult ByCountry(int id)
        {
            CountryModel model = new CountryModel();
            using (CarMateEntities context = new CarMateEntities())
            {
                List<Countries> CountryList = context.Countries.ToList();
                model.CountryList = CountryList.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Country,
                        Value = x.Id.ToString()
                    });
            }
            return PartialView(model);
        }

    }
}
