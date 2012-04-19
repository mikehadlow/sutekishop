using System.Web.Mvc;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Filters
{
	public class CopyMessageFromTempDataToViewData : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext) 
		{
			var result = filterContext.Result as ViewResult;

			if(result != null && filterContext.Controller.TempData.ContainsKey("message"))
			{
				var model = result.ViewData.Model as ShopViewData;

				if(model != null && string.IsNullOrEmpty(model.Message))
				{
					model.Message = filterContext.Controller.TempData["message"] as string;
				}
			}
		}
	}
}