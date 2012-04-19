using System.Web.Mvc;
using Microsoft.Web.Mvc;
using Suteki.Shop.Exports.HtmlHelpers;
using Suteki.Shop.StockControl.AddIn.Controllers;

namespace Suteki.Shop.StockControl.AddIn.HtmlHelpers
{
    public class StockControlUi : IStockControlUi
    {
        public void RenderForProductId(HtmlHelper htmlHelper, string productUrlName)
        {
            htmlHelper.RenderAction<StockControlController>(x => x.List(productUrlName));
        }
    }
}