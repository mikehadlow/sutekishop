using Suteki.Common;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class OutfitProduct : IOrderable, IEntity
    {
        public virtual int Position { get; set; }
        public virtual int Id { get; set; }
        public virtual Outfit Outfit { get; set; }
        public virtual Product Product { get; set; }
    }
}