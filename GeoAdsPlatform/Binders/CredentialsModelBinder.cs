using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using GeoAdsPlatform.Models;

namespace GeoAdsPlatform.Binders
{
    public class CredentialsModelBinder : AbstractModelBinder
    {
        public override object BindForModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;

            StreamReader r = new StreamReader(request.InputStream);
            string s = r.ReadToEnd();

            var queryParams = HttpUtility.ParseQueryString(s);

            Credentials c = new Credentials();
            c.Email = queryParams["email"];
            c.Password = queryParams["password"];

            return c;
        }
    }
}