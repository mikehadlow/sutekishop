using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Suteki.Shop.StockControl.AddIn.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromThisAssembly()
                    .Where(Component.IsInNamespace("Suteki.Shop.StockControl.AddIn.Services"))
                    .WithService.DefaultInterface()
                    .Configure(x => x.LifeStyle.Transient)
                );
        }
    }
}