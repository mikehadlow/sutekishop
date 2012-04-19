// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Shop.Services;
using Suteki.Shop.Services.ProductBuilderContributors;

namespace Suteki.Shop.Tests.Services.ProductBuilderContributors
{
    [TestFixture]
    public class CategoriesTests : ProductBuilderContributorTestBase
    {
        IRepository<Category> categoryRepository;

        protected override IProductBuilderContributor InitContributor()
        {
            categoryRepository = MockRepository.GenerateStub<IRepository<Category>>();
            return new Categories(categoryRepository);
        }

        [Test]
        public void Should_bind_correct_categories_to_product()
        {
            var category14 = new Category { Id = 14 };
            var category15 = new Category { Id = 15 };
            categoryRepository.Stub(r => r.GetById(14)).Return(category14);
            categoryRepository.Stub(r => r.GetById(15)).Return(category15);

            var product = new Product();
            context.SetProduct(product);
            context.ProductViewData.CategoryIds.Add(14);
            context.ProductViewData.CategoryIds.Add(15);

            contributor.ContributeTo(context);

            product.ProductCategories.Count.ShouldEqual(2);
            product.ProductCategories[0].Category.ShouldBeTheSameAs(category14);
            product.ProductCategories[1].Category.ShouldBeTheSameAs(category15);
        }

        [Test]
        public void Should_delete_categories_that_are_no_longer_required()
        {
            var category11 = new Category { Id = 11 };
            categoryRepository.Stub(r => r.GetById(11)).Return(category11);
            
            var category14 = new Category { Id = 14 };
            var category15 = new Category { Id = 15 };

            var product = new Product();
            product.AddCategory(category14);
            product.AddCategory(category15);
            context.SetProduct(product);
            context.ProductViewData.CategoryIds.Add(11);

            contributor.ContributeTo(context);

            product.ProductCategories.Count.ShouldEqual(1);
            product.ProductCategories[0].Category.Id.ShouldEqual(11);
        }

        [Test]
        public void Should_add_new_categories_remove_old_ones_and_leave_existing()
        {
            var category14 = new Category { Id = 14 }; // delete this one
            var category15 = new Category { Id = 15 }; // leave this one
            var category16 = new Category { Id = 16 }; // add this one

            categoryRepository.Stub(r => r.GetById(16)).Return(category16);

            var product = new Product();
            product.AddCategory(category14);
            product.AddCategory(category15);

            context.SetProduct(product);
            context.ProductViewData.CategoryIds.Add(15);
            context.ProductViewData.CategoryIds.Add(16);

            contributor.ContributeTo(context);

            product.ProductCategories.Count.ShouldEqual(2);
            product.ProductCategories[0].Category.ShouldBeTheSameAs(category15);
            product.ProductCategories[1].Category.ShouldBeTheSameAs(category16);
        }

        [Test]
        public void Should_add_model_error_if_no_categories_have_been_selected()
        {
            contributor.ContributeTo(context);
            context.ModelStateDictionary.IsValid.ShouldBeFalse();
        }
    }
}
// ReSharper restore InconsistentNaming