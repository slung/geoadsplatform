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
                "Home",                                              // Route name
                "home",                                             // URL with parameters
                new { controller = "Home", action = "Index" }  // Parameter defaults
            );

            routes.MapRoute(
                "About",                                              // Route name
                "about",                                             // URL with parameters
                new { controller = "Home", action = "About" }  // Parameter defaults
            );

            routes.MapRoute(
                "Login", // Route name
                "login", // URL with parameters
                new { controller = "Login", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "Logout", // Route name
                "logout", // URL with parameters
                new { controller = "Login", action = "Logout" } // Parameter defaults
            );

            routes.MapRoute(
                "Register", // Route name
                "register", // URL with parameters
                new { controller = "Register", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "GetLocationBasedAds", // Route name
                "getads.{format}", // URL with parameters
                new { controller = "Api", action = "GetAds", name = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Ads", // Route name
                "ads", // URL with parameters
                new { controller = "Ads", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "SaveAd", // Route name
                "ads/create", // URL with parameters
                new { controller = "Ads", action = "Create" } // Parameter defaults
            );

            routes.MapRoute(
                "DeleteAd", // Route name
                "ads/delete", // URL with parameters
                new { controller = "Ads", action = "Delete" } // Parameter defaults
            );

            routes.MapRoute(
                "Default",                                              // Route name
                "",                                                      // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //Add binders
            ModelBinders.Binders.Add(typeof(Credentials), new CredentialsModelBinder());
            ModelBinders.Binders.Add(typeof(AdRequest), new AdRequestModelBinder());
            ModelBinders.Binders.Add(typeof(AdInfo), new AdModelBinder());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            #if DEBUG
                        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
                        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
            #endif
        }

        #region EVENTS
        protected void Session_Start()
        { 
        }

        protected void Session_End()
        { 
        }
        #endregion
    }
}