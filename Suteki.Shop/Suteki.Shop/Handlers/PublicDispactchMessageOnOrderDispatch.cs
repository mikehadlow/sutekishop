using System;
using Suteki.Common.Events;
using Suteki.Shop.Events;
using Suteki.Shop.Exports.Events;

using EventOrder = Suteki.Shop.Exports.Events.Order;
using EventOrderLine = Suteki.Shop.Exports.Events.OrderLine;

namespace Suteki.Shop.Handlers
{
    public class PublicDispactchMessageOnOrderDispatch : IHandle<OrderDispatched>
    {
        private readonly IDomainEventService domainEventService;

        public PublicDispactchMessageOnOrderDispatch(IDomainEventService domainEventService)
        {
            this.domainEventService = domainEventService;
        }

        public void Handle(OrderDispatched orderDispatched)
        {
            if (orderDispatched == null)
            {
                throw new ArgumentNullException("orderDispatched");
            }

            var order = new EventOrder(orderDispatched.Order.Id);
            foreach (var orderLine in orderDispatched.Order.OrderLines)
            {
                order.Lines.Add(new EventOrderLine(orderLine.ProductUrlName, orderLine.SizeName, orderLine.Quantity));
            }

            var externalOrderDispatched = new OrderDispatchedEvent(order);
            domainEventService.Raise(externalOrderDispatched);
        }
    }
}