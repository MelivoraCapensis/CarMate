using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CarMate.Classes;
using Newtonsoft.Json;

namespace CarMate.Controllers
{
    public class CarEventsController : BaseController
    {
        //private CarMateEntity db = new CarMateEntity();

        //
        // GET: /CarEvents/

        public ActionResult Index(int carId)
        {
            //var carevents = db.CarEvents.Include(c => c.EventTypes).Include(c => c.FuelCategories).Include(c => c.Cars);
            // Загрузка всех событий выбраной машины
            var carEvents = Db.CarEvents
                .Where(x => x.CarId == carId)
                .OrderByDescending(x=>x.DateEvent).ToList();

            Owner(HttpContext);

            var car = Db.Cars.Find(carId);
            ViewBag.IsOwner = this.UserId == car.UserId;
            InitViewBagEmpty(car.Users);
            // Конвертация данных машины (Одометра, Расхода топлива, Объема бака) в выбраную систему измерения
            LoadCarUnits(car);
            CarAndUserInit(carId);

            foreach (CarEvents carEvent in carEvents)
            {
                // Конвертация данных события (Одометра, Кол. залитого топлива) в выбраную систему измерения
                ConvertOdometrLoad(carEvent);
                ConvertFuelCountLoad(carEvent);
            }

            return View(carEvents);
        }

        //
        // GET: /CarEvents/Details/5
        public ActionResult Details(int id)
        {
            // Поиск выбраного события по id
            CarEvents carEvents = Db.CarEvents.Find(id);
            if (carEvents == null)
            {
                return HttpNotFound();
            }

            InitViewBag(carEvents);
            // Конвертация данных машины (Одометра, Расхода топлива, Объема бака) в выбраную систему измерения
            LoadCarUnits(carEvents.Cars);
            CarAndUserInit(carEvents.CarId);
            Owner(HttpContext);
            ViewBag.IsOwner = this.UserId == carEvents.Cars.UserId;
            ViewBag.EventType = carEvents.EventTypes.Name;

            // Конвертация данных события (Одометра, Кол. залитого топлива) в выбраную систему измерения
            ConvertOdometrLoad(carEvents);
            ConvertFuelCountLoad(carEvents);

            return View(carEvents);
        }

        //
        // GET: /CarEvents/Create

        public ActionResult Create(int carId)
        {
            var car = Db.Cars.Find(carId);
            if (car == null)
            {
                return HttpNotFound();
            }

            InitViewBagEmpty(car.Users);
            // Конвертация данных машины (Одометра, Расхода топлива, Объема бака) в выбраную систему измерения
            LoadCarUnits(car);
            CarAndUserInit(carId);
            Owner(HttpContext);

            
            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            var carEvents = new CarEvents
            {
                CarId = carId,
                DateEvent = DateTime.Now,
                Odometer = car.Odometer
            };
           
            ViewBag.EventTypeId = new SelectList(Db.EventTypes.Where(x=>x.LanguageId == this.CurrentLang.Id).OrderBy(x => x.Name), "Id", "Name");

            return View(carEvents);
        }

