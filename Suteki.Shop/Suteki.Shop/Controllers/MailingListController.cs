using System.Linq;
using System.Web.Mvc;
using MvcContrib.Pagination;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Binders;
using Suteki.Shop.Filters;
using Suteki.Shop.ViewData;
using MvcContrib;
namespace Suteki.Shop.Controllers
{
	public class MailingListController : ControllerBase
	{
	    readonly IRepository<Country> countryRepository;
	    readonly IRepository<MailingListSubscription> subscriptionRepository;

		public MailingListController(IRepository<Country> countryRepository, IRepository<MailingListSubscription> subscriptionRepository)
		{
			this.countryRepository = countryRepository;
			this.subscriptionRepository = subscriptionRepository;
		}

		public ActionResult Index()
		{
			return View(
				ShopView.Data.WithCountries(countryRepository.GetAll())
			);
		}

		[AcceptVerbs(HttpVerbs.Post), UnitOfWork]
		public ActionResult Index([BindMailingList] MailingListSubscription mailingListSubscription, bool? addAnother)
		{
			if(ModelState.IsValid)
			{
				subscriptionRepository.SaveOrUpdate(mailingListSubscription);
				return addAnother.GetValueOrDefault() ? 
                    this.RedirectToAction(c => c.Index()) : 
                    this.RedirectToAction(c => c.Confirm());
			}

			return View(
				ShopView.Data.WithCountries(countryRepository.GetAll())
				.WithSubscription(mailingListSubscription)
			);
		}

		public ActionResult Confirm()
		{
			return View();
		}

		[AdministratorsOnly, UnitOfWork]
		public ActionResult List(int? page)
		{
			var subscriptions = subscriptionRepository
				.GetAll()
				.OrderBy(x => x.Contact.Lastname)
				.AsPagination(page.GetValueOrDefault(1));
			return View(ShopView.Data.WithSubscriptions(subscriptions));
		}

        [AdministratorsOnly, UnitOfWork]
		public ActionResult Edit(int id)
		{
			var subscription = subscriptionRepository.GetById(id);
			var countries = countryRepository.GetAll();
			return View(ShopView.Data.WithSubscription(subscription).WithCountries(countries));
		}

		[AdministratorsOnly, AcceptVerbs(HttpVerbs.Post), UnitOfWork]
		public ActionResult Edit([BindMailingList(Fetch = true, ValidateConfirmEmail = false)] MailingListSubscription mailingListSubscription)
		{
			if(ModelState.IsValid)
			{
				Message = "Changed have been saved.";
				return this.RedirectToAction(c => c.List(null));
			}

			var countries = countryRepository.GetAll();

			return View(
				ShopView.Data.WithSubscription(mailingListSubscription).WithCountries(countries)
			);
		}

		[AdministratorsOnly, AcceptVerbs(HttpVerbs.Post), UnitOfWork]
		public ActionResult Delete(int id)
		{
			var subscription = subscriptionRepository.GetById(id);
			subscriptionRepository.DeleteOnSubmit(subscription);
			Message = "Subscription deleted.";

			return this.RedirectToAction(c => c.List(null));
		}
	}
}