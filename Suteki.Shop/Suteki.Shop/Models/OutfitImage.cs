using Suteki.Common;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class OutfitImage : IOrderable, IEntity
    {
        public virtual int Position { get; set; }
        public virtual int Id { get; set; }
        public virtual Image Image { get; set; }
        public virtual Outfit Outfit { get; set; }
    }
}