using System;

namespace Suteki.Shop.StockControl.AddIn.Models
{
    public class StockItemSetInStock : StockItemHistoryBase
    {
        protected StockItemSetInStock()
        {
        }

        public StockItemSetInStock(DateTime dateTime, string user, StockItem stockItem, int level) : base(dateTime, user, stockItem, level)
        {
        }

        public override string Description
        {
            get { return "Set in stock"; }
        }
    }
}