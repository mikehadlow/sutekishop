using System;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Services;
using Suteki.Shop.Filters;

namespace Suteki.Shop.Tests.Filters
{
	[TestFixture]
	public class EnsureSslFilterTester
	{
		AuthorizationContext context;
		EnsureSsl filter;
		IAppSettings appSettings;

		[SetUp]
		public void Setup()
		{
			var tcb = new TestControllerBuilder();
			context = new AuthorizationContext {HttpContext = tcb.HttpContext};
			appSettings = MockRepository.GenerateStub<IAppSettings>();
			filter = new EnsureSsl(appSettings);
		}

		[Test]
		public void Redirects_to_ssl_url_when_not_using_ssl()
		{
			appSettings.Expect(x => x.GetSetting(AppSettings.UseSsl)).Return("true");

			var uri = new Uri("http://www.sutekishop.co.uk");
			context.HttpContext.Request.Expect(x => x.Url).Return(uri).Repeat.Any();

			filter.OnAuthorization(context);

			context.Result.ShouldBe<RedirectResult>();
			context.Result.CastTo<RedirectResult>().Url.ShouldEqual("https://www.sutekishop.co.uk/");
		}

		[Test]
		public void Does_not_redirect_when_appSettings_disables_ssl()
		{
			var uri = new Uri("http://www.sutekishop.co.uk");
			context.HttpContext.Request.Expect(x => x.Url).Return(uri).Repeat.Any();
			filter.OnAuthorization(context);

			context.Result.ShouldBeNull();
		}

		[Test]
		public void Does_nothing_when_already_using_ssl()
		{
			appSettings.Expect(x => x.GetSetting(AppSettings.UseSsl)).Return("true");

			var uri = new Uri("https://www.sutekishop.co.uk");
			context.HttpContext.Request.Expect(x => x.Url).Return(uri).Repeat.Any();
			filter.OnAuthorization(context);
            
			context.Result.ShouldBeNull();
		}
	}
}