// ReSharper disable InconsistentNaming
using System;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Shop.Events;
using Suteki.Shop.Handlers;

namespace Suteki.Shop.Tests.Handlers
{
    [TestFixture]
    public class AddToMailingListOnOrderConfirmedTests
    {
        AddToMailingListOnOrderConfirmed handler;
        IRepository<MailingListSubscription> mailingListRepository;

        [SetUp]
        public void SetUp()
        {
            mailingListRepository = MockRepository.GenerateStub<IRepository<MailingListSubscription>>();
            handler = new AddToMailingListOnOrderConfirmed(mailingListRepository);
        }

        [Test]
        public void Should_do_nothing_if_ContactMe_is_false()
        {
            var order = new Order
            {
                ContactMe = false
            };
            var @event = new OrderConfirmed(order);
            handler.Handle(@event);

            mailingListRepository.AssertWasNotCalled(r => r.SaveOrUpdate(Arg<MailingListSubscription>.Is.Anything));
        }

        [Test]
        public void Should_create_new_mailing_list_subscription()
        {
            MailingListSubscription mailingListSubscription = null;
            mailingListRepository.Stub(r => r.SaveOrUpdate(null)).IgnoreArguments()
                .Do((Action<MailingListSubscription>)(m => mailingListSubscription = m));

            var order = new Order
            {
                ContactMe = true,
                Email = "mike@mike.com",
                UseCardHolderContact = true,
                CardContact = new Contact()
            };

            handler.Handle(new OrderConfirmed(order));

            mailingListSubscription.Contact.ShouldBeTheSameAs(order.PostalContact);
            mailingListSubscription.Email.ShouldEqual("mike@mike.com");
            mailingListSubscription.DateSubscribed.Date.ShouldEqual(DateTime.Now.Date);
        }
    }
}
// ReSharper restore InconsistentNaming