using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Services
{
    public interface IProductBuilder
    {
        Product ProductFromProductViewData(
            ProductViewData productViewData, 
            ModelStateDictionary modelStateDictionary,
            HttpRequestBase httpRequestBase);
    }

    public interface IProductBuilderContributor
    {
        void ContributeTo(ProductBuildingContext context);
        int Order { get; }
    }

    public class ProductBuildingContext
    {
        public ProductBuildingContext(
            ProductViewData productViewData, 
            ModelStateDictionary modelStateDictionary,
            HttpRequestBase httpRequestBase)
        {
            if (productViewData == null)
            {
                throw new ArgumentNullException("productViewData");
            }
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException("modelStateDictionary");
            }
            if (httpRequestBase == null)
            {
                throw new ArgumentNullException("httpRequestBase");
            }

            ProductViewData = productViewData;
            ModelStateDictionary = modelStateDictionary;
            HttpRequestBase = httpRequestBase;
        }

        public void SetProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }
            Product = product;
        }

        public Product Product { get; private set; }
        public ProductViewData ProductViewData { get; private set; }
        public ModelStateDictionary ModelStateDictionary { get; private set; }
        public HttpRequestBase HttpRequestBase { get; private set; }
    }

    public class ProductBuilder : IProductBuilder
    {
        readonly IProductBuilderContributor[] contributors;

        public ProductBuilder(IProductBuilderContributor[] contributors)
        {
            if (contributors == null)
            {
                throw new ArgumentNullException("contributors");
            }
            if (contributors.Length == 0)
            {
                throw new ArgumentException("No IProductBuilderContributor implementations have been registered");
            }

            this.contributors = contributors;
        }

        public Product ProductFromProductViewData(
            ProductViewData productViewData, 
            ModelStateDictionary modelStateDictionary, 
            HttpRequestBase httpRequestBase)
        {
            if (productViewData == null)
            {
                throw new ArgumentNullException("productViewData");
            }
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException("modelStateDictionary");
            }

            var context = new ProductBuildingContext(productViewData, modelStateDictionary, httpRequestBase);

            foreach (var contributor in contributors.OrderBy(x => x.Order))
            {
                if(!context.ModelStateDictionary.IsValid) continue;
                contributor.ContributeTo(context);
            }

            return context.Product;
        }
    }
}