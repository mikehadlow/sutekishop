using System.Web.Mvc;
using Suteki.Common.Repositories;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.Controllers
{
    public class SiteMapController : ControllerBase
    {
        private readonly IRepository<Product> productRepository;
        private readonly IRepository<Content> contentRepository;
        private readonly IUserService userService;

        public SiteMapController(IRepository<Product> productRepository, IRepository<Content> contentRepository, IUserService userService)
        {
            this.productRepository = productRepository;
            this.userService = userService;
            this.contentRepository = contentRepository;
        }

        public ActionResult Index()
        {
            var products = productRepository.GetAll().ActiveFor(userService.CurrentUser);
            var contents = contentRepository.GetAll().WithAnyParent().ActiveFor(userService.CurrentUser);

            return View("Index", ShopView.Data
                .WithProducts(products)
                .WithContents(contents));
        }
    }
}
