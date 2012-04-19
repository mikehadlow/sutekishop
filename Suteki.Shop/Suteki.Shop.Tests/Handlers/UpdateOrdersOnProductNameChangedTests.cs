// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Suteki.Common.Repositories;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.Handlers;

namespace Suteki.Shop.Tests.Handlers
{
    [TestFixture]
    public class UpdateOrdersOnProductNameChangedTests
    {
        private UpdateOrdersOnProductNameChanged handler;
        private FakeRepository<OrderLine> orderLineRepository;

        [SetUp]
        public void SetUp()
        {
            orderLineRepository = new FakeRepository<OrderLine>();
            handler = new UpdateOrdersOnProductNameChanged(orderLineRepository);
        }

        [Test]
        public void Handler_should_update_orderLine_product_urlNames()
        {
            const string oldName = "Widget";
            const string newName = "Gadget";

            orderLineRepository.EntitesToReturnFromGetAll.Add(new OrderLine { ProductUrlName = oldName });
            orderLineRepository.EntitesToReturnFromGetAll.Add(new OrderLine { ProductUrlName = "some_other" });
            orderLineRepository.EntitesToReturnFromGetAll.Add(new OrderLine { ProductUrlName = oldName });

            var @event = new ProductNameChangedEvent(oldName, newName);
            handler.Handle(@event);

            orderLineRepository.EntitesToReturnFromGetAll[0].ProductUrlName.ShouldEqual("Gadget");
            orderLineRepository.EntitesToReturnFromGetAll[1].ProductUrlName.ShouldEqual("some_other");
            orderLineRepository.EntitesToReturnFromGetAll[2].ProductUrlName.ShouldEqual("Gadget");
        }
    }
}

// ReSharper restore InconsistentNaming
