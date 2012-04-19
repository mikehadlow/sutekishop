using Suteki.Common;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class ProductImage : IOrderable, IEntity
    {
        public virtual int Id { get; set; }
        public virtual int Position { get; set; }
        public virtual Image Image { get; set; }
        public virtual Product Product { get; set; }
    }
}
