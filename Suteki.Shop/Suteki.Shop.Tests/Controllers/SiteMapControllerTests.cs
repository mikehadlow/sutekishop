using System.Linq;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;
using Rhino.Mocks;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class SiteMapControllerTests
    {
        private SiteMapController siteMapController;
        private IRepository<Product> productRepository;
        private IRepository<Content> contentRepository;
        private IUserService userService;

        [SetUp]
        public void SetUp()
        {
            productRepository = MockRepository.GenerateStub<IRepository<Product>>();
            contentRepository = MockRepository.GenerateStub<IRepository<Content>>();
            userService = MockRepository.GenerateStub<IUserService>();

            siteMapController = new SiteMapController(productRepository, contentRepository, userService);

            userService.Expect(c => c.CurrentUser).Return(new User{ Role = Role.Guest });
        }

        [Test]
        public void Index_ShouldShowListOfProductsAndContent()
        {
            var products = new List<Product>().AsQueryable();
            var contents = new List<Content>().AsQueryable();

            productRepository.Expect(pr => pr.GetAll()).Return(products);
            contentRepository.Expect(cr => cr.GetAll()).Return(contents);

            siteMapController.Index()
                .ReturnsViewResult()
                .ForView("Index")
                .WithModel<ShopViewData>()
                .AssertNotNull(vd => vd.Products)
                .AssertNotNull(vd => vd.Contents);
        }
    }
}
