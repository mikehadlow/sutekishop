using System.Collections;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.Tests.Repositories
{
    [TestFixture]
    public class UrlNamedExtensionsTests
    {
        private UrlNamedThing[] things;

        [SetUp]
        public void SetUp()
        {
            things = new[]
            {
                new UrlNamedThing { UrlName = "Thing1" },
                new UrlNamedThing { UrlName = "Thing2" },
                new UrlNamedThing { UrlName = "Thing3" }
            };
        }

        [Test]
        public void Should_find_item_with_urlName()
        {
            var thing = things.AsQueryable().WithUrlName("Thing2");
            Assert.That(thing, Is.SameAs(things[1]));
        }

        [Test, ExpectedException(typeof(UrlNameNotFoundException))]
        public void Should_throw_if_name_is_not_found()
        {
            var thing = things.AsQueryable().WithUrlName("Thing0");            
        }

        private class UrlNamedThing : IUrlNamed
        {
            public string UrlName { get; set;}
        }
    }
}