using System.Web.Mvc;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;
using MvcContrib;

namespace Suteki.Shop.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly IUserService userService;

        public LoginController(IUserService userService)
        {
        	this.userService = userService;
        }

        public ActionResult Index()
        {
            return View("Index", ShopView.Data);
        }

		[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(string email, string password, string returnUrl)
        {
			if (userService.Authenticate(email, password))
            {
                userService.SetAuthenticationCookie(email);

				if(! string.IsNullOrEmpty(returnUrl))
				{
					return Redirect(returnUrl);
				}

				return this.RedirectToAction<HomeController>(c => c.Index());
            }

            return View(ShopView.Data.WithErrorMessage("Unknown email or password"));
        }

        public ActionResult Logout()
        {
            userService.RemoveAuthenticationCookie();
			return this.RedirectToAction<HomeController>(c => c.Index());
        }
    }
}
