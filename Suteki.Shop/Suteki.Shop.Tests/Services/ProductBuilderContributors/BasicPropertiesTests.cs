// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Suteki.Common.Models;
using Suteki.Shop.Services;
using Suteki.Shop.Services.ProductBuilderContributors;

namespace Suteki.Shop.Tests.Services.ProductBuilderContributors
{
    [TestFixture]
    public class BasicPropertiesTests : ProductBuilderContributorTestBase
    {
        protected override IProductBuilderContributor InitContributor()
        {
            return new BasicProperties();
        }

        [Test]
        public void Should_correctly_set_basic_product_properties()
        {
            var viewData = context.ProductViewData;
            viewData.Name = "Gadget";
            viewData.Weight = 234;
            viewData.Price = new Money(495.34M);
            viewData.IsActive = true;
            viewData.Description = "some description";

            var product = new Product();
            context.SetProduct(product);

            contributor.ContributeTo(context);

            product.Name.ShouldEqual(viewData.Name);
            product.Weight.ShouldEqual(viewData.Weight);
            product.Price.ShouldEqual(viewData.Price);
            product.IsActive.ShouldEqual(viewData.IsActive);
            product.Description.ShouldEqual(viewData.Description);
        }
    }
}
// ReSharper restore InconsistentNaming