        //
        // POST: /CarEvents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarEvents carEvents)
        {
            Users user = Db.Cars.Find(carEvents.CarId).Users;
            if (carEvents.Odometer != null)
            {
                if (ModelState.IsValid)
                {
                    carEvents.State = Consts.StateNew;
                    carEvents.DateCreate = DateTime.Now;

                    carEvents.CostTotal = Math.Round(carEvents.CostTotal, 2);

                    //string unitDistance = Db.Users.Find(carTmp.UserId).UnitDistance.NameUnit;
                    InitViewBagEmpty(user);
                    // Конвертация данных события (Одометра) в "стандартную" систему измерения 
                    ConvertOdometrSave(carEvents);

                    var carTmp = Db.Cars.Find(carEvents.CarId);
                    if (carTmp.Odometer < carEvents.Odometer)
                    {
                        carTmp.Odometer = carEvents.Odometer;
                    }

                    // Конвертация данных события (Кол. залитого топлива) в "стандартную" систему измерения 
                    ConvertFuelCountSave(carEvents);
                    //var unitDistanceLang =
                    //    Db.Users.Find(carTmp.UserId)
                    //        .UnitDistance.UnitDistanceLang.FirstOrDefault(x => x.LanguageId == CurrentLang.Id);
                    //if (unitDistanceLang != null)
                    //{
                    //    string unitDistance = unitDistanceLang.NameUnit;
                    //    if (carEvents.Odometer != null)
                    //    {
                    //        carEvents.Odometer = (int) Math.Round(
                    //            ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, unitDistance,
                    //                (int) carEvents.Odometer));
                    //    }
                    //}

                    //if (carEvents.FuelCount != null)
                    //{
                    //    string unitVolume = Db.Users.Find(carTmp.UserId).UnitVolume.NameUnit;
                    //    carEvents.FuelCount = Math.Round(
                    //        ConverterUnitVolume.ConvertVolumeToLiters(unitVolume, (double) carEvents.FuelCount), 2);
                    //}

                    Db.CarEvents.Add(carEvents);
                    Db.SaveChanges();
                    return RedirectToAction("Index", new {carId = carEvents.CarId});
                }

            }
            else
            {
                ModelState.AddModelError("Odometer", Resources.CarEvents.OdometerRequired);
            }

            
            InitViewBagEmpty(user);
            CarAndUserInit(carEvents.CarId);
            Owner(HttpContext);
            var car = Db.Cars.Find(carEvents.CarId);
            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            //ViewBag.UnitDistance = Db.Users.Find(carEvents.Cars.UserId).UnitDistance.NameUnit;
            //if (ViewBag.UnitDistance != null)
            //{
            //    if (carEvents.Odometer != null)
            //    {
            //        carEvents.Odometer = (int) Math.Round(
            //            ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, ViewBag.UnitDistance,
            //                (int) carEvents.Odometer));
            //    }
            //}
            ViewBag.EventTypeId = new SelectList(Db.EventTypes.Where(x => x.LanguageId == this.CurrentLang.Id).OrderBy(x => x.Name), "Id", "Name");
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carEvents.FuelCategoryId);
            //ViewBag.CarId = new SelectList(db.Cars, "id", "imgPath", carevents.CarId);
            return View(carEvents);
        }

       

        //
        // GET: /CarEvents/Edit/5

        public ActionResult Edit(int id)
        {
            CarEvents carEvents = Db.CarEvents.Find(id);
            if (carEvents == null)
            {
                return HttpNotFound();
            }

            ViewBag.EventType = carEvents.EventTypes.Name;

            InitViewBag(carEvents);
            // Конвертация данных машины (Одометра, Расхода топлива, Объема бака) в выбраную систему измерения
            LoadCarUnits(carEvents.Cars);
            CarAndUserInit(carEvents.CarId);
            Owner(HttpContext);
            var car = Db.Cars.Find(carEvents.CarId);
            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            // Конвертация данных события (Одометра, Кол. залитого топлива) в выбраную систему измерения
            ConvertOdometrLoad(carEvents);
            ConvertFuelCountLoad(carEvents);
            
            return View(carEvents);
        }



        //public PartialViewResult GetEventDetails(string carEventJson, string name = "")
        //{
        //    CarEvents carEventsModel = null;
        //    if (carEventJson != null)
        //    {
        //        carEventJson = carEventJson.Replace("&quot;", "\"");
        //        carEventsModel = JsonConvert.DeserializeObject<CarEvents>(carEventJson);
        //        carEventsModel.FuelCategories = db.FuelCategories.Find(carEventsModel.FuelCategoryId);
        //    }

        //    if (name.Equals("Заправка", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
        //        //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
        //        return PartialView("_PartEventFillingDetails", carEventsModel);
        //    }
        //    if (name.Equals("Ремонт", StringComparison.OrdinalIgnoreCase))
        //    {
        //        //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
        //        return PartialView("_PartEventRepairDetails", carEventsModel);
        //    }

        //    return PartialView("_PartEventOtherDetails", carEventsModel);
        //}

