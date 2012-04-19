// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Models;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class OrderAdjustmentControllerTests
    {
        OrderAdjustmentController orderAdjustmentController;
        IRepository<Order> orderRepository;

        [SetUp]
        public void SetUp()
        {
            orderRepository = MockRepository.GenerateStub<IRepository<Order>>();
            orderAdjustmentController = new OrderAdjustmentController(orderRepository);    
        }

        [Test]
        public void Add_should_show_add_view()
        {
            var order = new Order();
            orderAdjustmentController.Add(order)
                .ReturnsViewResult()
                .ForView("Add")
                .WithModel<OrderAdjustment>()
                .AssertAreSame(order, vd => vd.Order);
        }

        [Test]
        public void AddAdjustment_POST_should_create_adjustment()
        {
            var order = new Order { Id = 66 };
            var adjustment = new OrderAdjustment
            {
                Order = order,
            };

            orderAdjustmentController.AddAdjustment(adjustment)
                .ReturnsRedirectToRouteResult()
                .ToAction("Item")
                .ToController("Order")
                .WithRouteValue("Id", "66");

            order.Adjustments[0].ShouldBeTheSameAs(adjustment);
            orderRepository.AssertWasCalled(r => r.SaveOrUpdate(order));
        }

        [Test]
        public void Delete_should_delete_adjustment()
        {
            var order = new Order { Id = 66 };
            var adjustment = new OrderAdjustment();
            order.AddAdjustment(adjustment);

            orderAdjustmentController.Delete(adjustment)
                .ReturnsRedirectToRouteResult()
                .ToAction("Item")
                .ToController("Order")
                .WithRouteValue("Id", "66");
            
            order.Adjustments.ShouldBeEmpty();
            orderRepository.AssertWasCalled(r => r.SaveOrUpdate(order));
        }
    }
}
// ReSharper restore InconsistentNaming