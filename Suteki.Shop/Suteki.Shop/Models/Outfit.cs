using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Suteki.Common;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class Outfit : IOrderable, IActivatable, IUrlNamed, INamedEntity
    {
        public virtual int Position { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string UrlName { get; set; }
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public virtual string Description { get; set; }

        IList<OutfitImage> outfitImages = new List<OutfitImage>();
        public virtual IList<OutfitImage> OutfitImages
        {
            get { return outfitImages; }
            set { outfitImages = value; }
        }

        IList<OutfitProduct> outfitProducts = new List<OutfitProduct>();
        public virtual IList<OutfitProduct> OutfitProducts
        {
            get { return outfitProducts; }
            set { outfitProducts = value; }
        }
    }
}