using System;
using Suteki.Common.Extensions;

namespace Suteki.Shop.StockControl.AddIn.Models
{
    public class DispatchedStock : StockItemHistoryBase
    {
        public virtual int NumberOfItemsDispatched { get; protected set; }
        public virtual int OrderNumber { get; protected set; }
        public override string Description
        {
            get { return "Dispatched {0} items for order {1}".With(NumberOfItemsDispatched, OrderNumber); }
        }

        protected DispatchedStock()
        {
            
        }

        public DispatchedStock(DateTime dateTime, int numberOfItemsDispatched, int orderNumber, string user, StockItem stockItem, int level)
            : base(dateTime, user, stockItem, level)
        {
            NumberOfItemsDispatched = numberOfItemsDispatched;
            OrderNumber = orderNumber;
        }
    }
}