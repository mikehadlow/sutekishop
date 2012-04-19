using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Suteki.Shop.Exports.HtmlHelpers;
using Suteki.Shop.StockControl.AddIn.HtmlHelpers;

namespace Suteki.Shop.StockControl.AddIn.Installers
{
    public class UiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IStockControlUi>().ImplementedBy<StockControlUi>()
                );
        }
    }
}