using System;
using System.Collections.Generic;
using System.Globalization;
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
            //Cars car = Db.Cars.Find(carId);
            Cars car = RepProvider.Cars.FindById(carId);
            ViewBag.CarId = car.Id;
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

        public JsonResult GetFuelConsumption(int carId, string startDate, string endDate)
        {
            var car = RepProvider.Cars.FindById(carId);
            InitViewBag(car.Users);

            List<CarEvents> carEvents = new List<CarEvents>();
            CultureInfo ci = new CultureInfo("ru");
            DateTime start;
            DateTime end;
            // Если удалось преобразовать дату начала из строки в дату, то фильтруем по дате начала
            if (DateTime.TryParseExact(startDate, "dd.MM.yyyy", ci, DateTimeStyles.None, out start))
            {
                var carEventsTmp = RepProvider.CarEvents
                    .Select(carId)
                    .Where(x => x.DateEvent >= start)
                    .OrderBy(x => x.DateEvent)
                    .ThenBy(x => x.Odometer);

                DateTime endTmp;
                // Если удалось преобразовать дату конца из строки в дату, то фильтруем по дате конца
                if (DateTime.TryParseExact(endDate, "dd.MM.yyyy", ci, DateTimeStyles.None, out endTmp))
                {
                    carEvents = carEventsTmp
                        .Where(x => x.CarId == carId && x.DateEvent <= endTmp)
                        .OrderBy(x => x.DateEvent)
                        .ThenBy(x => x.Odometer)
                        .ToList();
                }
            }
            // Если не удалось преобразовать дату начала, но удалось преобразовать дату конца, то фильтруем по дате конца
            else if (DateTime.TryParseExact(endDate, "dd.MM.yyyy", ci, DateTimeStyles.None, out end))
            {
                carEvents = RepProvider.CarEvents
                    .Select(carId)
                    .Where(x => x.DateEvent <= end)
                    .OrderBy(x => x.DateEvent)
                    .ThenBy(x => x.Odometer)
                    .ToList();
            }
            else
            {
                carEvents = RepProvider.CarEvents
                    .Select(carId)
                    .OrderBy(x => x.DateEvent)
                    .ThenBy(x => x.Odometer)
                    .ToList();
            }

            double minConsumption = 0;
            double avgConsumption = 0;
            double maxConsumption = 0;
            List<CarConsumptionJson> fullTankCharging = new List<CarConsumptionJson>();

            //// Получаем все события автомобиля заправок из БД
            //var carEvents = RepProvider.CarEvents
            //    .Select(carId)
            //    .Where(x => x.EventTypes.Name.Equals(Consts.EventTypeNameAzs, StringComparison.OrdinalIgnoreCase))
            //    .OrderBy(x => x.DateEvent)
            //    .ThenBy(x => x.Odometer)
            //    .ToList();

            // Ссылка на первую заправку "Полный бак"
            CarEvents firstCarEvent = null;
            // Ссылка на вторую заправку "Полный бак"
            CarEvents secondCarEvent = null;
            // Сумма всех расходов топлива
            double sumConsumption = 0;
            int count = 0;
            // Сумма залитого топлива между 2-мя заправками "Полный бак"
            double sumFuel = 0;
            foreach (var carEvent in carEvents)
            {
                // Если заправка "Полный бак"
                if (carEvent.IsFullTank)
                {
                    // Если это первое событие "Полный бак"
                    if (firstCarEvent == null)
                    {
                        // Сохраняем ссылку на первую заправку "Полный бак"
                        firstCarEvent = carEvent;
                        if (firstCarEvent.FuelCount != null)
                        {
                            sumFuel += (double)firstCarEvent.FuelCount;
                        }
                        continue;
                    }
                    // Сохраняем ссылку на вторую заправку "Полный бак"
                    secondCarEvent = carEvent;
                    if (secondCarEvent.FuelCount != null)
                    {
                        // Прибавляем кол. топлива, которое заправили на второй заправке "Полный бак"
                        sumFuel += (double)secondCarEvent.FuelCount;
                    }
                    // Считаем пройденное расстояние, между 2-мя заправками "Полный бак"
                    int distance = (int)(secondCarEvent.Odometer - firstCarEvent.Odometer);
                    // Считаем расход топлива, между 2-мя заправками "Полный бак"
                    double consumption = (sumFuel * 100) / distance;
                    // Если это первый расчет расхода топлива или минимальный расход топлива больше, чем текущий расход топлива
                    if (count == 0 || minConsumption > consumption)
                    {
                        // Запоминаем минимальный расход топлива
                        minConsumption = consumption;
                    }
                    // Если это первый расчет расхода топлива или максимальный расход топлива меньше, чем текущий расход топлива
                    if (count == 0 || maxConsumption > consumption)
                    {
                        // Запоминаем максимальный расход топлива
                        maxConsumption = consumption;
                    }
                    // Суммируем расход топлива
                    sumConsumption += consumption;
                    // Запоминаем, сколько расчетов расхода топлива произвели
                    count++;


                    // Конвертируем расход топлива между событиями заправка "полный бак" в выбранные единицы измерения
                    consumption = ConverterUnitFuelConsumption.ConverterFuelConsumptionLoad(ViewBag.UnitFuelConsumption,
                        consumption);
                    // Округляем значение до 2-х знаков после запятой
                    consumption = Math.Round(consumption, 2);

                    // Конвертируем расстояние между событиями заправка "полный бак" в выбранные единицы измерения
                    var distanceTmp = ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id,
                        ViewBag.UnitDistance, distance);
                    // Округляем значение
                    distance = (int)Math.Round(distanceTmp);

                    int firstCarEventOdometer = firstCarEvent.Odometer ?? 0;
                    firstCarEventOdometer = (int)ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id,
                        ViewBag.UnitDistance, firstCarEventOdometer);
                    int secondCarEventOdometer = secondCarEvent.Odometer ?? 0;
                    secondCarEventOdometer = (int)ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id,
                        ViewBag.UnitDistance, secondCarEventOdometer);

                    fullTankCharging.Add(new CarConsumptionJson(
                        new EventJson
                        {
                            DateEvent = firstCarEvent.DateEvent.ToString("dd.MM.yyyy"),
                            Odometer = firstCarEventOdometer,
                            Id = firstCarEvent.Id
                        },
                        new EventJson
                        {
                            DateEvent = secondCarEvent.DateEvent.ToString("dd.MM.yyyy"),
                            Odometer = secondCarEventOdometer,
                            Id = secondCarEvent.Id
                        }
                        , consumption, distance));
                    // Первым событием становится второе
                    firstCarEvent = secondCarEvent;

                    // Запоминаем, сколько потратили бензина на первом событии "Полный бак"
                    // Если количество залитого топлива null (firstCarEvent.FuelCount == null), то заносим 0
                    sumFuel = firstCarEvent.FuelCount ?? 0;
                }
                // Если это событие заправка и была заправка "Полный бак"
                else if (carEvent.EventTypes.Name.Equals(Consts.EventTypeNameAzs) && firstCarEvent != null)
                {
                    if (carEvent.FuelCount != null)
                    {
                        // Запоминаем, сколько всего потратили топлива между двумя заправками "Полный бак"
                        sumFuel += (double)carEvent.FuelCount;
                    }
                }
            }
            // Считаем средний расход топлива
            avgConsumption = Math.Round(sumConsumption / count, 2);

            // Конвертируем средний расход топлива в выбранные единицы измерения
            avgConsumption = ConverterUnitFuelConsumption.ConverterFuelConsumptionLoad(ViewBag.UnitFuelConsumption,
                avgConsumption);
            // Округляем значение до 2-х знаков после запятой
            avgConsumption = Math.Round(avgConsumption, 2);

            // Конвертируем минимальный расход топлива в выбранные единицы измерения
            minConsumption = ConverterUnitFuelConsumption.ConverterFuelConsumptionLoad(ViewBag.UnitFuelConsumption,
                minConsumption);
            // Округляем значение до 2-х знаков после запятой
            minConsumption = Math.Round(minConsumption, 2);

            // Конвертируем максимальный расход топлива в выбранные единицы измерения
            maxConsumption = ConverterUnitFuelConsumption.ConverterFuelConsumptionLoad(ViewBag.UnitFuelConsumption,
                maxConsumption);
            // Округляем значение до 2-х знаков после запятой
            maxConsumption = Math.Round(maxConsumption, 2);


            foreach (var item in fullTankCharging)
            {
                item.MinConsumption = minConsumption;
                item.AvgConsumption = avgConsumption;
                item.MaxConsumption = maxConsumption;
            }

            // Конвертировать данные в выбранную систему измерения

            fullTankCharging = fullTankCharging.OrderByDescending(x => x.NewCarEvent.DateEvent).ToList();

            return Json(fullTankCharging, JsonRequestBehavior.AllowGet);
        }


        public void Owner(HttpContextBase httpContext)
        {
            // Если пользователь авторизован
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var user = RepProvider.Users.FindByName(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    this.UserId = user.Id;
                }
                //this.UserId = Db.Users
                //    .Where(x => x.Nickname.Equals(HttpContext.User.Identity.Name))
                //    .Select(x => x.Id)
                //    .FirstOrDefault();

                ViewBag.Owner = this.UserId;
            }
        }

        //public List<CarConsumptionViewModel> SelectFullTankCharging(int carId, out double minConsumption, out double avgConsumption, out double maxConsumption)
        //{
        //    minConsumption = 0;
        //    avgConsumption = 0;
        //    maxConsumption = 0;

        //    // Получаем все события заправок из базы
        //    var carEvents = RepProvider.CarEvents
        //        .Select(carId)
        //        .Where(x => x.EventTypes.Name.Equals(Consts.EventTypeNameAzs, StringComparison.OrdinalIgnoreCase))
        //        .OrderBy(x => x.DateEvent)
        //        .ToList();

        //    // Ссылка на пердыдущее событие заправки
        //    CarEvents tmpCarEvent = null;
        //    // Список событий заправок 
        //    // (идут парами: 
        //    // - если 2 заправки подряд с полный бак, то 1 пара
        //    // - если 3 заправки подряд с полным баком, то 2 пары
        //    List<CarConsumptionViewModel> fullTankCharging = new List<CarConsumptionViewModel>();

        //    double summConsumption = 0;
        //    foreach (var carEvent in carEvents)
        //    {
        //        // Если у текущей заправки заправили полный бак
        //        if (carEvent.IsFullTank)
        //        {
        //            // Это первая заправка полынм баком
        //            // - пропустил предыдущую заправку
        //            // - до этого заправился не поным баком
        //            if (tmpCarEvent == null || carEvent.IsMissedFilling)
        //            {
        //                tmpCarEvent = carEvent;
        //            }
        //            // Это НЕ первая заправка полынм баком
        //            else
        //            {
        //                double consumption = 0;
        //                int mileage = 0;
        //                if (carEvent.Odometer != null && tmpCarEvent.Odometer != null)
        //                {
        //                    mileage = (int)(carEvent.Odometer - tmpCarEvent.Odometer);
        //                }

        //                if (carEvent.FuelCount != null && mileage != 0)
        //                {
        //                    consumption = Math.Round(((double)carEvent.FuelCount * 100 / mileage), 2);
        //                    if (Math.Abs(minConsumption) <= 0|| minConsumption > consumption)
        //                        minConsumption = consumption;

        //                    if (Math.Abs(maxConsumption) <= 0 || maxConsumption < consumption)
        //                        maxConsumption = consumption;

        //                    summConsumption += consumption;
        //                }

        //                fullTankCharging.Add(new CarConsumptionViewModel(tmpCarEvent, carEvent, consumption, mileage));
        //                tmpCarEvent = carEvent;
        //            }
        //        }
        //        // Заправка не с полным баком
        //        else
        //        {
        //            tmpCarEvent = null;
        //        }
        //    }
        //    avgConsumption = Math.Round(summConsumption/carEvents.Count);

            
        //    return fullTankCharging.OrderByDescending(x=>x.NewCarEvent.DateEvent).ToList();
        //}

        public List<CarConsumptionViewModel> SelectFullTankCharging(int carId, out double minConsumption, out double avgConsumption, out double maxConsumption)
        {
            var car = RepProvider.Cars.FindById(carId);
            InitViewBag(car.Users);
            minConsumption = 0;
            avgConsumption = 0;
            maxConsumption = 0;
            List<CarConsumptionViewModel> fullTankCharging = new List<CarConsumptionViewModel>();

            // Получаем все события автомобиля заправок из БД
            var carEvents = RepProvider.CarEvents
                .Select(carId)
                .Where(x => x.EventTypes.Name.Equals(Consts.EventTypeNameAzs, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.DateEvent)
                .ThenBy(x => x.Odometer)
                .ToList();

            // Ссылка на первую заправку "Полный бак"
            CarEvents firstCarEvent = null;
            // Ссылка на вторую заправку "Полный бак"
            CarEvents secondCarEvent = null;
            // Сумма всех расходов топлива
            double sumConsumption = 0;
            int count = 0;
            // Сумма залитого топлива между 2-мя заправками "Полный бак"
            double sumFuel = 0;
            foreach (var carEvent in carEvents)
            {
                // Если заправка "Полный бак"
                if (carEvent.IsFullTank)
                {
                    // Если это первое событие "Полный бак"
                    if (firstCarEvent == null)
                    {
                        // Сохраняем ссылку на первую заправку "Полный бак"
                        firstCarEvent = carEvent;
                        if (firstCarEvent.FuelCount != null)
                        {
                            sumFuel += (double) firstCarEvent.FuelCount;
                        }
                        continue;
                    }
                    // Сохраняем ссылку на вторую заправку "Полный бак"
                    secondCarEvent = carEvent;
                    if (secondCarEvent.FuelCount != null)
                    {
                        // Прибавляем кол. топлива, которое заправили на второй заправке "Полный бак"
                        sumFuel += (double) secondCarEvent.FuelCount;
                    }
                    // Считаем пройденное расстояние, между 2-мя заправками "Полный бак"
                    int distance = (int) (secondCarEvent.Odometer - firstCarEvent.Odometer);
                    // Считаем расход топлива, между 2-мя заправками "Полный бак"
                    // Округляем до 2-х десятичных знаков
                    double consumption = (sumFuel*100)/distance;
                    // Если это первый расчет расхода топлива или минимальный расход топлива больше, чем текущий расход топлива
                    if (count == 0 || minConsumption > consumption)
                    {
                        // Запоминаем минимальный расход топлива
                        minConsumption = consumption;
                    }
                    // Если это первый расчет расхода топлива или максимальный расход топлива меньше, чем текущий расход топлива
                    if (count == 0 || maxConsumption > consumption)
                    {
                        // Запоминаем максимальный расход топлива
                        maxConsumption = consumption;
                    }
                    // Суммируем расход топлива
                    sumConsumption += consumption;
                    // Запоминаем, сколько расчетов расхода топлива произвели
                    count++;

                    // Конвертируем расход топлива между событиями заправка "полный бак" в выбранные единицы измерения
                    consumption = ConverterUnitFuelConsumption.ConverterFuelConsumptionLoad(ViewBag.UnitFuelConsumption,
                        consumption);
                    // Округляем значение до 2-х знаков после запятой
                    consumption = Math.Round(consumption, 2);

                    // Конвертируем расстояние между событиями заправка "полный бак" в выбранные единицы измерения
                    var distanceTmp = ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id,
                        ViewBag.UnitDistance, distance);
                    // Округляем значение
                    distance = (int)Math.Round(distanceTmp);

                    firstCarEvent.Odometer = firstCarEvent.Odometer ?? 0;
                    firstCarEvent.Odometer = (int)ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id,
                        ViewBag.UnitDistance, (int)firstCarEvent.Odometer);
                    secondCarEvent.Odometer = secondCarEvent.Odometer ?? 0;
                    secondCarEvent.Odometer = (int)ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id,
                        ViewBag.UnitDistance, (int)secondCarEvent.Odometer);

                    fullTankCharging.Add(new CarConsumptionViewModel(firstCarEvent, secondCarEvent, consumption, distance));
                    // Первым событием становится второе
                    firstCarEvent = secondCarEvent;

                    // Запоминаем, сколько потратили бензина на первом событии "Полный бак"
                    // Если количество залитого топлива null (firstCarEvent.FuelCount == null), то заносим 0
                    sumFuel = firstCarEvent.FuelCount ?? 0;
                }
                // Если это событие заправка и была заправка "Полный бак"
                else if (carEvent.EventTypes.Name.Equals(Consts.EventTypeNameAzs) && firstCarEvent != null)
                {
                    if (carEvent.FuelCount != null)
                    {
                        // Запоминаем, сколько всего потратили топлива между двумя заправками "Полный бак"
                        sumFuel += (double) carEvent.FuelCount;
                    }
                }
            }
            // Считаем средний расход топлива
            avgConsumption = sumConsumption/count;

            // Конвертируем средний расход топлива в выбранные единицы измерения
            avgConsumption = ConverterUnitFuelConsumption.ConverterFuelConsumptionLoad(ViewBag.UnitFuelConsumption,
                avgConsumption);
            // Округляем значение до 2-х знаков после запятой
            avgConsumption = Math.Round(avgConsumption, 2);

            // Конвертируем минимальный расход топлива в выбранные единицы измерения
            minConsumption = ConverterUnitFuelConsumption.ConverterFuelConsumptionLoad(ViewBag.UnitFuelConsumption,
                minConsumption);
            // Округляем значение до 2-х знаков после запятой
            minConsumption = Math.Round(minConsumption, 2);

            // Конвертируем максимальный расход топлива в выбранные единицы измерения
            maxConsumption = ConverterUnitFuelConsumption.ConverterFuelConsumptionLoad(ViewBag.UnitFuelConsumption,
                maxConsumption);
            // Округляем значение до 2-х знаков после запятой
            maxConsumption = Math.Round(maxConsumption, 2);








            //// Ссылка на пердыдущее событие заправки
            //CarEvents tmpCarEvent = null;
            //// Список событий заправок 
            //// (идут парами: 
            //// - если 2 заправки подряд с полный бак, то 1 пара
            //// - если 3 заправки подряд с полным баком, то 2 пары
            //List<CarConsumptionViewModel> fullTankCharging = new List<CarConsumptionViewModel>();

            //double summConsumption = 0;
            //foreach (var carEvent in carEvents)
            //{
            //    // Если у текущей заправки заправили полный бак
            //    if (carEvent.IsFullTank)
            //    {
            //        // Это первая заправка полынм баком
            //        // - пропустил предыдущую заправку
            //        // - до этого заправился не поным баком
            //        if (tmpCarEvent == null || carEvent.IsMissedFilling)
            //        {
            //            tmpCarEvent = carEvent;
            //        }
            //        // Это НЕ первая заправка полынм баком
            //        else
            //        {
            //            double consumption = 0;
            //            int mileage = 0;
            //            if (carEvent.Odometer != null && tmpCarEvent.Odometer != null)
            //            {
            //                mileage = (int)(carEvent.Odometer - tmpCarEvent.Odometer);
            //            }

            //            if (carEvent.FuelCount != null && mileage != 0)
            //            {
            //                consumption = Math.Round(((double)carEvent.FuelCount * 100 / mileage), 2);
            //                if (Math.Abs(minConsumption) <= 0 || minConsumption > consumption)
            //                    minConsumption = consumption;

            //                if (Math.Abs(maxConsumption) <= 0 || maxConsumption < consumption)
            //                    maxConsumption = consumption;

            //                summConsumption += consumption;
            //            }

            //            fullTankCharging.Add(new CarConsumptionViewModel(tmpCarEvent, carEvent, consumption, mileage));
            //            tmpCarEvent = carEvent;
            //        }
            //    }
            //    // Заправка не с полным баком
            //    else
            //    {
            //        tmpCarEvent = null;
            //    }
            //}
            //avgConsumption = Math.Round(summConsumption / carEvents.Count);


            return fullTankCharging.OrderByDescending(x => x.NewCarEvent.DateEvent).ToList();
        }


        //public List<double> SelectFullTankCharging(int carId, DateTime beginDate, DateTime endDate)
        //{
        //    // Получаем все события заправок из базы
        //    var carEvents = RepProvider.CarEvents
        //        .Select(carId)
        //        .Where(x => x.EventTypes.Name.Equals(Consts.EventTypeNameAzs, StringComparison.OrdinalIgnoreCase) &&
        //                    x.DateEvent >= beginDate &&
        //                    x.DateEvent <= endDate)
        //        .OrderBy(x => x.DateCreate)
        //        .ThenBy(x => x.Odometer)
        //        .ToList();
        //    //var carEvents = Db.CarEvents
        //    //    .Where(
        //    //        x =>
        //    //            x.CarId == carId &&
        //    //            x.EventTypes.Name.Equals(Consts.EventTypeNameAzs, StringComparison.OrdinalIgnoreCase) &&
        //    //            x.DateEvent >= beginDate &&
        //    //            x.DateEvent <= endDate)
        //    //    .OrderBy(x => x.DateCreate)
        //    //    .ToList();

        //    // Ссылка на пердыдущее событие заправки
        //    CarEvents tmpCarEvent = null;
        //    // Список событий заправок 
        //    // (идут парами: 
        //    // - если 2 заправки подряд с полный бак, то 1 пара
        //    // - если 3 заправки подряд с полным баком, то 2 пары
        //    List<double> fullTankCharging = new List<double>();

        //    foreach (var carEvent in carEvents)
        //    {
        //        // Если у текущей заправки заправили полный бак
        //        if (carEvent.IsFullTank)
        //        {
        //            // Это первая заправка полынм баком
        //            // - пропустил предыдущую заправку
        //            // - до этого заправился не поным баком
        //            if (tmpCarEvent == null || carEvent.IsMissedFilling)
        //            {
        //                tmpCarEvent = carEvent;
        //            }
        //            // Это НЕ первая заправка полынм баком
        //            else
        //            {
        //                double consumption = 0;
        //                int mileage = 0;
        //                if (carEvent.Odometer != null && tmpCarEvent.Odometer != null)
        //                {
        //                    mileage = (int)(carEvent.Odometer - tmpCarEvent.Odometer);
        //                }
        //                if (carEvent.FuelCount != null && mileage != 0)
        //                {
        //                    consumption = Math.Round(((double)carEvent.FuelCount * 100 / mileage), 2);
        //                }

        //                fullTankCharging.Add(consumption);
        //                tmpCarEvent = carEvent;
        //            }
        //        }
        //        // Заправка не с полным баком
        //        else
        //        {
        //            tmpCarEvent = null;
        //        }
        //    }
        //    return fullTankCharging;
        //}

        public void CarAndUserInit(int carId)
        {
            Cars car = RepProvider.Cars.FindById(carId);
            //var car = Db.Cars.Find(carId);
            ViewBag.Car = car;
            ViewBag.User = RepProvider.Users.FindById(car.UserId);
            //ViewBag.User = Db.Users.Find(car.UserId);

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
            ViewBag.LanguageName = this.CurrentLang.Code;
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carEvents.FuelCategoryId);
        }


        public JsonResult GetConsumptionFromPeriod(int carId, string startDate, string endDate)
        {
            Cars car = RepProvider.Cars.FindById(carId);
            //var car = Db.Cars.Find(carId);
            if (car == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            // Предществующее событие первому из списка событий, чтобы получить показания одометра события
            CarEvents firstEvent = null;
            // Список событий выбранного автомобиля за определенный период (если период задан)
            List<CarEvents> carEvents = new List<CarEvents>();


            CultureInfo ci = new CultureInfo("ru");
            DateTime start;
            DateTime end;
            // Если удалось преобразовать дату начала из строки в дату, то фильтруем по дате начала
            if(DateTime.TryParseExact(startDate, "dd.MM.yyyy", ci, DateTimeStyles.None, out start))
            {
                var carEventsTmp = RepProvider.CarEvents
                    .Select(carId)
                    .Where(x => x.DateEvent >= start)
                    .OrderBy(x => x.DateEvent)
                    .ThenBy(x => x.Odometer);
                //var carEventsTmp = Db.CarEvents
                //    .Where(x => x.CarId == carId && x.DateEvent >= start)
                //    .OrderBy(x => x.DateEvent);

                // Получаем все события, которые произошли до событий, попавших в фильтр
                var firstEvents = RepProvider.CarEvents
                    .Select(carId)
                    .Where(x => x.DateEvent <= start);
                //var firstEvents = Db.CarEvents
                //    .Where(x => x.CarId == carId && x.DateEvent <= start);

                firstEvent = firstEvents
                    .FirstOrDefault(x => x.DateEvent == firstEvents.Max(y => y.DateEvent));

                DateTime endTmp;
                // Если удалось преобразовать дату конца из строки в дату, то фильтруем по дате конца
                if (DateTime.TryParseExact(endDate, "dd.MM.yyyy", ci, DateTimeStyles.None, out endTmp))
                {
                    carEvents = carEventsTmp
                        .Where(x => x.CarId == carId && x.DateEvent <= endTmp)
                        .OrderBy(x => x.DateEvent)
                        .ThenBy(x => x.Odometer)
                        .ToList();
                }
            }
            // Если не удалось преобразовать дату начала, но удалось преобразовать дату конца, то фильтруем по дате конца
            else if (DateTime.TryParseExact(endDate, "dd.MM.yyyy", ci, DateTimeStyles.None, out end))
            {
                carEvents = RepProvider.CarEvents
                    .Select(carId)
                    .Where(x => x.DateEvent <= end)
                    .OrderBy(x => x.DateEvent)
                    .ThenBy(x => x.Odometer)
                    .ToList();
                //carEvents = Db.CarEvents
                //    .Where(x => x.CarId == carId && x.DateEvent <= end)
                //    .OrderBy(x => x.DateEvent).ToList();
            }
            else
            {
                carEvents = RepProvider.CarEvents
                    .Select(carId)
                    .OrderBy(x => x.DateEvent)
                    .ThenBy(x => x.Odometer)
                    .ToList();
                //carEvents = Db.CarEvents
                //    .Where(x => x.CarId == carId)
                //    .OrderBy(x => x.DateEvent)
                //    .ToList();
            }

            
            InitViewBag(car.Users);

            List<CarConsumptionDistance> distance = new List<CarConsumptionDistance>();
            // Проехано, за определенный период
            int allDistance = 0;
            int tmpOdometr = 0;

            for (int i = 0; i < carEvents.Count; i++)
            {
                // Определяем общий пробег
                if (i == 0)
                {
                    int beginDistance = 0;
                    if (firstEvent != null && firstEvent.Odometer != null)
                    {
                        beginDistance = carEvents[i].Odometer == null ? 0 : (int)(carEvents[i].Odometer - firstEvent.Odometer);
                    }
                    else
                    {
                        beginDistance = carEvents[i].Odometer == null ? 0 : (int)carEvents[i].Odometer;
                    }
                    //int beginDistance = carEvents[i].Odometer == null ? 0 : (int)carEvents[i].Odometer;
                    int endDistance = carEvents[carEvents.Count - 1].Odometer == null ? 0 : (int)carEvents[carEvents.Count - 1].Odometer;
                    allDistance = endDistance - beginDistance;
                    // Конвертируем всю пройденную дистанцию в выбранные единицы измерения.
                    allDistance = (int)Math.Round(ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, ViewBag.UnitDistance, allDistance));
                    tmpOdometr = beginDistance;
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
                    CarConsumptionDistance t = new CarConsumptionDistance
                    {
                        DateCreate = carEvents[i].DateEvent,
                        Ticks = carEvents[i].DateEvent.ToLocalTime().Ticks
                    };
                    if (carEvents[i].Odometer != null)
                    {
                        t.Distance = (int)carEvents[i].Odometer - tmpOdometr;
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
            foreach (CarConsumptionDistance d in distance)
            {
                d.AllDistance = allDistance;
                d.Distance = Math.Round(ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, ViewBag.UnitDistance, d.Distance), 2);
            }

            return Json(distance, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetConsumptionStatistics(int carId = 0)
        //{
        //    var car = Db.Cars.Find(carId);
        //    if (car == null)
        //    {
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //    var carEvents = Db.CarEvents.Where(x => x.CarId == carId).OrderBy(x => x.DateEvent).ToList();
        //    InitViewBag(car.Users);

        //    List<TestDistance> distance = new List<TestDistance>();
        //    // Проехано, за определенный период
        //    //Dictionary<DateTime, double> distance = new Dictionary<DateTime, double>();
        //    int allDistance = 0;
        //    int tmpOdometr = 0;
        //    //string unitDistance = Db.Users.Find(car.UserId).UnitDistance.NameUnit;
        //    //ViewBag.UnitDistance = Db.Users.Find(car.UserId).UnitDistance.NameUnit;
            
            

        //    for (int i = 0; i < carEvents.Count; i++)
        //    {
        //        // Определяем общий пробег
        //        if (i == 0)
        //        {
        //            int begin = carEvents[i].Odometer == null ? 0 : (int)carEvents[i].Odometer;
        //            int end = carEvents[carEvents.Count - 1].Odometer == null ? 0 : (int)carEvents[carEvents.Count - 1].Odometer;
        //            allDistance = end;
        //            // Конвертируем всю пройденную дистанцию в выбранные единицы измерения.
        //            allDistance = (int)Math.Round(ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, ViewBag.UnitDistance, allDistance));
        //            tmpOdometr = begin;
        //        }

        //        var result = distance.FirstOrDefault(x => x.DateCreate == carEvents[i].DateEvent);

        //        if (result != null && result.DateCreate != DateTime.MinValue && carEvents[i].Odometer != null)
        //        {
        //            result.Distance += (int)carEvents[i].Odometer - tmpOdometr;
        //            //result.Distance = (int)Math.Round(ConverterUnitDistance.ConvertDistanceFromKm(unitDistance, result.Distance));
        //            //result.Ticks = new DateTime(carEvents[i].DateEvent.Year, carEvents[i].DateEvent.Month, 1).ToLocalTime().Ticks;
        //            if (carEvents[i].FuelCount != null)
        //                result.SumLiters = (double)carEvents[i].FuelCount;
        //            else
        //                result.SumLiters = 0;
        //            result.SumMoney = carEvents[i].CostTotal;
        //            result.Name = carEvents[i].NameEvent;
        //        }
        //        else
        //        {
        //            TestDistance t = new TestDistance
        //            {
        //                DateCreate = carEvents[i].DateEvent,
        //                Ticks = carEvents[i].DateEvent.ToLocalTime().Ticks
        //            };
        //            if (carEvents[i].Odometer != null)
        //            {
        //                t.Distance = (int) carEvents[i].Odometer - tmpOdometr;
        //            }
        //            //t.Distance = (int)Math.Round(ConverterUnitDistance.ConvertDistanceFromKm(unitDistance, t.Distance));
        //            if (carEvents[i].FuelCount != null)
        //                t.SumLiters = (double)carEvents[i].FuelCount;
        //            else
        //                t.SumLiters = 0;
        //            t.SumMoney = carEvents[i].CostTotal;
        //            t.Name = carEvents[i].NameEvent;
        //            distance.Add(t);
        //        }
        //        if (carEvents[i].Odometer != null)
        //            tmpOdometr = (int)carEvents[i].Odometer;
        //    }
        //    foreach (TestDistance d in distance)
        //    {
        //        d.AllDistance = allDistance;
        //        d.Distance = Math.Round(ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, ViewBag.UnitDistance, d.Distance), 2);
        //    }

            

        //    return Json(distance, JsonRequestBehavior.AllowGet);
        //}
    }

    public class CarConsumptionDistance
    {
        public DateTime DateCreate { set; get; }
        public double Ticks { set; get; }
        public double Distance { set; get; }
        public int AllDistance { set; get; }

        public string Name { set; get; }
        public double SumLiters { set; get; }
        public double SumMoney { set; get; }
    }

    public class CarConsumptionJson
    {
        public EventJson NewCarEvent { set; get; }
        public EventJson OldCarEvent { set; get; }
        public double Consumption { set; get; }
        // Пробег между первой и второй заправкой с полным баком
        public int Mileage { set; get; }

        public double MinConsumption { set; get; }
        public double AvgConsumption { set; get; }
        public double MaxConsumption { set; get; }

        public CarConsumptionJson(EventJson oldCarEvent, EventJson newCarEvent, double consumption, int mileage)
        {
            OldCarEvent = oldCarEvent;
            NewCarEvent = newCarEvent;
            if (newCarEvent.Odometer != null && oldCarEvent.Odometer != null)
                Mileage = (int)(newCarEvent.Odometer - oldCarEvent.Odometer);
            Consumption = consumption;
        }
    }

    public class EventJson
    {
        public int Id { set; get; }
        public int? Odometer { set; get; }
        public string DateEvent { set; get; }
    }
}
