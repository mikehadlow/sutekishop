using Suteki.Common.Events;

namespace Suteki.Shop.Events
{
    public class OrderDispatched : IDomainEvent
    {
        public Order Order { get; private set; }

        public OrderDispatched(Order order)
        {
            Order = order;
        }
    }
}