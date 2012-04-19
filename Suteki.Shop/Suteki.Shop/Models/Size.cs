using System.Collections.Generic;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class Size : IActivatable, IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsInStock { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Product Product { get; set; }

        IList<BasketItem> basketItems = new List<BasketItem>();
        public virtual IList<BasketItem> BasketItems
        {
            get { return basketItems; }
            set { basketItems = value; }
        }

        public virtual string NameAndStock
        {
            get
            {
                return Name + (IsInStock ? "" : " Out of Stock");
            }
        }
    }
}
