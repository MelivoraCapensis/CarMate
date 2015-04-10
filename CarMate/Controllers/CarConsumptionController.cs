using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarMate.Classes;
using CarMate.ViewModel;

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
                .OrderBy(x => x.DateCreate)
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

            
            return fullTankCharging;
        }

        public void CarAndUserInit(int carId)
        {
            var car = db.Cars.Find(carId);
            ViewBag.Car = car;
            ViewBag.User = db.Users.Find(car.UserId);
        }
    }

    
}
