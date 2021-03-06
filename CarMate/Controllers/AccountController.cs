﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using CarMate.Filters;
using CarMate.Models;

namespace CarMate.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : BaseController
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //ViewBag.ReturnUrl = returnUrl;
            return View(new LoginModel());
            //return PartialView("_PartLogin");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                //return RedirectToLocal(returnUrl);
                //return View(model);
                //using (CarMateEntities db = new CarMateEntities())
                //{
                var user = RepProvider.Users.FindByName(model.UserName);

                return RedirectToAction("Details", "User", new { id = user.Id });
                //}
                
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            ModelState.AddModelError("", "Имя пользователя или пароль указаны неверно.");
            return View(model);
            //return PartialView("_PartLogin", model);
        }

        //
        // POST: /Account/LogOff

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.UnitFuelConsumptionId = new SelectList(Db.UnitFuelConsumption.OrderBy(x => x.NameUnit), "Id", "NameUnit");
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Попытка зарегистрировать пользователя
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);


                    HttpPostedFileBase pf = Request.Files["file"];
                    string imgPath = "";
                    if (pf != null && pf.ContentLength != 0)
                    {
                        string fileName = model.UserName + Path.GetExtension(pf.FileName);
                        imgPath = "/Content/Images/" + fileName;
                        pf.SaveAs(Server.MapPath(imgPath));
                    }
                    else
                        imgPath = "/Content/Images/anonim.jpg";

                    Users user = RepProvider.Users.GetNewWithDefaultInitialization();
                    DateTime date = DateTime.Now;
                    user.Nickname = model.UserName;
                    user.UserEmail = model.Email;
                    user.DateRegister = date;
                    user.DateCreate = date;

                    var unitFuelConsumption = Db.UnitFuelConsumption.Find(model.UnitFuelConsumptionId);
                    if (unitFuelConsumption != null)
                    {
                        user.UnitFuelConsumptionId = unitFuelConsumption.Id;

                        var unitVolume = unitFuelConsumption.UnitVolume
                            .FirstOrDefault(x => x.UnitFuelConsumptionId == unitFuelConsumption.Id);
                        if (unitVolume != null)
                        {
                            user.UnitVolumeId = unitVolume.Id;
                        }

                        var unitDistance = RepProvider.UnitDistance
                            .Select(this.CurrentLang.Id)
                            .FirstOrDefault(x => x.UnitFuelConsumptionId == unitFuelConsumption.Id);
                        if (unitDistance != null)
                        {
                            user.UnitDistanceId = unitDistance.Id;
                        }
                    }

                    if (!String.IsNullOrEmpty(imgPath))
                    {
                        user.ImgPath = imgPath;
                    }

                    RepProvider.Users.Add(user);
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Details", "User", new {id = user.Id});
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            ViewBag.UnitFuelConsumptionId = new SelectList(Db.UnitFuelConsumption.OrderBy(x => x.NameUnit), "Id", "NameUnit");
            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Удалять связь учетной записи, только если текущий пользователь — ее владелец
            if (ownerAccount == User.Identity.Name)
            {
                // Транзакция используется, чтобы помешать пользователю удалить учетные данные последнего входа
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public JsonResult DeleteAccount()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                using (UsersContext db = new UsersContext())
                {
                    string userName = HttpContext.User.Identity.Name;
                    var user = db.UserProfiles.FirstOrDefault(x => x.UserName.Equals(userName));
                    if (user != null)
                    {
                        // Выход из учетной записи
                        WebSecurity.Logout();

                        #region Таблицы, созданные с помощью стандартной программы aspnet_regsql.exe
                        // Удаление профиля пользователя
                        var webpagesMembership = Db.webpages_Membership.FirstOrDefault(x => x.UserId == user.UserId);
                        if (webpagesMembership != null)
                            Db.webpages_Membership.Remove(webpagesMembership);

                        // Удаление внешних учетных записей
                        var webpagesOAuthMembership = Db.webpages_OAuthMembership.FirstOrDefault(x => x.UserId == user.UserId);
                        if (webpagesOAuthMembership != null)
                            Db.webpages_OAuthMembership.Remove(webpagesOAuthMembership);

                        // Удаление профиля пользователя
                        db.UserProfiles.Remove(user);
                        db.SaveChanges();
                        #endregion

                        #region Наши таблицы
                        // Удаление всех событий пользователя
                        var carEvents = Db.CarEvents.Where(x => x.Cars.Users.Nickname.Equals(userName));
                        foreach (var carEvent in carEvents)
                        {
                            Db.CarEvents.Remove(carEvent);
                        }

                        // Удаление всех транспортных средств пользователя
                        var cars = Db.Cars.Where(x => x.Users.Nickname.Equals(userName));
                        foreach (var car in cars)
                        {
                            Db.Cars.Remove(car);
                        }

                        // Удаление пользователя
                        var users = RepProvider.Users.FindByName(userName);
                        //var users = Db.Users.FirstOrDefault(x => x.Nickname.Equals(userName));
                        if(users != null)
                        {
                            Db.Users.Remove(users);
                        }
                        Db.SaveChanges();
                        #endregion
                    }
                }
            }
            return Json("Ok", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Пароль изменен."
                : message == ManageMessageId.SetPasswordSuccess ? "Пароль задан."
                : message == ManageMessageId.RemoveLoginSuccess ? "Внешняя учетная запись удалена."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            
            InitViewBag();

            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        public void InitViewBag()
        {
            // Заменить на DAL
            //using (CarMateEntities carMateDb = new CarMateEntities())
            //{
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var user = RepProvider.Users.FindByName(HttpContext.User.Identity.Name);
                    //var user = carMateDb.Users.FirstOrDefault(x => x.Nickname.Equals(HttpContext.User.Identity.Name));
                    if (user != null)
                    {
                        //var unitDistanceLang =
                        //    user.UnitDistance.UnitDistanceLang.FirstOrDefault(x => x.LanguageId == CurrentLang.Id);
                        ViewBag.UnitDistanceId = new SelectList(
                            //carMateDb.UnitDistance.OrderBy(x => x.NameUnit).ToList(),
                            RepProvider.UnitDistance.Select(CurrentLang.Id).OrderBy(x => x.NameUnit).ToList(),
                            "Id", "NameUnit", user.UnitDistanceId);
                        ViewBag.UnitVolumeId = new SelectList(Db.UnitVolume.OrderBy(x => x.NameUnit).ToList(),
                            "Id", "NameUnit", user.UnitVolumeId);
                        ViewBag.UnitFuelConsumptionId = new SelectList(Db.UnitFuelConsumption.OrderBy(x => x.NameUnit).ToList(),
                            "Id", "NameUnit", user.UnitFuelConsumptionId);
                        ViewBag.UnitFuelConsumption = new SelectList(
                            Db.UnitFuelConsumption.OrderBy(x => x.NameUnit).ToList(), "Id", "NameUnit",
                            user.UnitFuelConsumptionId);
                    }
                    else
                    {
                        ViewBag.UnitDistanceId = new SelectList(
                            //carMateDb.UnitDistance.OrderBy(x => x.NameUnit).ToList(),
                            RepProvider.UnitDistance.Select(CurrentLang.Id).OrderBy(x => x.NameUnit).ToList(),
                            "Id", "NameUnit");
                        ViewBag.UnitVolumeId = new SelectList(
                            Db.UnitVolume.OrderBy(x => x.NameUnit).ToList(), "Id", "NameUnit");
                        ViewBag.UnitFuelConsumptionId = new SelectList(
                            Db.UnitFuelConsumption.OrderBy(x => x.NameUnit).ToList(), "Id", "NameUnit");
                        ViewBag.UnitFuelConsumption = new SelectList(
                            Db.UnitFuelConsumption.OrderBy(x => x.NameUnit).ToList(), "Id", "NameUnit");
                    }
                    
                }
            //}
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");

            InitViewBag();

            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // В ряде случаев при сбое ChangePassword породит исключение, а не вернет false.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неправильный текущий пароль или недопустимый новый пароль.");
                    }
                }
            }
            else
            {
                // У пользователя нет локального пароля, уберите все ошибки проверки, вызванные отсутствующим
                // полем OldPassword
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            ViewBag.UnitFuelConsumptionId = new SelectList(Db.UnitFuelConsumption.OrderBy(x => x.NameUnit), "Id", "NameUnit");
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // Если текущий пользователь вошел в систему, добавляется новая учетная запись
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // Новый пользователь, запрашиваем желаемое имя участника
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Добавление нового пользователя в базу данных
                using (UsersContext db = new UsersContext())
                {

                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Проверка наличия пользователя в базе данных
                    if (user == null)
                    {
                        // Добавление имени в таблицу профиля
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        // Добавить пользователя в свою БД
                        using (CarMateEntities carMateDb = new CarMateEntities())
                        {
                            DateTime date = DateTime.Now;
                            Users u = new Users
                            {
                                Nickname = model.UserName,
                                //UserEmail = model.Email,
                                UserEmail = "Test",
                                FirstName = "Test",
                                LastName = "Test",
                                //dateBirth = model.Year,
                                //country = model.Country,
                                //region = model.Region,
                                //city = model.City,
                                DateRegister = date,
                                UserPassword = "Test",

                                DateCreate = date,
                                State = 0
                            };

                            var unitFuelConsumption = Db.UnitFuelConsumption.Find(model.UnitFuelConsumptionId);
                            if (unitFuelConsumption != null)
                            {
                                u.UnitFuelConsumptionId = unitFuelConsumption.Id;

                                var unitVolume = unitFuelConsumption.UnitVolume
                                    .FirstOrDefault(x => x.UnitFuelConsumptionId == unitFuelConsumption.Id);
                                if (unitVolume != null)
                                {
                                    u.UnitVolumeId = unitVolume.Id;
                                }

                                var unitDistance = RepProvider.UnitDistance
                                    .Select(this.CurrentLang.Id)
                                    .FirstOrDefault(x => x.UnitFuelConsumptionId == unitFuelConsumption.Id);
                                if (unitDistance != null)
                                {
                                    u.UnitDistanceId = unitDistance.Id;
                                }
                            }

                            carMateDb.Users.Add(u);
                            carMateDb.SaveChanges();
                        }
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "Имя пользователя уже существует. Введите другое имя пользователя.");
                    }
                }
            }

            ViewBag.UnitFuelConsumptionId = new SelectList(Db.UnitFuelConsumption.OrderBy(x => x.NameUnit), "Id", "NameUnit");
            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Вспомогательные методы
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    //using (CarMateEntities db = new CarMateEntities())
                    //{
                    //    var userId = db.Users
                    //    .Where(x => x.Nickname.Equals(HttpContext.User.Identity.Name))
                    //    .Select(x => x.Id)
                    //    .FirstOrDefault();
                    var user = RepProvider.Users.FindByName(HttpContext.User.Identity.Name);

                    return RedirectToAction("Details", "User", new {id = user.Id});
                    //}
                }
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Полный список кодов состояния см. по адресу http://go.microsoft.com/fwlink/?LinkID=177550
            //.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Имя пользователя уже существует. Введите другое имя пользователя.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Имя пользователя для данного адреса электронной почты уже существует. Введите другой адрес электронной почты.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Указан недопустимый пароль. Введите допустимое значение пароля.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Указан недопустимый адрес электронной почты. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Указан недопустимый ответ на вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Указан недопустимый вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Указано недопустимое имя пользователя. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.ProviderError:
                    return "Поставщик проверки подлинности вернул ошибку. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                case MembershipCreateStatus.UserRejected:
                    return "Запрос создания пользователя был отменен. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                default:
                    return "Произошла неизвестная ошибка. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";
            }
        }
        #endregion
    }
}
