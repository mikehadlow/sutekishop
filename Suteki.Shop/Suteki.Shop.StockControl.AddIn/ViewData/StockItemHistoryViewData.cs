using System;
using System.Collections.Generic;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.ViewData
{
    public class StockItemHistoryViewData
    {
        public StockItem StockItem { get; set; }
        public IList<StockItemHistoryBase> History { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}