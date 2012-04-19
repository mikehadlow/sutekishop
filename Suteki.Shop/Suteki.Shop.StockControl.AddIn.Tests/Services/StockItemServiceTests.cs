// ReSharper disable InconsistentNaming
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Suteki.Shop.StockControl.AddIn.Models;
using Suteki.Shop.StockControl.AddIn.Services;

namespace Suteki.Shop.StockControl.AddIn.Tests.Services
{
    [TestFixture]
    public class StockItemServiceTests
    {
        private IStockItemService stockItemService;
        private DummyRepository<StockItem> stockItemRepository;
        private DummyRepository<StockItemHistoryBase> historyRepository;
        private readonly DateTime now = new DateTime(2011, 2, 20);
        private const string user = "mike@mike.com";

        [SetUp]
        public void SetUp()
        {
            stockItemRepository = new DummyRepository<StockItem>();
            historyRepository = new DummyRepository<StockItemHistoryBase>();
            stockItemService = new StockItemService(stockItemRepository, historyRepository);
        }

        [Test]
        public void GetById_should_return_a_stock_item_by_id()
        {
            const int stockItemId = 888;
            var stockItem = StockItem.Create("Widget", "Small", now, user);
            stockItemRepository.GetByIdDelegate = id =>
            {
                id.ShouldEqual(stockItemId);
                return stockItem;
            };

            var returnedStockItem = stockItemService.GetById(stockItemId);
            returnedStockItem.ShouldBeTheSameAs(stockItem);
        }

        [Test]
        public void GetAllForProduct_should_return_active_stockItems_for_product()
        {
            Func<int, string, string, StockItem> createStockItem = (id, size, product) =>
                StockItem.Create(product, size, new DateTime(2011, 2, 20), "mike@mike.com").SetId(id);

            var stockItems = new System.Collections.Generic.List<StockItem>
            {
                createStockItem(1, "-", "Widget"),
                createStockItem(2, "Error", "Widget"),
                createStockItem(3, "s", "Widget"),
                createStockItem(4, "m", "Widget"),
                createStockItem(5, "l", "Widget"),
                createStockItem(6, "-", "Gadget"),
                createStockItem(7, "Large", "Gadget"),
            };

            stockItems[0].Deactivate(new DateTime(2011, 2, 20), "mike");
            stockItems[1].Deactivate(new DateTime(2011, 2, 20), "mike");

            stockItemRepository.GetAllDelegate = () => stockItems.AsQueryable();

            var returnedItems = stockItemService.GetAllForProduct("Widget");

            returnedItems.Count().ShouldEqual(3);
            returnedItems.First().ShouldBeTheSameAs(stockItems[2]);
            returnedItems.Last().ShouldBeTheSameAs(stockItems[4]);
        }

        [Test]
        public void When_there_are_no_active_items_GetAllForProduct_should_return_the_default()
        {
            Func<int, string, string, StockItem> createStockItem = (id, size, product) =>
                StockItem.Create(product, size, new DateTime(2011, 2, 20), "mike@mike.com").SetId(id);

            var stockItems = new System.Collections.Generic.List<StockItem>
            {
                createStockItem(1, "-", "Widget"),
                createStockItem(2, "Error", "Widget"),
                createStockItem(3, "s", "Widget"),
                createStockItem(4, "m", "Widget"),
                createStockItem(5, "l", "Widget"),
                createStockItem(6, "-", "Gadget"),
                createStockItem(7, "Large", "Gadget"),
            };

            stockItems[0].Deactivate(new DateTime(2011, 2, 20), "mike");
            stockItems[1].Deactivate(new DateTime(2011, 2, 20), "mike");
            stockItems[2].Deactivate(new DateTime(2011, 2, 20), "mike");
            stockItems[3].Deactivate(new DateTime(2011, 2, 20), "mike");
            stockItems[4].Deactivate(new DateTime(2011, 2, 20), "mike");

            stockItemRepository.GetAllDelegate = () => stockItems.AsQueryable();

            var returnedItems = stockItemService.GetAllForProduct("Widget");

            returnedItems.Count().ShouldEqual(1);
            returnedItems.First().ShouldBeTheSameAs(stockItems[0]);
        }

        [Test]
        public void GetHistory_should_get_a_stockItems_history_between_the_given_dates()
        {
            var stockItem = StockItem.Create("widget", "small", new DateTime(2011, 1, 1), user);
            var history1 = stockItem.ReceiveStock(10, new DateTime(2011, 2, 1), user);
            var history2 = stockItem.Dispatch(2, 1, new DateTime(2011, 3, 1), user);
            var history3 = stockItem.Dispatch(2, 2, new DateTime(2011, 4, 1), user);
            stockItem.Dispatch(2, 3, new DateTime(2011, 5, 1), user);

            var start = new DateTime(2011, 2, 1);
            var end = new DateTime(2011, 4, 1);

            historyRepository.GetAllDelegate = () => stockItem.History.AsQueryable();

            var history = stockItemService.GetHistory(stockItem, start, end);

            history.Count().ShouldEqual(3);
            history.ElementAt(0).ShouldBeTheSameAs(history1);
            history.ElementAt(1).ShouldBeTheSameAs(history2);
            history.ElementAt(2).ShouldBeTheSameAs(history3);
        }
    }

    public class DummyStockItemService : IStockItemService
    {
        public Func<int, StockItem> GetByIdDelegate { get; set; }
        public Func<string, IEnumerable<StockItem>> GetAllForProductDelegate { get; set; }
        public Func<StockItem, DateTime, DateTime, IEnumerable<StockItemHistoryBase>> GetHistoryDelegate { get; set; }

        public StockItem GetById(int stockItemId)
        {
            return GetByIdDelegate(stockItemId);
        }

        public IEnumerable<StockItem> GetAllForProduct(string productName)
        {
            return GetAllForProductDelegate(productName);
        }

        public IEnumerable<StockItemHistoryBase> GetHistory(StockItem stockItem, DateTime start, DateTime end)
        {
            return GetHistoryDelegate(stockItem, start, end);
        }
    }
}

// ReSharper restore InconsistentNaming
