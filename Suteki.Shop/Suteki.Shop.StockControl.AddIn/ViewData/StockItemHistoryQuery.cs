using System;

namespace Suteki.Shop.StockControl.AddIn.ViewData
{
    public class StockItemHistoryQuery
    {
        public int StockItemId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}