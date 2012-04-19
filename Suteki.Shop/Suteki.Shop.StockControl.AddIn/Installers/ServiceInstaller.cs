using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Suteki.Shop.StockControl.AddIn.Installers
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Now>().Instance(() => DateTime.Now),
                Component.For<CurrentUser>().Instance(SimpleServices.CurrentUser)
                );
        }
    }
}