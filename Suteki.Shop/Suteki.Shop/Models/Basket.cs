using System;
using System.Collections.Generic;
using System.Linq;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class Basket : IEntity, IAmOwnedBy
    {
        public virtual int Id { get; set; }
        public virtual DateTime OrderDate { get; set; }
        public virtual Country Country { get; set; }
        public virtual User User { get; set; }

        IList<BasketItem> basketItems = new List<BasketItem>();
        public virtual IList<BasketItem> BasketItems
        {
            get { return basketItems; }
            set { basketItems = value; }
        }

        public virtual bool IsEmpty
        {
            get
            {
                return !BasketItems.Any();
            }
        }

        public virtual Money Total
        {
            get
            {
                return new Money(BasketItems.Sum(item => item.Total.Amount));
            }
        }

        public virtual void AddBasketItem(BasketItem basketItem)
        {
            basketItem.Basket = this;
            basketItems.Add(basketItem);
        }

        public virtual void RemoveBasketItem(BasketItem basketItem)
        {
            basketItem.Basket = null;
            basketItems.Remove(basketItem);
        }
    }
}
