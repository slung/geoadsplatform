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

namespace GeoAdsPlatform.Controllers
{
    public class AdsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AllowCrossSiteJson]
        [HttpPost]
        public ActionResult Save(AdInfo adInfo)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            string pointToGeography = string.Format(@"ST_GeomFromText('POINT({0} {1})', 4326)", adInfo.Lon, adInfo.Lat);

            NpgsqlCommand command = new NpgsqlCommand(string.Format(@"INSERT INTO ads (name, description, radius, longitude, latitude, the_geog )
            VALUES ( @name, @description, @radius, @longitude, @latitude, {0} )", pointToGeography));

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

    }
}
