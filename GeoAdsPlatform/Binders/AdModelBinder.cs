﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using GeoAdsPlatform.Models;

namespace GeoAdsPlatform.Binders
{
    public class AdModelBinder : AbstractModelBinder
    {
        public override object BindForModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;

            StreamReader r = new StreamReader(request.InputStream);
            string s = r.ReadToEnd();

            var queryParams = HttpUtility.ParseQueryString(s);

            AdInfo a = new AdInfo();
            a.InternalId = Int32.Parse(queryParams["internalId"]);
            a.Name = queryParams["name"].Replace("amp;", "&");
            a.Description = queryParams["description"].Replace("amp;", "&");
            a.Radius = Int32.Parse(queryParams["radius"]);
            a.Lat = double.Parse(queryParams["lat"]);
            a.Lon = double.Parse(queryParams["lon"]);

            return a;
        }
    }
}