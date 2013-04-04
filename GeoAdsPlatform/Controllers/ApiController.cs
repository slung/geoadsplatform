using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using System.Configuration;
using GeoAdsPlatform.Models;
using System.IO;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using GeoAdsPlatform.Results;
using GeoAdsPlatform.Attributes;

namespace GeoAdsPlatform.Controllers
{
    public class ApiController : Controller
    {
        [HttpPost]
        public ActionResult GetAds( AdRequest adRequest )
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            // DataReader to read database record.
            NpgsqlDataReader reader = null;

            string pointToGeography = string.Format(@"ST_GeomFromText('POINT({0} {1})', 4326)::geography", adRequest.Lon, adRequest.Lat);

            NpgsqlCommand command = new NpgsqlCommand(string.Format(@"SELECT * FROM ads WHERE ST_DWithin( the_geog, {0}, radius)", pointToGeography));

            command.Connection = conn;

            try
            {
                conn.Open();

                reader = command.ExecuteReader();

                List<AdRequestInfo> adRequestInfoList = new List<AdRequestInfo>();
                while (reader.Read())
                {
                    adRequestInfoList.Add(new AdRequestInfo()
                    {
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Description = reader.GetString(reader.GetOrdinal("description"))
                    });
                }

                reader.Close();

                if (adRequestInfoList.Count > 0)
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    var json = jsonSerializer.Serialize(adRequestInfoList);

                    return new WebResult(json);
                }
                
            }
            catch (NpgsqlException e)
            {
                return new WebResult(new ResultInfo() { GreatSuccess = false, Message = e.Message });
            }
            finally
            {
                conn.Close();
            }

            return new WebResult(new ResultInfo() { GreatSuccess = false });
        }

    }
}
