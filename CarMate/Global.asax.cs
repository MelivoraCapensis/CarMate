using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CarMate
{
    // Примечание: Инструкции по включению классического режима IIS6 или IIS7 
    // см. по ссылке http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        //private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        //protected void Application_AcquireRequestState(object sender, EventArgs e)
        //{
        //    var handler = Context.Handler as MvcHandler;
        //    var routeData = handler != null ? handler.RequestContext.RouteData : null;
        //    var routeCulture = routeData != null ? routeData.Values["lang"].ToString() : null;
        //    var languageCookie = HttpContext.Current.Request.Cookies["lang"];
        //    var userLanguages = HttpContext.Current.Request.UserLanguages;

        //    // Set the Culture based on a route, a cookie or the browser settings,
        //    // or default value if something went wrong
        //    var cultureInfo = new CultureInfo
        //    (
        //        routeCulture ?? (languageCookie != null ? languageCookie.Value : userLanguages != null ? userLanguages[0] : "ru")
        //    );

        //    Thread.CurrentThread.CurrentUICulture = cultureInfo;
        //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
        //}
    }
}