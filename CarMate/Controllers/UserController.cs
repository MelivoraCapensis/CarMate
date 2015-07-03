using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using CarMate.Classes;

namespace CarMate.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    this.UserId = Db.Users
            //        .Where(x => x.Nickname.Equals(HttpContext.User.Identity.Name))
            //        .Select(x => x.Id)
            //        .FirstOrDefault();

            //    ViewBag.Owner = this.UserId;
            //}
            //else
            //{
            //    this.UserId = 0;
            //}
            Owner(HttpContext);

            var users = RepProvider.Users.SelectAll().ToList();

            //for(int i = 0; i < users.Count; i++)
            ////foreach (Users user in users)
            //{
            //    //usersList[i].Cars = repProvider.Cars.Select(usersList[i].Id).ToList();
            //    //var user1 = user;
            //    users[i].Cars = RepProvider.Cars.Select(users[i].Id).ToList();
            //        //Db.Cars.Where(x => x.UserId == user1.Id && x.State != Consts.StateDelete).ToList();
            //}


            return View(users);
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0, bool defaultCarError = false)
        {
            var user = RepProvider.Users.FindById(id);
            //var user = Db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            Owner(HttpContext);
            ViewBag.IsOwner = user.Id == this.UserId;
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    this.UserId = Db.Users
            //        .Where(x => x.Nickname.Equals(HttpContext.User.Identity.Name))
            //        .Select(x => x.Id)
            //        .FirstOrDefault();
            //}
            //else
            //{
            //    this.UserId = 0;
            //}
            //user.Cars = RepProvider.Cars.Select(user.Id).ToList();
            if (defaultCarError)
            {
                ViewBag.DefaultCarError = "Автомобиль по умолчанию не выбран!";
            }

            return View(user);
        }

        [HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        //public JsonResult Edit(int unitDistanceId, int unitVolumeId)
        public JsonResult Edit(int unitFuelConsumptionId)
        {
            //return Json("Ok");
            Owner(HttpContext);
            //if (this.UserId != user.Id)
            //{
            //    return Json("Error in controller1");
            //}
            

            if (ModelState.IsValid)
            {
                var userFromDb = RepProvider.Users.FindById(UserId);
                //Users userFromDb = Db.Users.Find(UserId);
                userFromDb.UnitFuelConsumptionId = unitFuelConsumptionId;
                var unitDistanceId = Db.UnitDistance.FirstOrDefault(x => x.UnitFuelConsumptionId == unitFuelConsumptionId);
                if (unitDistanceId != null)
                    userFromDb.UnitDistanceId = unitDistanceId.Id;

                var unitVolumeId = Db.UnitVolume.FirstOrDefault(x => x.UnitFuelConsumptionId == unitFuelConsumptionId);
                if (unitVolumeId != null)
                    userFromDb.UnitVolumeId = unitVolumeId.Id;
                //userFromDb.UnitVolumeId = unitVolumeId;

                Db.SaveChanges();
                return Json("Ok");
            }

            return Json("Error in controller");
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

        public ActionResult Garage()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                this.UserId = Db.Users
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

            return RedirectToAction("Details", new {id = this.UserId});
        }

        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }

        public void SendMessage()
        {
            
        }

        public JsonResult ChangeDefaultCar(int userId, int carId)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // Ищем пользователя с заданным id
                var user = RepProvider.Users.FindById(userId);
                // Если такого пользователя не существует выходим
                if(user == null)
                    return null;
                // Если ник авторизованого пользователя не совпадает с ником пользователя, которому хотят поменять 
                // автомобиль по умолчанию выходим
                if(!user.Nickname.Equals(HttpContext.User.Identity.Name, StringComparison.OrdinalIgnoreCase))
                    return null;
                // Ищем автомобиль с заданным id, у текущего пользователя
                var car = RepProvider.Cars.Select(userId).FirstOrDefault(x => x.Id == carId);
                // Если такой автомобиль существует
                if (car != null)
                {
                    // Изменяем автомобиль по умолчанию
                    user.DefaultCarId = carId;
                    user.State = Consts.StateUpdate;
                    user.DateCreate = DateTime.Now;
                    Db.SaveChanges();
                    return Json("Ok", JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            return null;
        }

        public ActionResult DefaultCarEvents()
        {
            // Если пользователь авторизован
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // Ищем пользователя по имени в БД
                var user = RepProvider.Users.FindByName(HttpContext.User.Identity.Name);
                // Если такой пользователь существует
                if (user != null)
                {
                    // Если у него выбрана машина по умолчанию
                    if (user.DefaultCarId != null)
                    {
                        // Ищем автомобиль у текущего пользовате в БД
                        var car = RepProvider.Cars.FindById((int)user.DefaultCarId);
                        // Если такой автомобиль существует
                        if (car != null)
                        {
                            // Если у пользователя есть автомобиль по умолчанию отправляем на события
                            return RedirectToAction("Index", "CarEvents", new { carId = car.Id });
                        }
                        // Если машина по умолчанию не выбрана отправляем пользователя в гараж
                        return RedirectToAction("Details", new { id = user.Id, defaultCarError = true });
                    }
                    //// Если машина по умолчанию не выбрана отправляем пользователя в гараж
                    return RedirectToAction("Details", new { id = user.Id, defaultCarError = true});
                }
                // Если пользователь в БД не найден отправляем на страницу ошибки
                return HttpNotFound();
            }
            // Если пользователь не авторизован отправляем его на окно авторизации
            return RedirectToAction("Login", "Account");
        }

        public ActionResult DefaultCarConsumprion()
        {
            // Если пользователь авторизован
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // Ищем пользователя по имени в БД
                var user = RepProvider.Users.FindByName(HttpContext.User.Identity.Name);
                // Если такой пользователь существует
                if (user != null)
                {
                    // Если у него выбрана машина по умолчанию
                    if (user.DefaultCarId != null)
                    {
                        // Ищем автомобиль у текущего пользовате в БД
                        var car = RepProvider.Cars.FindById((int)user.DefaultCarId);
                        // Если такой автомобиль существует
                        if (car != null)
                        {
                            // Если у пользователя есть автомобиль по умолчанию отправляем на расход топлива
                            return RedirectToAction("Index", "CarConsumption", new { carId = car.Id });
                        }
                        // Если машина по умолчанию не выбрана отправляем пользователя в гараж
                        return RedirectToAction("Details", new { id = user.Id, defaultCarError = true });
                    }
                    //// Если машина по умолчанию не выбрана отправляем пользователя в гараж
                    return RedirectToAction("Details", new { id = user.Id, defaultCarError = true });
                }
                // Если пользователь в БД не найден отправляем на страницу ошибки
                return HttpNotFound();
            }
            // Если пользователь не авторизован отправляем его на окно авторизации
            return RedirectToAction("Login", "Account");
        }

        public ActionResult DefaultCarCostStatistics()
        {
            // Если пользователь авторизован
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // Ищем пользователя по имени в БД
                var user = RepProvider.Users.FindByName(HttpContext.User.Identity.Name);
                // Если такой пользователь существует
                if (user != null)
                {
                    // Если у него выбрана машина по умолчанию
                    if (user.DefaultCarId != null)
                    {
                        // Ищем автомобиль у текущего пользовате в БД
                        var car = RepProvider.Cars.FindById((int)user.DefaultCarId);
                        // Если такой автомобиль существует
                        if (car != null)
                        {
                            // Если у пользователя есть автомобиль по умолчанию отправляем на статистику
                            return RedirectToAction("Index", "CarCostStatistics", new { carId = car.Id });
                        }
                        // Если машина по умолчанию не выбрана отправляем пользователя в гараж
                        return RedirectToAction("Details", new { id = user.Id, defaultCarError = true });
                    }
                    //// Если машина по умолчанию не выбрана отправляем пользователя в гараж
                    return RedirectToAction("Details", new { id = user.Id, defaultCarError = true });
                }
                // Если пользователь в БД не найден отправляем на страницу ошибки
                return HttpNotFound();
            }
            // Если пользователь не авторизован отправляем его на окно авторизации
            return RedirectToAction("Login", "Account");
        }

        
    }
}