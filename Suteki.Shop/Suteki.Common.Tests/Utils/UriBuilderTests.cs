// ReSharper disable InconsistentNaming
using System.Web.Routing;
using System.Web.Mvc;
using System.Web;
using NUnit.Framework;

namespace Suteki.Common.Tests.Utils
{
    [TestFixture]
    public class UriBuilderTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void CreateUriFromRouteValues_should_create_the_correct_virtual_path()
        {
            var routes = new RouteCollection();
            routes.MapRoute(
                "ProgRock",
                "{band}/{album}/{track}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            var uriBuilder = new Suteki.Common.Utils.UriBuilder(() => routes, () => new FakeHttpContext());

            var uri = uriBuilder.CreateUriFromRouteValues(new
            {
                band = "Yes",
                album = "Fragile",
                track = "Roundabout",
                info = "great keyboard solo"
            });

            uri.ShouldEqual("/Yes/Fragile/Roundabout?info=great%20keyboard%20solo");
        }        
    }

    public class FakeHttpContext : HttpContextBase
    {
        public override HttpRequestBase Request
        {
            get { return new FakeRequest(); }
        }

        public override HttpResponseBase Response
        {
            get { return new FakeResponse(); }
        }
    }

    public class FakeRequest : HttpRequestBase
    {
        public override string ApplicationPath
        {
            get { return "/"; }
        }
    }

    public class FakeResponse : HttpResponseBase
    {
        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }
    }
}

// ReSharper restore InconsistentNaming
