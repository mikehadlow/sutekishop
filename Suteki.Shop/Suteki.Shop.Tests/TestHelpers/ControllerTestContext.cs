using System.Web.Mvc;
using System.Web.Routing;

namespace Suteki.Shop.Tests
{
    public class ControllerTestContext
    {
        public MockViewEngine ViewEngine { get; private set; }
        public HttpContextTestContext TestContext { get; private set; }

        public ControllerTestContext(ControllerBase controller)
        {
            ViewEngine = new MockViewEngine();

            TestContext = new HttpContextTestContext();

            var controllerContext = new ControllerContext(
                new RequestContext(TestContext.Context, new RouteData()),
                controller);

            controller.ControllerContext = controllerContext;
        }
    }
}
