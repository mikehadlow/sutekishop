using System.Linq;
using Suteki.Common.Events;
using Suteki.Common.Repositories;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Handlers
{
    public class ChangeStockItemsOnProductNameChangedEvent : IHandle<ProductNameChangedEvent>
    {
        private readonly IRepository<StockItem> stockItemRepository;
        private readonly Now now;
        private readonly CurrentUser currentUser;

        public ChangeStockItemsOnProductNameChangedEvent(IRepository<StockItem> stockItemRepository, Now now, CurrentUser currentUser)
        {
            this.stockItemRepository = stockItemRepository;
            this.now = now;
            this.currentUser = currentUser;
        }

        public void Handle(ProductNameChangedEvent productNameChangedEvent)
        {
            stockItemRepository
                .GetAll()
                .Where(x => x.ProductName == productNameChangedEvent.OldProductName)
                .ToList()
                .ForEach(x => x.ChangeProductName(productNameChangedEvent.OldProductName, productNameChangedEvent.NewProductName, now(), currentUser()));
        }
    }
}