using System.Linq;
using System.Web.Mvc;
using Suteki.Common.Extensions;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Filters;
using Suteki.Shop.Models;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.Controllers
{
    [AdministratorsOnly]
    public class ReportController : ControllerBase
    {
        readonly IRepository<Order> orderRepository;
		readonly IRepository<MailingListSubscription> mailingListRepository;

        public ReportController(IRepository<Order> orderRepository, IRepository<MailingListSubscription> mailingListRepository)
        {
        	this.orderRepository = orderRepository;
        	this.mailingListRepository = mailingListRepository;
        }

    	public ActionResult Index()
        {
            return View("Index");
        }

        [UnitOfWork]
        public ActionResult Orders()
        {
            string ordersCsv = orderRepository.GetAll().ToList().Select(o => new 
            {
                OrderId = o.Id, 
                o.Email,
                OrderStatus = o.OrderStatus.Name, 
                o.CreatedDate, 
                o.Total.Amount
            }).AsCsv();

            return Content(ordersCsv, "text/csv");
        }

		[UnitOfWork]
		public ActionResult MailingListSubscriptions()
		{
            var mailingListCsv = mailingListRepository.GetAll().ToList().EnsureNoDuplicates().Select(x => new 
			{
				x.Contact.Firstname,
				x.Contact.Lastname,
				x.Contact.Address1,
				x.Contact.Address2,
				x.Contact.Address3,
				x.Contact.Town,
				x.Contact.County,
				x.Contact.Postcode,
				x.Contact.Country.Name,
				x.Contact.Telephone,
				x.Email
			}).AsCsv();

			return Content(mailingListCsv, "text/csv");
		}

        [UnitOfWork]
        public ActionResult MailingListEmails()
		{
			string mailingListEmails = string.Join(";", mailingListRepository.GetAll().Select(x => x.Email).Distinct().ToArray());
			return Content(mailingListEmails, "text/plain");
		}
    }
}
