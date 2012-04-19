using System;
using Suteki.Common.Events;
using Suteki.Common.Repositories;
using Suteki.Shop.Events;

namespace Suteki.Shop.Handlers
{
    public class AddToMailingListOnOrderConfirmed : IHandle<OrderConfirmed>
    {
        readonly IRepository<MailingListSubscription> mailingListRepository;

        public AddToMailingListOnOrderConfirmed(IRepository<MailingListSubscription> mailingListRepository)
        {
            this.mailingListRepository = mailingListRepository;
        }

        public void Handle(OrderConfirmed orderConfirmed)
        {
            if (orderConfirmed == null)
            {
                throw new ArgumentNullException("orderConfirmed");
            }

            var order = orderConfirmed.Order;
            if (!order.ContactMe) return;

            var mailingListSubscription = new MailingListSubscription
            {
                Contact = order.PostalContact,
                Email = order.Email,
                DateSubscribed = DateTime.Now
            };

            mailingListRepository.SaveOrUpdate(mailingListSubscription);
        }
    }
}