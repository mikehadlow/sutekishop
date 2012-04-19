using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Suteki.Common.Extensions;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Filters;
using Suteki.Shop.ViewData;
using System.Collections.Specialized;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.Controllers
{
	[AdministratorsOnly]
    public class StockController : ControllerBase
    {
	    readonly IRepository<Category> categoryRepository;
	    readonly IRepository<Size> sizeRepository;
	    readonly IRepository<Product> productRepository;

        public StockController(
            IRepository<Category> categoryRepository,
            IRepository<Size> sizeRepository, 
            IRepository<Product> productRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
            this.sizeRepository = sizeRepository;
        }

        public ActionResult Index()
        {
            return RenderIndexView();
        }

        private ActionResult RenderIndexView()
        {
            Category root = categoryRepository.GetRootCategory();
            return View("Index", ShopView.Data.WithCategory(root));
        }

		[AcceptVerbs(HttpVerbs.Post), UnitOfWork]
        public ActionResult Index(FormCollection form)
        {
            var sizes = sizeRepository.GetAll().ToList();
            UpdateFromForm(sizes, form);
            return RenderIndexView();
        }

        private static void UpdateFromForm(IEnumerable<Size> sizes, NameValueCollection form)
        {
            foreach (var size in sizes)
            {
                if (form["stockitem_{0}".With(size.Id)] != null)
                {
                    size.IsInStock = form["stockitem_{0}".With(size.Id)].Contains("true");
                }
            }
        }

        [ChildActionOnly, UnitOfWork]
	    public ActionResult ProductStock(string id)
        {
            var product = productRepository.GetAll().WithUrlName(id);
            return View("ProductStock", product);
        }

        // nasty hack until I can get the ModelBinder to understand collections properly
        [HttpPost, UnitOfWork]
        public ActionResult ProductStockUpdate(FormCollection form)
        {
            int id;
            if(!int.TryParse(form["Id"], out id))
            {
                throw new ApplicationException("could not find 'Id' in post values");
            }

            var product = productRepository.GetById(id);
            for (var i = 0; i < product.Sizes.Count; i++)
            {
                var key = string.Format("Sizes[{0}].IsInStock", i);
                var value = form[key];
                if(string.IsNullOrEmpty(value)) continue;
                product.Sizes[i].IsInStock = GetValueAsBool(value);
            }

            return RedirectToAction("Item", "Product", new { urlName = product.UrlName });
        }

	    public static bool GetValueAsBool(string value)
	    {
	        return value.Split(',').Select(s => bool.Parse(s)).Aggregate(false, (a, b) => a | b);
	    }
    }
}