        //[HttpPost]
        //public JsonResult Test(CarEvents carEvents)
        //{
        //    //if (carEventJson != null)
        //    //{
        //    //    carEventJson = carEventJson.Replace("&quot;", "\"");
                
        //        //var carEvents = JsonConvert.DeserializeObject<CarEvents>(carEventJson);
        //        //return Json(carEvents.Comment);
        //        //carEvents.FuelCategories = db.FuelCategories.Find(carEvents.FuelCategoryId);

        //        Owner(HttpContext);
        //        var car = db.Cars.Find(carEvents.CarId);
        //        if (this.UserId != car.UserId)
        //        {
        //            //return HttpNotFound();
        //            return Json("Error in controller");
        //        }
        //        //if (ModelState.IsValid)
        //        //{
                
        //            CarEvents carEventsFromDb = db.CarEvents.Find(carEvents.Id);
                    
        //            // При любом событии
        //            carEventsFromDb.Comment = carEvents.Comment;
        //            carEventsFromDb.CostTotal = carEvents.CostTotal;
        //            carEventsFromDb.DateEvent = carEvents.DateEvent;
        //            carEventsFromDb.EventTypeId = carEvents.EventTypeId;
        //            carEventsFromDb.Latitute = carEvents.Latitute;
        //            carEventsFromDb.Longitute = carEvents.Longitute;
        //            carEventsFromDb.Odometer = carEvents.Odometer;
        //            // Если название события пустое, то в название события записываем его тип
        //            //carEventsFromDb.NameEvent = String.IsNullOrEmpty(carEvents.NameEvent) ? carEvents.EventTypes.Name : carEvents.NameEvent;

        //            // Нет в списке
        //            carEventsFromDb.NameEvent = carEvents.NameEvent;
        //            // Заправка
        //            carEventsFromDb.FuelCategoryId = carEvents.FuelCategoryId;
        //            carEventsFromDb.FuelCount = carEvents.FuelCount;
        //            carEventsFromDb.IsFullTank = carEvents.IsFullTank;
        //            carEventsFromDb.IsMissedFilling = carEvents.IsMissedFilling;
        //            carEventsFromDb.PricePerLitr = carEvents.PricePerLitr;

        //            // Системное
        //            carEventsFromDb.DateCreate = DateTime.Now;
        //            carEventsFromDb.State = Consts.StateUpdate;

        //            db.SaveChanges();

        //            //return RedirectToAction("Index", new { carId = carEvents.CarId });
        //            return Json("Response from Create");
        //        //}
        //    //}



        //    //InitViewBag(carEvents);
        //    //ViewBag.EventType = carEvents.EventTypes.Name;
        //    ////ViewBag.EventTypeId = new SelectList(db.EventTypes, "Id", "Name", carevents.EventTypeId);
        //    ////ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carEvents.FuelCategoryId);
        //    ////ViewBag.CarId = new SelectList(db.Cars, "id", "imgPath", carEvents.CarId);
        //    //CarAndUserInit(carEvents.CarId);
        //    ////return View(carEvents);
        //    //return Json("Response from Create");
        //    return Json("carEventJson == null");
        //}

