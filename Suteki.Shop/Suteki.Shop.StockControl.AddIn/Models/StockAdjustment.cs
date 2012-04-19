using System;
using Suteki.Common.Extensions;

namespace Suteki.Shop.StockControl.AddIn.Models
{
    public class StockAdjustment : StockItemHistoryBase
    {
        public virtual int NewLevel { get; protected set; }
        public override string Description
        {
            get { return "Manual Adjustment to {0}".With(NewLevel); }
        }

        protected StockAdjustment()
        {
        }

        public StockAdjustment(DateTime dateTime, int newLevel, string user, StockItem stockItem, int level)
            : base(dateTime, user, stockItem, level)
        {
            NewLevel = newLevel;
        }
    }
}