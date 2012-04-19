using System;
using System.Linq;
using System.Collections.Generic;
using Suteki.Common.Extensions;
using Suteki.Common.Repositories;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Services
{
    public interface IStockItemService
    {
        StockItem GetById(int stockItemId);
        IEnumerable<StockItem> GetAllForProduct(string productName);
        IEnumerable<StockItemHistoryBase> GetHistory(StockItem stockItem, DateTime start, DateTime end);
    }

    public class StockItemService : IStockItemService
    {
        private readonly IRepository<StockItem> stockItemRepository;
        private readonly IRepository<StockItemHistoryBase> historyRepository;

        public StockItemService(
            IRepository<StockItem> stockItemRepository, 
            IRepository<StockItemHistoryBase> historyRepository)
        {
            this.stockItemRepository = stockItemRepository;
            this.historyRepository = historyRepository;
        }

        public StockItem GetById(int stockItemId)
        {
            return stockItemRepository.GetById(stockItemId);
        }

        public IEnumerable<StockItem> GetAllForProduct(string productName)
        {
            var stockItems = stockItemRepository
                .GetAll()
                .Where(x => x.ProductName == productName && x.IsActive).AsEnumerable();

            if (stockItems.Count() > 0) return stockItems;

            var defaultItem = stockItemRepository
                .GetAll()
                .Where(x => x.ProductName == productName && x.SizeName == "-")
                .SingleOrDefault();

            if (defaultItem == null)
            {
                throw new StockControlException(
                    "No default StockItem (size named '-') exists for product named '{0}'", productName);
            }

            return defaultItem.ToEnumerable();
        }

        public IEnumerable<StockItemHistoryBase> GetHistory(StockItem stockItem, DateTime start, DateTime end)
        {
            return stockItem.History.Where(h => h.DateTime >= start && h.DateTime <= end);
        }
    }
}