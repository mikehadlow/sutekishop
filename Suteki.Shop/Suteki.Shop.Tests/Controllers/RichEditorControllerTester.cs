using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.Services;
using Suteki.Shop.Tests.Repositories;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Tests.Controllers
{
	[TestFixture]
	public class RichEditorControllerTester
	{
		RichEditorController controller;

		[SetUp]
		public void Setup()
		{
			controller = new RichEditorController(MockRepositoryBuilder.CreateProductRepository(), MockRepositoryBuilder.CreateCategoryRepository(), MockRepositoryBuilder.CreateContentRepository());
		}

		[Test]
		public void Links_should_return_view()
		{
			controller.Links()
				.ReturnsViewResult()
				.WithModel<ShopViewData>()
				.AssertNotNull(x => x.Products)
				.AssertNotNull(x => x.Categories)
				.AssertNotNull(x => x.Contents);
		}
	}
}