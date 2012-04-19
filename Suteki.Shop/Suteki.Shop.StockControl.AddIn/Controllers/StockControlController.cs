using System;
using System.Linq;
using System.Web.Mvc;
using Suteki.Common.AddIns;
using Suteki.Common.Filters;
using Suteki.Shop.StockControl.AddIn.Services;
using Suteki.Shop.StockControl.AddIn.ViewData;

namespace Suteki.Shop.StockControl.AddIn.Controllers
{
    public class StockControlController : AddinControllerBase
    {
        private readonly IStockItemService stockItemService;
        private readonly Now now;
        private readonly CurrentUser currentUser;

        public StockControlController(IStockItemService stockItemService, Now now, CurrentUser currentUser)
        {
            this.stockItemService = stockItemService;
            this.now = now;
            this.currentUser = currentUser;
        }

        [UnitOfWork]
        public ActionResult List(string id) // id is productName
        {
            var viewData = stockItemService.GetAllForProduct(id);
            return View("List", viewData);
        }

        [UnitOfWork]
        public ActionResult History(int id)
        {
            var stockItem = stockItemService.GetById(id);
            var viewData = new StockItemHistoryViewData
            {
                StockItem = stockItem,
                History = stockItem.History,
                Start = stockItem.History.Min(x => x.DateTime),
                End = stockItem.History.Max(x => x.DateTime)
            };
            return View("History", viewData);
        }

        [HttpPost, UnitOfWork]
        public ActionResult Update(StockUpdateViewData stockUpdateViewData)
        {
            if (stockUpdateViewData == null)
            {
                throw new ArgumentNullException("stockUpdateViewData");
            }

            foreach (var updateItem in stockUpdateViewData.UpdateItems)
            {
                var stockItem = stockItemService.GetById(updateItem.StockItemId);
                if (updateItem.HasReceivedValue())
                {
                    stockItem.ReceiveStock(updateItem.GetReceivedValue(), now(), currentUser())
                        .SetComment(stockUpdateViewData.Comment);
                }
                if (updateItem.HasAdjustedValue())
                {
                    stockItem.AdjustStockLevel(updateItem.GetAdjustmentValue(), now(), currentUser())
                        .SetComment(stockUpdateViewData.Comment);
                }
                if (updateItem.IsInStock != stockItem.IsInStock)
                {
                    if (updateItem.IsInStock)
                    {
                        stockItem.SetInStock(now(), currentUser()).SetComment(stockUpdateViewData.Comment);
                    }
                    else
                    {
                        stockItem.SetOutOfStock(now(), currentUser()).SetComment(stockUpdateViewData.Comment);
                    }
                }
            }

            return Redirect(stockUpdateViewData.ReturnUrl);
        }

        [HttpPost, UnitOfWork]
        public ActionResult HistoryQuery(StockItemHistoryQuery query)
        {
            var stockItem = stockItemService.GetById(query.StockItemId);
            var history = stockItemService.GetHistory(stockItem, query.Start, query.End).ToList();
            var viewData = new StockItemHistoryViewData
            {
                StockItem = stockItem,
                History = history,
                Start = query.Start,
                End = query.End
            };
            return View("History", viewData);
        }
    }
}