// ReSharper disable InconsistentNaming
using System;
using System.Linq;
using NUnit.Framework;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.StockControl.AddIn.Handlers;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Tests.Handlers
{
    [TestFixture]
    public class ChangeStockItemsOnProductNameChangedEventTests
    {
        private ChangeStockItemsOnProductNameChangedEvent handler;
        private DummyRepository<StockItem> stockItemRepository;
        private readonly DateTime now = new DateTime(2010, 2, 15);
        private const string user = "mike@mike.com";

        [SetUp]
        public void SetUp()
        {
            stockItemRepository = new DummyRepository<StockItem>();
            handler = new ChangeStockItemsOnProductNameChangedEvent(stockItemRepository, () => now, () => user);
        }

        [Test]
        public void Handle_should_update_product_names_on_stockItems()
        {
            var dateCreated = new DateTime(2011, 2, 15);

            var stockItems = new System.Collections.Generic.List<StockItem>()
            {
                StockItem.Create("Widget", "Small", dateCreated, "mike@mike.com"),
                StockItem.Create("Widget", "Medium", dateCreated, "mike@mike.com"),
                StockItem.Create("Widget", "Large", dateCreated, "mike@mike.com"),
                StockItem.Create("Gadget", "Small", dateCreated, "mike@mike.com"),
            };

            stockItemRepository.GetAllDelegate = () => stockItems.AsQueryable();

            var @event = new ProductNameChangedEvent("Widget", "Widget_Plus");

            handler.Handle(@event);

            stockItems[0].ProductName.ShouldEqual("Widget_Plus");
            stockItems[1].ProductName.ShouldEqual("Widget_Plus");
            stockItems[2].ProductName.ShouldEqual("Widget_Plus");
            stockItems[3].ProductName.ShouldEqual("Gadget");

            stockItems[2].History[1].Description.ShouldEqual("Product name changed from 'Widget' to 'Widget_Plus'");
        }
    }
}

// ReSharper restore InconsistentNaming
