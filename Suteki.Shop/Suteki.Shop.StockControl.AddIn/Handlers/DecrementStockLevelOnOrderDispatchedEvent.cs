using System.Linq;
using Suteki.Common;
using Suteki.Common.Events;
using Suteki.Common.Repositories;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Handlers
{
    public class DecrementStockLevelOnOrderDispatchedEvent : IHandle<OrderDispatchedEvent>
    {
        private readonly IRepository<StockItem> stockItemRepository;
        private readonly Now now;
        private readonly CurrentUser currentUser;

        public DecrementStockLevelOnOrderDispatchedEvent(IRepository<StockItem> stockItemRepository, Now now, CurrentUser currentUser)
        {
            this.stockItemRepository = stockItemRepository;
            this.now = now;
            this.currentUser = currentUser;
        }

        public void Handle(OrderDispatchedEvent orderDispatchedEvent)
        {
            foreach (var orderLine in orderDispatchedEvent.Order.Lines)
            {
                var productName = orderLine.ProductName;
                var sizeName = orderLine.SizeName;

                var stockItem =
                    stockItemRepository
                        .GetAll()
                        .Where(x => x.ProductName == productName && x.SizeName == sizeName && x.IsActive)
                        .SingleOrDefault() 
                    ??
                    stockItemRepository
                        .GetAll()
                        .Where(x => x.ProductName == productName && x.SizeName == sizeName && !x.IsActive)
                        .SingleOrDefault();

                if (stockItem == null)
                {
                    throw new SutekiCommonException("No StockItem found with ProductName {0} and SizeName '{1}'",
                                                    productName, sizeName);
                }

                stockItem.Dispatch(orderLine.Quantity, orderDispatchedEvent.Order.OrderId, now(), currentUser());
            }
        }
    }
}