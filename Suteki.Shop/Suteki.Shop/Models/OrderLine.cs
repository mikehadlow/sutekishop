using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class OrderLine : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string ProductUrlName { get; set; }
        public virtual int Quantity { get; set; }
        public virtual Money Price { get; set; }
        public virtual Order Order { get; set; }
        public virtual int ProductId { get; set; }
        public virtual string SizeName { get; set; }

        public virtual Money Total
        {
            get { return Price * Quantity; }
        }
    }
}