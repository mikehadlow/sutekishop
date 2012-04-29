// ReSharper disable InconsistentNaming

using System;
using System.Linq;
using NUnit.Framework;
using Suteki.Common.Extensions;

namespace Suteki.Common.Tests.Extensions
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void Intersperse_should_intersperse_an_item_between_each_element_of_a_list()
        {
            var items = new[] {1, 2, 3, 4, 5}.Intersperse(0);

            var expectedItems = new[] {1, 0, 2, 0, 3, 0, 4, 0, 5};

            for (int i = 0; i < 9; i++)
            {
                items.ElementAt(i).ShouldEqual(expectedItems[i]);
            }
        }
    }
}

// ReSharper restore InconsistentNaming
