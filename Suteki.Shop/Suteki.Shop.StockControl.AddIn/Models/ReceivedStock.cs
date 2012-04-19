using System;
using Suteki.Common.Extensions;

namespace Suteki.Shop.StockControl.AddIn.Models
{
    public class ReceivedStock : StockItemHistoryBase
    {
        public virtual int NumberOfItemsRecieved { get; protected set; }
        public override string Description
        {
            get { return "{0} Received".With(NumberOfItemsRecieved); }
        }

        protected ReceivedStock()
        {
        }

        public ReceivedStock(int numberOfItemsRecieved, DateTime dateTime, string user, StockItem stockItem, int level)
            : base(dateTime, user, stockItem, level)
        {
            NumberOfItemsRecieved = numberOfItemsRecieved;
        }
    }
}