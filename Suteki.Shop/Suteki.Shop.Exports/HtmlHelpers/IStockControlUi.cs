using System.Web.Mvc;

namespace Suteki.Shop.Exports.HtmlHelpers
{
    public interface IStockControlUi
    {
        void RenderForProductId(HtmlHelper htmlHelper, string productUrlName);
    }
}