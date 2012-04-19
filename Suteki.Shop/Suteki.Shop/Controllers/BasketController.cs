using System.Linq;
using System.Web.Mvc;
using Suteki.Common.Binders;
using Suteki.Common.Extensions;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Binders;
using Suteki.Shop.Filters;
using Suteki.Shop.Services;
using MvcContrib;
namespace Suteki.Shop.Controllers
{
    public class BasketController : ControllerBase
    {
        readonly IBasketService basketService;
        readonly IRepository<Basket> basketRepository;

    	public BasketController(
            IBasketService basketService, 
            IRepository<Basket> basketRepository)
        {
    	    this.basketService = basketService;
    	    this.basketRepository = basketRepository;
        }

		[FilterUsing(typeof(EnsureSsl))]
        public ActionResult Index()
        {
            return  View("Index", basketService.GetCurrentBasketForCurrentUser());
        }

        [UnitOfWork, AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Update([CurrentBasket] Basket basket, [EntityBind(Fetch = false)] BasketItem basketItem)
        {
            if (!basketItem.Size.IsInStock)
            {
                Message = RenderIndexViewWithError(basketItem.Size);
                return this.RedirectToAction<ProductController>(c => c.Item(basketItem.Size.Product.UrlName));
            }

            basket.AddBasketItem(basketItem);
			return this.RedirectToAction(c => c.Index());
        }

        [UnitOfWork, ChildActionOnly]
        public ActionResult Readonly(int id)
        {
            var basket = basketRepository.GetById(id);
            return View("Readonly", basket);
        }

        private static string RenderIndexViewWithError(Size size)
        {
        	if (size.Product.HasSize)
            {
                return "Sorry, {0}, Size {1} is out of stock.".With(size.Product.Name, size.Name);
            }

        	return "Sorry, {0} is out of stock.".With(size.Product.Name);
        }

		[HttpGet, UnitOfWork]
        public ActionResult Remove(int id)
        {
            var basket = basketService.GetCurrentBasketForCurrentUser();
            var basketItem = basket.BasketItems.Where(item => item.Id == id).SingleOrDefault();

            if (basketItem != null)
            {
                basket.RemoveBasketItem(basketItem);
            }

			return this.RedirectToAction(c => c.Index());
        }

		[AcceptVerbs(HttpVerbs.Post), UnitOfWork]
		public ActionResult UpdateCountry(Country country)
		{
			if (country.Id != 0) 
			{
				basketService.GetCurrentBasketForCurrentUser().Country = country;
			}

			return this.RedirectToAction(c => c.Index());
		}

		[AcceptVerbs(HttpVerbs.Post), UnitOfWork]
        public ActionResult GoToCheckout(Country country)
		{
		    var currentBasket = basketService.GetCurrentBasketForCurrentUser();
            if (country.Id != 0)
			{
                currentBasket.Country = country;
            }

            return this.RedirectToAction<CheckoutController>(c => c.Index(currentBasket.Id));
		}
    }
}
