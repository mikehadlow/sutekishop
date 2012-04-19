using System;
using System.Web.Mvc;
using Suteki.Common.Services;

namespace Suteki.Shop.Filters
{
	public class EnsureSsl : IAuthorizationFilter
	{
		readonly IAppSettings settings;

		public EnsureSsl(IAppSettings settings)
		{
			this.settings = settings;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			bool enableSsl = settings.GetSetting(AppSettings.UseSsl) == "true";

			if(enableSsl && filterContext.HttpContext.Request.Url.Scheme == "http")
			{
				filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.Url.ToString().Replace("http", "https"));
			}
		}
	}
}