using System;
using NUnit.Framework;
using Suteki.Common.Events;
using Suteki.Common.Models;
using Suteki.Common.TestHelpers;

namespace Suteki.Shop.Tests.Maps
{
    [TestFixture]
    public class ProductMapTests : MapTestBase
    {
        int categoryId = 0;

        [SetUp]
        public void SetUp()
        {
            var category = new Category
            {
                Name = "Sloops",
                IsActive = true,
                Position = 1
            };

            InSession(session => session.Save(category));
            categoryId = category.Id;
        }

        [Test]
        public void Should_be_able_to_create_a_product()
        {
            var product = new Product
            {
                Name = "Sophie",
                Description = "A nice sloop",
                Price = new Money(86.22M),
                IsActive = true,
                Position = 1
            };

            var size = new Size
            {
                Name = "10cm"
            };

            var image = new Image
            {
                Description = "sophie1.jpg",
                FileName = new Guid("3DF66C48-ED24-4004-A258-8CD9D56A18C6")
            };

            InSession(session =>
            {
                var category = session.Get<Category>(categoryId);
                using (DomainEvent.TurnOff())
                {
                    product.AddSize(size);
                }
                product.AddCategory(category, 0);
                product.AddProductImage(image, 1);
                session.SaveOrUpdate(product);
            });

            // now delete the image
            InSession(session =>
            {
                var sameProduct = session.Get<Product>(product.Id);
                var productImage = sameProduct.ProductImages[0];
                sameProduct.ProductImages.Remove(productImage);
            });
        }
    }
}