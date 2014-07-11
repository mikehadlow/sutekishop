using System.Linq;
using System.Web.Mvc;
using MvcContrib;
using Suteki.Common.Binders;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Shop.Binders;
using Suteki.Shop.Extensions;
using Suteki.Shop.Filters;
using Suteki.Shop.Repositories;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;
using Suteki.Shop.ViewDataMaps;

namespace Suteki.Shop.Controllers
{
	public class ProductController : ControllerBase
	{
		readonly IRepository<Product> productRepository;
		readonly IRepository<Category> categoryRepository;
		readonly IOrderableService<Product> productOrderableService;
		readonly IUserService userService;
		readonly IUnitOfWorkManager uow;
	    readonly IProductBuilder productBuilder;

		public ProductController(
            IRepository<Product> productRepository, 
            IRepository<Category> categoryRepository, 
            IOrderableService<Product> productOrderableService, 
            IUserService userService, 
            IUnitOfWorkManager uow,
            IProductBuilder productBuilder)
		{
			this.productRepository = productRepository;
			this.uow = uow;
			this.userService = userService;
			this.categoryRepository = categoryRepository;
			this.productOrderableService = productOrderableService;
		    this.productBuilder = productBuilder;
		}

		public override string GetControllerName()
		{
			return "";
		}

		public ActionResult Index(int id)
		{
			return RenderIndexView(id);
		}

        public ActionResult Category(string urlName)
        {
            return RenderIndexView(urlName);
        }

	    public ActionResult RootCategory()
	    {
	        return RenderIndexView(1);
	    }

        ActionResult RenderIndexView(string urlName)
        {
            var category = categoryRepository.GetAll().WithUrlName(urlName);

			AppendTitle(category.Name);

			var products = category.Products.InOrder();

			if (!userService.CurrentUser.IsAdministrator)
			{
				products = products.Active();
			}

			return View("Index", ShopView.Data.WithProducts(products).WithCategory(category));
		}

		ActionResult RenderIndexView(int id)
		{
			var category = categoryRepository.GetById(id);
            

			AppendTitle(category.Name);

			var products = category.Products.InOrder();

			if (!userService.CurrentUser.IsAdministrator)
			{
				products = products.Active();
			}

			return View("Index", ShopView.Data.WithProducts(products).WithCategory(category));
		}

		public ActionResult Item(string urlName)
		{
			return RenderItemView(urlName);
		}

		ActionResult RenderItemView(string urlName)
		{
		    try
		    {
		        var product = productRepository.GetAll().WithUrlName(urlName);

                if(!product.IsVisibleTo(userService.CurrentUser))
                {
                    return View("ItemNotAvailable");
                }

                AppendTitle(product.Name);
                AppendMetaDescription(product.PlainTextDescription);
                return View("Item", ShopView.Data.WithProduct(product));
            }
		    catch (UrlNameNotFoundException)
		    {
		        return View("NotFound");		        
		    }
		}

		[AdministratorsOnly]
		public ActionResult New(int id)
		{
		    var category = categoryRepository.GetById(id);
			var defaultProduct = Product.DefaultProduct(category, productOrderableService.NextPosition);
			return View("Edit", ProductViewDataMap.FromModel(defaultProduct));
		}

		[AdministratorsOnly, UnitOfWork, AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
		public ActionResult New(ProductViewData productViewData)
		{
		    var product = productBuilder.ProductFromProductViewData(productViewData, ModelState, Request);
		    if (ModelState.IsValid)
			{
				productRepository.SaveOrUpdate(product);
				uow.Commit(); //Need explicit commit in order to get the product id.
				return this.RedirectToAction(x => x.Edit(product.Id));
			}
            return View("Edit", productViewData.WithErrorMessage("There were errors, please correct them and resubmit."));
		}

	    [AdministratorsOnly]
		public ActionResult Edit(int id)
		{
			return RenderEditView(id);
		}

		[AcceptVerbs(HttpVerbs.Post), UnitOfWork, AdministratorsOnly, ValidateInput(false)]
        public ActionResult Edit(ProductViewData productViewData)
		{
            var product = productBuilder.ProductFromProductViewData(productViewData, ModelState, Request);
		    if (ModelState.IsValid)
			{
				return this.RedirectToAction(x => x.Edit(product.Id));
			}
            return View("Edit", productViewData.WithErrorMessage("There were errors, please correct them and resubmit."));
		}

	    ActionResult RenderEditView(int id)
		{
			var product = productRepository.GetById(id);
            return View("Edit", ProductViewDataMap.FromModel(product));
		}

        // TODO: Will constrained by work with property value?
		[AdministratorsOnly, UnitOfWork]
		public ActionResult MoveUp(int id, int position)
		{
			productOrderableService
				.MoveItemAtPosition(position)
				.ConstrainedBy(product => product.ProductCategories.Any(pc => pc.Category.Id == id))
				.UpOne();


			return this.RedirectToAction(x => x.Index(id));
		}

		[AdministratorsOnly, UnitOfWork]
		public ActionResult MoveDown(int id, int position)
		{
			productOrderableService
				.MoveItemAtPosition(position)
                .ConstrainedBy(product => product.ProductCategories.Any(pc => pc.Category.Id == id))
				.DownOne();

			return this.RedirectToAction(x => x.Index(id));
		}

		

		[AdministratorsOnly, UnitOfWork]
		public ActionResult ClearSizes(int id)
		{
			var product = productRepository.GetById(id);
            product.ClearAllSizes();
			Message = "Sizes have been cleared.";

			return this.RedirectToAction(c => c.Edit(id));
		}
	}
}