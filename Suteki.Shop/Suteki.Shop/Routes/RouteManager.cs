using System;
using System.Web.Routing;
using System.Web.Mvc;

namespace Suteki.Shop.Routes
{
    public class RouteManager
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Handler to stop URL routing for Web resources.
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Cms",
                "cms/{urlname}",
                new { controller = "Cms", action = "Index", urlname = "" },
                new { urlname = @"[^\.]*" });

            routes.MapRoute("Product",
                "product/{urlname}",
                new { controller = "Product", action = "Item", urlname = "" },
                new { urlname = @"[^\.]*" });
            
            routes.MapRoute("Category",
                "category/{urlName}",
                new {controller = "Product", action = "Category", urlName = ""},
                new { urlName = @"[^\.]*" });

            routes.MapRoute("reports",
                "report/{action}.csv",
                new { controller = "Report" });

            routes.MapRoute("Shop",
                "shop/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { controller = @"[^\.]*" });

            routes.MapRoute("Root",
                "",
                new { controller = "Cms", action = "Index", urlName = "" });

            routes.MapRoute("Home",
                "home",
                new { controller = "Cms", action = "Index", urlName = "" });

            routes.MapRoute("Rsd",
                "rsd.xml",
                new { controller = "Rsd", action = "Index" });
        }
    }
}
