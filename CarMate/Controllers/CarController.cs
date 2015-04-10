using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime;
using CarMate.Classes;
using CarMate.Models;

namespace CarMate.Controllers
{
    public class CarController : BaseController
    {

        //
        // GET: /Car/
        public ActionResult Index()
        {
            var cars = db.Cars.Include(c => c.CarModels).Include(c => c.CarModifications).Include(c => c.Users);
            Owner(HttpContext);

            return View(cars.ToList());
        }

        // Назвать метод по другому
        // Получает Id авторизованного юзера
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

        //
        // GET: /Car/Details/5

        public ActionResult Details(int id = 0)
        {
            Cars cars = db.Cars.Find(id);
            if (cars == null)
            {
                return HttpNotFound();
            }
            
            // Если пользователь авторизирован, ищем его Id в БД
            Owner(HttpContext);

            CarAndUserInit(id);

            // Этот пользователь владелец?
            ViewBag.IsOwner = this.UserId == cars.UserId;

            return View(cars);
        }

        public void CarAndUserInit(int carId)
        {
            var car = db.Cars.Find(carId);
            ViewBag.Car = car;
            ViewBag.User = db.Users.Find(car.UserId);
        }

        //
        // GET: /Car/Create
        [Authorize]
        public ActionResult Create(int userId = 0)
        {
            Owner(HttpContext);
            if (this.UserId != userId)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                return HttpNotFound();
            }

            Cars car = new Cars
            {
                UserId = this.UserId,
                DateBuy = DateTime.Now
            };
            
            InitViewBag(car);
            return View(car);
            
        }

        //
        // POST: /Car/Create

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cars car)
        {
            Owner(HttpContext);
            if (this.UserId != car.UserId)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    HttpPostedFileBase pf = Request.Files["file"];
                    string imgPath = "";
                    if (pf != null && pf.ContentLength != 0)
                    {
                        string fileName = car.UserId + car.Odometer + Path.GetExtension(pf.FileName);
                        imgPath = "/Content/Images/" + fileName;
                        pf.SaveAs(Server.MapPath(imgPath));
                    }
                    else
                        imgPath = "/Content/Images/auto.jpg";
                    if (!String.IsNullOrEmpty(imgPath))
                    {
                        car.ImgPath = imgPath;
                    }

                    car.DateCreate = DateTime.Now;
                    car.State = 0;
                    db.Cars.Add(car);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Car", new { id = car.Id });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError("", MyException.GetFullExceptionMessage(exc));
                }
            }

            InitViewBag(car);
            return View(car);
        }

        //
        // GET: /Car/Edit/5
        //[ChildActionOnly]
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Owner(HttpContext);

            Cars car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            ViewBag.brandId = new SelectList(db.CarBrands, "id", "brand", car.CarModels.CarBrands.id);
            ViewBag.modelId = new SelectList(db.CarModels, "id", "model", car.ModelId);
            ViewBag.modificationId = new SelectList(db.CarModifications, "id", "modification", car.ModificationId);
            
            return View(car);
        }

        //
        // POST: /Car/Edit/5

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cars car)
        {
            Owner(HttpContext);
            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                Cars carFromDb = db.Cars.Find(car.Id);
                carFromDb.ModelId = car.ModelId;
                carFromDb.ModificationId = car.ModificationId;
                carFromDb.Consumption = car.Consumption;
                carFromDb.Odometer = car.Odometer;
                carFromDb.Tank = car.Tank;

                db.SaveChanges();
                return RedirectToAction("Details", "User", new { id = car.UserId });
            }

            ViewBag.brandId = new SelectList(db.CarBrands, "id", "brand", car.CarModels.CarBrands.id);
            ViewBag.modelId = new SelectList(db.CarModels, "id", "model", car.ModelId);
            ViewBag.modificationId = new SelectList(db.CarModifications, "id", "modification", car.ModificationId);
            return View(car);
        }

        //
        // GET: /Car/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Owner(HttpContext);

            Cars car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            return View(car);
        }

        //
        // POST: /Car/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Owner(HttpContext);

            Cars car = db.Cars.Find(id);

            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Details", "User", new {id = car.UserId});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public void InitViewBag(Cars car)
        {
            ViewBag.User = db.Users.Find(this.UserId);
            ViewBag.brandId = new SelectList(db.CarBrands, "id", "brand");
        }

        public bool IsAuthenticated()
        {
            return HttpContext.User.Identity.IsAuthenticated;
        }

        public PartialViewResult GetModels(int brandId = 0)
        {
            ViewBag.ModelId = new SelectList(
                db.CarModels.Where(x => x.brandId == brandId)
                    .Select(x => new {modelId = x.id, x.model})
                    .OrderBy(x => x.model)
                    .ToList(),
                "ModelId",
                "Model"
                );

            return PartialView("_PartCarModel");
        }

        public PartialViewResult GetModifications(int modelId = 0)
        {
            ViewBag.ModificationId = new SelectList(
                db.CarModifications.Where(x => x.modelId == modelId)
                    .Select(x => new {modificationId = x.id, x.modification})
                    .OrderBy(x => x.modification)
                    .ToList(),
                "ModificationId",
                "Modification"
                );

            return PartialView("_PartCarModification");
        }



    }
}