using System;
using System.Collections.Generic;
using Suteki.Common.Extensions;
using Suteki.Common.Services;

namespace Suteki.Shop.Services
{
	public class EmailService : IEmailService
	{
		readonly IEmailBuilder builder;
		readonly IEmailSender sender;
		readonly IBaseControllerService baseService;
		const string OrderConfirmationTemplate = "OrderConfirmation";
		const string OrderDispatchTemplate = "OrderDispatch";

		public EmailService(IEmailBuilder builder, IEmailSender sender, IBaseControllerService service)
		{
			this.builder = builder;
			this.sender = sender;
			this.baseService = service;
		}

		public void SendOrderConfirmation(Order order)
		{
			var viewdata = new Dictionary<string, object>
			{
				{ "order", order }
			};

			var email = builder.GetEmailContent(OrderConfirmationTemplate, viewdata);
			var subject = "{0}: your order".With(baseService.ShopName);
			sender.Send(new[] { order.Email, baseService.EmailAddress }, subject, email, true);
		}

		public void SendDispatchNotification(Order order)
		{
			var viewdata = new Dictionary<string, object>
			{
				{ "order", order },
				{ "shopName", baseService.ShopName }
			};

			var email = builder.GetEmailContent(OrderDispatchTemplate, viewdata);
			var subject = "{0}: Your Order has Shipped".With(baseService.ShopName);
			sender.Send(order.Email, subject, email, true);
		}
	}

	public interface IEmailService
	{
		void SendOrderConfirmation(Order order);
		void SendDispatchNotification(Order order);
	}
}