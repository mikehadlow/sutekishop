using System.Linq;
using System.Web;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;
using Suteki.Shop.Tests.Repositories;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private CategoryController categoryController;

    	private IRepository<Category> categoryRepository;
        private IOrderableService<Category> orderableService;
    	private IHttpFileService fileService;
		private IRepository<Image> imageRepository;

    	[SetUp]
        public void SetUp()
        {
            categoryRepository = MockRepositoryBuilder.CreateCategoryRepository();
            orderableService = MockRepository.GenerateStub<IOrderableService<Category>>();
			fileService = MockRepository.GenerateStub<IHttpFileService>();
			imageRepository = MockRepository.GenerateStub<IRepository<Image>>();

            categoryController = new CategoryController(
                categoryRepository, 
                orderableService, 
				fileService,
				imageRepository
                );
        }

        [Test]
        public void Index_ShouldDisplayAListOfCategories()
        {
            categoryController.Index()
                .ReturnsViewResult()
                .ForView("Index")
                .WithModel<ShopViewData>()
                .AssertAreEqual("root", vd => vd.CategoryViewData.Name);
        }

        [Test]
        public void New_ShouldDisplayCategoryEditView()
        {
            var result = categoryController.New(1);
            AssertEditViewIsCorrectlyShown(result);
        }

        [Test]
        public void Edit_ShouldDisplayCategoryEditViewWithCorrectCategory()
        {
            const int categoryId = 3;

            var category = new Category
            {
                Id = categoryId,
                Name = "My Category",
                Parent = new Category { Id = 23 }
            };

            categoryRepository.Stub(cr => cr.GetById(categoryId)).Return(category);

            var result = categoryController.Edit(categoryId);
            AssertEditViewIsCorrectlyShown(result);
        }

    	[Test]
    	public void EditWithPost_should_redirect_when_binding_succeeded()
    	{
			fileService.Expect(x => x.GetUploadedImages(null, null)).IgnoreArguments().Return(new List<Image>());
    		var category = new Category();
			categoryController.Edit(category)
				.ReturnsRedirectToRouteResult()
				.ToAction("Index");
    	}

    	[Test]
    	public void EditWithPost_uploads_image()
    	{
			var image = new Image();
			fileService.Expect(x => x.GetUploadedImages(Arg<HttpRequestBase>.Is.Anything, Arg<string[]>.List.ContainsAll(new[] { ImageDefinition.CategoryImage }))).Return(new[] { image });

    		var category = new Category();
			categoryController.Edit(category);

			category.Image.ShouldBeTheSameAs(image);
    	}

    	[Test]
    	public void EditWithPost_should_render_view_with_error_when_binding_fails()
    	{
            fileService.Expect(x => x.GetUploadedImages(null, null)).IgnoreArguments().Return(new List<Image>());
            categoryController.ModelState.AddModelError("foo", "bar");

			var category = new Category();
			categoryController.Edit(category)
				.ReturnsViewResult()
				.WithModel<ShopViewData>()
				.AssertAreEqual(category, x => x.Category)
				.AssertNull(x => x.Message);
    	}

   
        private static void AssertEditViewIsCorrectlyShown(ActionResult result)
        {
            result
                .ReturnsViewResult()
                .ForView("Edit")
                .WithModel<ShopViewData>()
                .AssertNotNull(vd => vd.Category)
                .AssertNotNull(vd => vd.Categories)
                .AssertAreEqual(7, vd => vd.Categories.Count());
        }


		[Test]
		public void NewWithPost_should_insert_new_category()
		{
			const int categoryId = 0;
			const string name = "My Category";
			const int parentid = 78;

			var category = new Category 
			{
				Id = categoryId,
				Name = name,
                Parent = new Category { Id = parentid }
			};

			fileService.Expect(x => x.GetUploadedImages(null, null)).IgnoreArguments().Return(new List<Image>());

			categoryController.New(category)
				.ReturnsRedirectToRouteResult()
				.ToAction("Index");

			categoryRepository.AssertWasCalled(x => x.SaveOrUpdate(category));
			categoryController.Message.ShouldNotBeNull();
		}

    	[Test]
    	public void NewWithPost_loads_image()
    	{
			var image = new Image();
			fileService.Expect(x => x.GetUploadedImages(Arg<HttpRequestBase>.Is.Anything, Arg<string[]>.List.ContainsAll(new[] { ImageDefinition.CategoryImage }))).Return(new[] { image });
			var category = new Category();

			categoryController.New(category);

			category.Image.ShouldBeTheSameAs(image);
    	}

    	[Test]
    	public void NewWithPost_should_render_view_on_error()
    	{
            fileService.Expect(x => x.GetUploadedImages(null, null)).IgnoreArguments().Return(new List<Image>());
            categoryController.ModelState.AddModelError("foo", "bar");

			categoryController.New(new Category())
				.ReturnsViewResult()
				.ForView("Edit")
				.WithModel<ShopViewData>();

			categoryRepository.AssertWasNotCalled(x => x.SaveOrUpdate(Arg<Category>.Is.Anything));

    	}

    	[Test]
    	public void DeleteImage_deletes_image()
    	{
			var image = new Image();
    		imageRepository.Expect(x => x.GetById(5)).Return(image);

			categoryController.DeleteImage(1, 5)
				.ReturnsRedirectToRouteResult()
				.ToController("Category")
				.ToAction("Edit")
				.WithRouteValue("Id", "1");

			imageRepository.AssertWasCalled(x => x.DeleteOnSubmit(image));
    	}

        [Test]
        public void LeftMenu_should_return_tree_of_CategoryViewData()
        {
            var rootCategory = categoryController.LeftMenu()
                .ReturnsViewResult()
                .WithModel<CategoryViewData>();

            Assert.That(rootCategory.Name, Is.EqualTo("root"));
        }
    }
}
