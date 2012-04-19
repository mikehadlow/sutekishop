using Suteki.Common.Events;
using Suteki.Common.Repositories;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Handlers
{
    public class CreateStockItemOnSizeCreatedEvent : IHandle<SizeCreatedEvent>
    {
        private readonly IRepository<StockItem> stockItemRepository;
        private readonly Now now;
        private readonly CurrentUser currentUser;

        public CreateStockItemOnSizeCreatedEvent(IRepository<StockItem> stockItemRepository, Now now, CurrentUser currentUser)
        {
            this.stockItemRepository = stockItemRepository;
            this.now = now;
            this.currentUser = currentUser;
        }

        public void Handle(SizeCreatedEvent sizeCreatedEvent)
        {
            var productName = sizeCreatedEvent.ProductName;
            var sizeName = sizeCreatedEvent.SizeName;
            var stockItem = StockItem.Create(productName, sizeName, now(), currentUser());

            if (!sizeCreatedEvent.IsActive)
            {
                stockItem.Deactivate(now(), currentUser());
            }

            stockItemRepository.SaveOrUpdate(stockItem);
        }
    }
}