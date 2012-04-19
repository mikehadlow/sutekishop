using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.Tests.Repositories;
using System.Collections.Generic;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Tests.Controllers
{
	[TestFixture]
	public class MailingListControllerTests
	{
		MailingListController controller;
		IRepository<Country> countryRepository;
		IRepository<MailingListSubscription> mailingListRepository;

		[SetUp]
		public void Setup()
		{
			countryRepository = MockRepository.GenerateStub<IRepository<Country>>();
			mailingListRepository = MockRepository.GenerateStub<IRepository<MailingListSubscription>>();
			controller = new MailingListController(countryRepository, mailingListRepository);
		}

		[Test]
		public void Index_RendersViewWithCountries()
		{
			var countries = new List<Country>().AsQueryable();
			countryRepository.Expect(x => x.GetAll()).Return(countries);

			controller.Index()
				.ReturnsViewResult()
				.WithModel<ShopViewData>()
				.AssertAreSame(countries, x => x.Countries);
		}

		[Test]
		public void IndexWithPost_RedirectsOnSuccessfulBindingAndInsertsSubscription()
		{
			var subscription = new MailingListSubscription() { Email = "foo" };
			controller.Index(subscription, null)
				.ReturnsRedirectToRouteResult()
				.ToAction("Confirm");

				mailingListRepository.AssertWasCalled(x => x.SaveOrUpdate(subscription));
			
		}

		[Test]
		public void IndexWithPost_RendersViewOnFailedBinding()
		{
			controller.ModelState.AddModelError("foo", "bar");
			var subscription = new MailingListSubscription() { Email = "foo"};

			controller.Index(subscription, null)
				.ReturnsViewResult()
				.WithModel<ShopViewData>()
				.AssertAreEqual(subscription, x => x.MailingListSubscription);

		}

		[Test]
		public void IndexWithPost_RedirectsToIndexOnSuccessfulBindingWithAddAnother()
		{
			controller.Index(new MailingListSubscription(), true)
				.ReturnsRedirectToRouteResult()
				.ToAction("Index");
		}

		[Test]
		public void List_DisplaysAllSubscriptions()
		{
			var subscriptions = new[]
    		{
    			new MailingListSubscription
    			{
    				Email = "foo@bar.com", DateSubscribed = new DateTime(2008, 1, 1), Contact = new Contact() { Country = new Country(), }
    			},
				new MailingListSubscription
				{
					Email = "foo@bar.com", DateSubscribed = new DateTime(2009, 1, 2), Contact = new Contact() { Country = new Country()}
				},
				new MailingListSubscription
				{
					Email = "baz@blah.com", DateSubscribed = new DateTime(2007, 1, 1), Contact = new Contact() { Country = new Country()}
				}
    		};
			mailingListRepository.Expect(x => x.GetAll()).Return(subscriptions.AsQueryable());

			var model = controller.List(null)
				.ReturnsViewResult()
				.WithModel<ShopViewData>();

			model.MailingListSubscriptions.Count().ShouldEqual(3);
		}

		[Test]
		public void Edit_DisplaysSubscription()
		{
			var countries = new List<Country>().AsQueryable();
			countryRepository.Expect(x => x.GetAll()).Return(countries);

			var subscription = new MailingListSubscription();
			mailingListRepository.Expect(x => x.GetById(5)).Return(subscription);

			controller.Edit(5)
				.ReturnsViewResult()
				.WithModel<ShopViewData>()
				.AssertAreSame(countries, x => x.Countries)
				.AssertAreSame(subscription, x => x.MailingListSubscription);
		}

		[Test]
		public void EditWithPost_RedirectsOnSuccessfulBinding()
		{
			var subscription = new MailingListSubscription();
			controller.Edit(subscription)
				.ReturnsRedirectToRouteResult()
				.ToAction("List");
		}

		[Test]
		public void EditWithPost_RendersViewOnFailedBinding()
		{
			var subscription = new MailingListSubscription();
			controller.ModelState.AddModelError("foo", "bar");

				var countries = new List<Country>().AsQueryable();
			countryRepository.Expect(x => x.GetAll()).Return(countries);


			controller.Edit(subscription)
				.ReturnsViewResult()
				.WithModel<ShopViewData>()
				.AssertAreSame(countries, x => x.Countries)
				.AssertAreSame(subscription, x => x.MailingListSubscription);
		}

		[Test]
		public void Delete_DeletesSubscription()
		{
			var subscription = new MailingListSubscription();
			mailingListRepository.Expect(x => x.GetById(5)).Return(subscription);
			controller.Delete(5)
				.ReturnsRedirectToRouteResult()
				.ToAction("List");
			mailingListRepository.AssertWasCalled(x => x.DeleteOnSubmit(subscription));
		}
	}
}