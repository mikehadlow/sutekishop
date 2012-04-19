using System.Linq;
using Suteki.Common.Events;
using Suteki.Common.Repositories;
using Suteki.Shop.Exports.Events;

namespace Suteki.Shop.Handlers
{
    public class UpdateOrdersOnProductNameChanged : IHandle<ProductNameChangedEvent>
    {
        private readonly IRepository<OrderLine> orderLineRepository;

        public UpdateOrdersOnProductNameChanged(IRepository<OrderLine> orderLineRepository)
        {
            this.orderLineRepository = orderLineRepository;
        }

        public void Handle(ProductNameChangedEvent @event)
        {
            var oldName = @event.OldProductName;
            var orderLines = orderLineRepository.GetAll().Where(x => x.ProductUrlName == oldName);

            foreach (var orderLine in orderLines)
            {
                orderLine.ProductUrlName = @event.NewProductName;
            }
        }
    }
}