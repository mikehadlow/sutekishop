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
    public class DecrementStockLevelOnOrderDispatchedEventTests
    {
        private DecrementStockLevelOnOrderDispatchedEvent decrementStockLevelOnOrderDispatchedEvent;
        private DummyRepository<StockItem> stockItemRepository;
        private readonly DateTime now = new DateTime(2010, 2, 15);
        private const string user = "mike@mike.com";

        [SetUp]
        public void SetUp()
        {
            stockItemRepository = new DummyRepository<StockItem>();
            decrementStockLevelOnOrderDispatchedEvent = new DecrementStockLevelOnOrderDispatchedEvent(stockItemRepository, () => now, () => user);
        }

        [Test]
        public void OrderDispatchedEvent_should_decrement_stock_items()
        {
            // create the event
            var order = new Order(345);
            order.Lines.Add(new OrderLine("Widget", "S", 2));
            order.Lines.Add(new OrderLine("Gadget", "L", 1));
            var @event = new OrderDispatchedEvent(order);

            // create existing stock items
            var dateCreated = new DateTime(2011, 2, 15);
            var stockItems = new System.Collections.Generic.List<StockItem>()
            {
                StockItem.Create("Thingy", "S", dateCreated, "mike@mike.com"),
                StockItem.Create("Widget", "S", dateCreated, "mike@mike.com"),
                StockItem.Create("Gadget", "L", dateCreated, "mike@mike.com"),
                StockItem.Create("Gadget", "M", dateCreated, "mike@mike.com"),
            };

            // create some initial stock
            stockItems[1].ReceiveStock(10, dateCreated, "");
            stockItems[2].ReceiveStock(10, dateCreated, "");

            stockItemRepository.GetAllDelegate = () => stockItems.AsQueryable();

            // execute
            decrementStockLevelOnOrderDispatchedEvent.Handle(@event);

            // assert
            stockItems[1].Level.ShouldEqual(8); // 10-2 = 8
            stockItems[2].Level.ShouldEqual(9); // 10-1 = 9
        }

        [Test]
        public void OrderDispatchedEvent_should_work_with_default_sizes()
        {
            // create the event
            var order = new Order(345);
            order.Lines.Add(new OrderLine("Widget", "-", 2));
            var @event = new OrderDispatchedEvent(order);

            // create existing stock items
            var dateCreated = new DateTime(2011, 2, 15);
            var stockItems = new System.Collections.Generic.List<StockItem>()
            {
                StockItem.Create("Widget", "-", dateCreated, "mike@mike.com"),
            };

            // create some initial stock
            stockItems[0].ReceiveStock(10, dateCreated, user);
            stockItems[0].Deactivate(dateCreated, user);

            stockItemRepository.GetAllDelegate = () => stockItems.AsQueryable();

            // execute
            decrementStockLevelOnOrderDispatchedEvent.Handle(@event);

            // assert
            stockItems[0].Level.ShouldEqual(8); // 10-2 = 8
        }
    }
}

// ReSharper restore InconsistentNaming
