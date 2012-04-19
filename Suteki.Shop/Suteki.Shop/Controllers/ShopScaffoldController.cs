using Suteki.Common;
using Suteki.Common.Controllers;
using Suteki.Common.Extensions;
using Suteki.Shop.Filters;
using Suteki.Shop.Services;

namespace Suteki.Shop.Controllers
{
	[Authenticate, CopyMessageFromTempDataToViewData]
    public abstract class ShopScaffoldController<T> : OrderableScaffoldController<T>, IProvidesBaseService where T : class, IOrderable, new()
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
            }
        }

        public virtual string GetControllerName()
        {
            return " - {0}".With(GetType().Name.Replace("Controller", ""));
        }
    }
}
