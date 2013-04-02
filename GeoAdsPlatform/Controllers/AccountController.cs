using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoAdsPlatform.Models;
using System.Net;
using Npgsql;
using System.Configuration;
using NpgsqlTypes;
using System.Data;
using System.Web.Security;
using GeoAdsPlatform.Results;
using GeoAdsPlatform.Attributes;
using System.Runtime.Serialization.Json;

namespace GeoAdsPlatform.Controllers
{
    public class AccountController : Controller
    {
        #region LOGIN

        [AllowCrossSiteJson]
        public ActionResult Login()
        {
            return View();
        }

        [AllowCrossSiteJson]
        [HttpPost]
        [ActionName("Login")]
        public ActionResult LoginPost(Credentials credentials)
        {
            // Output
            Response.ContentType = "application/json";
            Response.StatusCode = (int)HttpStatusCode.OK;

            if (this.LoginUser(credentials))
            {
                this.CreateAuthenticationTicket(credentials);
                User user = Session["user"] as User;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return new WebResult(new ResultInfo() { GreatSuccess = false });
            }
        }

        #endregion

        #region REGISTER

        [AllowCrossSiteJson]
        public ActionResult Register()
        {
            // Output
            Response.ContentType = "application/json";
            Response.StatusCode = (int)HttpStatusCode.OK;

            return View();
        }

        [AllowCrossSiteJson]
        [HttpPost]
        [ActionName("Register")]
        public ActionResult RegisterPost(Credentials credentials)
        {
            // Output
            Response.ContentType = "application/json";
            Response.StatusCode = (int)HttpStatusCode.OK;

            if (this.RegisterUser(credentials))
            {
                return new WebResult(new ResultInfo() { GreatSuccess = true });
            }
            else
            {
                return new WebResult(new ResultInfo() { GreatSuccess = false });
            }
        }

        #endregion

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

        private bool RegisterUser(Credentials credentials)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            NpgsqlCommand command = new NpgsqlCommand(@"INSERT INTO clients (email, password ) VALUES ( @email, @password)");
            command.Parameters.Add("@email", NpgsqlDbType.Varchar, 100).Value = credentials.Email;
            command.Parameters.Add("@password", NpgsqlDbType.Varchar, 100).Value = credentials.Password;
            command.Connection = conn;

            try
            {
                conn.Open();

                int result = command.ExecuteNonQuery();

                if (result != -1)
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

        private void CreateAuthenticationTicket( Credentials credentials )
        {
            Session["user"] = new User() { Email = credentials.Email, Password = credentials.Password };

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                credentials.Email,
                DateTime.Now,
                DateTime.Now.AddYears(1),
                true,
                credentials.Email + " " + credentials.Password,
                FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            Response.Cookies[FormsAuthentication.FormsCookieName].Value = encTicket.ToString();
            Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddYears(1);
        }
        #endregion

    }
}
