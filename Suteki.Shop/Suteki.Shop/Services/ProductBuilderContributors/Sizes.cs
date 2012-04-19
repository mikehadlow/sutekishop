using System;
using System.Linq;
using Suteki.Common.Extensions;

namespace Suteki.Shop.Services.ProductBuilderContributors
{
    public class Sizes : IProductBuilderContributor
    {
        public void ContributeTo(ProductBuildingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (context.Product.DefaultSizeMissing)
            {
                context.Product.AddDefaultSize();
            }
            context.ProductViewData.Sizes
                .Where(size => !string.IsNullOrEmpty(size))
                .ForEach(size => context.Product.AddSize(new Size
                {
                    Name = size, 
                    IsActive = true, 
                    IsInStock = true
                }));
        }

        public int Order
        {
            get { return 3; }
        }
    }
}