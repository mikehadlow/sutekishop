using System;
using Suteki.Common.Events;

namespace Suteki.Shop.Exports.Events
{
    public class StockItemInStockChanged : IDomainEvent
    {
        public string SizeName { get; private set; }
        public string ProductName { get; private set; }
        public bool IsInStock { get; private set; }

        public StockItemInStockChanged(string sizeName, string productName, bool isInStock)
        {
            if (sizeName == null)
            {
                throw new ArgumentNullException("sizeName");
            }
            if (productName == null)
            {
                throw new ArgumentNullException("productName");
            }

            SizeName = sizeName;
            ProductName = productName;
            IsInStock = isInStock;
        }
    }
}