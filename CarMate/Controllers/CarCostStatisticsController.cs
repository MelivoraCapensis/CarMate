using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarMate.Classes;

namespace CarMate.Controllers
{
    public class CarCostStatisticsController : BaseController
    {
        //
        // GET: /CarCostStatistics/

        public ActionResult Index(int carId)
        {
            Cars car = Db.Cars.Find(carId);
            InitViewBag(car.Users);
            ConvertTankLoad(car);
            ConvertOdometrLoad(car);
            ConvertConsumptionLoad(car);

            CarAndUserInit(carId);
            ViewBag.EventTypes = Db.EventTypes.OrderBy(x => x.Name).Select(x => x.Name).ToList();
            var carEevEvents = Db.CarEvents.Where(x => x.CarId == carId).OrderBy(x => x.DateEvent).ToList();

            Owner(HttpContext);
            
            ViewBag.IsOwner = this.UserId == car.UserId;

            return View(carEevEvents);
        }

        private void InitViewBag(Users user)
        {
            var unitDistanceLang = user.UnitDistance.UnitDistanceLang.FirstOrDefault(x => x.LanguageId == CurrentLang.Id);
            if (unitDistanceLang != null)
            {
                ViewBag.UnitDistance = unitDistanceLang.NameUnit;
            }

            ViewBag.UnitFuelConsumption = user.UnitFuelConsumption.NameUnit;

            ViewBag.UnitVolume = user.UnitVolume.NameUnit;

            ViewBag.LanguageId = this.CurrentLang.Id;
            ViewBag.LanguageName = this.CurrentLang.Code;
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carEvents.FuelCategoryId);
        }

