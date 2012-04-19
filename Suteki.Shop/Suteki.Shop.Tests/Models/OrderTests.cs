using System;
using NUnit.Framework;
using Suteki.Common.Events;
using Suteki.Common.Models;
using Suteki.Shop.Events;

// ReSharper disable InconsistentNaming

namespace Suteki.Shop.Tests.Models
{
    [TestFixture]
    public class OrderTests
    {
        [Test]
        public void UserAsString_should_return_user_email()
        {
            var order = new Order
            {
                ModifiedBy = new User
                {
                    Email = "mike@mike.com"
                }
            };

            order.UserAsString.ShouldEqual("mike@mike.com");
        }

        [Test]
        public void Total_should_return_sum_of_order_line_totals()
        {
            var price1 = new Money(3.40M);
            var price2 = new Money(6.23M);
            var price3 = new Money(10.44M);

            var order = new Order();
            order.AddLine("line1", 2, price1, "", 1, "");
            order.AddLine("line2", 1, price2, "", 1, "");
            order.AddLine("line3", 3, price3, "", 1, "");
            order.AddLine("line4", 2, price1, "", 1, "");

            var expectedTotal = (2*price1) + (1*price2) + (3*price3) + (2*price1);

            order.Total.ShouldEqual(expectedTotal);
        }

        [Test]
        public void TotalWithPostage_should_return_postageResult_postage()
        {
            var postageCost = new Money(5.66M);
            var itemPrice = new Money(101.43M);

            var order = new Order
            {
                Postage = PostageResult.WithPrice(postageCost, "postage desc")
            };
            order.AddLine("line1", 1, itemPrice, "", 1, "");

            var expectedTotalWithPostage = postageCost + itemPrice;

            order.TotalWithPostage.ShouldEqual(expectedTotalWithPostage.ToStringWithSymbol());
        }

        [Test]
        public void PostageDescription_should_return_Postage_Description()
        {
            var order = new Order
            {
                Postage = new PostageResult {Description = "some postage description"}
            };

            order.PostageDescription.ShouldEqual("some postage description");
        }

        [Test]
        public void Confirm_should_change_status_to_Created()
        {
            using (DomainEvent.TurnOff())
            {
                var order = new Order
                {
                    OrderStatus = OrderStatus.Pending
                };

                order.Confirm();

                order.IsCreated.ShouldBeTrue();
            }
        }

        [Test]
        public void Confirm_should_raise_OrderConfirmed_event()
        {
            OrderConfirmed orderConfirmed = null;
            using(DomainEvent.TestWith(e => orderConfirmed = e as OrderConfirmed))
            {
                var order = new Order {OrderStatus = OrderStatus.Pending};
                order.Confirm();

                orderConfirmed.ShouldNotBeNull();
                orderConfirmed.Order.ShouldBeTheSameAs(order);
            }
        }

        [Test]
        public void Dispatch_should_change_status_to_Dispatched()
        {
            using (DomainEvent.TurnOff())
            {
                var order = new Order
                {
                    OrderStatus = OrderStatus.Created
                };
                var user = new User();

                order.Dispatch(user);
                order.IsDispatched.ShouldBeTrue();
                order.DispatchedDate.Date.ShouldEqual(DateTime.Now.Date);
                order.ModifiedBy.ShouldBeTheSameAs(user);
            }
        }

        [Test]
        public void Dispatch_should_raise_OrderDispatched_event()
        {
            OrderDispatched orderDispatched = null;
            using (DomainEvent.TestWith(e => orderDispatched = e as OrderDispatched))
            {
                var order = new Order {OrderStatus = OrderStatus.Created};

                order.Dispatch(new User());

                orderDispatched.ShouldNotBeNull();
                orderDispatched.Order.ShouldBeTheSameAs(order);
            }
        }

        [Test]
        public void Reject_should_change_status_to_rejected()
        {
            var order = new Order {OrderStatus = OrderStatus.Created};
            var user = new User();

            order.Reject(user);

            order.IsRejected.ShouldBeTrue();
            order.ModifiedBy.ShouldBeTheSameAs(user);
        }

        [Test]
        public void UndoStatus_should_change_status_to_created()
        {
            var order = new Order
            {
                OrderStatus = OrderStatus.Dispatched,
                ModifiedBy = new User()
            };
            order.ResetStatus();
            order.IsCreated.ShouldBeTrue();
            order.ModifiedBy.ShouldBeNull();
        }
    }
}
// ReSharper restore InconsistentNaming
