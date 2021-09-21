using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_Sasema_test
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
            // # This allows "yrno/index.html" View to transfer user input location data as a method parameter to controller (SaveToDB method): 
            routes.MapRoute(
            name: "yrno",
            url: "{controller}/{action}/{location}",
            defaults: new { controller = "yrno", action = "SaveToDB", location = "" }
            );
        }
    }
}
