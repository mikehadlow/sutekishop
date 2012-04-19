using System.Web.Mvc;
using Suteki.Common.Binders;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Filters;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;
using Suteki.Shop.Repositories;
using MvcContrib;
namespace Suteki.Shop.Controllers
{
	[AdministratorsOnly]
    public class UserController : ControllerBase
    {
        readonly IRepository<User> userRepository;
        readonly IRepository<Role> roleRepository;
    	private readonly IUserService userService;

    	public UserController(IRepository<User> userRepository, IRepository<Role> roleRepository, IUserService userService)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        	this.userService = userService;
        }

        public ActionResult Index()
        {
            var users = userRepository.GetAll().Editable();
            return View("Index", ShopView.Data.WithUsers(users));
        }

        public ActionResult New()
        {
            return View("Edit", EditViewData.WithUser(Shop.User.DefaultUser));
        }

		[HttpPost, UnitOfWork]
		public ActionResult New(User user, string password)
		{
            if(!ModelState.IsValid)
            {
                return View("Edit", EditViewData.WithUser(user));
            }

			if(!string.IsNullOrEmpty(password))
			{
				user.Password = userService.HashPassword(password);
			}
			else
			{
			    ModelState.AddModelError("password", "Password is required");
                return View("Edit", EditViewData.WithUser(user));
			}

			userRepository.SaveOrUpdate(user);
			Message = "User has been added.";

			return this.RedirectToAction(c => c.Index());
		}

        public ActionResult Edit(int id)
        {
            var user = userRepository.GetById(id);
            return View("Edit", EditViewData.WithUser(user));
        }

        [HttpPost, UnitOfWork]
		public ActionResult Edit(User user, string password)
		{
            if (!ModelState.IsValid)
            {
                return View("Edit", EditViewData.WithUser(user));
            }

            if (!string.IsNullOrEmpty(password))
			{
				user.Password = userService.HashPassword(password);
			}

			return View("Edit", EditViewData.WithUser(user).WithMessage("Changes have been saved")); 
		}

        public ShopViewData EditViewData
        {
            get
            {
                return ShopView.Data.WithRoles(roleRepository.GetAll());
            }
        }
    }
}
