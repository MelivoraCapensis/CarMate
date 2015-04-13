using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarMate.Controllers
{
    public class CarCostStatisticsController : BaseController
    {
        //
        // GET: /CarCostStatistics/

        public ActionResult Index(int carId)
        {
            CarAndUserInit(carId);
            ViewBag.EventTypes = db.EventTypes.OrderBy(x => x.Name).Select(x => x.Name).ToList();
            var carEevEvents = db.CarEvents.Where(x => x.CarId == carId).ToList();
            return View(carEevEvents);
        }

        public void CarAndUserInit(int carId)
        {
            var car = db.Cars.Find(carId);
            ViewBag.Car = car;
            ViewBag.User = db.Users.Find(car.UserId);
        }

    }
}
