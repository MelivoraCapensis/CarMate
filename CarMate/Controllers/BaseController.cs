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
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        protected CarMateEntities Db = new CarMateEntities();
        protected int UserId;
        //public static string HostName = string.Empty;

        public string CurrentLangCode { get; protected set; }

        public Languages CurrentLang { get; protected set; }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //UserId = 0;
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
                //{
                //    NumberFormat = {NumberDecimalSeparator = ".", CurrencyDecimalSeparator = "."},
                    
                //};
                //var ci = (CultureInfo) new CultureInfo(CurrentLangCode).Clone();
                //ci.NumberFormat.NumberDecimalSeparator = ".";
                //NumberFormatInfo nfi = (NumberFormatInfo) new CultureInfo(CurrentLangCode).Clone();
                //NumberFormatInfo nfi = new CultureInfo(CurrentLangCode).NumberFormat;
                //nfi.NumberDecimalSeparator = ".";
                //nfi.CurrencyDecimalSeparator = ".";
                //nfi.PercentDecimalSeparator = ".";
                //nfi.CurrencyGroupSeparator = ".";
                //nfi.NumberGroupSeparator = ".";
                //nfi.PercentGroupSeparator = ".";
                //ci.NumberFormat = nfi;
                Thread.CurrentThread.CurrentUICulture = ci;
                //Thread.CurrentThread.CurrentCulture.NumberFormat = nfi;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                //Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator = ".";
            }
            ViewBag.UnitFuelConsumptionId = new SelectList(Db.UnitFuelConsumption.OrderBy(x => x.NameUnit), "Id", "NameUnit");

            base.Initialize(requestContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            logger.Error(filterContext.Exception.ToString() + "\r\n********************");
            base.OnException(filterContext);
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
