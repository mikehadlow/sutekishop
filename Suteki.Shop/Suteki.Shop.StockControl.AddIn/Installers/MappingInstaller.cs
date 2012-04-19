using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Suteki.Common.Repositories;
using Suteki.Shop.StockControl.AddIn.Repositories;

namespace Suteki.Shop.StockControl.AddIn.Installers
{
    public class MappingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IMappingConfigurationContributor>().ImplementedBy<StockControlMappingConfiguration>().LifeStyle.Transient
                );
        }
    }
}