using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.Services;
using Suteki.Shop.Services;

namespace Suteki.Shop.Tests.Services
{
	[TestFixture]
	public class OrderSearchServiceTests
	{
		IOrderSearchService service;
		List<Order> orders;
		IHttpContextService contextService;

		[SetUp]
		public void Setup()
		{
			orders = new List<Order>();
			var repository = MockRepository.GenerateStub<IRepository<Order>>();
			contextService = MockRepository.GenerateStub<IHttpContextService>();
			repository.Stub(x => x.GetAll()).Return(orders.AsQueryable());
			service = new OrderSearchService(repository, contextService);
		}

		[Test]
		public void ShouldBuildCriteriaAndExecuteSearch() {
			orders.Add(new Order { Id = 2, OrderStatus = OrderStatus.Created });
			orders.Add(new Order { Id = 3, OrderStatus = OrderStatus.Dispatched });

			var results = service.PerformSearch(new OrderSearchCriteria() { OrderId = 3 });
			
			results.Single().ShouldBeTheSameAs(orders[1]);
		}

		[Test]
		public void ShouldExcludePending()
		{
			orders.Add(new Order() { OrderStatus = new OrderStatus { Id = OrderStatus.PendingId } });
			orders.Add(new Order() { OrderStatus = OrderStatus.Created });
            orders.Add(new Order() { OrderStatus = OrderStatus.Created });

			var results = service.PerformSearch(new OrderSearchCriteria());
			results.Count.ShouldEqual(2);
		}

		//TODO: Test coverage here is lacking.
	}
}