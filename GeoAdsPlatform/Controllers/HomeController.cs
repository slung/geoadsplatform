using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using System.Configuration;
using NpgsqlTypes;
using System.Data;
using GeoAdsPlatform.Attributes;
using GeoAdsPlatform.Models;
using GeoAdsPlatform.Results;

namespace GeoAdsPlatform.Controllers
{
    public class HomeController : Controller
    {
        [AllowCrossSiteJson]
        public ActionResult Index()
        {
            User user = Session["user"] as User;

            if ( user!=null )
                ViewBag.ServerScript = "GA.client = " + user.Email;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
