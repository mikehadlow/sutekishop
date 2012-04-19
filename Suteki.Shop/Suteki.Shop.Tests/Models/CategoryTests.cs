using NUnit.Framework;

namespace Suteki.Shop.Tests.Models
{
    [TestFixture]
    public class CategoryTests
    {
        [Test]
        public void HasMainImage_should_return_false_when_category_has_no_products()
        {
            var category = new Category();
            category.HasMainImage.ShouldBeFalse();
        }

        [Test]
        public void HasMainImage_should_return_false_when_category_has_only_inactive_products()
        {
            var category = GetCategoryWithOnlyInactiveProducts();
            category.HasMainImage.ShouldBeFalse();
        }

        static Category GetCategoryWithInactiveAndActiveProducts()
        {
            var category = GetCategoryWithOnlyInactiveProducts();
            var product = new Product
            {
                IsActive = true
            };
            category.ProductCategories.Add(new ProductCategory { Product = product });
            return category;
        }

        static Category GetCategoryWithOnlyInactiveProducts()
        {
            var category = new Category();
            var product = new Product
            {
                IsActive = false
            };
            category.ProductCategories.Add(new ProductCategory{ Product = product });
            return category;
        }

        [Test]
        public void HasActiveProducts_should_be_false_when_category_only_has_inactive_products()
        {
            var category = GetCategoryWithOnlyInactiveProducts();
            category.HasActiveProducts.ShouldBeFalse();
        }

        [Test]
        public void HasActiveProducts_should_be_true_when_category_has_at_least_one_active_product()
        {
            var category = GetCategoryWithInactiveAndActiveProducts();
            category.HasActiveProducts.ShouldBeTrue();
        }
    }
}