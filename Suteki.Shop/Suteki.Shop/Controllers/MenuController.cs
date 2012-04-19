using System;
using System.Linq;
using System.Web.Mvc;
using MvcContrib;
using Suteki.Common.Binders;
using Suteki.Common.Extensions;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Shop.Filters;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Controllers
{
	public class MenuController : ControllerBase
	{
		private readonly IRepository<Menu> menuRepository;
		private readonly IOrderableService<Content> contentOrderableService;
	    private readonly IRepository<Content> contentRepository;

	    public MenuController(
            IRepository<Menu> menuRepository, 
            IOrderableService<Content> contentOrderableService, 
            IRepository<Content> contentRepository)
		{
			this.menuRepository = menuRepository;
			this.contentOrderableService = contentOrderableService;
		    this.contentRepository = contentRepository;
		}

        [UnitOfWork]
		public ActionResult MainMenu()
		{
		    var menu = menuRepository.GetById(1);
            return View(menu);
		}

		[AdministratorsOnly]
        [HttpGet, UnitOfWork]
        public ActionResult List(int id)
		{
			return View(CmsView.Data.WithContent(menuRepository.GetById(id)));
		}

		[AdministratorsOnly]
        [HttpGet, UnitOfWork]
        public ViewResult Edit(int id)
		{
            return View(CmsView.Data.WithContent(menuRepository.GetById(id)));
		}

		[AdministratorsOnly, AcceptVerbs(HttpVerbs.Post), UnitOfWork]
		public ActionResult Edit(Menu menu)
		{
            CheckForDuplicateName(menu);
			if (ModelState.IsValid)
			{
				Message = "Changes have been saved.";
				return this.RedirectToAction(c => c.List(menu.ParentContent.Id));
			}

            return View("Edit", CmsView.Data.WithContent(menu));
		}

		[AdministratorsOnly]
        [HttpGet, UnitOfWork]
        public ViewResult New(int id)
		{
			var parentMenu = menuRepository.GetById(id);

			if (parentMenu == null)
			{
				throw new ApplicationException("Content with id = {0} is not a menu".With(id));
			}

			var menu = Menu.CreateDefaultMenu(contentOrderableService.NextPosition, parentMenu);
            return View("Edit", CmsView.Data.WithContent(menu));
		}

		[AcceptVerbs(HttpVerbs.Post), AdministratorsOnly, UnitOfWork]
		public ActionResult New([EntityBind(Fetch = false)] Menu menu)
		{
		    CheckForDuplicateName(menu);
			if (ModelState.IsValid)
			{
				menuRepository.SaveOrUpdate(menu);
				Message = "New menu has been successfully added.";
                return this.RedirectToAction(c => c.List(menu.ParentContent.Id));
			}

            return View("Edit", CmsView.Data.WithContent(menu));
		}

        private void CheckForDuplicateName(Content content)
        {
            var contentWithNameAlreadyExists =
                contentRepository.GetAll().Any(x => x.Id != content.Id && x.UrlName == content.UrlName);

            if (contentWithNameAlreadyExists)
            {
                ModelState.AddModelError("Name",
                    string.Format("A menu or page with the name '{0}' already exists", content.Name));
            }
        }

	}
}