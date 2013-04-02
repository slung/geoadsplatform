using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace GeoAdsPlatform.Models
{
    [Serializable]
    [DataContract( Namespace="" )]
    public class User
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}