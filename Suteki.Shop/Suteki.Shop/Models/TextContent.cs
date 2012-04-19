using System.Web.Mvc;
using Microsoft.Web.Mvc;
using Suteki.Shop.Controllers;

namespace Suteki.Shop
{
	public class TextContent : Content, ITextContent
	{
	    public virtual string Text { get; set; }

	    public override MvcHtmlString EditLink(HtmlHelper htmlHelper)
		{
			return htmlHelper.ActionLink<CmsController>(c => c.EditText(Id), "Edit");
		}

		public static TextContent DefaultTextContent(Menu parentContent, int nextPosition)
		{
			return new TextContent
			{
				ParentContent = parentContent,
				IsActive = true,
				Position = nextPosition
			};
		}
	}
}