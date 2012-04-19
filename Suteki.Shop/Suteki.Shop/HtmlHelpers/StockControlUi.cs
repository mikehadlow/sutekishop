using System.Web.Mvc;
using Microsoft.Web.Mvc;
using Suteki.Shop.Exports.HtmlHelpers;
using Suteki.Shop.Controllers;

namespace Suteki.Shop.HtmlHelpers
{
    public class StockControlUi : IStockControlUi
    {
        public void RenderForProductId(HtmlHelper htmlHelper, string productUrlName)
        {
            htmlHelper.RenderAction<StockController>(c => c.ProductStock(productUrlName));
        }
    }
}