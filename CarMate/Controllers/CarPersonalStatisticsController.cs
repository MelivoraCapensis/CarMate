using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarMate.Controllers
{
    public class CarPersonalStatisticsController : BaseController
    {
        //
        // GET: /CarPersonalStatistics/

        public ActionResult Index(int carId)
        {
            CarAndUserInit(carId);
            var carEvents = db.CarEvents.Where(x => x.CarId == carId).OrderBy(x => x.DateEvent).ToList();

            return View(carEvents);
        }

        public void CarAndUserInit(int carId)
        {
            var car = db.Cars.Find(carId);
            ViewBag.Car = car;
            ViewBag.User = db.Users.Find(car.UserId);
        }

    }
}
