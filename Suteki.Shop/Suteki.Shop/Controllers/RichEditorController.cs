using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Filters;
using Suteki.Shop.Repositories;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Controllers
{
	public class RichEditorController : ControllerBase
	{
		readonly IRepository<Product> productRepository;
		readonly IRepository<Category> categoryRepository;
		readonly IRepository<Content> contentRepository; 

		public RichEditorController(IRepository<Product> productRepository, IRepository<Category> categoryRepository, IRepository<Content> contentRepository)
		{
			this.productRepository = productRepository;
			this.contentRepository = contentRepository;
			this.categoryRepository = categoryRepository;
		}

		[OutputCache(Duration = 600, VaryByParam="x")]
		public ActionResult Links()
		{
			var products = productRepository.GetAll().Active().OrderBy(x => x.Name);
			var categories = categoryRepository.GetAll().Active().OrderBy(x => x.Name);
			var content = contentRepository.GetAll().Active().OrderBy(x => x.Name);

			return View(
				ShopView.Data
				.WithProducts(products)
				.WithCategories(categories)
				.WithContents(content)
			);
		}
	}
}