using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GeoAdsPlatform.Binders;
using GeoAdsPlatform.Models;

namespace GeoAdsPlatform
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Register", // Route name
                "register", // URL with parameters
                new { controller = "Account", action = "Register" } // Parameter defaults
            );

            routes.MapRoute(
                "GetAds", // Route name
                "getads/{name}/{lat}/{lon}", // URL with parameters
                new { controller = "Home", action = "GetAds", name = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "SaveAd", // Route name
                "savead/{name}/{lat}/{lon}/{radius}", // URL with parameters
                new { controller = "Home", action = "SaveAd" } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.Add(typeof(Credentials), new CredentialsModelBinder());
        }
    }
}