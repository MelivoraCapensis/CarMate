using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarMate.Classes;

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
            var carEvents = db.CarEvents.Where(x => x.CarId == carId).OrderByDescending(x=>x.DateCreate).ToList();
            CarAndUserInit(carId);
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

            return View(carEvents);
        }

        //
        // GET: /CarEvents/Create

        public ActionResult Create(int carId)
        {
            //ViewBag.EventTypeId = new SelectList(db.EventTypes.OrderBy(x => x.Name), "Id", "Name");
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category");
            //ViewBag.CarId = carId;
            var carEvents = new CarEvents
            {
                CarId = carId,
                DateEvent = DateTime.Today
            };
            //InitViewBag(carEvents);
            ViewBag.EventTypeId = new SelectList(db.EventTypes.OrderBy(x => x.Name), "Id", "Name", 1);
            CarAndUserInit(carId);
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
                carEvents.State = Consts.StateNew;
                carEvents.DateCreate = DateTime.Now;

                db.CarEvents.Add(carEvents);
                db.SaveChanges();
                return RedirectToAction("Index", new {carId = carEvents.CarId});
            }

            InitViewBag(carEvents);
            CarAndUserInit(carEvents.CarId);
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
            //ViewBag.EventTypeId = new SelectList(db.EventTypes, "Id", "Name", carevents.EventTypeId);
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carevents.FuelCategoryId);
            //ViewBag.CarId = new SelectList(db.Cars, "id", "imgPath", carevents.CarId);
            return View(carEvents);
        }

        //
        // POST: /CarEvents/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CarEvents carEvents)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carEvents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { carId = carEvents.CarId });
            }

            InitViewBag(carEvents);
            ViewBag.EventType = carEvents.EventTypes.Name;
            //ViewBag.EventTypeId = new SelectList(db.EventTypes, "Id", "Name", carevents.EventTypeId);
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carEvents.FuelCategoryId);
            //ViewBag.CarId = new SelectList(db.Cars, "id", "imgPath", carEvents.CarId);
            CarAndUserInit(carEvents.CarId);
            return View(carEvents);
        }

        //
        // GET: /CarEvents/Delete/5

        public ActionResult Delete(int id)
        {
            CarEvents carEvents = db.CarEvents.Find(id);
            if (carEvents == null)
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
            CarEvents carEvents = db.CarEvents.Find(id);
            db.CarEvents.Remove(carEvents);
            db.SaveChanges();
            return RedirectToAction("Index", new { carId = carEvents.CarId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void InitViewBag(CarEvents carEvents)
        {
            ViewBag.EventTypeId = new SelectList(db.EventTypes.OrderBy(x => x.Name), "Id", "Name", carEvents.EventTypeId);
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carEvents.FuelCategoryId);
        }

        public PartialViewResult GetEvent(string name = "")
        {

            if (name.Equals("Заправка", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
                return PartialView("_PartEventFilling");
            }
            if (name.Equals("Ремонт", StringComparison.OrdinalIgnoreCase))
            {
                //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories.OrderBy(x => x.category).Distinct(), "Id", "Category", 1);
                return PartialView("_PartEventRepair");
            }

            return PartialView("_PartEventOther");
        }
    }
}