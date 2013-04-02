﻿using System;
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
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

            routes.MapRoute(
                "Login", // Route name
                "login", // URL with parameters
                new { controller = "Account", action = "Login" } // Parameter defaults
            );

            routes.MapRoute(
                "Register", // Route name
                "register", // URL with parameters
                new { controller = "Account", action = "Register" } // Parameter defaults
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
                "ads/save", // URL with parameters
                new { controller = "Ads", action = "Save" } // Parameter defaults
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
    }
}