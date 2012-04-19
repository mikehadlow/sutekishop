using System;
using System.Reflection;
using NUnit.Framework;

namespace Suteki.Common.Tests.Spikes
{
    [TestFixture]
    public class ReflectionSpikes
    {
        [Test]
        public void DeclaringTypeIsNotInterfaceType()
        {
            var thing = new Thing();
            PropertyInfo propertyInfo = thing.GetType().GetProperty("Id");
            Assert.That(propertyInfo.DeclaringType, Is.Not.EqualTo(typeof(IThing)));
        }

        [Test]
        public void ReflectionOnAnonymousTypes()
        {
            var id = 4;
            var name = "Freddy";
            var now = DateTime.Now;

            var anonymous = new { id, name, now };

            var idValue = (int)anonymous.GetType().GetProperty("id").GetValue(anonymous, null);
            Assert.That(idValue, Is.EqualTo(id));
            var nameValue = (string)anonymous.GetType().GetProperty("name").GetValue(anonymous, null);
            Assert.That(nameValue, Is.EqualTo(name));
            var nowValue = (DateTime)anonymous.GetType().GetProperty("now").GetValue(anonymous, null);
            Assert.That(nowValue, Is.EqualTo(now));
        }

        public interface IThing
        {
            int Id { get; set; }
        }

        public class Thing : IThing
        {
            public int Id { get; set; }
        }
    }
}