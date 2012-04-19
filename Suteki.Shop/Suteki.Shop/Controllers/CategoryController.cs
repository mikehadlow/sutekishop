using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Suteki.Common.Binders;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Common.Validation;
using Suteki.Shop.Filters;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;
using Suteki.Shop.Repositories;
using MvcContrib;

namespace Suteki.Shop.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Category> categoryRepository;
        private readonly IOrderableService<Category> orderableService;
		private readonly IHttpFileService httpFileService;
		private readonly IRepository<Image> imageRepository;

        public CategoryController(IRepository<Category> categoryRepository, IOrderableService<Category> orderableService, IHttpFileService httpFileService, IRepository<Image> imageRepository)
        {
            this.categoryRepository = categoryRepository;
        	this.imageRepository = imageRepository;
        	this.httpFileService = httpFileService;
        	this.orderableService = orderableService;
        }

        [AdministratorsOnly, HttpGet, UnitOfWork]
        public ActionResult Index()
        {
            var root = categoryRepository.GetAll().MapToViewData().GetRoot();
			return View("Index", ShopView.Data.WithCategoryViewData(root));
        }

        [UnitOfWork]
        public ActionResult LeftMenu()
        {
            var rootCategory = categoryRepository.GetAll().MapToViewData().GetRoot();
            return View(rootCategory);
        }

        [AdministratorsOnly, HttpGet, UnitOfWork]
        public ActionResult New(int id)
        {
            var parentCategory = categoryRepository.GetById(id);
            var defaultCategory = Category.DefaultCategory(parentCategory, orderableService.NextPosition);
            return View("Edit", EditViewData.WithCategory(defaultCategory)); 
        }

        [AdministratorsOnly, AcceptVerbs(HttpVerbs.Post), UnitOfWork]
		public ActionResult New([EntityBind(Fetch = false)] Category category)
		{
		    Image image = null;
            Validator.Validate(ModelState, () =>
			    image = httpFileService.GetUploadedImages(Request, ImageDefinition.CategoryImage).SingleOrDefault());

            if (string.IsNullOrWhiteSpace(category.UrlName))
                category.UrlName = GetUrlSafeCategoryName(category.Name);

            if (!ModelState.IsValid)
            {
                return View("Edit", EditViewData.WithCategory(category));
            }

            if (image != null)
			{
				category.Image = image;
			}

			categoryRepository.SaveOrUpdate(category);
			Message = "New category has been added.";

			return this.RedirectToAction(c => c.Index());
		}

        private string GetSafeUrlName(string name)
        {
            var replaced = Regex.Replace(name, @"[\W]+", "_");
            return replaced;
        }

        private string GetUrlSafeCategoryName(string name)
        {
            var safeName = GetSafeUrlName(name);
            var checkExistance = categoryRepository.GetAll().Where(x => x.UrlName == safeName);
            if (checkExistance.Count() > 0)
                return safeName += checkExistance.Count() + 1;
            return safeName;
        }

        [AdministratorsOnly, HttpGet, UnitOfWork]
        public ActionResult Edit(int id)
        {
            var category = categoryRepository.GetById(id);
            return View("Edit", EditViewData.WithCategory(category));
        }

        [AdministratorsOnly, AcceptVerbs(HttpVerbs.Post), UnitOfWork]
		public ActionResult Edit(Category category)
		{
			var viewData = EditViewData.WithCategory(category);

            Image image = null;
            Validator.Validate(ModelState, () =>
                image = httpFileService.GetUploadedImages(Request, ImageDefinition.CategoryImage).SingleOrDefault());

			if(ModelState.IsValid)
			{
				if (image != null) {
					category.Image = image;
				}

				Message = "The category has been saved.";
				return this.RedirectToAction(c => c.Index());
			}
			else
			{
				return View(viewData);					
			}
		}

        private ShopViewData EditViewData
        {
            get
            {
                return ShopView.Data.WithCategories(categoryRepository.GetAll().Alphabetical());
            }
        }

        [AdministratorsOnly, UnitOfWork]
        public ActionResult MoveUp(int id)
        {
            MoveThis(id).UpOne();
			return this.RedirectToAction(c => c.Index());
        }

        [AdministratorsOnly, UnitOfWork]
        public ActionResult MoveDown(int id)
        {
            MoveThis(id).DownOne();
			return this.RedirectToAction(c => c.Index());
        }

        [AdministratorsOnly, UnitOfWork]
		public ActionResult DeleteImage(int id, int imageId)
		{
			var category = categoryRepository.GetById(id);
			var productImage = imageRepository.GetById(imageId);
			category.Image = null;
			imageRepository.DeleteOnSubmit(productImage);

			Message = "Image deleted.";

			return this.RedirectToAction(c => c.Edit(id));
		}

        private IOrderServiceWithConstrainedPosition<Category> MoveThis(int id)
        {
            var category = categoryRepository.GetById(id);
            return orderableService
                .MoveItemAtPosition(category.Position)
                .ConstrainedBy(c => c.Parent.Id == category.Parent.Id);
        }
    }
}
