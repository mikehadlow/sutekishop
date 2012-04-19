using System;

namespace Suteki.Shop.StockControl.AddIn.Models
{
    public class StockItemSetOutOfStock : StockItemHistoryBase
    {
        protected StockItemSetOutOfStock()
        {
        }

        public StockItemSetOutOfStock(DateTime dateTime, string user, StockItem stockItem, int level) : base(dateTime, user, stockItem, level)
        {
        }

        public override string Description
        {
            get { return "Set out of stock"; }
        }
    }
}