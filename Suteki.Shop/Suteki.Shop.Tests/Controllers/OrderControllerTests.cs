using System.Linq;
using System.Security.Principal;
using System.Threading;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Extensions;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.ViewData;
using System.Collections.Generic;
using System.Collections.Specialized;
using Suteki.Shop.Services;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class OrderControllerTests
    {
        private OrderController orderController;

        private IRepository<Order> orderRepository;
        private IRepository<Country> countryRepository;
        private IRepository<CardType> cardTypeRepository;

        private IEncryptionService encryptionService;
        private IUserService userService;
		IOrderSearchService searchService;

        private ControllerTestContext testContext;
    	IRepository<OrderStatus> statusRepository;

    	[SetUp]
        public void SetUp()
        {
            // you have to be an administrator to access the order controller
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin"), new[] { "Administrator" });

            orderRepository = MockRepository.GenerateStub<IRepository<Order>>();
            countryRepository = MockRepository.GenerateStub<IRepository<Country>>();
            cardTypeRepository = MockRepository.GenerateStub<IRepository<CardType>>();
			

            encryptionService = MockRepository.GenerateStub<IEncryptionService>();
            userService = MockRepository.GenerateStub<IUserService>();
			searchService = MockRepository.GenerateStub<IOrderSearchService>();

            var mocks = new MockRepository();
    		statusRepository = MockRepository.GenerateStub<IRepository<OrderStatus>>();
    		orderController = new OrderController(
                orderRepository,
                countryRepository,
                cardTypeRepository,
                encryptionService,
                userService,
				searchService,
				statusRepository
				);

            testContext = new ControllerTestContext(orderController);

            userService.Expect(us => us.CurrentUser).Return(new User { Id = 4, Role = Role.Administrator });

            testContext.TestContext.Context.User = new User { Id = 4 };
            testContext.TestContext.Request.RequestType = "GET";
            testContext.TestContext.Request.Stub(r => r.QueryString).Return(new NameValueCollection());
            testContext.TestContext.Request.Stub(r => r.Form).Return(new NameValueCollection());
			statusRepository.Expect(x => x.GetAll()).Return(new List<OrderStatus>().AsQueryable());

            mocks.ReplayAll();
        }



        [Test]
        public void Index_ShouldDisplayAListOfOrders()
        {
            var orders = new PagedList<Order>(new List<Order>(), 1, 1);
			searchService.Expect(x => x.PerformSearch(null)).IgnoreArguments().Return(orders);

			orderController.Index(null)
				.ReturnsViewResult()
				.ForView("Index")
				.WithModel<ShopViewData>()
				.AssertAreSame(orders, vd => vd.Orders)
				.AssertNotNull(x => x.OrderStatuses);
        }



        [Test]
        public void Index_ShouldBuildCriteriaAndExecuteSearch()
        {
			var criteria = new OrderSearchCriteria();
        	var results = new PagedList<Order>(new List<Order>(), 1, 1);
			searchService.Expect(x => x.PerformSearch(criteria)).Return(results);

			orderController.Index(criteria)
				.ReturnsViewResult()
				.ForView("Index")
				.WithModel<ShopViewData>()
				.AssertAreSame(criteria, vd => vd.OrderSearchCriteria)
				.AssertAreSame(results, vd => vd.Orders);

        }

        [Test]
        public void ShowCard_ShouldDecryptCardAndShowOrder()
        {
            const int orderId = 10;
            const string privateKey = "abcd";

            var order = new Order
            {
                Card = new Card
                {
                    CardType = new CardType{ Id = 1 },
                    Holder = "Jon Anderson",
                    IssueNumber = "",
                    StartMonth = 1,
                    StartYear = 2004,
                    ExpiryMonth = 3,
                    ExpiryYear = 2010
                }
            };
            order.Card.SetEncryptedNumber("asldfkjaslfjdslsdjkfjflkdjdlsakj");
            order.Card.SetEncryptedSecurityCode("asldkfjsadlfjdskjfdlkd");

            orderRepository.Stub(or => or.GetById(orderId)).Return(order);

            orderController.ShowCard(orderId, privateKey)
                .ReturnsViewResult()
                .ForView("Item")
                .WithModel<ShopViewData>()
                .AssertAreEqual(order.Card.Number, vd => vd.Card.Number)
                .AssertAreEqual(order.Card.ExpiryYear, vd => vd.Card.ExpiryYear);

            encryptionService.AssertWasCalled(es => es.DecryptCard(Arg<Card>.Is.Anything));
        }

    	[Test]
    	public void UpdateNote_should_redirect_back_to_item()
    	{
			orderController.UpdateNote(new Order { Id = 5 })
				.ReturnsRedirectToRouteResult()
				.ToAction("Item")
				.WithRouteValue("id", "5");

			orderController.Message.ShouldNotBeNull();
    	}
    }
}
