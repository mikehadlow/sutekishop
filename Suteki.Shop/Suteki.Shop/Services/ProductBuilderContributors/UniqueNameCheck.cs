using System;
using System.Linq;
using Suteki.Common.Extensions;
using Suteki.Common.Repositories;

namespace Suteki.Shop.Services.ProductBuilderContributors
{
    public class UniqueNameCheck : IProductBuilderContributor
    {
        readonly IRepository<Product> productRepository;

        public UniqueNameCheck(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public void ContributeTo(ProductBuildingContext context)
        {
            var viewData = context.ProductViewData;
            var productWithNameAlreadyExists =
                productRepository.GetAll().Any(x => x.Id != viewData.ProductId && x.Name == viewData.Name);

            if (!productWithNameAlreadyExists) return;

            context.ModelStateDictionary.AddModelError("ProductId",
                "Product names must be unique and there is already a product called '{0}'".With(viewData.Name));
        }

        public int Order
        {
            get { return -1; }
        }
    }
}