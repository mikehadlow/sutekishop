using System.Web.Mvc;
using NUnit.Framework;
using Suteki.Shop.Filters;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Tests.Filters
{
	[TestFixture]
	public class CopyMessageFromTempDataToViewDataTester
	{
		private TestController controller;
		private ActionExecutedContext context;
		private ShopViewData model;

		[SetUp]
		public void Setup()
		{
			controller = new TestController();
			context = new ActionExecutedContext() { Controller = controller };
			model = new ShopViewData();
		}

		[Test]
		public void Should_copy_message_from_tempdata_to_viewdata()
		{
			controller.TempData["message"] = "Foo";
			context.Result = new ViewResult() { ViewData = new ViewDataDictionary<ShopViewData>() { Model = model } };
			var filter = new CopyMessageFromTempDataToViewData();
			filter.OnActionExecuted(context);
			model.Message.ShouldEqual("Foo");
		}

		[Test]
		public void Should_not_copy_message_when_message_has_already_been_set()
		{
			model.Message = "bar";
			context.Result = new ViewResult() { ViewData = new ViewDataDictionary<ShopViewData>() { Model = model } };
			var filter = new CopyMessageFromTempDataToViewData();
			filter.OnActionExecuted(context);
			model.Message.ShouldEqual("bar");
		}
	}
}