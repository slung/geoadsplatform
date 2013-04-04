using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoAdsPlatform.Models;
using System.Web.Security;
using System.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace GeoAdsPlatform.Filters
{
    public class GeoAdsAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            User user = HttpContext.Current.Session[SessionVars.User] as User;
            HttpCookie c = httpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            #region Persistent Login

            if (user == null && c != null && !string.IsNullOrEmpty(c.Value))
            {
                FormsAuthenticationTicket t = FormsAuthentication.Decrypt(c.Value);

                string email = t.UserData.Split(' ')[0];
                string password = t.UserData.Split(' ')[1];

                Credentials credentials = new Credentials() { Email = email, Password = password };

                if (this.LoginUser(credentials))
                {
                    user = new User() { Email = credentials.Email, Password = credentials.Password };

                    httpContext.Session["user"] = user;
                }

            }
            #endregion

            if (user != null)
                return true;

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/login");
        }

        #region HELPERS
        private bool LoginUser(Credentials credentials)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            NpgsqlCommand command = new NpgsqlCommand(@"SELECT COUNT(*) FROM clients WHERE email=@email AND password=@password");
            command.Parameters.Add("@email", NpgsqlDbType.Varchar, 100).Value = credentials.Email;
            command.Parameters.Add("@password", NpgsqlDbType.Varchar, 100).Value = credentials.Password;
            command.Connection = conn;

            try
            {
                conn.Open();

                int result = Convert.ToInt32(command.ExecuteScalar());

                if (result > 0)
                    return true;
                else
                    return false;
            }
            catch (NpgsqlException e)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
    }
}