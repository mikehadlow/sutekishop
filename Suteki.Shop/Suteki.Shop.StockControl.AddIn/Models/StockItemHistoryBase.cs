using System;
using Suteki.Common.Events;

namespace Suteki.Shop.StockControl.AddIn.Models
{
    public abstract class StockItemHistoryBase : IDomainEvent
    {
        public virtual int Id { get; set; }
        public virtual DateTime DateTime { get; protected set; }
        public virtual string User { get; protected set; }
        public virtual int Level { get; protected set; }
        public virtual StockItem StockItem { get; protected set; }
        public virtual string Comment { get; protected set; }
        public abstract string Description { get; }

        protected StockItemHistoryBase()
        {
        }

        protected StockItemHistoryBase(DateTime dateTime, string user, StockItem stockItem, int level)
        {
            DateTime = dateTime;
            User = user;
            StockItem = stockItem;
            Level = level;
        }

        public virtual void SetComment(string comment)
        {
            Comment = comment;
        }
    }
}