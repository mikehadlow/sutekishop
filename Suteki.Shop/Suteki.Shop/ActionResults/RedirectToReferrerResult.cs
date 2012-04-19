using System.Web.Mvc;

namespace Suteki.Shop.ActionResults
{
	public class RedirectToReferrerResult : ActionResult
	{
		public override void ExecuteResult(ControllerContext context)
		{
			var url = context.HttpContext.Request.UrlReferrer.ToString();
			context.HttpContext.Response.Redirect(url, false);
		}
	}
}