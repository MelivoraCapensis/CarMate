using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using CarMate.DAL;

namespace CarMate.Controllers
{
    public class BaseController : Controller
    {
        //protected Entities Db;
        protected RepositoryProvider RepProvider;

        protected CarMateEntities Db = new CarMateEntities();
        protected int UserId;
        //public static string HostName = string.Empty;

        public string CurrentLangCode { get; protected set; }

        public Languages CurrentLang { get; protected set; }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            RepProvider = new RepositoryProvider(Db);

            //if (requestContext.HttpContext.Request.Url != null)
            //{
            //    HostName = requestContext.HttpContext.Request.Url.Authority;
            //}

            if (requestContext.RouteData.Values["lang"] != null && requestContext.RouteData.Values["lang"] as string != "null")
            {
                CurrentLangCode = requestContext.RouteData.Values["lang"] as string;
                CurrentLang = Db.Languages.FirstOrDefault(p => p.Code == CurrentLangCode);

                var ci = new CultureInfo(CurrentLangCode);
                Thread.CurrentThread.CurrentUICulture = ci;
                //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
            ViewBag.UnitFuelConsumptionId = new SelectList(Db.UnitFuelConsumption.OrderBy(x => x.NameUnit), "Id", "NameUnit");

            base.Initialize(requestContext);
        }


        //#region С хабра методы, для перенаправления на страницы

        //protected static string ErrorPage = "~/Error";
        //protected static string NotFoundPage = "~/NotFoundPage";
        //protected static string LoginPage = "~/Login";

        //public RedirectResult RedirectToNotFoundPage
        //{
        //    get { return Redirect(NotFoundPage); }
        //}

        //public RedirectResult RedirectToLoginPage
        //{
        //    get { return Redirect(LoginPage); }
        //}

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    base.OnException(filterContext);

        //    filterContext.Result = Redirect(ErrorPage);
        //}
        //#endregion
    }
}
