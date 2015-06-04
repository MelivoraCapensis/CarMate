using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace CarMate.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/

        public ActionResult Index()
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
                this.UserId = 0;
            }

            return View(db.Users.ToList());
            //return View(new List<Users> { new Users(){
            //    LastName = "qewr", 
            //    FirstName = "fgdggf",
            //    Nickname = "nick",
            //    Id = 555, 
            //    DateCreate = DateTime.Now,
            //    DateRegister = DateTime.Now                
            //}, 
            //new Users(){
            //    LastName = "qewr", 
            //    FirstName = "fgdggf",
            //    Nickname = "nick",
            //    Id = 555, 
            //    DateCreate = DateTime.Now,
            //    DateRegister = DateTime.Now                
            //}});
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

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
            ViewBag.IsOwner = UserId == user.Id;
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
                Users userFromDb = db.Users.Find(UserId);
                userFromDb.UnitFuelConsumptionId = unitFuelConsumptionId;
                var unitDistanceId = db.UnitDistance.FirstOrDefault(x => x.UnitFuelConsumptionId == unitFuelConsumptionId);
                if (unitDistanceId != null)
                    userFromDb.UnitDistanceId = unitDistanceId.Id;

                var unitVolumeId = db.UnitVolume.FirstOrDefault(x => x.UnitFuelConsumptionId == unitFuelConsumptionId);
                if (unitVolumeId != null)
                    userFromDb.UnitVolumeId = unitVolumeId.Id;
                //userFromDb.UnitVolumeId = unitVolumeId;

                db.SaveChanges();
                return Json("Ok");
            }

            return Json("Error in controller2");
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

                //ViewBag.Owner = this.UserId;
            }
        }

        public ActionResult Garage()
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

            return RedirectToAction("Details", new {id = this.UserId});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public void SendMessage()
        {
            
        }
    }
}