using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoAdsPlatform.Attributes;
using GeoAdsPlatform.Models;
using System.Net;
using GeoAdsPlatform.Results;
using System.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace GeoAdsPlatform.Controllers
{
    public class RegisterController : Controller
    {
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

            if (this.RegisterUser(credentials))
            {
                return new WebResult(new ResultInfo() { GreatSuccess = true });
            }
            else
            {
                return new WebResult(new ResultInfo() { GreatSuccess = false });
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
