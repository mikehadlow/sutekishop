using System;
using System.Linq;
using Suteki.Common.Repositories;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.Services
{
    public class BasketService : IBasketService
    {
        const string defaultCountryName = "United Kingdom";

        readonly IRepository<Country> countryRepository;
        readonly IUserService userService;

        public BasketService(IRepository<Country> countryRepository, IUserService userService)
        {
            this.countryRepository = countryRepository;
            this.userService = userService;
        }

        public Basket GetCurrentBasketForCurrentUser()
        {
            var user = userService.CurrentUser;
            return user.Baskets.Count == 0 ? 
                CreateNewBasketForCurrentUser() : 
                user.Baskets.CurrentBasket();
        }

        public Basket CreateNewBasketForCurrentUser()
        {
            var country = countryRepository.GetAll().SingleOrDefault(c => c.Name == defaultCountryName);
            if (country == null)
            {
                throw new ApplicationException(string.Format(
                    "The Default Country is missing from the database. " + 
                    "Expected to find a country with Name == '{0}'. (this name is coded in BasketService.defaultCountryName)",
                    defaultCountryName));
            }
            var basket = new Basket
            {
                OrderDate = DateTime.Now,
                Country = country
            };
            userService.CurrentUser.AddBasket(basket);
            return basket;
        }
    }
}