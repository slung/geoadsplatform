using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;
using GeoAdsPlatform.Util;

namespace GeoAdsPlatform.Results
{
    #region Factory
    public class OutputFactory
    {
        static public IOutput CreateOutput(ContentFormat format, ControllerContext context, object content, List<Type> knownTypes = null)
        {
            return new JsonOutput(context, content, knownTypes);
        }

        static public IOutput CreateOutput(ControllerContext context, object content, List<Type> knownTypes = null)
        {
            return CreateOutput(ContentFormatProvider.GetFormat(context), context, content, knownTypes);
        }

    }
    #endregion

    #region Outputs
    public interface IOutput
    {
        void Execute();
    }

    public abstract class OutputBase : IOutput
    {

        public ControllerContext Context { get; set; }
        public object Content { get; set; }

        public OutputBase(ControllerContext context, object content)
        {
            Context = context;
            Content = content;
        }

        abstract public void Execute();
    }

    public class XmlOutput : OutputBase
    {
        List<Type> KnownTypes { get; set; }

        public XmlOutput(ControllerContext context, object content, List<Type> knownTypes = null)
            : base(context, content)
        {
            KnownTypes = knownTypes;
        }

        override public void Execute()
        {
            Context.HttpContext.Response.ContentType = "text/xml";
            XmlObjectSerializer serializer = ContentFormatProvider.GetSerializer(ContentFormat.Xml, Content.GetType(), KnownTypes);
            serializer.WriteObject(Context.HttpContext.Response.OutputStream, Content);
        }


    }

    public class JsonOutput : OutputBase
    {
        List<Type> KnownTypes { get; set; }

        public JsonOutput(ControllerContext context, object content, List<Type> knownTypes = null)
            : base(context, content)
        {
            KnownTypes = knownTypes;
        }

        override public void Execute()
        {
            Context.HttpContext.Response.ContentType = "application/json";

            XmlObjectSerializer serializer = ContentFormatProvider.GetSerializer(ContentFormat.Json, Content.GetType(), KnownTypes);
            serializer.WriteObject(Context.HttpContext.Response.OutputStream, Content);
        }
    }
#endregion
}