using System;
using System.Web.Mvc;
using Suteki.Common.Models;

namespace Suteki.Common.Binders
{
    public class MoneyBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (!typeof (Money).IsAssignableFrom(bindingContext.ModelType))
            {
                throw new SutekiCommonException(
                    "MoneyBinder has attempted to bind to type '{0}', but may only bind to Money",
                                                bindingContext.ModelType);
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            try
            {
                var decimalValue = (decimal)valueProviderResult.ConvertTo(typeof(decimal));
                return new Money(decimalValue);
            }
            catch (Exception)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Not a valid price.");
                return new Money(0M);
            }
        }
    }
}