using System;

namespace Suteki.Shop.StockControl.AddIn.Models
{
    public class StockItemCreated : StockItemHistoryBase
    {
        public override string Description
        {
            get { return "Created"; }
        }

        protected StockItemCreated()
        {
        }

        public StockItemCreated(DateTime dateTime, string user, StockItem stockItem, int level)
            : base(dateTime, user, stockItem, level)
        {
        }
    }
}