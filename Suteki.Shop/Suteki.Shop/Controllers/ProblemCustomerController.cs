using System.Linq;
using System.Web.Mvc;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Filters;

namespace Suteki.Shop.Controllers
{
    [AdministratorsOnly]
    public class ProblemCustomerController : ControllerBase
    {
        private readonly IRepository<Order> orderRepository;

        public ProblemCustomerController(IRepository<Order> orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [ChildActionOnly, UnitOfWork]
        public ActionResult Index(Order order)
        {
            var customers = orderRepository.GetAll()
                .Where(x => 
                    x.Problem && 
                    x.Id != order.Id &&
                    (   
                        (x.CardContact.Firstname == order.CardContact.Firstname &&
                        x.CardContact.Lastname == order.CardContact.Lastname) ||
                        x.Email == order.Email ||
                        x.CardContact.Postcode == order.CardContact.Postcode
                    ));
            return View("Index", customers.ToList());
        }
    }
}