// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Suteki.Shop.Services;
using Suteki.Shop.Services.ProductBuilderContributors;

namespace Suteki.Shop.Tests.Services.ProductBuilderContributors
{
    [TestFixture]
    public class SizesTests : ProductBuilderContributorTestBase
    {
        protected override IProductBuilderContributor InitContributor()
        {
            return new Sizes();
        }

        [Test]
        public void Should_add_default_size()
        {
            var product = new Product();
            context.SetProduct(product);

            contributor.ContributeTo(context);

            product.DefaultSizeMissing.ShouldBeFalse();
            product.Sizes[0].Name.ShouldEqual("-");
        }

        [Test]
        public void Should_add_sizes()
        {
            var product = new Product();
            context.SetProduct(product);
            context.ProductViewData.Sizes.Add("Little");
            context.ProductViewData.Sizes.Add("Big");

            contributor.ContributeTo(context);

            product.Sizes.Count.ShouldEqual(3);
            product.Sizes[1].Name.ShouldEqual("Little");
            product.Sizes[2].Name.ShouldEqual("Big");
        }
    }
}
// ReSharper restore InconsistentNaming