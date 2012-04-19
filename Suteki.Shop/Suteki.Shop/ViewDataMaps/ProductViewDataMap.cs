using System;
using System.Linq;
using Suteki.Shop.Repositories;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.ViewDataMaps
{
    public class ProductViewDataMap
    {
        public static ProductViewData FromModel(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }
            if (product.ProductCategories == null)
            {
                throw new ArgumentNullException("product.ProductCategories");
            }
            if (product.Sizes == null)
            {
                throw new ArgumentNullException("product.Sizes");
            }
            if (product.ProductImages == null)
            {
                throw new ArgumentNullException("product.ProductImages");
            }

            return new ProductViewData
            {
                ProductId = product.Id,
                Position = product.Position,
                Name = product.Name,
                UrlName = product.UrlName,
                Weight = product.Weight,
                Price = product.Price,
                IsActive = product.IsActive,
                Description = product.Description,
                CategoryIds = product.ProductCategories.Select(x => x.Category.Id).ToList(),
                Sizes = product.Sizes.Active().Select(x => x.Name).ToList(),
                ProductImages = product.ProductImages
            };
        }
    }
}