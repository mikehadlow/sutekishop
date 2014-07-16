using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Suteki.Common;
using Suteki.Common.Models;
using Suteki.Common.Repositories;
using Suteki.Shop.Models.ModelHelpers;

namespace Suteki.Shop
{
    public class Outfit : IOrderable, IActivatable, IUrlNamed, INamedEntity
    {
        public Outfit()
        {
            ProductIds = new List<int>();
        }

        public virtual int Position { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string UrlName { get; set; }
        public virtual int Id { get; set; }

        string name;
        [Required(ErrorMessage = "Name is required")]
        public virtual string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                UrlName = Name.ToUrlFriendly();
            }
        }

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

        // only used when acting as view data.
        public virtual IList<int> ProductIds { get; set; }

        public virtual bool HasMainImage
        {
            get
            {
                return OutfitImages.Count > 0;
            }
        }

        public virtual bool HasOriginalImages
        {
            get { return OutfitImages.All(x => x.Image.HasOriginal); }
        }

        public virtual Image MainImage
        {
            get
            {
                if (HasMainImage) return OutfitImages.InOrder().First().Image;
                return null;
            }
        }
    }
}