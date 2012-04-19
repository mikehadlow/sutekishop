using System;
using Suteki.Common.Events;

namespace Suteki.Shop.Events
{
    public class OrderConfirmed : IDomainEvent
    {
        public Order Order { get; private set; }

        public OrderConfirmed(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            Order = order;
        }
    }
}