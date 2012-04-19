using System.Web.Mvc;

namespace Suteki.Shop.Filters
{
	public class AdministratorsOnlyAttribute : AuthorizeAttribute
	{
		public AdministratorsOnlyAttribute()
		{
			Roles = "Administrator";
			Order = 1; //Must come AFTER AuthenticateAttribute
		}
	}
}