using System;
using Suteki.Common.Services;

namespace Suteki.Shop.Services.ProductBuilderContributors
{
    public class Position : IProductBuilderContributor
    {
        readonly IOrderableService<Product> productOrderableService;

        public Position(IOrderableService<Product> productOrderableService)
        {
            this.productOrderableService = productOrderableService;
        }

        public void ContributeTo(ProductBuildingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (context.ProductViewData.ProductId != 0) return;
            context.Product.Position = productOrderableService.NextPosition;
        }

        public int Order
        {
            get { return 2; }
        }
    }
}