using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using GeoAdsPlatform.Results;

namespace GeoAdsPlatform.Results
{
    public class WebResult : ActionResult
    {
        #region Fields
        private object content;
        private List<Type> knownTypes = new List<Type>();
        #endregion

        #region Constructors

        public WebResult(object content)
        {
            this.content = content;

        }

        public WebResult(object content, bool loadAssemblyToKnownTypes)
            : this(content)
        {
            if (loadAssemblyToKnownTypes)
                knownTypes.AddRange(content.GetType().Assembly.GetTypes());
        }

        public WebResult(object content, List<Type> knownTypes)
            : this(content)
        {
            this.knownTypes.AddRange(knownTypes);
        }

        #endregion

        #region Properties

        public object Content
        {
            get { return content; }
        }

        #endregion

        #region Methods
        public override void ExecuteResult(ControllerContext context)
        {

            IOutput output;

            output = OutputFactory.CreateOutput(context, content, knownTypes);

            if (output == null)
                throw new Exception("Format is unknown");

            output.Execute();
        }
        #endregion
    }
}