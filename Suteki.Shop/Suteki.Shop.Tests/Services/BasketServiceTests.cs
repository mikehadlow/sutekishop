using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Shop.Services;

// ReSharper disable InconsistentNaming
namespace Suteki.Shop.Tests.Services
{
    [TestFixture]
    public class BasketServiceTests
    {
        IBasketService basketService;
        IRepository<Country> countryRepository;
        IUserService userService;
        User user;

        [SetUp]
        public void SetUp()
        {
            countryRepository = MockRepository.GenerateStub<IRepository<Country>>();
            userService = MockRepository.GenerateStub<IUserService>();
            basketService = new BasketService(countryRepository, userService);

            user = new User();
            userService.Stub(u => u.CurrentUser).Return(user).Repeat.Any();
        }

        [Test]
        public void GetCurrentBasket_should_return_the_basket_collections_current_basket()
        {
            var basket = new Basket {Id = 201};
            var oldBasket = new Basket {Id = 200};

            user.AddBasket(basket);
            user.AddBasket(oldBasket);

            var currentBasket = basketService.GetCurrentBasketForCurrentUser();

            currentBasket.ShouldBeTheSameAs(basket);
        }

        [Test]
        public void GetCurrentBasket_should_return_a_new_basket_when_the_user_has_no_baskets()
        {
            var country = new Country { Name = "United Kingdom" }; // expect the default country to be United Kingdom.
            countryRepository.Stub(r => r.GetAll()).Return(new[] {country}.AsQueryable());

            var currentBasket = basketService.GetCurrentBasketForCurrentUser();

            currentBasket.Country.ShouldBeTheSameAs(country);
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void GetCurrentBasket_should_throw_when_the_default_country_is_not_in_the_database()
        {
            var country = new Country { Name = "France" }; // expect the default country to be UK.
            countryRepository.Stub(r => r.GetAll()).Return(new[] { country }.AsQueryable());

            basketService.GetCurrentBasketForCurrentUser();
        }
    }
    // ReSharper restore InconsistentNaming
}