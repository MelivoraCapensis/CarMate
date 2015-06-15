using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarMate
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            // Моя локализация
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{culture}/{controller}/{action}/{id}",
            //    defaults: new {culture = "ru", controller = "Home", action = "Index", id = UrlParameter.Optional}
            //);

            routes.MapRoute(
                name: "lang",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { lang = @"ru|en" },
                namespaces: new[] { "CarMate.Controllers" }
            );

            routes.MapRoute(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, lang = "ru" },
                namespaces: new[] { "CarMate.Controllers" }
            );



            routes.MapRoute("ByCountry",
                "country/bycountry",
                new {controller = "Country", action = "ByCountry"},
                new[] {"CarMate.Controllers"});

            routes.MapRoute("RegionByCountry",
                "region/regionbycountry",
                new {controller = "Region", action = "RegionByCountry"},
                new[] {"CarMate.Controllers"});

            // БЕЗ НЕГО НЕ РАБОТАЕТ КАРТА
            routes.MapRoute("PostlstSquirrel",
                "map/postlstSquirrel",
                new {controller = "Map", action = "PostlstSquirrel"},
                new[] {"CarMate.Controllers"});

            routes.MapRoute("CategoryByCategory",
                "category/categorybycountry",
                new {controller = "Category", action = "CategoryByCountry"},
                new[] {"MvcJsonPartialForm.Controllers"});

            routes.MapRoute("CheckFuelCategory",
                "fuelcategory/checkfuelcategory",
                new {controller = "FuelCategory", action = "CheckFuelCategory"},
                new[] {"MvcJsonPartialForm.Controllers"});

            routes.MapRoute("PostlstSVendor",
                "map/postlstsvendor",
                new {controller = "Map", action = "PostlstSVendor"},
                new[] {"MvcJsonPartialForm.Controllers"});


        }
    }
}