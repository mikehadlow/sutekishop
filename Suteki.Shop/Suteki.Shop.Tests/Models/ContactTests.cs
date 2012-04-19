using NUnit.Framework;

namespace Suteki.Shop.Tests.Models
{
    [TestFixture]
    public class ContactTests
    {
        [Test]
        public void Lines_ShouldOnlyReturnLinesThatHaveAValue()
        {
            var contact = new Contact
            {
                Firstname = "John",
                Lastname = "Smith",
                Address1 = "34 Mape Road",
                Country = new Country { Name = "UK" }
            };

            var enumerator = contact.GetAddressLines().GetEnumerator();

            enumerator.MoveNext();
            Assert.AreEqual("John Smith", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("34 Mape Road", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("UK", enumerator.Current);
        }

        [Test]
        public void Lines_ShouldReturnAllLinesWhenContactIsFullyPopulated()
        {
            var contact = new Contact
            {
                Firstname = "John",
                Lastname = "Smith",
                Address1 = "Flat 6",
                Address2 = "Turner Building",
                Address3 = "Heath Road",
                Town = "Hove",
                County = "East Sussex",
                Country = new Country { Name = "UK" }
            };

            var enumerator = contact.GetAddressLines().GetEnumerator();

            enumerator.MoveNext();
            Assert.AreEqual("John Smith", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("Flat 6", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("Turner Building", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("Heath Road", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("Hove", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("East Sussex", enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual("UK", enumerator.Current);
        }
    }
}