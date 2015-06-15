﻿using System;
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
            Cars car = Db.Cars.Find(carId);
            
            InitViewBag(car.Users);
            ConvertTankLoad(car);
            ConvertOdometrLoad(car);
            ConvertConsumptionLoad(car);

            CarAndUserInit(carId);
            ViewBag.MinConsumption = minConsumption;
            ViewBag.AvgConsumption = avgConsumption;
            ViewBag.MaxConsumption = maxConsumption;

            Owner(HttpContext);
            
            ViewBag.IsOwner = this.UserId == car.UserId;

            return View(fullTankCharging);
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

        public List<CarConsumptionViewModel> SelectFullTankCharging(int carId, out double minConsumption, out double avgConsumption, out double maxConsumption)
        {
            minConsumption = 0;
            avgConsumption = 0;
            maxConsumption = 0;

            // Получаем все события заправок из базы
            var carEvents = Db.CarEvents
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
                        int mileage = 0;
                        if (carEvent.Odometer != null && tmpCarEvent.Odometer != null)
                        {
                            mileage = (int)(carEvent.Odometer - tmpCarEvent.Odometer);
                        }

                        if (carEvent.FuelCount != null && mileage != 0)
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
            var carEvents = Db.CarEvents
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
                        int mileage = 0;
                        if (carEvent.Odometer != null && tmpCarEvent.Odometer != null)
                        {
                            mileage = (int)(carEvent.Odometer - tmpCarEvent.Odometer);
                        }
                        if (carEvent.FuelCount != null && mileage != 0)
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
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carEvents.FuelCategoryId);
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
            var car = Db.Cars.Find(carId);
            if (car == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            var carEvents = Db.CarEvents.Where(x => x.CarId == carId).OrderBy(x => x.DateEvent).ToList();


            List<TestDistance> distance = new List<TestDistance>();
            // Проехано, за определенный период
            //Dictionary<DateTime, double> distance = new Dictionary<DateTime, double>();
            int allDistance = 0;
            int tmpOdometr = 0;
            string unitDistance = Db.Users.Find(car.UserId).UnitDistance.NameUnit;
            ViewBag.UnitDistance = Db.Users.Find(car.UserId).UnitDistance.NameUnit;
            
            

            for (int i = 0; i < carEvents.Count; i++)
            {
                // Определяем общий пробег
                if (i == 0)
                {
                    int begin = carEvents[i].Odometer == null ? 0 : (int)carEvents[i].Odometer;
                    int end = carEvents[carEvents.Count - 1].Odometer == null ? 0 : (int)carEvents[carEvents.Count - 1].Odometer;
                    allDistance = end;
                    // Конвертируем всю пройденную дистанцию в выбранные единицы измерения.
                    allDistance = (int)Math.Round(ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, unitDistance, allDistance));
                    tmpOdometr = begin;
                }

                var result = distance.FirstOrDefault(x => x.DateCreate == carEvents[i].DateEvent);

                if (result != null && result.DateCreate != DateTime.MinValue && carEvents[i].Odometer != null)
                {
                    result.Distance += (int)carEvents[i].Odometer - tmpOdometr;
                    //result.Distance = (int)Math.Round(ConverterUnitDistance.ConvertDistanceFromKm(unitDistance, result.Distance));
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
                        Ticks = carEvents[i].DateEvent.ToLocalTime().Ticks
                    };
                    if (carEvents[i].Odometer != null)
                    {
                        t.Distance = (int) carEvents[i].Odometer - tmpOdometr;
                    }
                    //t.Distance = (int)Math.Round(ConverterUnitDistance.ConvertDistanceFromKm(unitDistance, t.Distance));
                    if (carEvents[i].FuelCount != null)
                        t.SumLiters = (double)carEvents[i].FuelCount;
                    else
                        t.SumLiters = 0;
                    t.SumMoney = carEvents[i].CostTotal;
                    t.Name = carEvents[i].NameEvent;
                    distance.Add(t);
                }
                if (carEvents[i].Odometer != null)
                    tmpOdometr = (int)carEvents[i].Odometer;
            }
            foreach (TestDistance d in distance)
            {
                d.AllDistance = allDistance;
                d.Distance = Math.Round(ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, unitDistance, d.Distance), 2);
            }

            return Json(distance, JsonRequestBehavior.AllowGet);
        }
    }

    public class TestDistance
    {
        public DateTime DateCreate { set; get; }
        public double Ticks { set; get; }
        public double Distance { set; get; }
        public int AllDistance { set; get; }

        public string Name { set; get; }
        public double SumLiters { set; get; }
        public double SumMoney { set; get; }
    }
    
}
