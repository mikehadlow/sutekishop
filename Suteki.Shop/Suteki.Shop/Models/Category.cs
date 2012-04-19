using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Suteki.Common;
using Suteki.Common.Models;
using Suteki.Common.Repositories;
using Suteki.Shop.Repositories;

namespace Suteki.Shop
{
    public class Category : IActivatable, IOrderable, INamedEntity, IUrlNamed
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public virtual string Name { get; set; }

        public virtual int PrestaId { get; set; }

        public virtual int Position { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Image Image { get; set; }
        public virtual Category Parent { get; set; }

        
        [RegularExpression(@"[\w]+", ErrorMessage = "Only alpha-numeric and underscore characters allowed")]
        public virtual string UrlName { get; set; }

        IList<Category> categories = new List<Category>();
        public virtual IList<Category> Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        public virtual void AddProduct(Product product)
        {
            var productCategory = new ProductCategory {Category = this, Product = product};
            product.ProductCategories.Add(productCategory);
            ProductCategories.Add(productCategory);
        }

        IList<ProductCategory> productCategories = new List<ProductCategory>();
        public virtual IList<ProductCategory> ProductCategories
        {
            get { return productCategories; }
            set { productCategories = value; }
        }

        public virtual bool HasProducts
        {
            get
            {
				return ProductCategories.Any();
            }
        }

        public virtual bool HasActiveProducts
        {
            get
            {
                return ProductCategories.Any(pc => pc.Product.IsActive);
            }
        }

        public virtual bool HasMainImage
        {
            get
            {
                return HasActiveProducts && Products.InOrder().Active().First().HasMainImage;
            }
        }

        public virtual Image MainImage
        {
            get
            {
                return HasMainImage ? Products.InOrder().Active().First().MainImage : null;
            }
        }

        public virtual IEnumerable<Product> Products
    	{
			get { return ProductCategories.Select(x => x.Product); }
    	}

    	public static Category DefaultCategory(Category parent, int position)
    	{
			return new Category 
			{
                Parent = parent,
				Position = position
			};
    	}
    }
}
