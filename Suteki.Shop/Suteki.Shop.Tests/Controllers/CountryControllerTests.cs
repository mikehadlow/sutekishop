using System.Linq;
using NUnit.Framework;
using Suteki.Common.Repositories;
using Suteki.Common.ViewData;
using Suteki.Shop.Controllers;
using System.Collections.Generic;
using System.Threading;
using System.Security.Principal;
using Rhino.Mocks;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class CountryControllerTests
    {
        CountryController countryController;

        IRepository<Country> countryRepository;

        [SetUp]
        public void SetUp()
        {
            // you have to be an administrator to access the user controller
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin"), new string[] { "Administrator" });

            countryRepository = MockRepository.GenerateStub<IRepository<Country>>();

            countryController = new CountryController {Repository = countryRepository};
        }

        [Test]
        public void GetLookupLists_ShouldGetPostZones()
        {
            var repositoryResolver = MockRepository.GenerateStub<IRepositoryResolver>();
            countryController.RepositoryResolver = repositoryResolver;

            // create a list of post zones
            var postZones = new List<PostZone>
            {
                new PostZone()
            }.AsQueryable();

            // setup expectations
            var postZoneRepository = MockRepository.GenerateStub<IRepository>();
            repositoryResolver.Expect(k => k.GetRepository(typeof(PostZone))).Return(postZoneRepository);
            postZoneRepository.Expect(pzr => pzr.GetAll()).Return(postZones);

            // now exercise the method
            var viewData = new ScaffoldViewData<Country>();
            countryController.AppendLookupLists(viewData);

            Assert.AreSame(postZones, viewData.GetLookupList(typeof(PostZone)));
        }
    }
}
