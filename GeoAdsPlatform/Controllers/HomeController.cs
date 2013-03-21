using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using System.Configuration;
using NpgsqlTypes;

namespace GeoAdsPlatform.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public string SaveAd(string name, float lat, float lon, int radius)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            
            string pointToGeography = string.Format(@"ST_GeomFromText('POINT({0} {1})', 4326)", lon, lat);

            NpgsqlCommand command = new NpgsqlCommand(string.Format(@"INSERT INTO test (name,radius,longitude, latitude, the_geog )
            VALUES ( @name, @radius, @longitude, @latitude, {0} )", pointToGeography));

            command.Parameters.Add("@name", NpgsqlDbType.Varchar, 100).Value = name;
            command.Parameters.Add("@radius", NpgsqlDbType.Integer).Value = radius;
            command.Parameters.Add("@longitude", NpgsqlDbType.Double).Value = lon;
            command.Parameters.Add("@latitude", NpgsqlDbType.Double).Value = lat;

            command.Connection = conn;

            try
            {
                conn.Open();

                command.ExecuteNonQuery();

            }
            catch (NpgsqlException e)
            {
                throw e;
            }

            return "Success";
        }
        
    }
}
