using Suteki.Common.Repositories;

namespace Suteki.Shop.Services.ProductBuilderContributors
{
    public class RetrieveProduct : IProductBuilderContributor
    {
        readonly IRepository<Product> productRepository;

        public RetrieveProduct(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public void ContributeTo(ProductBuildingContext context)
        {
            if (context.ProductViewData.ProductId == 0) return;
            context.SetProduct(productRepository.GetById(context.ProductViewData.ProductId));
        }

        public int Order
        {
            get { return 0; }
        }
    }
}