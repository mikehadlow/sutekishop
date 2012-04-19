using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NUnit.Framework;
using Suteki.Shop.Controllers;
using Suteki.Shop.IoC;
using Suteki.Shop.Services;
using ControllerBase=Suteki.Shop.Controllers.ControllerBase;

namespace Suteki.Shop.Tests
{
	[TestFixture]
	public class IoCTests
	{
		private IWindsorContainer container;

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			//Hackery in order to get the PerWebRequest lifecycle working in a test environment
			//Surely there must be a better way to do this?
			HttpContext.Current = new HttpContext(new HttpRequest("foo", "http://localhost", ""), new HttpResponse(new StringWriter()))
			{
			    ApplicationInstance = new HttpApplication()
			};
		    var module = new PerWebRequestLifestyleModule();
			module.Init(HttpContext.Current.ApplicationInstance);

			container = ContainerBuilder.Build("Windsor.config");
		    container.Install(FromAssembly.Containing<HomeController>());
		}

		[TestFixtureTearDown]
		public void TestFixtureTeardown()
		{
			HttpContext.Current = null;
			container.Dispose();
		}

		[Test]
		public void ShouldConstructControllers()
		{
			var controllerTypes = from type in typeof (HomeController).Assembly.GetExportedTypes()
			                      where typeof (Controller).IsAssignableFrom(type)
			                      where !type.IsAbstract
			                      select type;


			foreach(var type in controllerTypes)
			{
				var controller = (Controller) container.Resolve(type);
				var baseController = controller as ControllerBase;

				if(baseController != null)
				{
					baseController.BaseControllerService.ShouldNotBeNull();
				}
			}
		}

		[Test]
		public void Should_resolve_all_filters()
		{
			var filterTypes = from type in typeof(HomeController).Assembly.GetExportedTypes()
							  where typeof(IActionFilter).IsAssignableFrom(type)
							  || typeof(IResultFilter).IsAssignableFrom(type)
							  || typeof(IAuthorizationFilter).IsAssignableFrom(type)
							  where !type.IsAbstract
							  where !typeof(Attribute).IsAssignableFrom(type)
							  select type;

			foreach(var type in filterTypes)
			{
				container.Resolve(type);
			}
		}

		[Test]
		public void Resolves_ImageService()
		{
			container.Resolve<IImageService>().ShouldBe<ImageService>();
		}
	}
}