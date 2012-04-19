// ReSharper disable InconsistentNaming
using System;
using NUnit.Framework;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.StockControl.AddIn.Handlers;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Tests.Handlers
{
    [TestFixture]
    public class CreateStockItemOnSizeCreatedEventTests
    {
        private CreateStockItemOnSizeCreatedEvent createStockItemOnSizeCreatedEvent;
        private DummyRepository<StockItem> stockItemRepository;
        private readonly DateTime now = new DateTime(2010, 2, 15);
        private const string user = "mike@mike.com";

        [SetUp]
        public void SetUp()
        {
            stockItemRepository = new DummyRepository<StockItem>();
            createStockItemOnSizeCreatedEvent = new CreateStockItemOnSizeCreatedEvent(stockItemRepository, () => now, () => user);
        }

        [Test]
        public void SizeCreatedEvent_should_create_new_StockItem()
        {
            const string productName = "Widget";
            const string sizeName = "Small";

            var @event = new SizeCreatedEvent(productName, sizeName, true);

            StockItem stockItem = null;
            stockItemRepository.SaveOrUpdateDelegate = x => stockItem = x;

            createStockItemOnSizeCreatedEvent.Handle(@event);

            stockItem.ShouldNotBeNull();
            stockItem.ProductName.ShouldEqual("Widget");
            stockItem.SizeName.ShouldEqual("Small");
            stockItem.IsActive.ShouldBeTrue();

            stockItem.History[0].DateTime.ShouldEqual(now);
            stockItem.History[0].User.ShouldEqual(user);
        }

        [Test]
        public void SizeCreatedEvent_should_create_deactivated_stockItem_if_isActive_is_false()
        {
            var @event = new SizeCreatedEvent("Widget", "Small", false);

            StockItem stockItem = null;
            stockItemRepository.SaveOrUpdateDelegate = x => stockItem = x;

            createStockItemOnSizeCreatedEvent.Handle(@event);

            stockItem.IsActive.ShouldBeFalse();
        }
    }
}

// ReSharper restore InconsistentNaming
