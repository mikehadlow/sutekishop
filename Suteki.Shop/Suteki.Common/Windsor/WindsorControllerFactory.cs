using System.Web.Mvc;
using Castle.Windsor;

namespace Suteki.Common.Windsor
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer container;

        public WindsorControllerFactory(IWindsorContainer container)
        {
            this.container = container;
        }

        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            var controllerComponentName = controllerName + "Controller";
            return container.Resolve<IController>(controllerComponentName);
        }

        public override void ReleaseController(IController controller)
        {
            container.Release(controller);
        }
    }
}