// ReSharper disable InconsistentNaming
using System;
using NUnit.Framework;
using Suteki.Common.TestHelpers;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Tests.Maps
{
    [TestFixture]
    public class StockItemMapTests : MapTestBase
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void StockItem_should_persist_to_database()
        {
            const string productName = "Harrington Black";
            const string sizeName = "Small";
            var now = new DateTime(2011, 2, 14);
            const string user = "mike@mike.com";
            var id = 0;

            InSession(session =>
            {
                var stockItem = StockItem.Create(productName, sizeName, now, user);
                session.Save(stockItem);
                id = stockItem.Id;
            });

            InSession(session =>
            {
                var stockItem = session.Get<StockItem>(id);
                stockItem.ProductName.ShouldEqual(productName);
                stockItem.SizeName.ShouldEqual(sizeName);
                stockItem.IsActive.ShouldBeTrue();

                stockItem.History.Count.ShouldEqual(1);
                stockItem.History[0].Description.ShouldEqual("Created");
                stockItem.History[0].User.ShouldEqual(user);
                stockItem.History[0].DateTime.ShouldEqual(now);
            });

            InSession(session =>
            {
                var stockItem = session.Get<StockItem>(id);
                stockItem.ReceiveStock(10, now, user);
            });

            InSession(session =>
            {
                var stockItem = session.Get<StockItem>(id);
                stockItem.Dispatch(4, 12345, now, user);
            });

            InSession(session =>
            {
                var stockItem = session.Get<StockItem>(id);
                stockItem.AdjustStockLevel(12, now, user);
            });

            InSession(session =>
            {
                var stockItem = session.Get<StockItem>(id);
                stockItem.Deactivate(now, user);
            });

            InSession(session =>
            {
                var stockItem = session.Get<StockItem>(id);
                stockItem.History[0].Description.ShouldEqual("Created");
                stockItem.History[1].Description.ShouldEqual("10 Received");
                stockItem.History[2].Description.ShouldEqual("Dispatched 4 items for order 12345");
                stockItem.History[3].Description.ShouldEqual("Manual Adjustment to 12");
                stockItem.History[4].Description.ShouldEqual("Deactivated");
            });
        }
    }
}

// ReSharper restore InconsistentNaming
