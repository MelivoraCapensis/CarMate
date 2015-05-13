using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarMate.Classes;
using CarMate.ViewModel;
using Newtonsoft.Json;

namespace CarMate.Controllers
{
    public class CarConsumptionController : BaseController
    {
        public ActionResult Index(int carId)
        {
            double minConsumption, avgConsumption, maxConsumption;
            var fullTankCharging = SelectFullTankCharging(carId, out minConsumption, out avgConsumption, out maxConsumption);
            CarAndUserInit(carId);
            ViewBag.MinConsumption = minConsumption;
            ViewBag.AvgConsumption = avgConsumption;
            ViewBag.MaxConsumption = maxConsumption;

            return View(fullTankCharging);
        }

        public List<CarConsumptionViewModel> SelectFullTankCharging(int carId, out double minConsumption, out double avgConsumption, out double maxConsumption)
        {
            minConsumption = 0;
            avgConsumption = 0;
            maxConsumption = 0;

            // Получаем все события заправок из базы
            var carEvents = db.CarEvents
                .Where(
                    x =>
                        x.CarId == carId &&
                        x.EventTypes.Name.Equals(Consts.EventTypeNameAzs, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.DateEvent)
                .ToList();

            // Ссылка на пердыдущее событие заправки
            CarEvents tmpCarEvent = null;
            // Список событий заправок 
            // (идут парами: 
            // - если 2 заправки подряд с полный бак, то 1 пара
            // - если 3 заправки подряд с полным баком, то 2 пары
            List<CarConsumptionViewModel> fullTankCharging = new List<CarConsumptionViewModel>();

            double summConsumption = 0;
            foreach (var carEvent in carEvents)
            {
                // Если у текущей заправки заправили полный бак
                if (carEvent.IsFullTank)
                {
                    // Это первая заправка полынм баком
                    // - пропустил предыдущую заправку
                    // - до этого заправился не поным баком
                    if (tmpCarEvent == null || carEvent.IsMissedFilling)
                    {
                        tmpCarEvent = carEvent;
                    }
                    // Это НЕ первая заправка полынм баком
                    else
                    {
                        double consumption = 0;
                        int mileage = carEvent.Odometer - tmpCarEvent.Odometer;
                        if (carEvent.FuelCount != null)
                        {
                            consumption = Math.Round(((double)carEvent.FuelCount * 100 / mileage), 2);
                            if (Math.Abs(minConsumption) <= 0|| minConsumption > consumption)
                                minConsumption = consumption;

                            if (Math.Abs(maxConsumption) <= 0 || maxConsumption < consumption)
                                maxConsumption = consumption;

                            summConsumption += consumption;
                        }

                        fullTankCharging.Add(new CarConsumptionViewModel(tmpCarEvent, carEvent, consumption, mileage));
                        tmpCarEvent = carEvent;
                    }
                }
                // Заправка не с полным баком
                else
                {
                    tmpCarEvent = null;
                    //isPreviousChargingFullTank = false;
                }
            }
            avgConsumption = Math.Round(summConsumption/carEvents.Count);

            
            return fullTankCharging.OrderByDescending(x=>x.NewCarEvent.DateEvent).ToList();
        }

        public List<double> SelectFullTankCharging(int carId, DateTime beginDate, DateTime endDate)
        {
            // Получаем все события заправок из базы
            var carEvents = db.CarEvents
                .Where(
                    x =>
                        x.CarId == carId &&
                        x.EventTypes.Name.Equals(Consts.EventTypeNameAzs, StringComparison.OrdinalIgnoreCase) &&
                        x.DateEvent >= beginDate &&
                        x.DateEvent <= endDate)
                .OrderBy(x => x.DateCreate)
                .ToList();

            // Ссылка на пердыдущее событие заправки
            CarEvents tmpCarEvent = null;
            // Список событий заправок 
            // (идут парами: 
            // - если 2 заправки подряд с полный бак, то 1 пара
            // - если 3 заправки подряд с полным баком, то 2 пары
            List<double> fullTankCharging = new List<double>();

            foreach (var carEvent in carEvents)
            {
                // Если у текущей заправки заправили полный бак
                if (carEvent.IsFullTank)
                {
                    // Это первая заправка полынм баком
                    // - пропустил предыдущую заправку
                    // - до этого заправился не поным баком
                    if (tmpCarEvent == null || carEvent.IsMissedFilling)
                    {
                        tmpCarEvent = carEvent;
                    }
                    // Это НЕ первая заправка полынм баком
                    else
                    {
                        double consumption = 0;
                        int mileage = carEvent.Odometer - tmpCarEvent.Odometer;
                        if (carEvent.FuelCount != null)
                        {
                            consumption = Math.Round(((double)carEvent.FuelCount * 100 / mileage), 2);
                        }

                        fullTankCharging.Add(consumption);
                        tmpCarEvent = carEvent;
                    }
                }
                // Заправка не с полным баком
                else
                {
                    tmpCarEvent = null;
                }
            }
            return fullTankCharging;
        }

        public void CarAndUserInit(int carId)
        {
            var car = db.Cars.Find(carId);
            ViewBag.Car = car;
            ViewBag.User = db.Users.Find(car.UserId);
        }

        public string GetConsumptionFromPeriod(int carId, DateTime beginDate, DateTime endDate)
        {
            if (beginDate == DateTime.MinValue)
                beginDate = DateTime.Now.AddMonths(-2);
            if (endDate == DateTime.MinValue)
                endDate = DateTime.Now.AddMonths(2);

            List<double> fullTankChargigng = SelectFullTankCharging(carId, beginDate, endDate);
            string json = JsonConvert.SerializeObject(fullTankChargigng.ToArray());

            return json;
        }

        public JsonResult GetConsumptionStatistics(int carId = 0)
        {
            var car = db.Cars.Find(carId);
            if (car == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            var carEvents = db.CarEvents.Where(x => x.CarId == carId).OrderBy(x => x.DateEvent).ToList();


            List<TestDistance> distance = new List<TestDistance>();
            // Проехано, за определенный период
            //Dictionary<DateTime, double> distance = new Dictionary<DateTime, double>();
            int allDistance = 0;
            int tmpOdometr = 0;
            for (int i = 0; i < carEvents.Count; i++)
            {
                // Определяем общий пробег
                if (i == 0)
                {
                    var begin = carEvents[i].Odometer;
                    var end = carEvents[carEvents.Count - 1].Odometer;
                    allDistance = end - begin;
                    tmpOdometr = begin;
                }

                var result = distance.FirstOrDefault(x => x.DateCreate == carEvents[i].DateEvent);

                if (result != null && result.DateCreate != DateTime.MinValue)
                {
                    result.Distance = carEvents[i].Odometer - tmpOdometr;
                    //result.Ticks = new DateTime(carEvents[i].DateEvent.Year, carEvents[i].DateEvent.Month, 1).ToLocalTime().Ticks;
                    if (carEvents[i].FuelCount != null)
                        result.SumLiters = (double)carEvents[i].FuelCount;
                    else
                        result.SumLiters = 0;
                    result.SumMoney = carEvents[i].CostTotal;
                    result.Name = carEvents[i].NameEvent;
                }
                else
                {
                    TestDistance t = new TestDistance
                    {
                        DateCreate = carEvents[i].DateEvent,
                        Ticks = carEvents[i].DateEvent.ToLocalTime().Ticks,
                        Distance = carEvents[i].Odometer - tmpOdometr
                    };
                    if (carEvents[i].FuelCount != null)
                        t.SumLiters = (double)carEvents[i].FuelCount;
                    else
                        t.SumLiters = 0;
                    t.SumMoney = carEvents[i].CostTotal;
                    t.Name = carEvents[i].NameEvent;
                    distance.Add(t);
                }
                tmpOdometr = carEvents[i].Odometer;
            }
            foreach (TestDistance d in distance)
            {
                d.AllDistance = allDistance;
            }

            return Json(distance, JsonRequestBehavior.AllowGet);
        }
    }

    public class TestDistance
    {
        public DateTime DateCreate { set; get; }
        public double Ticks { set; get; }
        public int Distance { set; get; }
        public int AllDistance { set; get; }

        public string Name { set; get; }
        public double SumLiters { set; get; }
        public double SumMoney { set; get; }
    }
    
}
