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
            var cars = Db.Cars.Include(c => c.CarModels).Include(c => c.CarModifications).Include(c => c.Users);
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
                this.UserId = Db.Users
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
            Cars cars = Db.Cars.Find(id);
            if (cars == null)
            {
                return HttpNotFound();
            }
            
            // Если пользователь авторизирован, ищем его Id в БД
            Owner(HttpContext);

            InitViewBag(cars);
            ConvertOdometrLoad(cars);
            ConvertTankLoad(cars);
            ConvertConsumptionLoad(cars);

            CarAndUserInit(id);

            // Этот пользователь владелец?
            ViewBag.IsOwner = this.UserId == cars.UserId;

            return View(cars);
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
                DateBuy = DateTime.Now,
                Rating = 0
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

            //if (car.ModelId != 0)
            //{


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

                        InitViewBag(car);
                        ConvertOdometrSave(car);
                        ConvertTankSave(car);
                        ConvertConsumptionSave(car);

                        car.DateCreate = DateTime.Now;
                        car.State = 0;
                        Db.Cars.Add(car);
                        Db.SaveChanges();
                        return RedirectToAction("Details", "Car", new {id = car.Id});
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError("", MyException.GetFullExceptionMessage(exc));
                    }
                }
            //}
            //else
            //{
            //    ModelState.AddModelError("brandId", "Поле Model обязательно для заполнения");
            //}

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

            Cars car = Db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            ViewBag.brandId = new SelectList(Db.CarBrands, "id", "brand", car.CarModels.CarBrands.Id);
            ViewBag.modelId = new SelectList(Db.CarModels, "id", "model", car.ModelId);
            ViewBag.modificationId = new SelectList(Db.CarModifications, "id", "modification", car.ModificationId);

            InitViewBag(car);
            ConvertOdometrLoad(car);
            ConvertTankLoad(car);
            ConvertConsumptionLoad(car);

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
                Cars carFromDb = Db.Cars.Find(car.Id);
                carFromDb.ModelId = car.ModelId;
                carFromDb.ModificationId = car.ModificationId;
                

                InitViewBag(car);
                ConvertOdometrSave(car);
                carFromDb.Odometer = car.Odometer;

                ConvertTankSave(car);
                carFromDb.Tank = car.Tank;

                ConvertConsumptionSave(car);
                carFromDb.Consumption = car.Consumption;

                carFromDb.FuelCategoryId = car.FuelCategoryId;
                carFromDb.DateBuy = car.DateBuy;
                carFromDb.CarTransmissionId = car.CarTransmissionId;
                carFromDb.Rating = car.Rating;
                carFromDb.ImgPath = car.ImgPath;

                Db.SaveChanges();
                return RedirectToAction("Details", "User", new { id = car.UserId });
            }

            ViewBag.brandId = new SelectList(Db.CarBrands, "id", "brand", car.CarModels.CarBrands.Id);
            ViewBag.modelId = new SelectList(Db.CarModels, "id", "model", car.ModelId);
            ViewBag.modificationId = new SelectList(Db.CarModifications, "id", "modification", car.ModificationId);
            InitViewBag(car);
            return View(car);
        }

        

        //
        // GET: /Car/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Owner(HttpContext);

            Cars car = Db.Cars.Find(id);
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

            Cars car = Db.Cars.Find(id);

            if (this.UserId != car.UserId)
            {
                return HttpNotFound();
            }

            var carEvents = Db.CarEvents.Where(x => x.CarId == id);
            foreach (var carEvent in carEvents)
            {
                Db.CarEvents.Remove(carEvent);
            }

            Db.Cars.Remove(car);
            Db.SaveChanges();
            return RedirectToAction("Details", "User", new {id = car.UserId});
        }

        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }

        public void InitViewBag(Cars car)
        {
            ViewBag.User = Db.Users.Find(this.UserId);
            if (car.CarModels != null)
                ViewBag.BrandId = new SelectList(Db.CarBrands.OrderBy(x => x.Brand), "Id", "Brand", car.CarModels.BrandId);
            else
            {
                ViewBag.BrandId = new SelectList(Db.CarBrands.OrderBy(x => x.Brand), "Id", "Brand");
            }
            ViewBag.FuelCategoryId = new SelectList(Db.FuelCategories.OrderBy(x => x.Category), "Id", "Category", car.FuelCategoryId);
            ViewBag.CarTransmissionId = new SelectList(RepProvider.CarTransmission.Select(this.CurrentLang.Id).OrderBy(x => x.NameTransmission), "Id", "NameTransmission", car.CarTransmissionId);

            Users user = Db.Users.Find(car.UserId);
            var unitDistanceLang = user.UnitDistance.UnitDistanceLang.FirstOrDefault(x => x.LanguageId == CurrentLang.Id);
            if (unitDistanceLang != null)
            {
                ViewBag.UnitDistance = unitDistanceLang.NameUnit;
            }
            ViewBag.UnitFuelConsumption = user.UnitFuelConsumption.NameUnit;
            ViewBag.UnitVolume = user.UnitVolume.NameUnit;
            ViewBag.LanguageId = this.CurrentLang.Id;
            ViewBag.LanguageName = this.CurrentLang.Code;
        }

        public bool IsAuthenticated()
        {
            return HttpContext.User.Identity.IsAuthenticated;
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

        public void ConvertTankSave(Cars car)
        {
            if (ViewBag.UnitVolume != null)
            {
                if (car.Tank != null)
                {
                    //ViewBag.UnitVolume = Db.Users.Find(carEvents.Cars.UserId).UnitVolume.NameUnit;
                    car.Tank = (int)Math.Round(
                        ConverterUnitVolume.ConvertVolumeToLiters(ViewBag.UnitVolume, (double)car.Tank));
                }
            }
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

        public void ConvertConsumptionSave(Cars car)
        {
            if (ViewBag.UnitFuelConsumption != null)
            {
                if (car.Consumption != null)
                {
                    //ViewBag.UnitVolume = Db.Users.Find(carEvents.Cars.UserId).UnitVolume.NameUnit;
                    car.Consumption = Math.Round(
                        ConverterUnitFuelConsumption.ConverterFuelConsumptionSave(ViewBag.UnitFuelConsumption, (double)car.Consumption), 2);
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

        public void ConvertOdometrSave(Cars car)
        {
            if (ViewBag.UnitDistance != null)
            {
                if (car.Odometer != null)
                {
                    car.Odometer = (int)Math.Round(
                        ConverterUnitDistance.ConverterDistanceSave(RepProvider, CurrentLang.Id, ViewBag.UnitDistance,
                            (int)car.Odometer));
                }
            }
        }

        public PartialViewResult GetModels(int brandId = 0)
        {
            ViewBag.ModelId = new SelectList(
                Db.CarModels.Where(x => x.BrandId == brandId)
                    .Select(x => new {modelId = x.Id, x.Model})
                    .OrderBy(x => x.Model)
                    .ToList(),
                "ModelId",
                "Model"
                );

            return PartialView("_PartCarModel");
        }

        public PartialViewResult GetModifications(int modelId = 0)
        {
            ViewBag.ModificationId = new SelectList(
                Db.CarModifications.Where(x => x.ModelId == modelId)
                    .Select(x => new {modificationId = x.Id, x.Modification})
                    .OrderBy(x => x.Modification)
                    .ToList(),
                "ModificationId",
                "Modification"
                );

            return PartialView("_PartCarModification");
        }
        
    }
}