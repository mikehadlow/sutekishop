using System;
using System.Web.Mvc;
using NUnit.Framework;
using Suteki.Common.Filters;
using Suteki.Common.Tests;
using Suteki.Common.Windsor;

namespace Suteki.Shop.Tests.Filters
{
	[TestFixture]
	public class FilterUsingAttributeTester
	{
		[SetUp]
		public void Setup()
		{
            IocContainer.SetResolveFunction(Activator.CreateInstance);
		}

		[TearDown]
		public void Teardown()
		{
            IocContainer.Reset();
		}

		[Test]
		public void Should_store_filter_types()
		{
			var attribute = new FilterUsingAttribute(typeof (TestActionFilter));
			attribute.FilterType.ShouldEqual(typeof (TestActionFilter));
		}

		[Test]
		public void Should_throw_if_type_is_not_filter()
		{
			typeof (InvalidOperationException).ShouldBeThrownBy(() => new FilterUsingAttribute(typeof (IDisposable)));
		}

		[Test]
		public void Should_delegate_to_actionfilter_executing()
		{
			var attribute = new FilterUsingAttribute(typeof(TestActionFilter));
			var context = new ActionExecutingContext();
			attribute.OnActionExecuting(context);
			context.Result.ShouldBe<EmptyResult>();
		}

		[Test]
		public void Should_delegate_to_actionfilter_executed()
		{
			var attribute = new FilterUsingAttribute(typeof(TestActionFilter));
			var context = new ActionExecutedContext();
			attribute.OnActionExecuted(context);
			context.Result.ShouldBe<EmptyResult>();
		}

		[Test]
		public void Should_delegate_to_authorization_filter()
		{
			var attribute = new FilterUsingAttribute(typeof(TestAuthFilter));
			var context = new AuthorizationContext();
			attribute.OnAuthorization(context);
			context.Result.ShouldBe<EmptyResult>();
		}

		[Test]
		public void Should_delegate_to_result_filter_executing()
		{
			var attribute = new FilterUsingAttribute(typeof(TestResultFilter));
			var context = new ResultExecutingContext();
			attribute.OnResultExecuting(context);
			context.Result.ShouldBe<EmptyResult>();
		}

		[Test]
		public void Should_delegate_to_result_filter_executed()
		{
			var attribute = new FilterUsingAttribute(typeof(TestResultFilter));
			var context = new ResultExecutedContext();
			attribute.OnResultExecuted(context);
			context.Result.ShouldBe<EmptyResult>();
		}


		private class TestAuthFilter : IAuthorizationFilter
		{
			public void OnAuthorization(AuthorizationContext filterContext)
			{
				filterContext.Result = new EmptyResult();
			}
		}

		private class TestResultFilter : IResultFilter
		{
			public void OnResultExecuting(ResultExecutingContext filterContext)
			{
				filterContext.Result = new EmptyResult();
			}

			public void OnResultExecuted(ResultExecutedContext filterContext)
			{
				filterContext.Result = new EmptyResult(); 
			}
		}

		private class TestActionFilter : IActionFilter
		{
			public void OnActionExecuting(ActionExecutingContext filterContext)
			{
				filterContext.Result = new EmptyResult();
			}

			public void OnActionExecuted(ActionExecutedContext filterContext)
			{
				filterContext.Result = new EmptyResult();
			}
		}
	}
}