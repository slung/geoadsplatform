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
    public class LoginController : Controller
    {
        #region LOGIN

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost(Credentials credentials)
        {
            // Output
            Response.ContentType = "application/json";
            Response.StatusCode = (int)HttpStatusCode.OK;

            if (this.LoginUser(credentials))
            {
                User user = Session[SessionVars.User] as User;

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(User));
                serializer.WriteObject(Response.OutputStream, Session[SessionVars.User]);

                return new EmptyResult();
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

            NpgsqlCommand command = new NpgsqlCommand("SELECT \"internalId\", email, password  FROM clients WHERE email=@email AND password=@password");
            command.Parameters.Add("@email", NpgsqlDbType.Varchar, 100).Value = credentials.Email;
            command.Parameters.Add("@password", NpgsqlDbType.Varchar, 100).Value = credentials.Password;
            command.Connection = conn;

            try
            {
                conn.Open();

                NpgsqlDataReader reader = command.ExecuteReader();

                List<User> users = new List<User>();

                while (reader.Read())
                {
                    users.Add(new User()
                    {
                        InternalId = reader.GetInt32(reader.GetOrdinal("internalId")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        Password = reader.GetString(reader.GetOrdinal("password")),
                    });
                }

                if (users.Count == 0 || users.Count > 1)
                    return false;
                else
                {
                    this.CreateAuthenticationTicket( users[0] );
                    return true;
                }
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

        private void CreateAuthenticationTicket( User user )
        {
            Session[SessionVars.User] = user;

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                user.Email,
                DateTime.Now,
                DateTime.Now.AddYears(1),
                true,
                user.Email + " " + user.Password,
                FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            //HttpCookie cookie = new HttpCookie("AuthCookie");
            //cookie.Value = encTicket.ToString();
            //cookie.Expires = DateTime.Now.AddYears(1);

            //Response.Cookies.Add(cookie);
        }
        #endregion

    }
}
