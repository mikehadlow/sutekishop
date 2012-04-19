using System;

namespace Suteki.Shop.Services.ProductBuilderContributors
{
    public class BasicProperties : IProductBuilderContributor
    {
        public void ContributeTo(ProductBuildingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var product = context.Product;
            var viewData = context.ProductViewData;

            product.Name = viewData.Name;
            product.Weight = viewData.Weight;
            product.Price = viewData.Price;
            product.IsActive = viewData.IsActive;
            product.Description = viewData.Description;
        }

        public int Order
        {
            get { return 1; }
        }
    }
}