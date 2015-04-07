using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarMate.Controllers
{
    public class CarConsumptionController : BaseController
    {
        public ActionResult Index(int carId)
        {
            var carEvents = db.CarEvents.Where(x => x.CarId == carId && x.IsFullTank == true).OrderByDescending(x => x.DateCreate).ToList();
            return View(carEvents);
        }

    }
}
