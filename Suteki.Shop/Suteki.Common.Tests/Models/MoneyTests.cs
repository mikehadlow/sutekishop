// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Suteki.Common.Models;

namespace Suteki.Common.Tests.Models
{
    [TestFixture]
    public class MoneyTests 
    {
        [Test]
        public void Should_be_able_to_add_money()
        {
            var result = new Money(2.32M) + new Money(3.44M);
            result.Amount.ShouldEqual(5.76M);
        }

        [Test]
        public void Should_be_able_to_do_other_arithmetic_with_money()
        {
            var x = new Money(43.5M);
            var result = x * 3 + x/2 - 4;
            result.Amount.ShouldEqual(148.25M);
        }

        [Test]
        public void ToString_should_not_include_currency_symbol()
        {
            new Money(342.11M).ToString().ShouldEqual("342.11");
        }

        [Test]
        public void ToStringWithSymbol_should_include_currency_symbol()
        {
            new Money(123.45M).ToStringWithSymbol().ShouldEqual("£123.45");
        }

        // ReSharper disable EqualExpressionComparison
        [Test]
        public void Equals_should_work()
        {
            var result1 = new Money(123.45M) == new Money(123.45M);
            result1.ShouldBeTrue();

            var result2 = new Money(123.45M) == null;
            result2.ShouldBeFalse();

            var result3 = null == new Money(123.45M);
            result3.ShouldBeFalse();

            var result4 = new Money(123.45M) == new Money(123.46M);
            result4.ShouldBeFalse();
        }
        // ReSharper restore EqualExpressionComparison
    }
}
// ReSharper restore InconsistentNaming