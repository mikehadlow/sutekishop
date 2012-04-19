// ReSharper disable InconsistentNaming
using System;
using NUnit.Framework;
using Suteki.Common.Utils;

namespace Suteki.Common.Tests.Utils
{
    [TestFixture]
    public class ExpressionHelperTests
    {
        [Test]
        public void Should_be_able_to_get_property_name_from_expression()
        {
            var propertyName = ExpressionHelper.GetDottedPropertyNameFromExpression<Grandparent, string>(g => g.Parent.Child.Name);
            propertyName.ShouldEqual("Parent.Child.Name");
        }
    }

    public class Grandparent
    {
        public Parent Parent { get; set; }
    }

    public class Parent
    {
        public Child Child { get; set; }
    }

    public class Child
    {
        public string Name { get; set; }
    }
}
// ReSharper restore InconsistentNaming