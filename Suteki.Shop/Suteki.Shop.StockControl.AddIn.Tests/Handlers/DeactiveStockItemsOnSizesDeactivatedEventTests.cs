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
    public class DeactiveStockItemsOnSizesDeactivatedEventTests
    {
        private DeactiveStockItemsOnSizesDeactivatedEvent deactiveStockItemsOnSizesDeactivatedEvent;
        private DummyRepository<StockItem> stockItemRepository;
        private readonly DateTime now = new DateTime(2010, 2, 15);
        private const string user = "mike@mike.com";

        [SetUp]
        public void SetUp()
        {
            stockItemRepository = new DummyRepository<StockItem>();
            deactiveStockItemsOnSizesDeactivatedEvent = new DeactiveStockItemsOnSizesDeactivatedEvent(stockItemRepository, () => now, () => user);
        }

        [Test]
        public void SizesDeactivatedEvent_should_deactivate_all_stockItems()
        {
            var dateCreated = new DateTime(2011, 2, 15);

            var stockItems = new System.Collections.Generic.List<StockItem>()
            {
                StockItem.Create("Widget", "Small", dateCreated, "mike@mike.com"),
                StockItem.Create("Widget", "Small", dateCreated, "mike@mike.com"),
                StockItem.Create("Widget", "Small", dateCreated, "mike@mike.com"),
                StockItem.Create("Gadget", "Small", dateCreated, "mike@mike.com"),
            };

            stockItemRepository.GetAllDelegate = () => stockItems.AsQueryable();

            var @event = new SizesDeactivatedEvent("Widget");

            deactiveStockItemsOnSizesDeactivatedEvent.Handle(@event);

            stockItems[0].IsActive.ShouldBeFalse();
            stockItems[0].History[1].DateTime.ShouldEqual(now);
            stockItems[0].History[1].User.ShouldEqual(user);

            stockItems[1].IsActive.ShouldBeFalse();
            stockItems[2].IsActive.ShouldBeFalse();
            stockItems[3].IsActive.ShouldBeTrue();
        }
    }
}

// ReSharper restore InconsistentNaming
