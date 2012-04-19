using System.Linq;
using Suteki.Common.Events;
using Suteki.Common.Extensions;
using Suteki.Common.Repositories;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Handlers
{
    public class DeactiveStockItemsOnSizesDeactivatedEvent : IHandle<SizesDeactivatedEvent>
    {
        private readonly IRepository<StockItem> stockItemRepository;
        private readonly Now now;
        private readonly CurrentUser currentUser;

        public DeactiveStockItemsOnSizesDeactivatedEvent(IRepository<StockItem> stockItemRepository, Now now, CurrentUser currentUser)
        {
            this.stockItemRepository = stockItemRepository;
            this.now = now;
            this.currentUser = currentUser;
        }

        public void Handle(SizesDeactivatedEvent sizesDeactivatedEvent)
        {
            var productName = sizesDeactivatedEvent.ProductName;
            stockItemRepository
                .GetAll()
                .Where(x => x.ProductName == productName)
                .ForEach(x => x.Deactivate(now(), currentUser()));
        }
    }
}