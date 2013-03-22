using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoAdsPlatform.Binders
{
    public abstract class AbstractModelBinder : IModelBinder
    {
        #region IModelBinder Members

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Object result = null;

            if (controllerContext == null)
                throw new Exception(" ControllerContext is cannot be null");

            if (bindingContext == null)
                throw new Exception(" ModelBindingContext is cannot be null");

            result = BindForModel(controllerContext, bindingContext);

            return result;
        }

        public abstract object BindForModel(ControllerContext controllerContext, ModelBindingContext bindingContext);

        #endregion
    }
}