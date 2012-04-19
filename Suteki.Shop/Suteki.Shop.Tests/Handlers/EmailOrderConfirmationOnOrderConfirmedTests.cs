// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Shop.Events;
using Suteki.Shop.Handlers;
using Suteki.Shop.Services;

namespace Suteki.Shop.Tests.Handlers
{
    [TestFixture]
    public class EmailOrderConfirmationOnOrderConfirmedTests
    {
        EmailOrderConfirmationOnOrderConfirmed handler;
        IEmailService emailService;

        [SetUp]
        public void SetUp()
        {
            emailService = MockRepository.GenerateStub<IEmailService>();
            handler = new EmailOrderConfirmationOnOrderConfirmed(emailService);
        }

        [Test]
        public void Should_send_email_confirmation()
        {
            var order = new Order();
            handler.Handle(new OrderConfirmed(order));

            emailService.AssertWasCalled(s => s.SendOrderConfirmation(order));
        }
    }
}
// ReSharper restore InconsistentNaming