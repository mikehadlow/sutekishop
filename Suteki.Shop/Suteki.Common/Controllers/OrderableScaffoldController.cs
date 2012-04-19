using System.Web.Mvc;
using MvcContrib.Pagination;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Common.ViewData;

namespace Suteki.Common.Controllers
{
    public class OrderableScaffoldController<T> : ScaffoldController<T> where T : class, IOrderable, new()
    {
        public IOrderableService<T> OrderableService { get; set; }

        protected override ActionResult RenderIndexView(int? page)
        {
            var items = Repository.GetAll().InOrder().AsPagination(page ?? 1);
            return View("Index", ScaffoldView.Data<T>().With(items));
        }

        public override ActionResult New()
        {
            T item = new T
            {
                Position = OrderableService.NextPosition
            };
            return View("Edit", (object)BuildEditViewData().With(item));
        }

		[UnitOfWork]
        public virtual ActionResult MoveUp(int id, int? page)
        {
            OrderableService.MoveItemAtPosition(id).UpOne();
			return RedirectToAction("Index");
        }
		[UnitOfWork]
        public virtual ActionResult MoveDown(int id, int? page)
        {
            OrderableService.MoveItemAtPosition(id).DownOne();
			return RedirectToAction("Index");
        }
    }
}