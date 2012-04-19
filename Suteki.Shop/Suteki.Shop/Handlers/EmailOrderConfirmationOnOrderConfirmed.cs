using System;
using Suteki.Common.Events;
using Suteki.Shop.Events;
using Suteki.Shop.Services;

namespace Suteki.Shop.Handlers
{
    public class EmailOrderConfirmationOnOrderConfirmed : IHandle<OrderConfirmed>
    {
        readonly IEmailService emailService;

        public EmailOrderConfirmationOnOrderConfirmed(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public void Handle(OrderConfirmed @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException("@event");
            }

            emailService.SendOrderConfirmation(@event.Order);            
        }
    }
}