using NUnit.Framework;
using Suteki.Common.Models;

namespace Suteki.Shop.Tests.Models
{
    [TestFixture]
    public class BasketTests
    {
        public static Basket Create350GramBasket()
        {
            return new Basket
            {
                Country = new Country
                {
                    PostZone = new PostZone
                    {
                        AskIfMaxWeight = false,
                        Multiplier = 2.5M,
                        FlatRate = new Money(10.00M)
                    }
                },
                BasketItems = new System.Collections.Generic.List<BasketItem>
                {
                    new BasketItem
                    {
                        Quantity = 10,
                        Size = new Size
                        {
                            Product = new Product {Weight = 10, Price = new Money(1.23M) }
                        }
                    },
                    new BasketItem
                    {
                        Quantity = 5,
                        Size = new Size
                        {
                            Product = new Product {Weight = 10, Price = new Money(5.88M) }
                        }
                    },
                    new BasketItem
                    {
                        Quantity = 4,
                        Size = new Size
                        {
                            Product = new Product {Weight = 50, Price = new Money(12.33M) }
                        }
                    }
                }
            };
        }

        public static Basket Create450GramBasket()
        {
            var basket = Create350GramBasket();
            basket.BasketItems.Add(new BasketItem
            {
                Quantity = 1,
                Size = new Size
                {
                    Product = new Product { Weight = 100, Price = new Money(0M) }
                }
            });
            return basket;
        }
    }
}
