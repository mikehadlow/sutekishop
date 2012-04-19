// ReSharper disable InconsistentNaming
using System;
using System.Linq;
using NUnit.Framework;
using Suteki.Common.Models;

namespace Suteki.Shop.Tests.Models
{
    [TestFixture]
    public class OrderAdjustmentTests 
    {

        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void Should_be_able_to_add_an_adjustment_to_an_order()
        {
            var order = new Order();
            order.AddLine("widget 1", 1, new Money(12.34M), "abc", 102, "-");
            order.AddLine("gadget 1", 2, new Money(3.55M), "def", 101, "-");
            order.AddLine("gadget 1", 2, new Money(3.55M), "def", 101, "-");
            order.Postage = PostageResult.WithPrice(new Money(1.33M), "for London");

            var expectedAdjustedTotal = order.Total + new Money(-4.50M);

            var adjustment = new OrderAdjustment
            {
                Description = "4.50 off for slight scuffing",
                Amount = new Money(-4.50M)
            };

            order.AddAdjustment(adjustment);

            order.Adjustments[0].ShouldBeTheSameAs(adjustment);
            adjustment.Order.ShouldBeTheSameAs(order);
            order.Adjustments[0].Description.ShouldEqual("4.50 off for slight scuffing");

            order.Total.ShouldEqual(expectedAdjustedTotal);
        }

        [Test]
        public void Should_be_able_to_remove_adjustment()
        {
            var order = new Order();
            var adjustment = new OrderAdjustment();

            order.AddAdjustment(adjustment);
            order.RemoveAdjustment(adjustment);

            adjustment.Order.ShouldBeNull();
            order.Adjustments.ShouldBeEmpty();
        }
    }
}
// ReSharper restore InconsistentNaming