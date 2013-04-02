using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace GeoAdsPlatform.Results
{
    [DataContract(Namespace = "")]
    public class ResultInfo
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public bool GreatSuccess { get; set; }
    }
}