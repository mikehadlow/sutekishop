using NUnit.Framework;
using Suteki.Common.Extensions;
using Suteki.Common.Models;

namespace Suteki.Common.Tests.Extensions
{
    [TestFixture]
    public class TypeExtensionTests 
    {
        private class SomeEntity : IEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        private class SomeOtherEntity : IEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        [Test]
        public void Should_be_able_to_get_primary_key_of_SomeEntity()
        {
            var id = typeof (SomeEntity).GetPrimaryKey();
            id.Name.ShouldEqual("Id");
        }

        [Test]
        public void Should_be_able_to_get_primary_key_of_SomeOtherType()
        {
            var id = typeof (SomeOtherEntity).GetPrimaryKey();
            id.Name.ShouldEqual("Id");
        }
    }
}