// ReSharper disable InconsistentNaming
using System;
using Castle.Windsor;
using NUnit.Framework;
using Suteki.Shop.StockControl.AddIn.Installers;

namespace Suteki.Shop.StockControl.AddIn.Tests.Installers
{
    [TestFixture]
    public class ServiceInstallerTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void ServiceInstaller_should_register_Now()
        {
            var container = new WindsorContainer();
            var serviceInstaller = new ServiceInstaller();
            serviceInstaller.Install(container, null);

            var now = container.Resolve<Now>();

            now().Date.ShouldEqual(DateTime.Now.Date);
        }
    }
}

// ReSharper restore InconsistentNaming
