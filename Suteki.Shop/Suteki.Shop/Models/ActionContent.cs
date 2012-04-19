using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Suteki.Shop
{
    public class ActionContent : Content
    {
        public virtual string Controller { get; set; }
        public virtual string Action { get; set; }

        public override MvcHtmlString Link(HtmlHelper htmlHelper)
        {
            return htmlHelper.ActionLink(Name, Action, Controller);
        }

		public override string Url(UrlHelper urlHelper) 
		{
			return urlHelper.Action(Action, Controller);
		}
    }
}
