using System.Web.Mvc;
using Microsoft.Web.Mvc;
using Suteki.Shop.Controllers;

namespace Suteki.Shop
{
    public class TopContent : Content, ITextContent
    {
        public virtual string Text { get; set; }

        public override MvcHtmlString EditLink(HtmlHelper htmlHelper)
        {
            return htmlHelper.ActionLink<CmsController>(c => c.EditTop(Id), "Edit");
        }
    }
}
