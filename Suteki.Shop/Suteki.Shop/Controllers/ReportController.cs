using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Mapping;
using Suteki.Common.Extensions;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Filters;
using Suteki.Shop.Models;
using Suteki.Shop.Repositories;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Controllers
{
    [AdministratorsOnly]
    public class ReportController : ControllerBase
    {
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<MailingListSubscription> mailingListRepository;
        private readonly IRepository<Referer> refererRepository;

        public ReportController(
            IRepository<Order> orderRepository,
            IRepository<MailingListSubscription> mailingListRepository, 
            IRepository<Referer> refererRepository)
        {
            this.orderRepository = orderRepository;
            this.mailingListRepository = mailingListRepository;
            this.refererRepository = refererRepository;
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
            string mailingListEmails = string.Join(";",
                mailingListRepository.GetAll().Select(x => x.Email).Distinct().ToArray());
            return Content(mailingListEmails, "text/plain");
        }

        [UnitOfWork]
        [HttpGet]
        public ActionResult HowDidYouHearOfUs()
        {
            return HowDidYouHearOfUs(new HowDidYouHearOfUsViewModel
            {
                From = new DateTime(2014, 7, 1), // date collection started
                To = DateTime.Now
            });
        }

        [UnitOfWork]
        [HttpPost]
        public ActionResult HowDidYouHearOfUs(HowDidYouHearOfUsViewModel request)
        {
            if (this.ModelState.IsValid)
            {

                request.Lines = (
                    from r in refererRepository.GetAll()
                    orderby r.Position
                    select new HowDidYouHearOfUsLine
                    {
                        Option = r.Name,
                        Count = r.Orders.Count(
                            x => x.CreatedDate > request.From && x.CreatedDate < request.To
                            )
                    })
                    .ToList();

            }

            return View("HowDidYouHearOfUs", request);
        }
    }
}