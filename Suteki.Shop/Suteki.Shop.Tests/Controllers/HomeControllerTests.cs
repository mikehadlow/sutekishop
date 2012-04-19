using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.ViewData;
using Rhino.Mocks;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        HomeController homeController;
        IRepository<Content> contentRepository;

        [SetUp]
        public void SetUp()
        {
            contentRepository = MockRepository.GenerateStub<IRepository<Content>>();
            homeController = new HomeController(contentRepository);
        }

        [Test]
        public void IndexShouldRenderViewIndex()
        {
            var contents = new List<Content>
                {
                    new TextContent { UrlName = HomeController.Shopfront }
                }.AsQueryable();

            contentRepository.Expect(cr => cr.GetAll()).Return(contents);

            homeController.Index()
                .ReturnsViewResult()
                .ForView("Index")
                .WithModel<CmsViewData>()
                .AssertAreSame(
                    contents.OfType<ITextContent>().First(), 
                    vd => vd.TextContent);

        }
    }
}
