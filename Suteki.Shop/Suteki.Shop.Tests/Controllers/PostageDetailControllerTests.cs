// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Models;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.Services;
using Suteki.Shop.Tests.Models;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class PostageDetailControllerTests
    {
        PostageDetailController postageDetailController;
        IRepository<Basket> basketRepository;
        IPostageService postageService;

        [SetUp]
        public void SetUp()
        {
            basketRepository = MockRepository.GenerateStub<IRepository<Basket>>();
            postageService = MockRepository.GenerateStub<IPostageService>();

            postageDetailController = new PostageDetailController(basketRepository, postageService);
        }

        [Test]
        public void Index_should_return_postage_view()
        {
            const int basketId = 89;

            var basket = BasketTests.Create350GramBasket();

            basketRepository.Stub(b => b.GetById(basketId)).Return(basket);

            var postageResult = PostageResult.WithPrice(new Money(34.56M), "postage description");
            postageService.Stub(p => p.CalculatePostageFor(basket)).Return(postageResult);

            postageDetailController.Index(basketId)
                .ReturnsViewResult()
                .ForView("Index")
                .WithModel<PostageResultViewData>()
                .AssertAreEqual("£34.56", vd => vd.PostageTotal)
                .AssertAreEqual("postage description", vd => vd.Description)
                .AssertAreEqual("£125.58", vd => vd.TotalWithPostage)
                .AssertAreSame(basket.Country, vd => vd.Country);
        }
    }
}
// ReSharper restore InconsistentNaming