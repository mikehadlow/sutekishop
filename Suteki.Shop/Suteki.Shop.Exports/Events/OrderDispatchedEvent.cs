using System;
using System.Collections.Generic;
using Suteki.Common.Events;

namespace Suteki.Shop.Exports.Events
{
    public class OrderDispatchedEvent : IDomainEvent
    {
        public Order Order { get; private set; }

        public OrderDispatchedEvent(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            Order = order;
        }
    }

    public class Order
    {
        public int OrderId { get; private set; }
        public IList<OrderLine> Lines { get; private set; }

        public Order(int orderId)
        {
            if (orderId == 0)
            {
                throw new ArgumentException("orderId is zero");
            }

            OrderId = orderId;
            Lines = new List<OrderLine>();
        }
    }

    public class OrderLine
    {
        public string SizeName { get; private set; }
        public int Quantity { get; private set; }
        public string ProductName { get; set; }

        public OrderLine(string productName, string sizeName, int quantity)
        {
            if (productName == null)
            {
                throw new ArgumentNullException("productName");
            }

            if (sizeName == null)
            {
                throw new ArgumentNullException("sizeName");
            }
            if (quantity == 0)
            {
                throw new ArgumentException("quantity is zero");
            }

            ProductName = productName;
            SizeName = sizeName;
            Quantity = quantity;
        }
    }
}