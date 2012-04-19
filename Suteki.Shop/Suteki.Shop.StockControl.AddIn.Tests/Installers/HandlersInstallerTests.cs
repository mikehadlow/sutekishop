// ReSharper disable InconsistentNaming
using System;
using Castle.Windsor;
using NUnit.Framework;
using Suteki.Common.Events;
using Suteki.Common.Windsor;
using Suteki.Shop.Exports.Events;
using Suteki.Shop.StockControl.AddIn.Installers;

namespace Suteki.Shop.StockControl.AddIn.Tests.Installers
{
    [TestFixture]
    public class HandlersInstallerTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void Install_should_install_all_handlers()
        {
            var installer = new HandlersInstaller();
            var container = new WindsorContainer();
            installer.Install(container, null);

//            var graphWriter = new DependencyGraphWriter(container, Console.Out);
//            graphWriter.Output();

            container.Kernel.HasComponent(typeof(IHandle<SizeCreatedEvent>)).ShouldBeTrue();
            container.Kernel.HasComponent(typeof(IHandle<SizesDeactivatedEvent>)).ShouldBeTrue();
            container.Kernel.HasComponent(typeof(IHandle<OrderDispatchedEvent>)).ShouldBeTrue();
        }
    }
}

// ReSharper restore InconsistentNaming
