using MvcContrib;
using System.Web.Mvc;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Controllers
{
	public class CheckoutController : ControllerBase
	{
		readonly IRepository<Basket> basketRepository;
		readonly IUserService userService;
		readonly IRepository<Order> orderRepository;
		readonly IUnitOfWorkManager unitOfWork;
	    readonly ICheckoutService checkoutService;

		public CheckoutController(
            IRepository<Basket> basketRepository, 
            IUserService userService, 
            IRepository<Order> orderRepository, 
            IUnitOfWorkManager unitOfWork, 
            ICheckoutService checkoutService)
		{
			this.basketRepository = basketRepository;
		    this.checkoutService = checkoutService;
			this.unitOfWork = unitOfWork;
			this.orderRepository = orderRepository;
			this.userService = userService;
		}

        [HttpGet, UnitOfWork]
        public ActionResult Index(int id)
        {
            var basket = basketRepository.GetById(id);
            userService.CurrentUser.EnsureCanView(basket);

            var viewData = CurrentOrder ?? CreateCheckoutViewData(basket);
            return View("Index", viewData);
		}

		[HttpPost, UnitOfWork]
		public ActionResult Index(CheckoutViewData checkoutViewData)
		{
		    var order = checkoutService.OrderFromCheckoutViewData(checkoutViewData, ModelState);

			if (ModelState.IsValid)
			{
				orderRepository.SaveOrUpdate(order);
				//we need an explicit Commit in order to obtain the db-generated Order Id
				unitOfWork.Commit();
				return this.RedirectToAction(c => c.Confirm(order.Id));
			}

		    return View("Index", checkoutViewData);
		}

        [HttpGet, UnitOfWork]
		public ActionResult Confirm(int id)
		{
			var order = orderRepository.GetById(id);
			userService.CurrentUser.EnsureCanView(order);
			return View(ShopView.Data.WithOrder(order));
		}

		[HttpPost, UnitOfWork]
		public ActionResult Confirm(Order order)
		{
            userService.CurrentUser.EnsureCanView(order);
			order.Confirm();

			return this.RedirectToAction<OrderController>(c => c.Item(order.Id));
		}

        [HttpPost, UnitOfWork]
		public ActionResult UpdateCountry(CheckoutViewData checkoutViewData)
		{
			//Ignore any errors - if there are any errors in modelstate then the UnitOfWork will not commit.
			ModelState.Clear(); 

			var basket = basketRepository.GetById(checkoutViewData.BasketId);

		    var country = checkoutViewData.UseCardholderContact
		                      ? checkoutViewData.CardContactCountry
		                      : checkoutViewData.DeliveryContactCountry;
			
            basket.Country = country;
            CurrentOrder = checkoutViewData;
            return this.RedirectToAction(c => c.Index(checkoutViewData.BasketId));
		}

        private CheckoutViewData CurrentOrder
		{
            get { return TempData["CheckoutViewData"] as CheckoutViewData; }
            set { TempData["CheckoutViewData"] = value; }
		}

        [NonAction]
	    public CheckoutViewData CreateCheckoutViewData(Basket basket)
	    {
	        return new CheckoutViewData
	        {
	            BasketId = basket.Id,
	            CardCardType = CardType.VisaDeltaElectron,
	            CardContactCountry = basket.Country,
	            DeliveryContactCountry = basket.Country,
	            UseCardholderContact = true,
                Referer = new Referer { Id = 1, }
	        };
	    }
	}
}