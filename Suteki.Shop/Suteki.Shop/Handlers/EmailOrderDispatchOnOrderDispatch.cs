using System;
using Suteki.Common.Events;
using Suteki.Shop.Events;
using Suteki.Shop.Services;

namespace Suteki.Shop.Handlers
{
    public class EmailOrderDispatchOnOrderDispatch : IHandle<OrderDispatched>
    {
        readonly IEmailService emailService;

        public EmailOrderDispatchOnOrderDispatch(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public void Handle(OrderDispatched orderDispatched)
        {
            if (orderDispatched == null)
            {
                throw new ArgumentNullException("orderDispatched");
            }
            emailService.SendDispatchNotification(orderDispatched.Order);
        }
    }
}