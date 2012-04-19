using System.Web.Mvc;
using Suteki.Common.Extensions;
using Suteki.Common.Repositories;
using Suteki.Shop.Filters;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Controllers
{
	[AdministratorsOnly]
	public class InvoiceController : ControllerBase
	{
		readonly IRepository<Order> orderRepository;

		public InvoiceController(IRepository<Order> orderRepository)
		{
			this.orderRepository = orderRepository;
		}

		public ActionResult Show(int id)
		{
			var order = orderRepository.GetById(id);

			AppendTitle("Invoice {0}".With(order.Id));

			return View(ShopView.Data.WithOrder(order));
		}
	}
}