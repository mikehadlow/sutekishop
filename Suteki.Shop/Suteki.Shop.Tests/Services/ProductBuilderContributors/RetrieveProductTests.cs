// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Shop.Services;
using Suteki.Shop.Services.ProductBuilderContributors;

namespace Suteki.Shop.Tests.Services.ProductBuilderContributors
{
    [TestFixture]
    public class RetrieveProductTests : ProductBuilderContributorTestBase
    {
        IRepository<Product> productRepository;

        protected override IProductBuilderContributor InitContributor()
        {
            productRepository = MockRepository.GenerateStub<IRepository<Product>>();
            return new RetrieveProduct(productRepository);
        }

        [Test]
        public void Should_not_retrieve_product_if_viewData_productId_is_zero()
        {
            context.ProductViewData.ProductId = 0;
            contributor.ContributeTo(context);
            context.Product.ShouldBeNull();
        }

        [Test]
        public void Should_retrieve_product_by_id_if_viewData_productId_is_non_zero()
        {
            var expectedProduct = new Product();
            productRepository.Stub(r => r.GetById(202)).Return(expectedProduct);

            context.ProductViewData.ProductId = 202;
            contributor.ContributeTo(context);
            context.Product.ShouldBeTheSameAs(expectedProduct);
        }
    }
}
// ReSharper restore InconsistentNaming