        //
        // POST: /CarEvents/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult  Edit(CarEvents carEvents)
        {
            Owner(HttpContext);
            var car = Db.Cars.Find(carEvents.CarId);
            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
                //return Json("Response from Create");
            }
            Users user = Db.Users.Find(car.UserId);
            if (carEvents.Odometer != null)
            {
                if (ModelState.IsValid)
                {
                    CarEvents carEventsFromDb = Db.CarEvents.Find(carEvents.Id);
                    // При любом событии
                    carEventsFromDb.Comment = carEvents.Comment;
                    carEventsFromDb.CostTotal = Math.Round(carEvents.CostTotal, 2);
                    carEventsFromDb.DateEvent = carEvents.DateEvent;
                    carEventsFromDb.EventTypeId = carEvents.EventTypeId;
                    carEventsFromDb.Latitute = carEvents.Latitute;
                    carEventsFromDb.Longitute = carEvents.Longitute;

                    InitViewBagEmpty(user);
                    ConvertOdometrSave(carEvents);
                    carEventsFromDb.Odometer = carEvents.Odometer;
                    ConvertFuelCountSave(carEvents);

                    //string unitDistance = Db.Users.Find(car.UserId).UnitDistance.NameUnit;
                    //if (carEvents.Odometer != null)
                    //{
                    //    carEventsFromDb.Odometer = (int)Math.Round(
                    //        ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, unitDistance, (int)carEvents.Odometer));
                    //}

                    //if (carEvents.FuelCount != null)
                    //{
                    //    string unitVolume = Db.Users.Find(car.UserId).UnitVolume.NameUnit;
                    //    carEvents.FuelCount = Math.Round(
                    //        ConverterUnitVolume.ConvertVolumeToLiters(unitVolume, (double)carEvents.FuelCount), 2);
                    //}

                    // Если название события пустое, то в название события записываем его тип
                    //carEventsFromDb.NameEvent = String.IsNullOrEmpty(carEvents.NameEvent) ? carEvents.EventTypes.Name : carEvents.NameEvent;

                    // Нет в списке
                    carEventsFromDb.NameEvent = carEvents.NameEvent;
                    // Заправка
                    carEventsFromDb.FuelCategoryId = carEvents.FuelCategoryId;
                    carEventsFromDb.FuelCount = carEvents.FuelCount;
                    carEventsFromDb.IsFullTank = carEvents.IsFullTank;
                    carEventsFromDb.IsMissedFilling = carEvents.IsMissedFilling;
                    carEventsFromDb.PricePerLitr = carEvents.PricePerLitr;




                    // Системное
                    carEventsFromDb.DateCreate = DateTime.Now;
                    carEventsFromDb.State = Consts.StateUpdate;

                    Db.SaveChanges();

                    return RedirectToAction("Index", new {carId = carEvents.CarId});
                    //return Json("Response from Create");
                }
            }
            else
            {
                ModelState.AddModelError("Odometer", Resources.CarEvents.OdometerRequired);
            }

            //ViewBag.UnitDistance = Db.Users.Find(car.UserId).UnitDistance.NameUnit;
            //if (carEvents.Odometer != null)
            //{
            //    carEvents.Odometer = (int)Math.Round(
            //        ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, ViewBag.UnitDistance, (int)carEvents.Odometer));
            //}

