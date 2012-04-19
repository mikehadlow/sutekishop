using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Suteki.Common;
using Suteki.Common.Events;
using Suteki.Common.Extensions;
using Suteki.Common.Models;
using Suteki.Common.Repositories;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.Repositories;
using Suteki.Shop.Models.ModelHelpers;

namespace Suteki.Shop
{
    public class Product : IOrderable, IActivatable, IUrlNamed, IEntity
    {
        public virtual int Id { get; set; }

        string name;
        [Required(ErrorMessage = "Name is required")]
        public virtual string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                var oldUrlName = UrlName;
                name = value;
                UrlName = Name.ToUrlFriendly();

                // don't raise event on initial name set.
                if (string.IsNullOrEmpty(oldUrlName)) return;
                DomainEvent.Raise(new ProductNameChangedEvent(oldUrlName, UrlName));
            }
        }

        [Required(ErrorMessage = "Description is required")]
        public virtual string Description { get; set; }

        public virtual Money Price { get; set; }
        public virtual int Position { get; set; }
        public virtual int Weight { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string UrlName { get; protected set; }

        IList<ProductImage> productImages = new List<ProductImage>();
        public virtual IList<ProductImage> ProductImages
        {
            get { return productImages; }
            protected set { productImages = value; }
        }

        IList<Size> sizes = new List<Size>();
        public virtual IList<Size> Sizes
        {
            get { return sizes; }
            protected set { sizes = value; }
        }

        IList<ProductCategory> productCategories = new List<ProductCategory>();
        public virtual IList<ProductCategory> ProductCategories
        {
            get { return productCategories; }
            protected set { productCategories = value; }
        }

        IList<Review> reviews = new List<Review>();
        public virtual IList<Review> Reviews
        {
            get { return reviews; }
            protected set { reviews = value; }
        }

        public virtual IEnumerable<Review> ActiveReviews
        {
            get { return reviews.Where(x => x.Approved); }
        }

        public virtual void AddSize(Size size)
        {
            Sizes.Add(size);
            size.Product = this;
            DomainEvent.Raise(new SizeCreatedEvent(UrlName, size.Name, size.IsActive));
        }

        public virtual bool HasMainImage
        {
            get
            {
                return ProductImages.Count > 0;
            }
        }

        public virtual Image MainImage
        {
            get
            {
                if (HasMainImage) return ProductImages.InOrder().First().Image;
                return null;
            }
        }

        public virtual bool HasSize
        {
            get
            {
                return Sizes.Active().Count() > 0;
            }
        }

        public virtual Size DefaultSize
        {
            get
            {
                if (DefaultSizeMissing) throw new ApplicationException("Product has no default size");
                return Sizes[0];
            }
        }

        public virtual bool DefaultSizeMissing
        {
            get
            {
                return Sizes.Count == 0;
            }
        }

        public virtual void AddDefaultSize()
        {
            AddSize(new Size { IsActive = false, Name = "-", IsInStock = true });
        }

        public virtual void ClearAllSizes()
        {
            Sizes.ForEach(size => size.IsActive = false);
            DomainEvent.Raise(new SizesDeactivatedEvent(UrlName));
        }

        public virtual string IsActiveAsString
        {
            get
            {
                if (IsActive) return string.Empty;
                return " Not Active";
            }
        }

        public virtual string PlainTextDescription
        {
            get
            {
                // thanks to Phil Haack :)
                const string matchHtml = @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>";
                const string matchUnwantedChars = @"[""\n\r]";
                return Regex.Replace(Regex.Replace(Description, matchHtml, ""), matchUnwantedChars, "");
            }
        }

        public virtual void AddCategory(Category category)
        {
            var productCategory = new ProductCategory {Category = category, Product = this};
            ProductCategories.Add(productCategory);
            category.ProductCategories.Add(productCategory);
        }

        public virtual void RemoveCategory(Category category)
        {
            var productCategory = ProductCategories
                .Where(pc => pc.Category.Id == category.Id)
                .SingleOrDefault();
            if (productCategory == null) return;
            ProductCategories.Remove(productCategory);
        }

        public virtual void AddProductImage(Image image, int position)
        {
            var productImage = new ProductImage
            {
                Image = image,
                Position = position,
                Product = this
            };
            image.ProductImages.Add(productImage);
            ProductImages.Add(productImage);
        }

        public static Product DefaultProduct(Category parentCategory, int position)
		{
			var product = new Product 
			{
				Id = 0,
				Position = position
			};
			product.ProductCategories.Add(new ProductCategory { Category = parentCategory });
			return product;
		}
    }
}
