// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Services;
using Suteki.Shop.Services;
using Suteki.Shop.Services.ProductBuilderContributors;

namespace Suteki.Shop.Tests.Services.ProductBuilderContributors
{
    [TestFixture]
    public class PositionTests : ProductBuilderContributorTestBase
    {
        IOrderableService<Product> productOrderableService;

        protected override IProductBuilderContributor InitContributor()
        {
            productOrderableService = MockRepository.GenerateStub<IOrderableService<Product>>();
            return new Position(productOrderableService);
        }

        [Test]
        public void Should_not_set_the_position_if_the_product_is_an_existing_product()
        {
            var product = new Product {Position = 10};
            context.SetProduct(product);
            context.ProductViewData.ProductId = 54;

            contributor.ContributeTo(context);
            product.Position.ShouldEqual(10);
        }

        [Test]
        public void Should_set_the_position_on_a_new_product()
        {
            productOrderableService.Stub(p => p.NextPosition).Return(23);

            var product = new Product();
            context.SetProduct(product);
            context.ProductViewData.ProductId = 0;

            contributor.ContributeTo(context);
            product.Position.ShouldEqual(23);
        }
    }
}
// ReSharper restore InconsistentNaming