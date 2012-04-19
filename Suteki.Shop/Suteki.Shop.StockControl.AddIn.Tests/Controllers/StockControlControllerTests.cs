// ReSharper disable InconsistentNaming
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Suteki.Common.TestHelpers;
using Suteki.Shop.StockControl.AddIn.Controllers;
using Suteki.Shop.StockControl.AddIn.Models;
using Suteki.Shop.StockControl.AddIn.Tests.Services;
using Suteki.Shop.StockControl.AddIn.ViewData;

namespace Suteki.Shop.StockControl.AddIn.Tests.Controllers
{
    [TestFixture]
    public class StockControlControllerTests
    {
        private StockControlController controller;
        private DummyStockItemService stockItemService;
        private const string user = "mike@mike.com";
        private readonly DateTime now = new DateTime(2011, 2, 18);

        [SetUp]
        public void SetUp()
        {
            stockItemService = new DummyStockItemService();
            controller = new StockControlController(stockItemService, () => now, () => user);
        }

        [Test]
        public void List_should_show_stockItems_for_product()
        {
            const string productName = "Widget";

            var dateCreated = new DateTime(2011, 2, 15);

            var stockItems = new List<StockItem>();

            stockItemService.GetAllForProductDelegate = x => stockItems;

            var viewData = controller.List(productName)
                .ReturnsViewResult()
                .WithModel<IEnumerable<StockItem>>();

            viewData.ShouldBeTheSameAs(stockItems);
        }

        [Test]
        public void History_should_show_stockItem_history()
        {
            const int stockItemId = 89;
            var stockItem = StockItem.Create("Widget", "Small", now, "mike@mike.com").SetId(stockItemId);
            stockItem.ReceiveStock(10, now, user);
            stockItem.Dispatch(2, 5, now, user);
            stockItem.Dispatch(2, 5, now, user);

            stockItemService.GetByIdDelegate = id =>
            {
                id.ShouldEqual(stockItemId);
                return stockItem;
            };

            var viewData = controller.History(stockItemId)
                .ReturnsViewResult()
                .WithModel<StockItemHistoryViewData>();

            viewData.StockItem.ShouldBeTheSameAs(stockItem);
            viewData.History.Count.ShouldEqual(stockItem.History.Count);
            viewData.Start.ShouldEqual(now);
            viewData.End.ShouldEqual(now);
        }

        [Test]
        public void History_should_show_stockItems_between_given_dates()
        {
            const int stockItemId = 89;
            var stockItem = StockItem.Create("Widget", "Small", now, "mike@mike.com").SetId(stockItemId);
            stockItem.ReceiveStock(10, now, user);
            stockItem.Dispatch(2, 5, now, user);
            stockItem.Dispatch(2, 5, now, user);

            var start = new DateTime(2011, 1, 1);
            var end = new DateTime(2011, 2, 1);

            stockItemService.GetHistoryDelegate = (stockItemArg, startArg, endArg) =>
            {
                stockItemArg.ShouldBeTheSameAs(stockItem);
                startArg.ShouldEqual(start);
                endArg.ShouldEqual(end);

                return stockItem.History;
            };
            stockItemService.GetByIdDelegate = id =>
            {
                id.ShouldEqual(stockItemId);
                return stockItem;
            };

            var query = new StockItemHistoryQuery
            {
                StockItemId = stockItemId,
                Start = start,
                End = end
            };

            var viewData = controller.HistoryQuery(query)
                .ReturnsViewResult()
                .WithModel<StockItemHistoryViewData>();

            viewData.StockItem.ShouldBeTheSameAs(stockItem);
            viewData.History.Count.ShouldEqual(stockItem.History.Count);
            viewData.Start.ShouldEqual(start);
            viewData.End.ShouldEqual(end);
        }

        [Test]
        public void Update_should_update_stock()
        {
            // create the view data posted from the form
            const string returnUrl = "http://return.url.com/something";
            const string comment= "some comment";

            var stockUpdateViewData = new StockUpdateViewData
            {
                ReturnUrl = returnUrl,
                Comment = comment,
                UpdateItems =
                    {
                        new UpdateItem
                        {
                            StockItemId    = 4,
                            Received = "10",
                            Adjustment = "",
                            IsInStock = true
                        },
                        new UpdateItem
                        {
                            StockItemId    = 5,
                            Received = "",
                            Adjustment = "22",
                            IsInStock = false
                        },
                        new UpdateItem
                        {
                            StockItemId    = 6,
                            Received = "",
                            Adjustment = "",
                            IsInStock = true
                        }
                    }
            };

            // create some stock items
            var dateCreated = new DateTime(2010, 6, 11);
            const string createdByUser = "jon@mike.com";
            var stockItems = new List<StockItem>
            {
                StockItem.Create("Widget", "Small", dateCreated, createdByUser).SetId(4),
                StockItem.Create("Widget", "Med", dateCreated, createdByUser).SetId(5),
                StockItem.Create("Widget", "Large", dateCreated, createdByUser).SetId(6),
                StockItem.Create("Widget", "Huge", dateCreated, createdByUser).SetId(7),
            };
            stockItems[0].ReceiveStock(10, dateCreated, createdByUser);
            stockItems[1].ReceiveStock(10, dateCreated, createdByUser);
            stockItems[2].ReceiveStock(10, dateCreated, createdByUser);
            stockItems[3].ReceiveStock(10, dateCreated, createdByUser);

            stockItemService.GetByIdDelegate = id => stockItems.Single(x => x.Id == id);

            // run the controller Update action
            var result = controller.Update(stockUpdateViewData).ReturnsRedirect();
            result.Url.ShouldEqual(returnUrl);

            // assert that stock levels are as expected
            stockItems[0].Level.ShouldEqual(20);
            stockItems[0].History[2].Comment.ShouldEqual(comment);
            stockItems[1].Level.ShouldEqual(22);
            stockItems[1].History[2].Comment.ShouldEqual(comment);
            stockItems[2].Level.ShouldEqual(10);
            stockItems[3].Level.ShouldEqual(10);

            // assert that stock item IsInStock is as expected
            stockItems[0].IsInStock.ShouldBeTrue();
            stockItems[1].IsInStock.ShouldBeFalse();
            stockItems[2].IsInStock.ShouldBeTrue();
            stockItems[3].IsInStock.ShouldBeTrue();
        }
    }
}

// ReSharper restore InconsistentNaming
