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
            var fullTankCharging = SelectFullTankCharging(carId);
            CarAndUserInit(carId);
            return View(fullTankCharging);
        }

        public List<CarConsumptionViewModel> SelectFullTankCharging(int carId)
        {
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
                        fullTankCharging.Add(new CarConsumptionViewModel(tmpCarEvent, carEvent));
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
