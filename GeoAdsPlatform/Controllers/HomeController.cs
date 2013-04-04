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
using GeoAdsPlatform.Filters;
using System.Runtime.Serialization.Json;
using System.Web.Security;
using System.Net;

namespace GeoAdsPlatform.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            User user = Session[SessionVars.User] as User;

            if ( user!=null )
                ViewBag.ServerScript = "'" + user.Email + "'";
            else
                ViewBag.ServerScript = "null";

            return View();
        }
    }
}
