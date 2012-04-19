using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Events;
using Suteki.Common.TestHelpers;

namespace Suteki.Shop.Tests.Maps
{
    [TestFixture]
    public class BasketMapTests : MapTestBase
    {
        int sizeId;
        int basketId;

        [SetUp]
        public void SetUp()
        {
            var size = new Size
            {
                Name = "Medium",
                IsActive = true,
                IsInStock = true
            };

            var product = new Product
            {
                Name = "Widget",
                Description = "Our best Widget",
                
            };

            using (DomainEvent.TurnOff())
            {
                product.AddSize(size);
            }

            InSession(session => session.Save(product));

            sizeId = size.Id;

            var basket = new Basket();

            InSession(session => session.Save(basket));

            basketId = basket.Id;
        }

        [Test]
        public void Should_be_able_to_add_a_basket_item()
        {
            InSession(session =>
            {
                var size = session.Get<Size>(sizeId);

                var basketItem = new BasketItem
                {
                    Size = size,
                    Quantity = 1
                };
                
                var basket = session.Get<Basket>(basketId);
                
                basket.AddBasketItem(basketItem);
            });

            // check basket was added
            InSession(session =>
            {
                var basket = session.Get<Basket>(basketId);
                basket.BasketItems.Count.ShouldEqual(1);
            });

            // delete basket item
            InSession(session =>
            {
                var basket = session.Get<Basket>(basketId);
                var basketItem = basket.BasketItems[0];
                basket.BasketItems.Remove(basketItem);
            });

            // check basket item has been deleted
            InSession(session =>
            {
                var basket = session.Get<Basket>(basketId);
                basket.BasketItems.Count.ShouldEqual(0);
            });
        }
    }
}