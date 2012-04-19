// ReSharper disable InconsistentNaming
using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Events;
using Suteki.Common.Models;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Shop.Services;

namespace Suteki.Shop.Tests.Services
{
    [TestFixture]
    public class ProductCopyServiceTests 
    {
        Product originalProduct;
        Category originalCategory;
        IProductCopyService productCopyService;
        IOrderableService<Product> productOrder;
        FakeRepository<Product> productRepository;

        [SetUp]
        public void SetUp()
        {
            DomainEvent.TurnOff();

            productOrder = MockRepository.GenerateStub<IOrderableService<Product>>();
            productRepository = new FakeRepository<Product>();
            productCopyService = new ProductCopyService(productOrder, productRepository);

            originalCategory = new Category();
            originalProduct = CreateProduct(originalCategory);
        }

        [TearDown]
        public void TearDown()
        {
            DomainEvent.Reset();
        }

        [Test]
        public void Should_copy_basic_attributes()
        {
            var copiedProduct = productCopyService.Copy(originalProduct);

            copiedProduct.Id.ShouldEqual(0);
            copiedProduct.Name.ShouldEqual(originalProduct.Name + " Copy");
            copiedProduct.Description.ShouldEqual(originalProduct.Description);
            copiedProduct.Price.ShouldEqual(originalProduct.Price);
            copiedProduct.Weight.ShouldEqual(originalProduct.Weight);
            copiedProduct.IsActive.ShouldBeFalse();
        }

        [Test]
        public void Should_have_the_same_category()
        {
            var copiedProduct = productCopyService.Copy(originalProduct);
            copiedProduct.ProductCategories[0].Category
                .ShouldBeTheSameAs(originalProduct.ProductCategories[0].Category);
        }

        [Test]
        public void Should_assign_next_position()
        {
            productOrder.Stub(p => p.NextPosition).Return(11);
            var copiedProduct = productCopyService.Copy(originalProduct);
            copiedProduct.Position.ShouldEqual(11);
        }

        [Test]
        public void Should_copy_sizes()
        {
            var copiedProduct = productCopyService.Copy(originalProduct);

            copiedProduct.Sizes.Count.ShouldEqual(2);

            copiedProduct.Sizes[0].Id.ShouldEqual(0);
            copiedProduct.Sizes[0].Name.ShouldEqual(originalProduct.Sizes[0].Name);
            copiedProduct.Sizes[0].IsActive.ShouldEqual(originalProduct.Sizes[0].IsActive);
            copiedProduct.Sizes[0].IsInStock.ShouldEqual(originalProduct.Sizes[0].IsInStock);

            copiedProduct.Sizes[1].Id.ShouldEqual(0);
            copiedProduct.Sizes[1].Name.ShouldEqual(originalProduct.Sizes[1].Name);
            copiedProduct.Sizes[1].IsActive.ShouldEqual(originalProduct.Sizes[1].IsActive);
            copiedProduct.Sizes[1].IsInStock.ShouldEqual(originalProduct.Sizes[1].IsInStock);
        }

        [Test]
        public void Should_not_allow_duplicate_names()
        {
            // a product with the name '<original name> Copy' already exists
            productRepository.EntitesToReturnFromGetAll.Add(new Product { Name = originalProduct.Name + " Copy" });

            var copiedProduct = productCopyService.Copy(originalProduct);
            copiedProduct.Name.ShouldEqual(originalProduct.Name + " Copy*");
        }

        static Product CreateProduct(Category category)
        {
            var product = new Product
            {
                Id = 144,
                Name = "Super Widget",
                Description = "Some product description",
                Price = new Money(34.56M),
                Position = 6,
                Weight = 345,
                IsActive = true
            };

            product.AddSize(new Size
            {
                Id = 21,
                IsActive = true,
                IsInStock = true,
                Name = "Small"
            });

            product.AddSize(new Size
            {
                Id = 22,
                IsActive = true,
                IsInStock = true,
                Name = "Medium"
            });

            product.AddCategory(category);

            product.AddProductImage(new Image { Id = 3 }, 1);
            return product;
        }
    }
}
// ReSharper restore InconsistentNaming