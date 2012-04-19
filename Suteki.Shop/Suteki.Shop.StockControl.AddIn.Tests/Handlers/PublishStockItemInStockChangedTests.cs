// ReSharper disable InconsistentNaming
using System;
using NUnit.Framework;
using Suteki.Common.Events;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.StockControl.AddIn.Handlers;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Tests.Handlers
{
    [TestFixture]
    public class PublishStockItemInStockChangedTests
    {
        private PublishStockItemInStockChanged publishStockItemInStockChanged;
        private DummyDomainEventService domainEventService;
        private StockItem stockItem;
        private readonly DateTime now = new DateTime(2011, 2, 27);
        private const string user = "mike@mike.com";

        [SetUp]
        public void SetUp()
        {
            stockItem = StockItem.Create("widget", "small", now, user);
            domainEventService = new DummyDomainEventService();
            publishStockItemInStockChanged = new PublishStockItemInStockChanged(domainEventService);
        }

        [Test]
        public void Handle_should_publish_new_StockItemInStockChanged_event()
        {
            stockItem.SetOutOfStock(now, user);
            var stockItemSetInStock = stockItem.SetInStock(now, user);
            IDomainEvent raisedEvent = null;

            domainEventService.RaiseDelegate = @event => raisedEvent = @event;

            publishStockItemInStockChanged.Handle(stockItemSetInStock);

            var stockItemInStockChanged = raisedEvent as StockItemInStockChanged;

            stockItemInStockChanged.ShouldNotBeNull();
            stockItemInStockChanged.SizeName.ShouldEqual("small");
            stockItemInStockChanged.ProductName.ShouldEqual("widget");
            stockItemInStockChanged.IsInStock.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class PublishStockItemInStockChangedOnOutOfStockTests
    {
        private PublishStockItemInStockChangedOnOutOfStock publishStockItemInStockChangedOnOutOfStock;
        private DummyDomainEventService domainEventService;
        private StockItem stockItem;
        private readonly DateTime now = new DateTime(2011, 2, 27);
        private const string user = "mike@mike.com";

        [SetUp]
        public void SetUp()
        {
            stockItem = StockItem.Create("widget", "small", now, user);
            domainEventService = new DummyDomainEventService();
            publishStockItemInStockChangedOnOutOfStock = new PublishStockItemInStockChangedOnOutOfStock(domainEventService);
        }

        [Test]
        public void Handle_should_publish_new_StockItemInStockChanged_event()
        {
            var stockItemSetOutOfStock = stockItem.SetOutOfStock(now, user);
            IDomainEvent raisedEvent = null;

            domainEventService.RaiseDelegate = @event => raisedEvent = @event;

            publishStockItemInStockChangedOnOutOfStock.Handle(stockItemSetOutOfStock);

            var stockItemInStockChanged = raisedEvent as StockItemInStockChanged;

            stockItemInStockChanged.ShouldNotBeNull();
            stockItemInStockChanged.SizeName.ShouldEqual("small");
            stockItemInStockChanged.ProductName.ShouldEqual("widget");
            stockItemInStockChanged.IsInStock.ShouldBeFalse();
        }
    }
}

// ReSharper restore InconsistentNaming
