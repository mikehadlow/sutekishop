using Suteki.Common;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class OutfitProduct : IOrderable, IEntity
    {
        public int Position { get; set; }
        public int Id { get; set; }
        public Outfit Outfit { get; set; }
        public Product Product { get; set; }
    }
}