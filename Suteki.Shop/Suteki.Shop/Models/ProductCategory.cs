using Suteki.Common;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class ProductCategory : IEntity, IOrderable
    {
        public virtual int Id { get; set; }
        public virtual Category Category { get; set; }
        public virtual Product Product { get; set; }
        public virtual int Position { get; set; }
    }
}