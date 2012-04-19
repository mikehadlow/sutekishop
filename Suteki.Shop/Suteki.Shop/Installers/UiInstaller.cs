using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Suteki.Shop.Exports.HtmlHelpers;
using Suteki.Shop.HtmlHelpers;

namespace Suteki.Shop.Installers
{
    public class UiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IStockControlUi>().ImplementedBy<StockControlUi>().LifeStyle.Transient
                );
        }
    }
}