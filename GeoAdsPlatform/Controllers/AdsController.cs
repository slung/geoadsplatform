using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeoAdsPlatform.Attributes;
using Npgsql;
using System.Configuration;
using GeoAdsPlatform.Models;
using NpgsqlTypes;
using GeoAdsPlatform.Results;
using GeoAdsPlatform.Filters;
using System.Runtime.Serialization.Json;

namespace GeoAdsPlatform.Controllers
{
    [GeoAdsAuthorize]
    public class AdsController : Controller
    {
        
        #region CREATE

        public ActionResult Create()
        {
            User user = Session[SessionVars.User] as User;

            if (user != null)
                ViewBag.ServerScript = "'" + user.Email + "'";
            else
                ViewBag.ServerScript = "null";

            return View();
        }

        [AllowCrossSiteJson]
        [HttpPost]
        [ActionName("Create")]
        public ActionResult CreatePost(AdInfo adInfo)
        {
            User user = Session[SessionVars.User] as User;

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            
            string pointToGeography = string.Format(@"ST_GeomFromText('POINT({0} {1})', 4326)", adInfo.Lon, adInfo.Lat);

            NpgsqlCommand command = new NpgsqlCommand(string.Format(@"INSERT INTO ads (client, name, description, radius, longitude, latitude, the_geog )
            VALUES ( @client, @name, @description, @radius, @longitude, @latitude, {0} )", pointToGeography));

            command.Parameters.Add("@client", NpgsqlDbType.Integer).Value = user.InternalId;
            command.Parameters.Add("@name", NpgsqlDbType.Varchar, 100).Value = adInfo.Name;
            command.Parameters.Add("@description", NpgsqlDbType.Varchar, 100).Value = adInfo.Description;
            command.Parameters.Add("@radius", NpgsqlDbType.Integer).Value = adInfo.Radius;
            command.Parameters.Add("@longitude", NpgsqlDbType.Double).Value = adInfo.Lon;
            command.Parameters.Add("@latitude", NpgsqlDbType.Double).Value = adInfo.Lat;

            command.Connection = conn;

            try
            {
                conn.Open();

                command.ExecuteNonQuery();

                return new WebResult(new ResultInfo() { GreatSuccess = true });
            }
            catch (NpgsqlException e)
            {
                return new WebResult(new ResultInfo() { GreatSuccess = false, Message = e.Message });
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region READ

        public ActionResult Index()
        {
            User user = Session[SessionVars.User] as User;

            //Save user Email in ViewBag to be displayed
            ViewBag.ServerScript = "'" + user.Email + "'";

            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost()
        {
            User user = Session[SessionVars.User] as User;

            //Save user Email in ViewBag to be displayed
            ViewBag.ServerScript = "'" + user.Email + "'";

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            NpgsqlCommand command = new NpgsqlCommand(string.Format("SELECT \"internalId\", name, description, radius, longitude, latitude FROM ads WHERE client={0}", user.InternalId));

            command.Connection = conn;

            try
            {
                conn.Open();

                NpgsqlDataReader reader = command.ExecuteReader();

                List<AdInfo> ads = new List<AdInfo>();

                while (reader.Read())
                {
                    ads.Add(new AdInfo()
                    {
                        InternalId = reader.GetInt32(reader.GetOrdinal("internalId")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Description = reader.GetString(reader.GetOrdinal("description")),
                        Lat = reader.GetDouble(reader.GetOrdinal("latitude")),
                        Lon = reader.GetDouble(reader.GetOrdinal("longitude")),
                        Radius = reader.GetInt32(reader.GetOrdinal("radius"))
                    });
                }

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<AdInfo>));
                serializer.WriteObject(Response.OutputStream, ads);

                return new EmptyResult();
            }
            catch (NpgsqlException e)
            {
                return new WebResult(new ResultInfo() { GreatSuccess = false, Message = e.Message });
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region UPDATE



        #endregion

        #region DELETE

        [HttpPost]
        public ActionResult Delete(int id)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            NpgsqlCommand command = new NpgsqlCommand(string.Format("DELETE FROM ads WHERE \"internalId\"={0}", id));
            command.Connection = conn;

            try
            {
                conn.Open();

                int result = Convert.ToInt32(command.ExecuteScalar());

                return new EmptyResult();
            }
            catch (NpgsqlException e)
            {
                return new EmptyResult();
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion
    }
}
