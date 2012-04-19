using System;
using System.Linq;
using System.Web.Mvc;
using MvcContrib;
using Suteki.Common.Binders;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Shop.Filters;
using Suteki.Shop.Repositories;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Controllers
{
	[ValidateInput(false)] //Html must be allowed in the Save actions. 
	public class CmsController : ControllerBase
	{
		private readonly IRepository<Content> contentRepository;
		private readonly IRepository<Menu> menuRepository;
        private readonly IOrderableService<Content> contentOrderableService;

	    public CmsController(
            IRepository<Content> contentRepository,
            IRepository<Menu> menuRepository,
            IOrderableService<Content> contentOrderableService)
		{
			this.contentRepository = contentRepository;
	        this.menuRepository = menuRepository;
			this.contentOrderableService = contentOrderableService;
		}

		public override string GetControllerName()
		{
			return "";
		}

		//TODO: Possibly look at slimming down this action.
        [HttpGet, UnitOfWork]
        public ActionResult Index(string urlName)
		{
		    Content content;

		    try
		    {
		        content = string.IsNullOrEmpty(urlName)
		                      ? contentRepository.GetAll().DefaultText(null)
		                      : contentRepository.GetAll().WithUrlName(urlName);
		    }
		    catch (UrlNameNotFoundException)
		    {
		        return View("NotFound");
		    }

			if (content is Menu)
			{
				content = contentRepository.GetAll()
					.WithParent(content)
					.DefaultText(content as Menu);
			}

			if (content is ActionContent)
			{
				var actionContent = content as ActionContent;
				return RedirectToAction(actionContent.Action, actionContent.Controller);
			}

			AppendTitle(content.Name);

			if (content is TopContent)
			{
				return View("TopPage", CmsView.Data.WithContent(content));
			}

			return View("SubPage", CmsView.Data.WithContent(content));
		}

        // allow site root head requests
        [AcceptVerbs(HttpVerbs.Head)]
	    public ActionResult Index()
        {
            return Content("");
        }

		[AdministratorsOnly]
        [HttpGet, UnitOfWork]
        public ActionResult Add(int id)
		{
			var menu = menuRepository.GetById(id);
			var textContent = TextContent.DefaultTextContent(menu, contentOrderableService.NextPosition);
            return View("Edit", CmsView.Data.WithContent(textContent));
		}

		[AdministratorsOnly, HttpPost, UnitOfWork]
		public ActionResult Add([EntityBind(Fetch = false)] TextContent content)
		{
		    CheckForDuplicateName(content);
			if(ModelState.IsValid)
			{
				contentRepository.SaveOrUpdate(content);
				Message = "Changes have been saved.";
				return this.RedirectToAction<MenuController>(c => c.List(content.ParentContent.Id));
			}

            return View("Edit", CmsView.Data.WithContent(content));
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

	    [AdministratorsOnly]
        [HttpGet, UnitOfWork]
        public ActionResult EditText(int id)
		{
		    return EditContent(id);
		}

	    ActionResult EditContent(int id)
	    {
	        var content = contentRepository.GetById(id);
            return View("Edit", CmsView.Data.WithContent(content));
	    }

	    [AdministratorsOnly, UnitOfWork, AcceptVerbs(HttpVerbs.Post)]
		public ActionResult EditText(TextContent content)
		{
		    return EditContent(content, "Edit");
		}

        ActionResult EditContent(Content content, string errorView)
	    {
            CheckForDuplicateName(content);
	        if (ModelState.IsValid)
	        {
	            Message = "Changes have been saved.";
	            return this.RedirectToAction<MenuController>(c => c.List(content.ParentContent.Id));
	        }

	        //Error
            return View(errorView, CmsView.Data.WithContent(content));
	    }

		[AdministratorsOnly, UnitOfWork]
		public ActionResult MoveUp(int id)
		{
			var content = contentRepository.GetById(id);

			contentOrderableService
				.MoveItemAtPosition(content.Position)
                .ConstrainedBy(c => c.ParentContent.Id == content.ParentContent.Id)
				.UpOne();

            return this.RedirectToAction<MenuController>(c => c.List(content.ParentContent.Id));
		}

		[AdministratorsOnly, UnitOfWork]
		public ActionResult MoveDown(int id)
		{
			var content = contentRepository.GetById(id);

			contentOrderableService
				.MoveItemAtPosition(content.Position)
                .ConstrainedBy(c => c.ParentContent.Id == content.ParentContent.Id)
				.DownOne();

            return this.RedirectToAction<MenuController>(c => c.List(content.ParentContent.Id));
		}

        [AdministratorsOnly]
        [HttpGet, UnitOfWork]
        public ActionResult EditTop(int id)
        {
            return EditContent(id);
        }

        [AdministratorsOnly, UnitOfWork, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditTop(TopContent content)
	    {
            return EditContent(content, "Edit");
        }
	}
}