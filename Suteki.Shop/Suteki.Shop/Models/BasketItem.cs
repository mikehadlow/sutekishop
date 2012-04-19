using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class BasketItem : IEntity
    {
        public virtual int Id { get; set; }
        public virtual int Quantity { get; set; }
        public virtual Size Size { get; set; }
        public virtual Basket Basket { get; set; }

        public virtual Money Total
        {
            get
            {
                return Size.Product.Price * Quantity;
            }
        }

        public virtual decimal TotalWeight
        {
            get
            {
                return Size.Product.Weight * Quantity;
            }
        }
    }
}
