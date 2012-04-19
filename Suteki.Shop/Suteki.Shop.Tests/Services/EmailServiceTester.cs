using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Services;
using Suteki.Shop.Services;

namespace Suteki.Shop.Tests.Services
{
	[TestFixture]
	public class EmailServiceTester
	{
		EmailService service;
		IEmailBuilder builder;
		IEmailSender sender;
		IBaseControllerService baseControllerService;

		[SetUp]
		public void Setup()
		{
			builder = MockRepository.GenerateStub<IEmailBuilder>();
			sender = MockRepository.GenerateStub<IEmailSender>();
			baseControllerService = MockRepository.GenerateStub<IBaseControllerService>();
			baseControllerService.ShopName = "Suteki Shop";
			baseControllerService.EmailAddress = "info@sutekishop.co.uk";

			service = new EmailService(builder, sender, baseControllerService);
		}

		[Test]
		public void SendOrderConfirmation_sends_email()
		{
			const string content = "Email Content";
			var order = new Order() { Email = "test@email.address.com" };
			builder.Expect(x => x.GetEmailContent(Arg<string>.Is.Equal("OrderConfirmation"), Arg<IDictionary<string, object>>.Is.Anything)).Return(content);
			service.SendOrderConfirmation(order);
			
			sender.AssertWasCalled(x => x.Send(new[] { order.Email, baseControllerService.EmailAddress }, "Suteki Shop: your order", content, true));
		}

		[Test]
		public void SendOrderConfirmation_builds_viewdata()
		{
			IDictionary<string, object> viewData = null;
			var order = new Order();

			builder.Expect(x => x.GetEmailContent(null, null)).IgnoreArguments().Do(new Func<string, IDictionary<string, object>, string>((template, vd) => { viewData = vd; return ""; }));
			service.SendOrderConfirmation(order);
			
			viewData["order"].ShouldBeTheSameAs(order);
		}

		[Test]
		public void SendDispatchNotification_sends_email()
		{
			const string content = "Email Content";
			var order = new Order() { Email = "test@email.address.com" };
			builder.Expect(x => x.GetEmailContent(Arg<string>.Is.Equal("OrderDispatch"), Arg<IDictionary<string, object>>.Is.Anything)).Return(content);
			service.SendDispatchNotification(order);

			sender.AssertWasCalled(x => x.Send(order.Email, "Suteki Shop: Your Order has Shipped", content, true));
			
		}

		[Test]
		public void SendDispatchNotification_builds_viewdata()
		{
			IDictionary<string, object> viewData = null;
			var order = new Order();

			builder.Expect(x => x.GetEmailContent(null, null)).IgnoreArguments().Do(new Func<string, IDictionary<string, object>, string>((template, vd) => { viewData = vd; return ""; }));
			service.SendDispatchNotification(order);

			viewData["order"].ShouldBeTheSameAs(order);
			viewData["shopName"].ShouldEqual("Suteki Shop");
		}
	}

}