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

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute("ByCountry",
               "country/bycountry",
               new { controller = "Country", action = "ByCountry" },
               new[] { "CarMate.Controllers" });
            routes.MapRoute("RegionByCountry",
                "region/regionbycountry",
                new { controller = "Region", action = "RegionByCountry" },
                new[] { "CarMate.Controllers" });
            routes.MapRoute("PostlstSquirrel",
                "map/postlstSquirrel",
                new { controller = "Map", action = "PostlstSquirrel" },
                new[] { "CarMate.Controllers" });

        }
    }
}