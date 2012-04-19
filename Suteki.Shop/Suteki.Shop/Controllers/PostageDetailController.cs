using System.Web.Mvc;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Controllers
{
    public class PostageDetailController : Controller
    {
        readonly IRepository<Basket> basketRepository;
        readonly IPostageService postageService;

        public PostageDetailController(IRepository<Basket> basketRepository, IPostageService postageService)
        {
            this.basketRepository = basketRepository;
            this.postageService = postageService;
        }

        [HttpGet, UnitOfWork, ChildActionOnly]
        public ViewResult Index(int id)
        {
            return View("Index", BuildViewData(id));
        }

        PostageResultViewData BuildViewData(int id)
        {
            var basket = basketRepository.GetById(id);
            var postage = postageService.CalculatePostageFor(basket);

            return new PostageResultViewData
            {
                Description = postage.Description,
                PostageTotal = postage.Phone ? "Phone" : postage.Price.ToStringWithSymbol(),
                TotalWithPostage = postage.Phone ? "Phone" : (basket.Total + postage.Price).ToStringWithSymbol(),
                Country = basket.Country
            };
        }

        [UnitOfWork, ChildActionOnly]
        public ViewResult ReadOnly(int id)
        {
            return View("ReadOnly", BuildViewData(id));
        }
    }
}