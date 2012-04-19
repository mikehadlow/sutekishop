// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Shop.Events;
using Suteki.Shop.Handlers;
using Suteki.Shop.Services;

namespace Suteki.Shop.Tests.Handlers
{
    [TestFixture]
    public class EmailOrderDispatchOnOrderDispatchTests
    {
        EmailOrderDispatchOnOrderDispatch handler;
        IEmailService emailService;

        [SetUp]
        public void SetUp()
        {
            emailService = MockRepository.GenerateStub<IEmailService>();
            handler = new EmailOrderDispatchOnOrderDispatch(emailService);
        }

        [Test]
        public void Should_send_dispatch_email()
        {
            var order = new Order();
            handler.Handle(new OrderDispatched(order));
            emailService.AssertWasCalled(e => e.SendDispatchNotification(order));
        }
    }
}
// ReSharper restore InconsistentNaming