        public void Owner(HttpContextBase httpContext)
        {
            // Если пользователь авторизован
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                this.UserId = Db.Users
                    .Where(x => x.Nickname.Equals(HttpContext.User.Identity.Name))
                    .Select(x => x.Id)
                    .FirstOrDefault();

                ViewBag.Owner = this.UserId;
            }
        }

        public void CarAndUserInit(int carId)
        {
            var car = Db.Cars.Find(carId);
            ViewBag.Car = car;
            ViewBag.User = Db.Users.Find(car.UserId);

            //var unitDistanceLang = Db.Users.Find(car.UserId).UnitDistance.UnitDistanceLang.FirstOrDefault(x => x.LanguageId == CurrentLang.Id);
            //if (unitDistanceLang != null)
            //{
            //    string unitDistance = unitDistanceLang.NameUnit;
            //    ViewBag.UnitDistance = unitDistance;
            //    if (car.Odometer != null)
            //    {
            //        car.Odometer = (int)Math.Round(ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, unitDistance, (double)car.Odometer));
            //    }
            //}

            //var unitDistanceLang = Db.Users.Find(car.UserId).UnitDistance.UnitDistanceLang.FirstOrDefault(x => x.LanguageId == CurrentLang.Id);
            //if (ViewBag.UnitDistance != null)
            //{
            //    if (car.Odometer != null)
            //    {
            //        car.Odometer = (int)Math.Round(
            //            ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, ViewBag.UnitDistance, (double)car.Odometer));
            //    }
            //}


            //string unitFuelConsumption = Db.Users.Find(car.UserId).UnitFuelConsumption.NameUnit;
            //ViewBag.UnitFuelConsumption = unitFuelConsumption;
            //if (ViewBag.UnitFuelConsumption != null)
            //{
            //    if (car.Consumption != null)
            //    {
            //        car.Consumption = Math.Round(
            //            ConverterUnitFuelConsumption.ConverterFuelConsumptionLoad(ViewBag.UnitFuelConsumption, (double)car.Consumption), 2);
            //    }
            //}


            //string unitVolume = Db.Users.Find(car.UserId).UnitVolume.NameUnit;
            //ViewBag.UnitVolume = unitVolume;
            //if (ViewBag.UnitVolume != null)
            //{
            //    if (car.Tank != null)
            //    {
            //        car.Tank = (int)Math.Round(ConverterUnitVolume.ConvertVolumeFromLiters(ViewBag.UnitVolume, (int)car.Tank));
            //    }
            //}
        }

        public void ConvertConsumptionLoad(Cars car)
        {
            if (ViewBag.UnitFuelConsumption != null)
            {
                if (car.Consumption != null)
                {
                    //ViewBag.UnitVolume = Db.Users.Find(carEvents.Cars.UserId).UnitVolume.NameUnit;
                    car.Consumption = Math.Round(
                        ConverterUnitFuelConsumption.ConverterFuelConsumptionLoad(ViewBag.UnitFuelConsumption, (double)car.Consumption), 2);
                }
            }
        }

        public void ConvertOdometrLoad(Cars car)
        {
            if (ViewBag.UnitDistance != null)
            {
                if (car.Odometer != null)
                {
                    car.Odometer = (int)Math.Round(
                        ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, ViewBag.UnitDistance,
                            (int)car.Odometer));
                }
            }
        }

        public void ConvertTankLoad(Cars car)
        {
            if (ViewBag.UnitVolume != null)
            {
                if (car.Tank != null)
                {
                    //ViewBag.UnitVolume = Db.Users.Find(carEvents.Cars.UserId).UnitVolume.NameUnit;
                    car.Tank = (int)Math.Round(
                        ConverterUnitVolume.ConvertVolumeFromLiters(ViewBag.UnitVolume, (double)car.Tank));
                }
            }
        }

        public JsonResult GetCostStatistics(int carId = 0)
        {
            var car = Db.Cars.Find(carId);
            if (car == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }


            var carEvents = Db.CarEvents.Where(x => x.CarId == carId).OrderBy(x => x.DateEvent).ToList();

            //Dictionary<string, double> costStatistics = new Dictionary<string, double>();
            List<Test> tests = new List<Test>();
            double allSumm = 0;
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
                    allSumm += carEvent.CostTotal;
                }
                else
                {
                    //costStatistics[carEvent.EventTypes.Name] = carEvent.CostTotal;
                    t = new Test {Summ = carEvent.CostTotal, Name = carEvent.EventTypes.Name};
                    allSumm += carEvent.CostTotal;
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
            allSumm = Math.Round(allSumm, 2);

            foreach (Test t in tests)
            {
                t.AllSumm = allSumm;
            }

            return Json(tests, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCostStatisticsFromPeriod(int carId, string startDate, string endDate)
        {
            var car = Db.Cars.Find(carId);
            if (car == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            List<CarEvents> carEvents = new List<CarEvents>();


            CultureInfo ci = new CultureInfo("ru");
            DateTime start;
            DateTime end;
            // Если удалось преобразовать дату начала из строки в дату, то фильтруем по дате начала
            if (DateTime.TryParseExact(startDate, "dd.MM.yyyy", ci, DateTimeStyles.None, out start))
            {
                var carEventsTmp = Db.CarEvents
                    .Where(x => x.CarId == carId && x.DateEvent >= start)
                    .OrderBy(x => x.DateEvent);

                DateTime endTmp;
                // Если удалось преобразовать дату конца из строки в дату, то фильтруем по дате конца
                if (DateTime.TryParseExact(endDate, "dd.MM.yyyy", ci, DateTimeStyles.None, out endTmp))
                {
                    carEvents = carEventsTmp
                        .Where(x => x.CarId == carId && x.DateEvent <= endTmp)
                        .OrderBy(x => x.DateEvent)
                        .ToList();
                }
            }
            // Если не удалось преобразовать дату начала, но удалось преобразовать дату конца, то фильтруем по дате конца
            else if (DateTime.TryParseExact(endDate, "dd.MM.yyyy", ci, DateTimeStyles.None, out end))
            {
                carEvents = Db.CarEvents
                    .Where(x => x.CarId == carId && x.DateEvent <= end)
                    .OrderBy(x => x.DateEvent)
                    .ToList();
            }
            else
            {
                carEvents = Db.CarEvents
                    .Where(x => x.CarId == carId)
                    .OrderBy(x => x.DateEvent)
                    .ToList();
            }
            //var carEvents = Db.CarEvents.Where(x => x.CarId == carId).OrderBy(x => x.DateEvent).ToList();

            //Dictionary<string, double> costStatistics = new Dictionary<string, double>();
            List<Test> tests = new List<Test>();
            double allSumm = 0;
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
                    allSumm += carEvent.CostTotal;
                }
                else
                {
                    //costStatistics[carEvent.EventTypes.Name] = carEvent.CostTotal;
                    t = new Test { Summ = carEvent.CostTotal, Name = carEvent.EventTypes.Name };
                    allSumm += carEvent.CostTotal;
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
            allSumm = Math.Round(allSumm, 2);

            foreach (Test t in tests)
            {
                t.AllSumm = allSumm;
            }

            return Json(tests, JsonRequestBehavior.AllowGet);
        }

    }

    public class Test
    {
        public string Name { set; get; }
        public double Summ { set; get; }
        public double AllSumm { set; get; }

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
