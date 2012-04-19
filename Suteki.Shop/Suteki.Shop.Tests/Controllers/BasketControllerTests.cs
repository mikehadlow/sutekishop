using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.Services;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class BasketControllerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            userService = MockRepository.GenerateStub<IUserService>();
            countryRepository = MockRepository.GenerateStub<IRepository<Country>>();
            basketRepository = MockRepository.GenerateStub<IRepository<Basket>>();

            basketService = new BasketService(countryRepository, userService);

            basketController = new BasketController(
                basketService,
                basketRepository);

            testContext = new ControllerTestContext(basketController);

			user = new User { Baskets = { new Basket { Id = 4, Country = new Country() } } };
			userService.Expect(x => x.CurrentUser).Return(user);
        }

        #endregion

        private User user;

        private BasketController basketController;
        private ControllerTestContext testContext;

        private IUserService userService;
        IBasketService basketService;
        private IRepository<Country> countryRepository;
        private IRepository<Basket> basketRepository;

        private static BasketItem CreateBasketItem()
        {
            var product = new Product { Name = "Denim Jacket", Weight = 10 };
            var size = new Size
            {
                Id = 5,
                Name = "S",
                IsInStock = true,
                IsActive = true,
                Product = product
            };
            product.Sizes.Add(size);

			return new BasketItem { Size = size, Quantity = 2 };
        }

        [Test]
        public void Index_ShouldShowIndexViewWithCurrentBasket()
        {
			basketController.Index()
				.ReturnsViewResult()
				.ForView("Index")
				.WithModel<Basket>()
				.AssertAreSame(user.Baskets[0], vd => vd);
        }

    	[Test]
    	public void GoToCheckout_UpdatesCountry()
    	{
    	    var country = new Country{ Id = 5 };

            basketController.GoToCheckout(country);
            basketService.GetCurrentBasketForCurrentUser().Country.ShouldBeTheSameAs(country);
    	}

    	[Test]
    	public void GoToCheckout_RedirectsToCheckout()
    	{
            basketController.GoToCheckout(new Country { Id = 5 })
				.ReturnsRedirectToRouteResult()
				.ToController("Checkout")
				.ToAction("Index")
                .WithRouteValue("id", basketService.GetCurrentBasketForCurrentUser().Id.ToString());
    	}

    	[Test]
    	public void UpdateCountry_UpdatesCountry()
    	{
            var country = new Country { Id = 5 };
            basketController.UpdateCountry(country);
            basketService.GetCurrentBasketForCurrentUser().Country.ShouldBeTheSameAs(country);
    	}

    	[Test]
    	public void UpdateCountry_RedirectsToIndex()
    	{
            basketController.UpdateCountry(new Country { Id = 5 }).ReturnsRedirectToRouteResult().ToAction("Index");
    	}

    	[Test]
        public void Remove_ShouldRemoveItemFromBasket()
        {
            const int basketItemIdToRemove = 3;

            var basketItem = new BasketItem
            {
                Id = basketItemIdToRemove,
                Quantity = 1,
                Size = new Size
                {
                    Product = new Product {Weight = 100}
                }
            };
            user.Baskets[0].BasketItems.Add(basketItem);
            testContext.TestContext.Context.User = user;

            basketController.Remove(basketItemIdToRemove)
				.ReturnsRedirectToRouteResult()
				.ToAction("Index");

    	    user.Baskets[0].BasketItems.Count.ShouldEqual(0);
        }

        [Test]
        public void Update_ShouldAddBasketLineToCurrentBasket()
        {
            var basketItem = CreateBasketItem();
            var basket = new Basket();

            basketController.Update(basket, basketItem);

            Assert.AreEqual(1, basket.BasketItems.Count, "expected BasketItem is missing");
            Assert.AreEqual(5, basket.BasketItems[0].Size.Id);
            Assert.AreEqual(2, basket.BasketItems[0].Quantity);
        }

        [Test]
        public void Update_ShouldShowErrorMessageIfItemIsOutOfStock()
        {
            var basketItem = CreateBasketItem();
            basketItem.Size.IsInStock = false;

            const string expectedMessage = "Sorry, Denim Jacket, Size S is out of stock.";

            basketController.Update(basketService.GetCurrentBasketForCurrentUser(), basketItem)
				.ReturnsRedirectToRouteResult()
				.ToController("Product")
				.ToAction("Item")
                .WithRouteValue("urlName", basketItem.Size.Product.UrlName);

			basketController.Message.ShouldEqual(expectedMessage);

            Assert.AreEqual(0, user.Baskets[0].BasketItems.Count, "should not be any basket items");
        }

        [Test]
        public void Readonly_should_return_basket_with_postage()
        {
            var basket = new Basket();
            basketRepository.Stub(b => b.GetById(4)).Return(basket);

            basketController.Readonly(4)
                .ReturnsViewResult()
                .ForView("Readonly")
                .WithModel<Basket>()
                .AssertAreSame(basket, vdBasket => vdBasket);
        }
    }
}