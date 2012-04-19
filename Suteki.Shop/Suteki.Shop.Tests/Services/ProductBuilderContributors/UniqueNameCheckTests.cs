// ReSharper disable InconsistentNaming
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Extensions;
using Suteki.Common.Repositories;
using Suteki.Shop.Services;
using Suteki.Shop.Services.ProductBuilderContributors;

namespace Suteki.Shop.Tests.Services.ProductBuilderContributors
{
    [TestFixture]
    public class UniqueNameCheckTests : ProductBuilderContributorTestBase
    {
        IRepository<Product> productRepository;

        protected override IProductBuilderContributor InitContributor()
        {
            productRepository = MockRepository.GenerateStub<IRepository<Product>>();
            return new UniqueNameCheck(productRepository);
        }

        [Test]
        public void Should_add_model_error_if_name_already_exists()
        {
            context.ProductViewData.ProductId = 0;
            context.ProductViewData.Name = "Widget";
            var existingProduct = new Product { Id = 333, Name = "Widget"};
            productRepository.Stub(r => r.GetAll()).Return(existingProduct.Single().AsQueryable());

            contributor.ContributeTo(context);

            context.ModelStateDictionary.IsValid.ShouldBeFalse();
        }

        [Test]
        public void Should_not_add_error_when_comparing_own_name()
        {
            context.ProductViewData.ProductId = 333;
            context.ProductViewData.Name = "Widget";
            var existingProduct = new Product { Id = 333, Name = "Widget" };
            productRepository.Stub(r => r.GetAll()).Return(existingProduct.Single().AsQueryable());

            contributor.ContributeTo(context);

            context.ModelStateDictionary.IsValid.ShouldBeTrue();
        }

        [Test]
        public void Should_not_add_error_when_name_does_not_match()
        {
            context.ProductViewData.ProductId = 0;
            context.ProductViewData.Name = "Widget x";
            var existingProduct = new Product { Id = 333, Name = "Widget" };
            productRepository.Stub(r => r.GetAll()).Return(existingProduct.Single().AsQueryable());

            contributor.ContributeTo(context);

            context.ModelStateDictionary.IsValid.ShouldBeTrue();
        }
    }
}
// ReSharper restore InconsistentNaming