using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace GeoAdsPlatform.Util
{
    public enum ContentFormat
    {
        Xml,
        Json,
        Jsonp,
        Csv,
        Xls,
        Kml,
        Unknown,
        GeoJson
    }

    public class ContentFormatProvider
    {
        static public ContentFormat GetFormat(ControllerContext context)
        {
            return GetFormat();
        }

        static public ContentFormat GetFormat()
        {
            return ContentFormat.Json;
        }

        static public XmlObjectSerializer GetSerializer(ContentFormat format, Type t, List<Type> knownTypes = null)
        {
            return new DataContractJsonSerializer(t, knownTypes);
        }
    }
}