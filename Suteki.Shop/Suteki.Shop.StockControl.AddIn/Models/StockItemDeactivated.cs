using System;

namespace Suteki.Shop.StockControl.AddIn.Models
{
    public class StockItemDeactivated : StockItemHistoryBase
    {
        protected StockItemDeactivated()
        {
        }

        public StockItemDeactivated(DateTime dateTime, string user, StockItem stockItem, int level) : base(dateTime, user, stockItem, level)
        {
        }

        public override string Description
        {
            get { return "Deactivated"; }
        }
    }
}