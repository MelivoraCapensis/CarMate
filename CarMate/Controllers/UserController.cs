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

        public ActionResult Details(int id = 0)
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
            
            return View(user);

            //return View(new Users()
            //{
            //    LastName = "qewr",
            //    FirstName = "fgdggf",
            //    Nickname = "nick",
            //    Id = 555,
            //    DateCreate = DateTime.Now,
            //    DateRegister = DateTime.Now
            //});
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
    }
}