            InitViewBagEmpty(user);
            //ConvertOdometrSave(carEvents);
            ViewBag.EventType = carEvents.EventTypes.Name;
            ViewBag.EventTypeId =
                new SelectList(Db.EventTypes.Where(x => x.LanguageId == this.CurrentLang.Id).OrderBy(x => x.Name), "Id",
                    "Name", carEvents.EventTypeId);
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carEvents.FuelCategoryId);
            //ViewBag.CarId = new SelectList(db.Cars, "id", "imgPath", carEvents.CarId);
            CarAndUserInit(carEvents.CarId);
            return View(carEvents);
            //return Json("Response from Create");
        }

        //
        // GET: /CarEvents/Delete/5

        public ActionResult Delete(int id)
        {
            Owner(HttpContext);
            CarEvents carEvents = Db.CarEvents.Find(id);
            if (carEvents == null)
            {
                return HttpNotFound();
            }

            var car = Db.Cars.Find(carEvents.CarId);
            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            InitViewBag(carEvents);
            // Конвертация данных машины (Одометра, Расхода топлива, Объема бака) в выбраную систему измерения
            LoadCarUnits(carEvents.Cars);
            CarAndUserInit(carEvents.CarId);
            return View(carEvents);
        }

        //
        // POST: /CarEvents/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Owner(HttpContext);

            CarEvents carEvents = Db.CarEvents.Find(id);

            var car = Db.Cars.Find(carEvents.CarId);
            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            Db.CarEvents.Remove(carEvents);
            Db.SaveChanges();
            return RedirectToAction("Index", new { carId = carEvents.CarId });
        }

        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
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

        private void InitViewBag(CarEvents carEvents)
        {
            Users user = Db.Users.Find(carEvents.Cars.UserId);

            //ViewBag.EventTypeId = new SelectList(Db.EventTypes.OrderBy(x => x.Name), "Id", "Name", carEvents.EventTypeId);
            ViewBag.EventTypeId = new SelectList(Db.EventTypes.Where(x => x.LanguageId == this.CurrentLang.Id).OrderBy(x => x.Name), "Id", "Name", carEvents.EventTypeId);
            InitViewBagEmpty(user);

            //var unitDistanceLang = user.UnitDistance.UnitDistanceLang.FirstOrDefault(x => x.LanguageId == CurrentLang.Id);
            //if (unitDistanceLang != null)
            //{
            //    ViewBag.UnitDistance  = unitDistanceLang.NameUnit;
            //}

            //ViewBag.UnitFuelConsumption = user.UnitFuelConsumption.NameUnit;

            //ViewBag.UnitVolume = user.UnitVolume.NameUnit;

            //ViewBag.LanguageId = this.CurrentLang.Id;
        }

        private void InitViewBagEmpty(Users user)
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
            //        car.Odometer = (int) Math.Round(
            //            ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, ViewBag.UnitDistance, (double) car.Odometer));
            //    }
            //}


            //string unitFuelConsumption = Db.Users.Find(car.UserId).UnitFuelConsumption.NameUnit;
            //ViewBag.UnitFuelConsumption = unitFuelConsumption;
            //if (ViewBag.UnitFuelConsumption != null)
            //{
            //    if (car.Consumption != null)
            //    {
            //        car.Consumption = Math.Round(
            //            ConverterUnitFuelConsumption.ConverterFuelConsumptionLoad(ViewBag.UnitFuelConsumption, (double) car.Consumption), 2);
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

        public void LoadCarUnits(Cars car)
        {
            if (ViewBag.UnitDistance != null)
            {
                if (car.Odometer != null)
                {
                    car.Odometer = (int)Math.Round(
                        ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, ViewBag.UnitDistance, (double)car.Odometer));
                }
            }

            if (ViewBag.UnitFuelConsumption != null)
            {
                if (car.Consumption != null)
                {
                    car.Consumption = Math.Round(
                        ConverterUnitFuelConsumption.ConverterFuelConsumptionLoad(ViewBag.UnitFuelConsumption, (double)car.Consumption), 2);
                }
            }


            if (ViewBag.UnitVolume != null)
            {
                if (car.Tank != null)
                {
                    car.Tank = (int)Math.Round(ConverterUnitVolume.ConvertVolumeFromLiters(ViewBag.UnitVolume, (int)car.Tank));
                }
            }
        }

        public void ConvertOdometrLoad(CarEvents carEvents)
        {
            if (ViewBag.UnitDistance != null)
            {
                if (carEvents.Odometer != null)
                {
                    carEvents.Odometer = (int)Math.Round(
                        ConverterUnitDistance.ConverterDistanceLoad(RepProvider, CurrentLang.Id, ViewBag.UnitDistance,
                            (int)carEvents.Odometer));
                }
            }
        }

        public void ConvertOdometrSave(CarEvents carEvents)
        {
            if (ViewBag.UnitDistance != null)
            {
                if (carEvents.Odometer != null)
                {
                    carEvents.Odometer = (int)Math.Round(
                        ConverterUnitDistance.ConverterDistanceSave(RepProvider, CurrentLang.Id, ViewBag.UnitDistance,
                            (int)carEvents.Odometer));
                }
            }
        }

        public void ConvertFuelCountLoad(CarEvents carEvents)
        {
            if (ViewBag.UnitVolume != null)
            {
                if (carEvents.FuelCount != null)
                {
                    //ViewBag.UnitVolume = Db.Users.Find(carEvents.Cars.UserId).UnitVolume.NameUnit;
                    carEvents.FuelCount = Math.Round(
                        ConverterUnitVolume.ConvertVolumeFromLiters(ViewBag.UnitVolume, (double)carEvents.FuelCount), 2);
                }
            }
        }

        public void ConvertFuelCountSave(CarEvents carEvents)
        {
            if (ViewBag.UnitVolume != null)
            {
                if (carEvents.FuelCount != null)
                {
                    //ViewBag.UnitVolume = Db.Users.Find(carEvents.Cars.UserId).UnitVolume.NameUnit;
                    carEvents.FuelCount = Math.Round(
                        ConverterUnitVolume.ConvertVolumeToLiters(ViewBag.UnitVolume, (double)carEvents.FuelCount), 2);
                }
            }
        }

        public PartialViewResult GetEvent(string carEventJson, string name = "")
        {
            
            CarEvents carEventsModel = null;
            if (carEventJson != null)
            {
                carEventJson = carEventJson.Replace("&quot;", "\"");
                carEventsModel = JsonConvert.DeserializeObject<CarEvents>(carEventJson);
            }


            if (name.Equals("Заправка", StringComparison.OrdinalIgnoreCase))
            {
                //for (int i = 0; i < HttpContext.Request.ServerVariables.Count; i++)
                //{
                //    string s = HttpContext.Request.ServerVariables[i];
                //}

                // Получаем путь с котого был вызван этот метод
                string path = HttpContext.Request.ServerVariables["HTTP_REFERER"];
                // Если путь заканчивается словом Create (View Create)
                if (path.IndexOf("Create", 0, StringComparison.OrdinalIgnoreCase) != -1 && carEventsModel != null)
                {
                    ViewBag.FuelCategoryId = new SelectList(Db.FuelCategories.OrderBy(x => x.Category).Distinct(), "Id", "Category", Db.Cars.Find(carEventsModel.CarId).FuelCategoryId);
                }
                else
                {
                    ViewBag.FuelCategoryId = new SelectList(Db.FuelCategories.OrderByDescending(x => x.Category).Distinct(), "Id", "Category", 1);
                }
                if (carEventsModel != null)
                {
                    Cars car = Db.Cars.Find(carEventsModel.CarId);
                    ViewBag.UnitVolume = Db.Users.Find(car.UserId).UnitVolume.NameUnit;
                }
                return PartialView("_PartEventFilling", carEventsModel);
            }
            if (name.Equals("Ремонт", StringComparison.OrdinalIgnoreCase))
            {
                //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
                return PartialView("_PartEventRepair", carEventsModel);
            }

            return PartialView("_PartEventOther", carEventsModel);
        }

        public PartialViewResult GetEventDetails(string carEventJson, int userId=0, string name = "")
        {
            CarEvents carEventsModel = null;
            if (carEventJson != null)
            {
                carEventJson = carEventJson.Replace("&quot;", "\"");
                carEventsModel = JsonConvert.DeserializeObject<CarEvents>(carEventJson);
                carEventsModel.FuelCategories = Db.FuelCategories.Find(carEventsModel.FuelCategoryId);
            }

            if (name.Equals("Заправка", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.UnitVolume = Db.Users.Find(userId).UnitVolume.NameUnit;
                ViewBag.FuelCategoryId = new SelectList(Db.FuelCategories.OrderBy(x => x.Category).Distinct(), "Id", "Category", 1);
                //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
                return PartialView("_PartEventFillingDetails", carEventsModel);
            }
            if (name.Equals("Ремонт", StringComparison.OrdinalIgnoreCase))
            {
                //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
                return PartialView("_PartEventRepairDetails", carEventsModel);
            }

            return PartialView("_PartEventOtherDetails", carEventsModel);
        }

        //public PartialViewResult GetEventDetails(int carEventId = 0, string name = "")
        //{
        //    CarEvents carEventsModel = db.CarEvents.Find(carEventId);

        //    if (name.Equals("Заправка", StringComparison.OrdinalIgnoreCase))
        //    {
        //        ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
        //        return PartialView("_PartEventFillingDetails", carEventsModel);
        //    }
        //    if (name.Equals("Ремонт", StringComparison.OrdinalIgnoreCase))
        //    {
        //        //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
        //        return PartialView("_PartEventRepairDetails", carEventsModel);
        //    }

        //    return PartialView("_PartEventOtherDetails", carEventsModel);
        //}

    }
}