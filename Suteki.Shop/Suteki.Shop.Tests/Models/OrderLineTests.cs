// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Suteki.Common.Models;

namespace Suteki.Shop.Tests.Models
{
    [TestFixture]
    public class OrderLineTests
    {
        private Order order;

        [SetUp]
        public void SetUp()
        {
            order = new Order();
        }

        [Test]
        public void Should_be_able_to_add_an_OrderLine_to_an_Order()
        {
            const string itemName = "Large Black Mac";
            const int quantity = 4;
            var price = new Money(23.45M);
            const string urlName = "Large_Black_Mac";
            const int productId = 999;
            const string sizeName = "Large";

            order.AddLine(itemName, quantity, price, urlName, productId, sizeName);

            order.OrderLines[0].ProductName.ShouldEqual("Large Black Mac");
            order.OrderLines[0].Quantity.ShouldEqual(4);
            order.OrderLines[0].Price.Amount.ShouldEqual(23.45M);
            order.OrderLines[0].Order.ShouldBeTheSameAs(order);
            order.OrderLines[0].ProductUrlName.ShouldEqual(urlName);
            order.OrderLines[0].ProductId.ShouldEqual(productId);
            order.OrderLines[0].SizeName.ShouldEqual(sizeName);
        }
    }
}
// ReSharper restore InconsistentNaming