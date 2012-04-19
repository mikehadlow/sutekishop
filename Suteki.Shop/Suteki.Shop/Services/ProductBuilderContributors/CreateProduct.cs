using System;

namespace Suteki.Shop.Services.ProductBuilderContributors
{
    public class CreateProduct : IProductBuilderContributor
    {
        public void ContributeTo(ProductBuildingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (context.ProductViewData.ProductId == 0)
            {
                var product = new Product();
                context.SetProduct(product);
            }
        }

        public int Order
        {
            get { return 0; }
        }
    }
}