using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
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

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                this.UserId = db.Users
                    .Where(x => x.Nickname.Equals(HttpContext.User.Identity.Name))
                    .Select(x => x.Id)
                    .FirstOrDefault();

                ViewBag.Owner = this.UserId;
            }
            else
            {
                this.UserId = 0;
            }



            return View(cars.ToList());
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
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                this.UserId = db.Users
                    .Where(x => x.Nickname.Equals(HttpContext.User.Identity.Name))
                    .Select(x => x.Id)
                    .FirstOrDefault();
            }
            else
            {
                this.UserId = 0;
            }

            // Этот пользователь владелец?
            ViewBag.IsOwner = this.UserId == cars.UserId;

            return View(cars);
        }

        //
        // GET: /Car/Create
        //[ChildActionOnly]
        public ActionResult Create(int userId = 0)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                this.UserId = db.Users
                    .Where(x => x.Nickname.Equals(HttpContext.User.Identity.Name))
                    .Select(x => x.Id)
                    .FirstOrDefault();

                ViewBag.Owner = this.UserId;
            }
            else
            {
                // Если пользователь не авторизирован, у него нет доступа к этому методу
                return HttpNotFound();
            }

            if (this.UserId != userId)
            {
                return HttpNotFound();
            }
            Cars car = new Cars
            {
                UserId = this.UserId,
                DateBuy = DateTime.Today
            };

            //ViewBag.User = db.Users.Find(this.UserId);
            //ViewBag.modelId = new SelectList(db.CarModels, "id", "model");
            //ViewBag.modificationId = new SelectList(db.CarModifications, "id", "modification");
            //ViewBag.brandId = new SelectList(db.CarBrands, "id", "brand");
            InitViewBag(car);
            return View(car);
            
        }

        //
        // POST: /Car/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cars cars)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpPostedFileBase pf = Request.Files["file"];
                    string imgPath = "";
                    if (pf != null && pf.ContentLength != 0)
                    {
                        string fileName = cars.UserId + cars.Odometer + Path.GetExtension(pf.FileName);
                        imgPath = "/Content/Images/" + fileName;
                        pf.SaveAs(Server.MapPath(imgPath));
                    }
                    else
                        imgPath = "/Content/Images/auto.jpg";
                    if (!String.IsNullOrEmpty(imgPath))
                    {
                        cars.ImgPath = imgPath;
                    }

                    cars.DateCreate = DateTime.Now;
                    cars.State = 0;
                    db.Cars.Add(cars);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Car", new { id = cars.Id });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError("", MyException.GetFullExceptionMessage(exc));
                }
            }

            //ViewBag.User = db.Users.Find(this.UserId);
            //ViewBag.modelId = new SelectList(db.CarModels, "id", "model");
            //ViewBag.modificationId = new SelectList(db.CarModifications, "id", "modification");
            //ViewBag.brandId = new SelectList(db.CarBrands, "id", "brand");
            InitViewBag(cars);
            return View(cars);
        }

        //
        // GET: /Car/Edit/5
        //[ChildActionOnly]
        public ActionResult Edit(int id = 0)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                this.UserId = db.Users
                    .Where(x => x.Nickname.Equals(HttpContext.User.Identity.Name))
                    .Select(x => x.Id)
                    .FirstOrDefault();

                //ViewBag.Owner = this.UserId;
            }
            else
            {
                // Если пользователь не авторизирован, у него нет доступа к этому методу
                return HttpNotFound();
            }

            Cars car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            

            // Если пользователь, который хочет редактировать этот автомобиль, не является его владельцем
            //if (this.UserId != cars.UserId)
            //{
            //    return HttpNotFound();
            //}


            ViewBag.brandId = new SelectList(db.CarBrands, "id", "brand", car.CarModels.CarBrands.id);
            ViewBag.modelId = new SelectList(db.CarModels, "id", "model", car.ModelId);
            ViewBag.modificationId = new SelectList(db.CarModifications, "id", "modification", car.ModificationId);
            
            return View(car);
        }

        //
        // POST: /Car/Edit/5

        [HttpPost]
        //[ChildActionOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cars car)
        {
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
        //[ChildActionOnly]
        public ActionResult Delete(int id = 0)
        {
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    this.UserId = db.Users
            //        .Where(x => x.Nickname.Equals(HttpContext.User.Identity.Name))
            //        .Select(x => x.Id)
            //        .FirstOrDefault();

            //    //ViewBag.Owner = this.UserId;
            //}
            //else
            //{
            //    // Если пользователь не авторизирован, у него нет доступа к этому методу
            //    return HttpNotFound();
            //}

            Cars cars = db.Cars.Find(id);
            if (cars == null)
            {
                return HttpNotFound();
            }

            //if (UserId != cars.UserId)
            //{
            //    return HttpNotFound();
            //}

            //InitViewBag(cars);
            return View(cars);
        }

        //
        // POST: /Car/Delete/5

        [HttpPost, ActionName("Delete")]
        //[ChildActionOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cars car = db.Cars.Find(id);
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
            //ViewBag.modelId = new SelectList(db.CarModels, "id", "model", car.ModelId);
            //ViewBag.modificationId = new SelectList(db.CarModifications, "id", "modification", car.ModificationId);
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