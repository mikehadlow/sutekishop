// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Suteki.Shop.Services;
using Suteki.Shop.Services.ProductBuilderContributors;

namespace Suteki.Shop.Tests.Services.ProductBuilderContributors
{
    [TestFixture]
    public class CreateProductTests : ProductBuilderContributorTestBase
    {
        protected override IProductBuilderContributor InitContributor()
        {
            return new CreateProduct();
        }

        [Test]
        public void Should_create_a_new_product_if_viewData_productId_is_zero()
        {
            context.ProductViewData.ProductId = 0;
            contributor.ContributeTo(context);
            context.Product.ShouldNotBeNull();
        }

        [Test]
        public void Should_not_create_a_product_if_viewData_productId_is_non_zero()
        {
            context.ProductViewData.ProductId = 1;
            contributor.ContributeTo(context);
            context.Product.ShouldBeNull();
        }
    }
}
// ReSharper restore InconsistentNaming