using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {///////////
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,
               "",
               new
               {
                   controller = "Good",
                   action = "List",
                   category = (string)null,
                   page = 1
               }
           );

            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Good", action = "List", category = (string)null },
                constraints: new { page = @"\d+" }
            );

            routes.MapRoute(null,
                "{category}",
                new { controller = "Good", action = "List", page = 1 }
            );

            routes.MapRoute(null,
                "{category}/Page{page}",
                new { controller = "Good", action = "List" },
                new { page = @"\d+" }
            );

            //routes.MapRoute(
            //    "Admin",                                           // Route name
            //    "Edit/{Id}",                            // URL with parameters
            //    new { controller = "Admin", action = "Edit" }  // Parameter defaults
            //);

            routes.MapRoute(null, "{controller}/{action}");
            //////////////////////
          



            //routes.MapRoute(
            //    name: null,
            //    url: "Page{page}",
            //    defaults: new { controller = "Good", action = "List" }
            //);

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Good", action = "List", id = UrlParameter.Optional }
            //);
        }
    }
}
