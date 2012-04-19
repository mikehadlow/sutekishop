using System;
using Suteki.Common.Extensions;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.Services
{
	public interface IOrderSearchService
	{
		PagedList<Order> PerformSearch(OrderSearchCriteria criteria);
	}
	public class OrderSearchService : IOrderSearchService
	{
		readonly IRepository<Order> orderRepository;
		readonly IHttpContextService httpContextService;

		public OrderSearchService(IRepository<Order> orderRepository, IHttpContextService httpContextService)
		{
			this.orderRepository = orderRepository;
			this.httpContextService = httpContextService;
		}


		public PagedList<Order> PerformSearch(OrderSearchCriteria criteria)
		{
			var orders = orderRepository
				.GetAll()
				.ThatMatch(criteria)
				.ByCreatedDate()
				.ToPagedList(httpContextService.FormOrQuerystring.PageNumber(), 20);

			return orders;
		}
	}
}