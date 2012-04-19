using System;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Shop.ActionResults;

namespace Suteki.Shop.Tests.ActionResults
{
	[TestFixture]
	public class RedirectToReferrerTests
	{
		TestController controller;

		[SetUp]
		public void Setup()
		{
			controller = new TestController();
			new ControllerTestContext(controller);
			controller.ControllerContext.HttpContext.Request.Expect(x => x.UrlReferrer).Return(new Uri("http://foo/"));
		}

		[Test]
		public void Redirects_to_referrer()
		{
			var result = new RedirectToReferrerResult();
			result.ExecuteResult(controller.ControllerContext);

			controller.ControllerContext.HttpContext.Response.AssertWasCalled(x => x.Redirect("http://foo/", false));
		}
	}
}