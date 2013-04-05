using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoAdsPlatform.Models
{
    public class AdInfo
    {
        public int InternalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int Radius { get; set; }
    }

    public class AdRequest
    {
        public float Lat { get; set; }
        public float Lon { get; set; }
    }

    public class AdRequestInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}