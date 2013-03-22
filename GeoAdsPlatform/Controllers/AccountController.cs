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

namespace GeoAdsPlatform.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Register()
        {
            // Output
            Response.ContentType = "application/json";
            Response.StatusCode = (int)HttpStatusCode.OK;

            return View();
        }
        [HttpPost]
        [ActionName("Register")]
        public string RegisterPost( Credentials credentials )
        {
            if (this.RegisterUser(credentials))
            {
                return "Great Success!";
            }
            else
            {
                return "Fail!";
            }
        }

        #region HELPERS
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
                
                throw e;
            }

            return false;
        }
        #endregion

    }
}
