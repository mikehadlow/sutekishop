using System.Web.Mvc;
using Castle.Core.Logging;
using Suteki.Common.Extensions;
using Suteki.Shop.Filters;
using Suteki.Shop.Services;

namespace Suteki.Shop.Controllers
{
    [Authenticate, CopyMessageFromTempDataToViewData]
    public abstract class ControllerBase : Controller, IProvidesBaseService
    {
        private IBaseControllerService baseControllerService;

        /// <summary>
        /// Supplies services and configuration to all controllers
        /// </summary>
        public IBaseControllerService BaseControllerService
        {
            get { return baseControllerService; }
            set 
            { 
                baseControllerService = value;

                ViewData["Title"] = "{0}{1}".With(
                    baseControllerService.ShopName,
                    GetControllerName());

                ViewData["MetaDescription"] = "\"{0}\"".With(baseControllerService.MetaDescription);
            }
        }

        public ILogger Logger { get; set; }

        public virtual string GetControllerName()
        {
            return " - {0}".With(GetType().Name.Replace("Controller", ""));
        }


        public virtual void AppendTitle(string text)
        {
            ViewData["Title"] = "{0} - {1}".With(ViewData["Title"], text);
        }

        public virtual void AppendMetaDescription(string text)
        {
            ViewData["MetaDescription"] = text;
        }

    	public string Message
    	{
			get { return TempData["message"] as string; }
			set { TempData["message"] = value; }
    	}

		protected override void OnException(ExceptionContext filterContext) {
			Response.Clear();
			base.OnException(filterContext);
		}
    }
}
