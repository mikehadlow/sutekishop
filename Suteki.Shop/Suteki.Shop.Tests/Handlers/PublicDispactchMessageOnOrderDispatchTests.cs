// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Suteki.Shop.Events;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.Handlers;
using Suteki.Common.Events;
using ShopOrder = Suteki.Shop.Order;
using ShopOrderLine = Suteki.Shop.OrderLine;

namespace Suteki.Shop.Tests.Handlers
{
    [TestFixture]
    public class PublicDispactchMessageOnOrderDispatchTests
    {
        private PublicDispactchMessageOnOrderDispatch dispactchMessageOnOrderDispatch;
        private DummyDomainEventService domainEventService;

        [SetUp]
        public void SetUp()
        {
            domainEventService = new DummyDomainEventService();
            dispactchMessageOnOrderDispatch = new PublicDispactchMessageOnOrderDispatch(domainEventService);
        }

        [Test]
        public void Raise_should_publish_OrderDispatchedEvent()
        {
            var shopOrder = new ShopOrder
            {
                Id = 123,
                OrderLines =
                    {
                        new ShopOrderLine
                        {
                            ProductUrlName = "ABC",
                            Quantity = 2,
                            SizeName = "Small"
                        },
                        new ShopOrderLine
                        {
                            ProductUrlName = "DEF",
                            Quantity = 1,
                            SizeName = "Large"
                        },
                        new ShopOrderLine
                        {
                            ProductUrlName = "HIJ",
                            Quantity = 10,
                            SizeName = "Huge"
                        },
                    }
            };

            var shopDispatchedEvent = new OrderDispatched(shopOrder);
            dispactchMessageOnOrderDispatch.Handle(shopDispatchedEvent);

            var externalDispatchedEvent = domainEventService.Event as OrderDispatchedEvent;
            externalDispatchedEvent.ShouldNotBeNull();
            externalDispatchedEvent.Order.OrderId.ShouldEqual(shopOrder.Id);

            externalDispatchedEvent.Order.Lines[0].ProductName.ShouldEqual(shopOrder.OrderLines[0].ProductUrlName);
            externalDispatchedEvent.Order.Lines[0].Quantity.ShouldEqual(shopOrder.OrderLines[0].Quantity);
            externalDispatchedEvent.Order.Lines[0].SizeName.ShouldEqual(shopOrder.OrderLines[0].SizeName);

            externalDispatchedEvent.Order.Lines[1].ProductName.ShouldEqual(shopOrder.OrderLines[1].ProductUrlName);
            externalDispatchedEvent.Order.Lines[1].Quantity.ShouldEqual(shopOrder.OrderLines[1].Quantity);
            externalDispatchedEvent.Order.Lines[1].SizeName.ShouldEqual(shopOrder.OrderLines[1].SizeName);

            externalDispatchedEvent.Order.Lines[2].ProductName.ShouldEqual(shopOrder.OrderLines[2].ProductUrlName);
            externalDispatchedEvent.Order.Lines[2].Quantity.ShouldEqual(shopOrder.OrderLines[2].Quantity);
            externalDispatchedEvent.Order.Lines[2].SizeName.ShouldEqual(shopOrder.OrderLines[2].SizeName);
        }
    }

    public class DummyDomainEventService : IDomainEventService
    {
        public IDomainEvent Event { get; private set; }

        public void Raise<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            Event = @event;
        }
    }
}

// ReSharper restore InconsistentNaming
