using System;
using System.Collections.Generic;
using System.Globalization;
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
            var carEevEvents = db.CarEvents.Where(x => x.CarId == carId).OrderBy(x => x.DateEvent).ToList();
            return View(carEevEvents);
        }

        public void CarAndUserInit(int carId)
        {
            var car = db.Cars.Find(carId);
            ViewBag.Car = car;
            ViewBag.User = db.Users.Find(car.UserId);
        }

        public JsonResult GetCostStatistics(int carId = 0)
        {
            var car = db.Cars.Find(carId);
            if (car == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            var carEvents = db.CarEvents.Where(x => x.CarId == carId).OrderBy(x => x.DateEvent).ToList();

            //Dictionary<string, double> costStatistics = new Dictionary<string, double>();
            List<Test> tests = new List<Test>();

            foreach (var carEvent in carEvents)
            {
                //if (costStatistics.ContainsKey(carEvent.EventTypes.Name))
                var t = tests.FirstOrDefault(x => x.Name.Equals(carEvent.EventTypes.Name));
                if (t != null)
                {
                    //costStatistics[carEvent.EventTypes.Name] += carEvent.CostTotal;

                    t.Details.Add(new Details
                    {
                        DateCreate = carEvent.DateEvent.ToString(CultureInfo.CurrentCulture),
                        Ticks = carEvent.DateEvent.ToLocalTime().Ticks,
                        Cost = carEvent.CostTotal,
                        Year = carEvent.DateEvent.Year,
                        Month = carEvent.DateEvent.Month
                    });
                    t.Summ += carEvent.CostTotal;
                }
                else
                {
                    //costStatistics[carEvent.EventTypes.Name] = carEvent.CostTotal;
                    t = new Test {Summ = carEvent.CostTotal, Name = carEvent.EventTypes.Name};
                    t.Details.Add(new Details
                    {
                        DateCreate = carEvent.DateEvent.ToString(CultureInfo.CurrentCulture),
                        Ticks = carEvent.DateEvent.ToLocalTime().Ticks,
                        Cost = carEvent.CostTotal,
                        Year = carEvent.DateEvent.Year,
                        Month = carEvent.DateEvent.Month
                    });
                    tests.Add(t);
                }
            }

            return Json(tests, JsonRequestBehavior.AllowGet);
        }

    }

    public class Test
    {
        public string Name { set; get; }
        public double Summ { set; get; }

        public List<Details> Details { set; get; }

        public Test()
        {
            Details = new List<Details>();
        }
    }

    public class Details
    {
        public string DateCreate { set; get; }
        public int Year { set; get; }
        public int Month { set; get; }
        public double Cost { set; get; }
        public double Ticks { set; get; }
    }

    public class CostStatistic
    {
        public string AllEventsSumm { set; get; }  // Сумма за все события, за определенный период
        public string EventName { set; get; }   // Название события
        public string EventSumm { set; get; }   // Сумма за событие, за определенный период
    }
}
