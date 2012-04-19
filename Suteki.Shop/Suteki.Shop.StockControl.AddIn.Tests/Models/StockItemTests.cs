// ReSharper disable InconsistentNaming
using System;
using NUnit.Framework;
using Suteki.Common.Events;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Tests.Models
{
    [TestFixture]
    public class StockItemTests
    {
        private StockItem stockItem;
        private readonly DateTime dateCreated = new DateTime(2010, 2, 14);
        private const string user = "mike@mike.com";
        private const string productName = "Widget";
        private const string sizeName = "Small";

        [SetUp]
        public void SetUp()
        {
            stockItem = StockItem.Create(productName, sizeName, dateCreated, user);
        }

        [Test]
        public void Create_should_create_StockItem()
        {
            stockItem.ShouldNotBeNull();
            stockItem.Level.ShouldEqual(0);
            stockItem.ProductName.ShouldEqual(productName);
            stockItem.SizeName.ShouldEqual(sizeName);
            stockItem.IsInStock.ShouldBeTrue();

            stockItem.History.Count.ShouldEqual(1);
            var itemCreated = stockItem.History[0] as StockItemCreated;
            itemCreated.ShouldNotBeNull();
            itemCreated.DateTime.ShouldEqual(dateCreated);
            itemCreated.Description.ShouldEqual("Created");
            itemCreated.User.ShouldEqual(user);
            itemCreated.Level.ShouldEqual(0);
        }

        [Test]
        public void AnyModification_should_raise_domain_event()
        {
            IDomainEvent capturedDomainEvent = null;
            using (DomainEvent.TestWith(domainEvent => { capturedDomainEvent = domainEvent; }))
            {
                stockItem = StockItem.Create(productName, sizeName, dateCreated, user);
                capturedDomainEvent.ShouldBe<StockItemCreated>();

                stockItem.ReceiveStock(10, dateCreated, user);
                capturedDomainEvent.ShouldBe<ReceivedStock>();

                stockItem.Dispatch(5, 1, dateCreated, user);
                capturedDomainEvent.ShouldBe<DispatchedStock>();
            }
        }

        [Test]
        public void StockItem_should_be_able_to_receive_stock()
        {
            const int numberOfItems = 7;
            var dateReceived = new DateTime(2011, 2, 14);

            stockItem.ReceiveStock(numberOfItems, dateReceived, user);

            stockItem.Level.ShouldEqual(7);
            stockItem.History.Count.ShouldEqual(2);
            var itemsRecieved = stockItem.History[1] as ReceivedStock;
            itemsRecieved.ShouldNotBeNull();
            itemsRecieved.DateTime.ShouldEqual(dateReceived);
            itemsRecieved.Description.ShouldEqual("7 Received");
            itemsRecieved.User.ShouldEqual(user);
            itemsRecieved.Level.ShouldEqual(7);
        }

        [Test]
        public void StockItem_should_be_able_to_be_dispatched()
        {
            stockItem.ReceiveStock(10, new DateTime(2010, 1, 3), user);

            const int numberOfItems = 3;
            var dateDispactched = new DateTime(2010, 2, 14);
            const int orderNumber = 12345;

            stockItem.Dispatch(numberOfItems, orderNumber, dateDispactched, user);

            stockItem.Level.ShouldEqual(7);
            stockItem.History.Count.ShouldEqual(3);
            var dispatched = stockItem.History[2] as DispatchedStock;
            dispatched.DateTime.ShouldEqual(dateDispactched);
            dispatched.Description.ShouldEqual("Dispatched 3 items for order 12345");
            dispatched.User.ShouldEqual(user);
            dispatched.Level.ShouldEqual(7);
        }

        [Test]
        public void StockItem_should_be_able_to_adjust_stock_level()
        {
            stockItem.ReceiveStock(10, new DateTime(2010, 1, 3), user);

            const int newLevel = 6;
            var dateAdjusted = new DateTime(2010, 2, 14);

            stockItem.AdjustStockLevel(newLevel, dateAdjusted, user);

            stockItem.Level.ShouldEqual(newLevel);
            stockItem.History.Count.ShouldEqual(3);
            var adjusted = stockItem.History[2] as StockAdjustment;
            adjusted.DateTime.ShouldEqual(dateAdjusted);
            adjusted.Description.ShouldEqual("Manual Adjustment to 6");
            adjusted.User.ShouldEqual(user);
            adjusted.Level.ShouldEqual(newLevel);
        }

        [Test]
        public void StockItem_should_be_deactivatable()
        {
            var dateDeactivated = new DateTime(2011, 2, 15);

            stockItem.Deactivate(dateDeactivated, user);

            stockItem.IsActive.ShouldBeFalse();
            stockItem.History.Count.ShouldEqual(2);

            var deactivated = stockItem.History[1] as StockItemDeactivated;
            deactivated.Description.ShouldEqual("Deactivated");
            deactivated.DateTime.ShouldEqual(dateDeactivated);
            deactivated.User.ShouldEqual(user);
            deactivated.Level.ShouldEqual(0);
        }

        [Test]
        public void StockItem_should_be_able_to_be_set_as_out_of_stock()
        {
            var dateOutOfStock = new DateTime(2011, 2, 27);

            stockItem.SetOutOfStock(dateOutOfStock, user);
            stockItem.IsInStock.ShouldBeFalse();

            stockItem.History.Count.ShouldEqual(2);
            var setOutOfStock = stockItem.History[1] as StockItemSetOutOfStock;
            setOutOfStock.Description.ShouldEqual("Set out of stock");
            setOutOfStock.DateTime.ShouldEqual(dateOutOfStock);
            setOutOfStock.User.ShouldEqual(user);
            setOutOfStock.Level.ShouldEqual(0);
        }

        [Test]
        public void StockItem_should_be_able_to_be_set_in_stock()
        {
            var dateInStock = new DateTime(2011, 2, 27);
            stockItem.SetOutOfStock(dateInStock, user);
            stockItem.SetInStock(dateInStock, user);

            stockItem.IsInStock.ShouldBeTrue();

            stockItem.History.Count.ShouldEqual(3);
            var setInStock = stockItem.History[2] as StockItemSetInStock;
            setInStock.Description.ShouldEqual("Set in stock");
            setInStock.DateTime.ShouldEqual(dateInStock);
            setInStock.User.ShouldEqual(user);
            setInStock.Level.ShouldEqual(0);
        }
    }
}

// ReSharper restore InconsistentNaming
