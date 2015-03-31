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
            var carevents = db.CarEvents.Where(x => x.CarId == carId).ToList();
            ViewBag.CarId = carId;
            return View(carevents.ToList());
        }

        //
        // GET: /CarEvents/Details/5

        public ActionResult Details(int id)
        {
            CarEvents carevents = db.CarEvents.Find(id);
            if (carevents == null)
            {
                return HttpNotFound();
            }

            return View(carevents);
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
            InitViewBag(carEvents);

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
            //ViewBag.EventTypeId = new SelectList(db.EventTypes, "Id", "Name", carevents.EventTypeId);
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carEvents.FuelCategoryId);
            //ViewBag.CarId = new SelectList(db.Cars, "id", "imgPath", carevents.CarId);
            return View(carEvents);
        }

       

        //
        // GET: /CarEvents/Edit/5

        public ActionResult Edit(int id)
        {
            CarEvents carevents = db.CarEvents.Find(id);
            if (carevents == null)
            {
                return HttpNotFound();
            }

            ViewBag.EventType = carevents.EventTypes.Name;

            InitViewBag(carevents);
            //ViewBag.EventTypeId = new SelectList(db.EventTypes, "Id", "Name", carevents.EventTypeId);
            //ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carevents.FuelCategoryId);
            //ViewBag.CarId = new SelectList(db.Cars, "id", "imgPath", carevents.CarId);
            return View(carevents);
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
            return View(carEvents);
        }

        //
        // GET: /CarEvents/Delete/5

        public ActionResult Delete(int id)
        {
            CarEvents carevents = db.CarEvents.Find(id);
            if (carevents == null)
            {
                return HttpNotFound();
            }
            return View(carevents);
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
            ViewBag.FuelCategoryId = new SelectList(db.FuelCategories, "id", "category", carEvents.FuelCategoryId);
        }

        public PartialViewResult GetEvent(string name = "")
        {
            //List<Book> books = db.Books.Where(a => a.Author.Contains(name)).ToList<Book>();
            //if (type.Equals("Заправка"))
            if(name.Equals("Заправка", StringComparison.OrdinalIgnoreCase))
                return PartialView("_PartEventFilling");

                return PartialView("_PartEventOther");
        }

        //public PartialViewResult GetModifications(int modelId = 0)
        //{
        //    ViewBag.ModificationId = new SelectList(
        //        db.CarModifications.Where(x => x.modelId == modelId)
        //            .Select(x => new { modificationId = x.id, x.modification })
        //            .OrderBy(x => x.modification)
        //            .ToList(),
        //        "ModificationId",
        //        "Modification"
        //        );

        //    return PartialView("_PartCarModification");
        //}
    }
}