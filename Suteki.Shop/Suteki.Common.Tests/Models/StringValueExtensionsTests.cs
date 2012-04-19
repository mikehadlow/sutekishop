using NUnit.Framework;
using Suteki.Common.Models;

namespace Suteki.Common.Tests.Models
{
    [TestFixture]
    public class StringValueExtensionsTests
    {
        [Test]
        public void ToStringValue_ShouldConvertIntEnumerableToStringValueEnumerable()
        {
            var ints = new[] {1, 2, 3, 4};
            var enumerator = ints.ToStringValues().GetEnumerator();

            enumerator.MoveNext();
            Assert.That(enumerator.Current.Value, Is.EqualTo("01"));
            enumerator.MoveNext();
            Assert.That(enumerator.Current.Value, Is.EqualTo("02"));
            enumerator.MoveNext();
            Assert.That(enumerator.Current.Value, Is.EqualTo("03"));
            enumerator.MoveNext();
            Assert.That(enumerator.Current.Value, Is.EqualTo("04"));
        }

        [Test]
        public void AddBlankFirstValue_ShouldAddABlankValueToBeginingOfList()
        {
            var values = new[]
                {
                    new StringValue("First"),
                    new StringValue("Second")
                };
            var enumerator = values.AddBlankFirstValue().GetEnumerator();

            enumerator.MoveNext();
            Assert.That(enumerator.Current.Value, Is.EqualTo("0"));
            enumerator.MoveNext();
            Assert.That(enumerator.Current.Value, Is.EqualTo("First"));
            enumerator.MoveNext();
            Assert.That(enumerator.Current.Value, Is.EqualTo("Second"));
        }
    }
}