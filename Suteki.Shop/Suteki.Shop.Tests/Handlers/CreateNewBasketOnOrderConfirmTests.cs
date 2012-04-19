// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Shop.Events;
using Suteki.Shop.Handlers;
using Suteki.Shop.Services;

namespace Suteki.Shop.Tests.Handlers
{
    [TestFixture]
    public class CreateNewBasketOnOrderConfirmTests
    {
        CreateNewBasketOnOrderConfirm handler;
        IBasketService basketService;

        [SetUp]
        public void SetUp()
        {
            basketService = MockRepository.GenerateStub<IBasketService>();
            handler = new CreateNewBasketOnOrderConfirm(basketService);
        }

        [Test]
        public void Should_create_new_basket()
        {
            handler.Handle(new OrderConfirmed(new Order()));
            basketService.AssertWasCalled(s => s.CreateNewBasketForCurrentUser());
        }
    }
}
// ReSharper restore InconsistentNaming