using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.ViewData;
using Suteki.Shop.Tests.Repositories;
using System.Threading;
using System.Security.Principal;
using Suteki.Shop.Services;
using System.Web.Mvc;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming
namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class ProductControllerTests
    {
        private ProductController productController;
    	private IRepository<Product> productRepository;
        private IRepository<Category> categoryRepository;
        private IOrderableService<ProductCategory> productOrderableService;
    	private IUserService userService;
        private IProductBuilder productBuilder;
        private const string urlName = "Product_4";
        private const string categoryUrlName = "oneTwo";

        [SetUp]
        public void SetUp()
        {
            // you have to be an administrator to access the product controller
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin"), new[] { "Administrator" });

            categoryRepository = MockRepositoryBuilder.CreateCategoryRepository();

            productRepository = MockRepositoryBuilder.CreateProductRepository();

            productOrderableService = MockRepository.GenerateStub<IOrderableService<ProductCategory>>();
            MockRepository.GenerateStub<IOrderableService<ProductImage>>();

        	userService = MockRepository.GenerateStub<IUserService>();
            productBuilder = MockRepository.GenerateStub<IProductBuilder>();

			productController = new ProductController(
                productRepository, 
                categoryRepository, 
                productOrderableService, 
                userService, 
                MockRepository.GenerateStub<IUnitOfWorkManager>(),
                productBuilder);

        	userService.Stub(c => c.CurrentUser).Return(new User { Role = Role.Administrator });
        }

        [Test]
        public void Index_ShouldShowProductListForCategoryOnIndexView()
        {
            const int categoryId = 4;

            var category = new Category
            {
                Id = categoryId,
            };
            category.AddProduct(new Product());
            category.AddProduct(new Product());

            categoryRepository.Stub(r => r.GetById(categoryId)).Return(category);

            var viewData = productController.Index(categoryId)
                .ReturnsViewResult()
                .ForView("Index")
                .WithModel<ShopViewData>()
                .AssertNotNull(vd => vd.ProductCategories)
                .AssertNotNull(vd => vd.Category)
                .AssertAreEqual(categoryId, vd => vd.Category.Id);
        }

        [Test]
        public void Item_ShouldShowItemView()
        {
            // product repository GetAll expectation is already set by
            // MockRepositoryBuilder.CreateProductRepository() in GetFullPath_ShouldReturnFullPage()

            productController.Item(urlName)
                .ReturnsViewResult()
                .ForView("Item")
                .WithModel<ShopViewData>()
                .AssertNotNull(vd => vd.Product)
                .AssertAreEqual(urlName, vd => vd.Product.UrlName);
        }

        [Test]
        public void Category_ShouldShowIndexView()
        {
            productController.Category(categoryUrlName)
                .ReturnsViewResult()
                .ForView("Index");
        }

        [Test]
        public void Item_should_show_plain_text_description_in_meta_description()
        {
            var viewResult = productController.Item(urlName)
                .ReturnsViewResult()
                .ForView("Item");

            viewResult.ViewData["MetaDescription"].ShouldEqual("Description 4");
        }

        [Test]
        public void Inactive_item_should_not_be_visible_to_non_admins()
        {
            // .Repeat.Any() causes previous stub to be overwritten.
            userService.Expect(c => c.CurrentUser).Return(User.Guest).Repeat.Any();

            productController.Item("Product_6")
                .ReturnsViewResult()
                .ForView("ItemNotAvailable");
        }

        [Test]
        public void New_ShouldShowDefaultProductInEditView()
        {
            const int categoryId = 4;
            categoryRepository.Stub(x => x.GetById(categoryId)).Return(new Category());

            var result = productController.New(categoryId) as ViewResult;

            AssertEditViewIsCorrectlyCalled(result);
        }

        private static void AssertEditViewIsCorrectlyCalled(ViewResultBase result)
        {
            Assert.AreEqual("Edit", result.ViewName);
            var viewData = result.ViewData.Model as ProductViewData;
            Assert.IsNotNull(viewData, "viewData is not ShopViewData");
        }

        [Test]
        public void Edit_ShouldShowProductInEditView()
        {
            const int productId = 44;

            var product = new Product();
            productRepository.Expect(r => r.GetById(productId)).Return(product);

            var result = productController.Edit(productId) as ViewResult;

            AssertEditViewIsCorrectlyCalled(result);
        }

    	[Test]
    	public void EditWithPost_ShouldRedirectOnSucessfulBinding()
    	{
			var product = new Product { Id = 5};
            var productViewData = new ProductViewData();
            productBuilder.Stub(
    	        x => x.ProductFromProductViewData(productViewData, productController.ModelState, productController.Request))
                .Return(product);

			productController.Edit(productViewData)
				.ReturnsRedirectToRouteResult()
				.ToAction("Edit")
				.WithRouteValue("id", "5");
    	}

    	[Test]
    	public void EditWithPost_ShouldRenderViewWhenBindingFails()
    	{
			productController.ModelState.AddModelError("foo", "bar");
			var product = new Product();
            var productViewData = new ProductViewData();
            productBuilder.Stub(
                x => x.ProductFromProductViewData(productViewData, productController.ModelState, productController.Request))
                .Return(product);

			productController.Edit(productViewData)
				.ReturnsViewResult()
				.ForView("Edit")
                .WithModel<ProductViewData>()
				.AssertAreSame(productViewData, x => x);
    	}

    	[Test]
    	public void NewWithPost_ShouldInsertNewProduct()
    	{
			var product = new Product { Id = 5};

            var productViewData = new ProductViewData();
            productBuilder.Stub(
                x => x.ProductFromProductViewData(productViewData, productController.ModelState, productController.Request))
                .Return(product);

			productController.New(productViewData)
				.ReturnsRedirectToRouteResult()
				.ToAction("Edit")
				.WithRouteValue("id", "5");

			productRepository.AssertWasCalled(x => x.SaveOrUpdate(product));
    	}

    	[Test]
    	public void NewWithPost_ShouldRenderViewWhenThereAreBindingErrors()
    	{
    		productController.ModelState.AddModelError("foo", "bar");
    		var product = new Product();
            var productViewData = new ProductViewData();
            productBuilder.Stub(
                x => x.ProductFromProductViewData(productViewData, productController.ModelState, productController.Request))
                .Return(product);

			productController.New(productViewData)
				.ReturnsViewResult()
				.ForView("Edit")
                .WithModel<ProductViewData>();
    	}

        [Test]
        public void Should_show_product_not_found_view_when_urlName_does_not_match()
        {
            productController.Item("xxx")
                .ReturnsViewResult()
                .ForView("NotFound");
        }
    }
}
// ReSharper restore InconsistentNaming
