using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarMate.Models;

namespace CarMate.Controllers
{
    public class CategoryController : Controller
    {
        //
        // GET: /Category/

        public ActionResult CategoryByCountry()
        {
            CategoryModel model = new CategoryModel();
            using (CarMateEntities context = new CarMateEntities())
            {
                var categories = context.Categories.Select(x => x).ToList();
                model.CategoryList = categories;
                model.SelectedCategoryIDs = new List<int>() { 1,5 };
            }
            return PartialView(model);
        }

    }
}
