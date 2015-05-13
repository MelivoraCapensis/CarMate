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
            var carEvents = db.CarEvents
                .Where(x => x.CarId == carId)
                .OrderByDescending(x=>x.DateEvent).ToList();

            CarAndUserInit(carId);

            Owner(HttpContext);
            var car = db.Cars.Find(carId);
            ViewBag.IsOwner = this.UserId == car.UserId;


            return View(carEvents);
            //return View(carevents.ToList());

        }

        //
        // GET: /CarEvents/Details/5

        public void CarAndUserInit(int carId)
        {
            var car = db.Cars.Find(carId);
            ViewBag.Car = car;
            ViewBag.User = db.Users.Find(car.UserId);
        }

        public ActionResult Details(int id)
        {
            CarEvents carEvents = db.CarEvents.Find(id);
            if (carEvents == null)
            {
                return HttpNotFound();
            }
            CarAndUserInit(carEvents.CarId);

            Owner(HttpContext);
            ViewBag.IsOwner = this.UserId == carEvents.Cars.UserId;

            ViewBag.EventType = carEvents.EventTypes.Name;
            InitViewBag(carEvents);

            return View(carEvents);
        }

        //
        // GET: /CarEvents/Create

        public ActionResult Create(int carId)
        {
            //var events =
            //    db.CarEvents.Where(x => x.DateEvent <= DateTime.Now)
            //        .OrderByDescending(x => x.DateEvent)
            //        .FirstOrDefault();
            var car = db.Cars.Find(carId);
            CarAndUserInit(carId);
            Owner(HttpContext);

            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            var carEvents = new CarEvents
            {
                CarId = carId,
                DateEvent = DateTime.Now
            };
            if (car.Odometer != null)
            {
                carEvents.Odometer = (int) car.Odometer;
            }
            //InitViewBag(carEvents);
            ViewBag.EventTypeId = new SelectList(db.EventTypes.OrderBy(x => x.Name), "Id", "Name", 1);
           

            return View(carEvents);
        }

        //
        // POST: /CarEvents/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarEvents carEvents)
        {
            if (ModelState.IsValid)
            {
                var carTmp = db.Cars.Find(carEvents.CarId);
                if (carTmp.Odometer < carEvents.Odometer)
                {
                    carTmp.Odometer = carEvents.Odometer;
                }
                

                carEvents.State = Consts.StateNew;
                carEvents.DateCreate = DateTime.Now;

                db.CarEvents.Add(carEvents);
                db.SaveChanges();
                return RedirectToAction("Index", new {carId = carEvents.CarId});
            }

            InitViewBag(carEvents);
            CarAndUserInit(carEvents.CarId);
            Owner(HttpContext);
            var car = db.Cars.Find(carEvents.CarId);
            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }
            //ViewBag.EventTypeId = new SelectList(db.EventTypes, "Id", "Name", carevents.EventTypeId);
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carEvents.FuelCategoryId);
            //ViewBag.CarId = new SelectList(db.Cars, "id", "imgPath", carevents.CarId);
            return View(carEvents);
        }

       

        //
        // GET: /CarEvents/Edit/5

        public ActionResult Edit(int id)
        {
            CarEvents carEvents = db.CarEvents.Find(id);
            if (carEvents == null)
            {
                return HttpNotFound();
            }

            ViewBag.EventType = carEvents.EventTypes.Name;

            InitViewBag(carEvents);
            CarAndUserInit(carEvents.CarId);
            Owner(HttpContext);
            var car = db.Cars.Find(carEvents.CarId);
            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }
            //ViewBag.EventTypeId = new SelectList(db.EventTypes, "Id", "Name", carevents.EventTypeId);
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carevents.FuelCategoryId);
            //ViewBag.CarId = new SelectList(db.Cars, "id", "imgPath", carevents.CarId);
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
            var car = db.Cars.Find(carEvents.CarId);
            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
                //return Json("Response from Create");
            }

            if (ModelState.IsValid)
            {
                CarEvents carEventsFromDb = db.CarEvents.Find(carEvents.Id);
                // При любом событии
                carEventsFromDb.Comment = carEvents.Comment;
                carEventsFromDb.CostTotal = carEvents.CostTotal;
                carEventsFromDb.DateEvent = carEvents.DateEvent;
                carEventsFromDb.EventTypeId = carEvents.EventTypeId;
                carEventsFromDb.Latitute = carEvents.Latitute;
                carEventsFromDb.Longitute = carEvents.Longitute;
                carEventsFromDb.Odometer = carEvents.Odometer;
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

                db.SaveChanges();

                return RedirectToAction("Index", new { carId = carEvents.CarId });
                //return Json("Response from Create");
            }

            InitViewBag(carEvents);
            ViewBag.EventType = carEvents.EventTypes.Name;
            //ViewBag.EventTypeId = new SelectList(db.EventTypes, "Id", "Name", carevents.EventTypeId);
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
            CarEvents carEvents = db.CarEvents.Find(id);
            if (carEvents == null)
            {
                return HttpNotFound();
            }

            var car = db.Cars.Find(carEvents.CarId);
            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

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

            CarEvents carEvents = db.CarEvents.Find(id);

            var car = db.Cars.Find(carEvents.CarId);
            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            db.CarEvents.Remove(carEvents);
            db.SaveChanges();
            return RedirectToAction("Index", new { carId = carEvents.CarId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public void Owner(HttpContextBase httpContext)
        {
            // Если пользователь авторизован
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                this.UserId = db.Users
                    .Where(x => x.Nickname.Equals(HttpContext.User.Identity.Name))
                    .Select(x => x.Id)
                    .FirstOrDefault();

                ViewBag.Owner = this.UserId;
            }
        }

        private void InitViewBag(CarEvents carEvents)
        {
            ViewBag.EventTypeId = new SelectList(db.EventTypes.OrderBy(x => x.Name), "Id", "Name", carEvents.EventTypeId);
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carEvents.FuelCategoryId);
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
                ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
                return PartialView("_PartEventFilling", carEventsModel);
            }
            if (name.Equals("Ремонт", StringComparison.OrdinalIgnoreCase))
            {
                //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
                return PartialView("_PartEventRepair", carEventsModel);
            }

            return PartialView("_PartEventOther", carEventsModel);
        }

        public PartialViewResult GetEventDetails(string carEventJson, string name = "")
        {
            CarEvents carEventsModel = null;
            if (carEventJson != null)
            {
                carEventJson = carEventJson.Replace("&quot;", "\"");
                carEventsModel = JsonConvert.DeserializeObject<CarEvents>(carEventJson);
                carEventsModel.FuelCategories = db.FuelCategories.Find(carEventsModel.FuelCategoryId);
            }

            if (name.Equals("Заправка", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
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