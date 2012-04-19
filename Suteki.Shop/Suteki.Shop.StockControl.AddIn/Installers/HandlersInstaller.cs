using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Suteki.Common.Events;

namespace Suteki.Shop.StockControl.AddIn.Installers
{
    public class HandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromThisAssembly()
                    .BasedOn(typeof (IHandle<>)).WithService.FromInterface()
                    .Configure(c => c.LifeStyle.Transient)
                );
        }
    }
}