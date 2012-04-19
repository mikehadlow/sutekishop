using Suteki.Common.Events;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Handlers
{
    // amalgamate these two classes when we're able to use a newer version of Windsor that supports multiple generic interface services
    // on one class

    public class PublishStockItemInStockChanged : IHandle<StockItemSetInStock>
    {
        private readonly IDomainEventService domainEventService;

        public PublishStockItemInStockChanged(IDomainEventService domainEventService)
        {
            this.domainEventService = domainEventService;
        }

        public void Handle(StockItemSetInStock stockItemSetInStock)
        {
            domainEventService.Raise(new StockItemInStockChanged(stockItemSetInStock.StockItem.SizeName, stockItemSetInStock.StockItem.ProductName, true));
        }
    }

    public class PublishStockItemInStockChangedOnOutOfStock : IHandle<StockItemSetOutOfStock>
    {
        private readonly IDomainEventService domainEventService;

        public PublishStockItemInStockChangedOnOutOfStock(IDomainEventService domainEventService)
        {
            this.domainEventService = domainEventService;
        }

        public void Handle(StockItemSetOutOfStock @event)
        {
            domainEventService.Raise(new StockItemInStockChanged(@event.StockItem.SizeName, @event.StockItem.ProductName, false));
        }
    }
}