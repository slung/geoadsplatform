using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using GeoAdsPlatform.Models;

namespace GeoAdsPlatform.Binders
{
    public class AdRequestModelBinder : AbstractModelBinder
    {
        public override object BindForModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;

            StreamReader r = new StreamReader(request.InputStream);
            string s = r.ReadToEnd();

            var queryParams = HttpUtility.ParseQueryString(s);

            AdRequest a = new AdRequest();
            a.Lat = float.Parse(queryParams["lat"]);
            a.Lon = float.Parse(queryParams["lon"]);

            return a;
        }
    }
}