using System;
using System.Linq;
using NUnit.Framework;
using Suteki.Common.Events;
using Suteki.Common.Repositories;
using Suteki.Shop.Services;
using System.Collections.Specialized;
using Rhino.Mocks;

namespace Suteki.Shop.Tests.Services
{
    [TestFixture]
    public class SizeServiceTests
    {
        ISizeService sizeService;

        [SetUp]
        public void SetUp()
        {
            DomainEvent.TurnOff();

            MockRepository.GenerateStub<IRepository<Size>>();
            sizeService = new SizeService();
        }

        [TearDown]
        public void TearDown()
        {
            DomainEvent.Reset();
        }

        [Test]
        public void Update_ShouldAddSizesInNameValueCollectionToProduct()
        {
            var form = new NameValueCollection
            {
                {"notASize_1", "abc"},
                {"size_1", "S"},
                {"size_2", "M"},
                {"size_3", "L"},
                {"size_4", ""},
                {"size_5", ""},
                {"someOther", "xyz"}
            };

            var product = new Product();

            sizeService.WithValues(form).Update(product);

            Assert.AreEqual(4, product.Sizes.Count, "incorrect number of sizes on product");
            Assert.That(product.Sizes[0].IsActive, Is.False); // default size
            Assert.AreEqual("S", product.Sizes[1].Name);
            Assert.AreEqual("M", product.Sizes[2].Name);
            Assert.AreEqual("L", product.Sizes[3].Name);
        }

        [Test]
        public void Update_ShouldNotMarkExistingSizesInactiveWhenNewOnesAreGiven()
        {
            var form = new NameValueCollection
            {
                {"size_1", "New 1"}, 
                {"size_2", "New 2"}
            };

            var product = CreateProductWithSizes();

            sizeService.WithValues(form).Update(product);

            Assert.AreEqual(5, product.Sizes.Count, "incorrect number of sizes");

            Assert.IsTrue(product.Sizes[0].IsActive);
            Assert.IsTrue(product.Sizes[1].IsActive);
            Assert.IsTrue(product.Sizes[2].IsActive);

            Assert.IsTrue(product.Sizes[3].IsActive);
            Assert.IsTrue(product.Sizes[4].IsActive);

            Assert.AreEqual("New 1", product.Sizes[3].Name);
            Assert.AreEqual("New 2", product.Sizes[4].Name);
        }

        [Test]
        public void Update_ShouldNotDeactivateExistingKeysWhenNoNewAreGiven()
        {
            var form = new NameValueCollection {{"someOtherKey", "xyz"}};

            var product = CreateProductWithSizes();

            sizeService.WithValues(form).Update(product);

            Assert.AreEqual(3, product.Sizes.Count, "incorrect number of sizes");
            Assert.IsTrue(product.Sizes[0].IsActive);
            Assert.IsTrue(product.Sizes[1].IsActive);
            Assert.IsTrue(product.Sizes[2].IsActive);
        }

        private static Product CreateProductWithSizes()
        {
            var product = new Product
            {
                Sizes =
                {
                    new Size { Name = "Old 1", IsActive = true },
                    new Size { Name = "Old 2", IsActive = true },
                    new Size { Name = "Old 3", IsActive = true }
                }
            };
            return product;
        }

        [Test]
        public void Update_Should_add_a_default_size_to_a_product_with_no_sizes()
        {
            var form = new NameValueCollection();
            var product = new Product();

            sizeService.WithValues(form).Update(product);

            Assert.That(product.Sizes.Count, Is.EqualTo(1));
            var defaultSize = product.Sizes[0];

            Assert.That(defaultSize.IsActive, Is.False);
            Assert.That(defaultSize.Name, Is.EqualTo("-"));
            Assert.That(defaultSize.IsInStock, Is.True);
        }
    }
}
