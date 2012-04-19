using System;
using Suteki.Common.Extensions;

namespace Suteki.Shop.StockControl.AddIn.Models
{
    public class StockItemProductNameChanged : StockItemHistoryBase
    {
        public virtual string OldProductName { get; private set; }
        public virtual string NewProductName { get; private set; }

        protected StockItemProductNameChanged()
        {
        }

        public StockItemProductNameChanged(DateTime dateTime, string user, StockItem stockItem, int level, string oldProductName, string newProductName) 
            : base(dateTime, user, stockItem, level)
        {
            if (oldProductName == null)
            {
                throw new ArgumentNullException("oldProductName");
            }
            if (newProductName == null)
            {
                throw new ArgumentNullException("newProductName");
            }

            OldProductName = oldProductName;
            NewProductName = newProductName;
        }

        public override string Description
        {
            get { return "Product name changed from '{0}' to '{1}'".With(OldProductName, NewProductName); }
        }
    }
}