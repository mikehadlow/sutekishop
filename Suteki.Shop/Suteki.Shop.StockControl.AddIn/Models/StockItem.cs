using System;
using System.Collections.Generic;
using Suteki.Common.Events;
using Suteki.Common.Models;

namespace Suteki.Shop.StockControl.AddIn.Models
{
    public class StockItem : IEntity, IActivatable
    {
        public virtual int Id { get; set; }
        public virtual string ProductName { get; protected set; }
        public virtual string SizeName { get; protected set; }
        public virtual int Level { get; protected set; }
        public virtual bool IsActive { get; set; }
        public virtual bool IsInStock { get; protected set; }

        public virtual IList<StockItemHistoryBase> History { get; protected set; }

        protected StockItem()
        {
        }

        protected StockItem(string productName, string sizeName)
        {
            History = new List<StockItemHistoryBase>();
            ProductName = productName;
            SizeName = sizeName;
            Level = 0;
            IsActive = true;
            IsInStock = true;
        }

        private T AddHistory<T>(T historyItem) where T : StockItemHistoryBase
        {
            History.Add(historyItem);
            DomainEvent.Raise(historyItem);
            return historyItem;
        }

        public static StockItem Create(string productName, string sizeName, DateTime dateCreated, string user)
        {
            var stockitem = new StockItem(productName, sizeName);
            stockitem.AddHistory(new StockItemCreated(dateCreated, user, stockitem, 0));
            return stockitem;
        }

        public virtual ReceivedStock ReceiveStock(int numberOfItems, DateTime dateReceived, string user)
        {
            Level += numberOfItems;
            return AddHistory(new ReceivedStock(numberOfItems, dateReceived, user, this, Level));
        }

        public virtual DispatchedStock Dispatch(int numberOfItems, int orderNumber, DateTime dateDispactched, string user)
        {
            Level -= numberOfItems;
            return AddHistory(new DispatchedStock(dateDispactched, numberOfItems, orderNumber, user, this, Level));
        }

        public virtual StockAdjustment AdjustStockLevel(int newLevel, DateTime dateAdjusted, string user)
        {
            Level = newLevel;
            return AddHistory(new StockAdjustment(dateAdjusted, newLevel, user, this, Level));
        }

        public virtual StockItemDeactivated Deactivate(DateTime dateDeactivated, string user)
        {
            IsActive = false;
            return AddHistory(new StockItemDeactivated(dateDeactivated, user, this, Level));
        }

        public virtual StockItemProductNameChanged ChangeProductName(string oldProductName, string newProductName, DateTime dateChanged, string currentUser)
        {
            if (ProductName != oldProductName)
            {
                throw new ArgumentException(string.Format("oldProductName '{0}' does not match ProductName '{1}'.",
                                                          oldProductName, ProductName));
            }
            ProductName = newProductName;
            return AddHistory(new StockItemProductNameChanged(
                dateChanged, currentUser, this, Level, oldProductName, newProductName));
        }

        public virtual StockItemSetOutOfStock SetOutOfStock(DateTime dateOutOfStock, string currentUser)
        {
            if(!IsInStock) return null;
            IsInStock = false;
            return AddHistory(new StockItemSetOutOfStock(dateOutOfStock, currentUser, this, Level));
        }

        public virtual StockItemSetInStock SetInStock(DateTime dateInStock, string currentUser)
        {
            if (IsInStock) return null;
            IsInStock = true;
            return AddHistory(new StockItemSetInStock(dateInStock, currentUser, this, Level));
        }
    }
}