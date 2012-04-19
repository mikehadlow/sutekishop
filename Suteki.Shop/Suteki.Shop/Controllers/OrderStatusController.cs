using System;
using System.Web.Mvc;
using MvcContrib;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Filters;
using Suteki.Shop.Services;

namespace Suteki.Shop.Controllers
{
	[AdministratorsOnly]
	public class OrderStatusController : ControllerBase
	{
		readonly IUserService userService;

		public OrderStatusController(IUserService userService)
		{
			this.userService = userService;
		}

        [HttpPost, UnitOfWork]
	    public ActionResult Dispatch(Order order)
	    {
            if (order.IsCreated)
            {
                order.Dispatch(userService.CurrentUser);
            }

            return this.RedirectToAction<OrderController>(c => c.Item(order.Id));
        }

        [HttpPost, UnitOfWork]
        public ActionResult Reject(Order order)
	    {
            if (order.IsCreated)
            {
                order.Reject(userService.CurrentUser);
            }

            return this.RedirectToAction<OrderController>(c => c.Item(order.Id));
        }

        [HttpPost, UnitOfWork]
        public ActionResult UndoStatus(Order order)
	    {
            if (order.IsDispatched || order.IsRejected)
            {
                order.ResetStatus();
            }

            return this.RedirectToAction<OrderController>(c => c.Item(order.Id));
        }
	}
}