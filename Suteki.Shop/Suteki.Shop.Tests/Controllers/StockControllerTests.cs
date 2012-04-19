// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Suteki.Common.Events;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.ViewData;
using Rhino.Mocks;
using Suteki.Common.Extensions;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class StockControllerTests
    {
        StockController stockController;
        IRepository<Category> categoryRepository;
        IRepository<Size> sizeRepository;
        IRepository<Product> productRepository;

        [SetUp]
        public void SetUp()
        {
            categoryRepository = MockRepository.GenerateStub<IRepository<Category>>();
            sizeRepository = MockRepository.GenerateStub<IRepository<Size>>();
            productRepository = MockRepository.GenerateStub<IRepository<Product>>();

            stockController = new StockController(
                categoryRepository,
                sizeRepository,
                productRepository);
        }

        [Test]
        public void Index_ShouldPassRootCategoryToIndexView()
        {
            var root = BuildCategories();

            categoryRepository.Expect(cr => cr.GetById(1)).Return(root);

            stockController.Index()
                .ReturnsViewResult()
                .ForView("Index")
                .WithModel<ShopViewData>()
                .AssertAreSame(root, vd => vd.Category);
        }

        private static Category BuildCategories()
        {
            var root = new Category { Id = 1, Name = "Root" };
            return root;
        }

        [Test]
        public void Update_ShouldUpdateStockItems()
        {
            var form = new FormCollection
            {
                {"stockitem_0", "false"},
                {"stockitem_1", "true,false"},
                {"stockitem_2", "false"}
            };

            var sizes = CreateSizes();

            sizeRepository.Expect(s => s.GetAll()).Return(sizes);

            var root = BuildCategories();
            categoryRepository.Expect(cr => cr.GetById(1)).Return(root);

            stockController.Index(form)
                .ReturnsViewResult()
                .ForView("Index")
                .WithModel<ShopViewData>()
                .AssertNotNull(vd => vd.Category);

            Assert.That(sizes.First().IsInStock, Is.True);
            Assert.That(sizes.Last().IsInStock, Is.False);
        }

        private static IQueryable<Size> CreateSizes()
        {
            return new List<Size>
            {
                new Size { Id = 1, IsInStock = false },
                new Size { Id = 2, IsInStock = true }
            }.AsQueryable();
        }

        [Test]
        public void ProductStock_should_return_product()
        {
            const string productName = "Widget";
            var product = new Product { Name = productName };
            productRepository.Stub(x => x.GetAll()).Return(product.ToEnumerable().AsQueryable());

            stockController.ProductStock(productName)
                .ReturnsViewResult()
                .ForView("ProductStock")
                .WithModel<Product>()
                .AssertAreSame(product, vd => vd);
        }

        [Test]
        public void ProductStockUpdate_should_update_product()
        {
            using(DomainEvent.TurnOff())
            {
                var product = new Product {Name = "Widget"};
                product.AddSize(new Size {IsInStock = true});
                product.AddSize(new Size {IsInStock = true});
                product.AddSize(new Size {IsInStock = true});

                var form = new FormCollection
                {
                    {"Id", "4"},
                    {"Sizes[0].IsInStock", "false,true"},
                    {"Sizes[1].IsInStock", "false"},
                    {"Sizes[2].IsInStock", "false,true"}
                };

                productRepository.Stub(r => r.GetById(4)).Return(product);

                stockController.ProductStockUpdate(form)
                    .ReturnsRedirectToRouteResult()
                    .ToController("Product")
                    .ToAction("Item")
                    .WithRouteValue("urlName", "Widget");

                product.Sizes[0].IsInStock.ShouldBeTrue();
                product.Sizes[1].IsInStock.ShouldBeFalse();
                product.Sizes[2].IsInStock.ShouldBeTrue();
            }
        }

        [Test]
        public void GetValueAsBoolTest()
        {
            StockController.GetValueAsBool("true").ShouldBeTrue();
            StockController.GetValueAsBool("false").ShouldBeFalse();
            StockController.GetValueAsBool("false,false").ShouldBeFalse();
            StockController.GetValueAsBool("true,false").ShouldBeTrue();
            StockController.GetValueAsBool("true,true").ShouldBeTrue();
        }
    }
}
// ReSharper restore InconsistentNaming
