using System;
using System.Web.Mvc;
using NUnit.Framework;
using Suteki.Common.Extensions;
using Rhino.Mocks;

namespace Suteki.Common.Tests.Extensions
{
    [TestFixture]
    public class UrlExtensionsHelperTests
    {
        private UrlExtensionsHelper urlExtensionsHelper;
        private MockRepository mocks;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            urlExtensionsHelper = mocks.PartialMock<UrlExtensionsHelper>();
            urlExtensionsHelper.Stub(eh => eh.UseSsl()).Return(true).Repeat.Any();
        }

        [Test]
        public void ToSslUrl_ShouldAddHttpsToExistingUrlWithQueryString()
        {
            const string currentUrl = "http://jtg.sutekishop.co.uk/shop/Order/UpdateCountry/66?countryId=1";
            urlExtensionsHelper.Stub(eh => eh.GetRequestUri()).Return(new Uri(currentUrl));

            var existingUrl = MvcHtmlString.Create("/shop/Order/PlaceOrder");
            const string expectedUrl = "https://jtg.sutekishop.co.uk/shop/Order/PlaceOrder";

            mocks.ReplayAll();

            Assert.That(urlExtensionsHelper.ToSslUrl(existingUrl).ToString(), Is.EqualTo(expectedUrl));
        }

        [Test]
        public void ToSslUrl_ShouldAddHttpsToExistingUrlWithoutQueryString()
        {
            const string currentUrl = "http://jtg.sutekishop.co.uk/shop/Order/UpdateCountry/66";
            urlExtensionsHelper.Expect(eh => eh.GetRequestUri()).Return(new Uri(currentUrl));

            var existingUrl = MvcHtmlString.Create("/shop/Order/PlaceOrder");
            const string expectedUrl = "https://jtg.sutekishop.co.uk/shop/Order/PlaceOrder";

            mocks.ReplayAll();

            Assert.That(urlExtensionsHelper.ToSslUrl(existingUrl).ToString(), Is.EqualTo(expectedUrl));
        }

        [Test]
        public void ToSslUrl_ShouldAddHttpsToExistingUrlWithHttps()
        {
            const string currentUrl = "https://jtg.sutekishop.co.uk/shop/Order/UpdateCountry/66";
            urlExtensionsHelper.Expect(eh => eh.GetRequestUri()).Return(new Uri(currentUrl));

            var existingUrl = MvcHtmlString.Create("/shop/Order/PlaceOrder");
            const string expectedUrl = "https://jtg.sutekishop.co.uk/shop/Order/PlaceOrder";

            mocks.ReplayAll();

            Assert.That(urlExtensionsHelper.ToSslUrl(existingUrl).ToString(), Is.EqualTo(expectedUrl));
        }
    }
}