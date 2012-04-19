using System;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Events;
using Suteki.Shop.Controllers;
using Suteki.Shop.Services;

namespace Suteki.Shop.Tests.Controllers
{
	[TestFixture]
	public class OrderStatusControllerTests
	{
		OrderStatusController controller;
		IUserService userService;

        [SetUp]
		public void Setup()
		{
			userService = MockRepository.GenerateStub<IUserService>();
			controller = new OrderStatusController(userService);

			userService.Expect(x => x.CurrentUser).Return(new User { Id = 4 });
		}


		[Test]
		public void Dispatch_ShouldChangeOrderStatusAndDispatchedDate()
		{
            using (DomainEvent.TurnOff())
            {
                var order = new Order
                {
                    OrderStatus = OrderStatus.Created
                };

                controller.Dispatch(order);

                order.IsDispatched.ShouldBeTrue();
            }
		}

	    [Test]
	    public void Reject_should_change_order_status_to_rejected()
	    {
	        var order = new Order {OrderStatus = OrderStatus.Created};
            controller.Reject(order);
            order.IsRejected.ShouldBeTrue();
	    }

		[Test]
		public void UndoStatus_ShouldChangeOrderStatusToCreated()
		{
			var order = new Order
			{
				OrderStatus = OrderStatus.Dispatched,
				DispatchedDate = DateTime.Now
			};

            controller.UndoStatus(order);
			order.IsCreated.ShouldBeTrue();
		}
	}
}