using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarMate.Models;

namespace CarMate.Controllers
{
    public class FuelCategoryController : Controller
    {
        //
        // GET: /FuelCategory/

        public ActionResult CheckFuelCategory(int id)
        {
            FuelCategoryModel model = new FuelCategoryModel();
            using (CarMateEntities context = new CarMateEntities())
            {
                var fuelcategories = context.FuelCategories.Where(x => x.CountryId == id).Select(x => x).ToList();
                model.FuelCategoryList = fuelcategories;
                model.SelectedFuelCategoryIDs = new List<int>() {7};
            }
            return PartialView(model);
        }

    }
}
