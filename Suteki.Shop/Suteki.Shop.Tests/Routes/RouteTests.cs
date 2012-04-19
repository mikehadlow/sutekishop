using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Extensions;
using Suteki.Shop.Routes;

namespace Suteki.Shop.Tests.Routes
{
    [TestFixture]
    public class RouteTests
    {
        RouteCollection Routes;

        [SetUp]
        public void SetUp()
        {
            Routes = new RouteCollection();
            RouteManager.RegisterRoutes(Routes);
        }

        [Test]
        public void RootShouldRouteToCms()
        {
            AssertRoutes(Routes, "~/", new { Controller = "Cms", Action = "Index", UrlName = "" });
        }

        [Test]
        public void DomainOnlyShouldRouteToHomeIndex()
        {
            AssertRoutes(Routes, "~/shop", new { Controller = "Home", Action = "Index", Id = "" });
        }

        [Test]
        public void ControllerOnlyShouldDefaultToIndex()
        {
            AssertRoutes(Routes, "~/shop/MyController", new { Controller = "MyController", Action = "Index" });
        }

        [Test]
        public void AnyControllerActionPairShouldRouteToCorrectControllerAction()
        {
            AssertRoutes(Routes, "~/shop/MyController/MyAction", new { Controller = "MyController", Action = "MyAction" });
        }

        [Test]
        public void IdNotSpecifiedShouldDefaultToEmptyString()
        {
            AssertRoutes(Routes, "~/shop/SomeController/SomeAction", new { Id = string.Empty });
        }

        [Test]
        public void IdSpecifiedShouldBecomeIdProperty()
        {
            AssertRoutes(Routes, "~/shop/SomeController/SomeAction/103", new { Id = "103" });
        }

        [Test]
        public void CmsUrlsShouldMapToCmsControllerIndexAction()
        {
            AssertRoutes(Routes, "~/cms/help", new { Controller = "Cms", Action = "Index", UrlName = "help" });
        }

        [Test]
        public void RootCmsShouldRouteToCmsIndex()
        {
            AssertRoutes(Routes, "~/cms", new { Controller = "Cms", Action = "Index", UrlName = "" });
        }

        /// <summary>
        /// Common method for asserting routes
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="relativeUrl"></param>
        /// <param name="propertyBag"></param>
        private static void AssertRoutes(RouteCollection routes, string relativeUrl, object propertyBag)
        {
            var testContext = new HttpContextTestContext();

            testContext.Request.Expect(request => request.AppRelativeCurrentExecutionFilePath)
                .Return(relativeUrl);

            testContext.Request.Expect(request => request.PathInfo).Return(string.Empty);

            var routeData = routes.GetRouteData(testContext.Context);

            foreach (var property in propertyBag.GetProperties())
            {
                Assert.AreEqual(property.Value, routeData.Values[property.Name]);
            }
        }
    }
}
