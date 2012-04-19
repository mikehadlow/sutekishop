using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Suteki.Common.Models;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.Tests.Models;
using Rhino.Mocks;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class ReportControllerTests
    {
        ReportController reportController;
        IRepository<Order> orderRepository;
    	IRepository<MailingListSubscription> mailingListRepository;

    	[SetUp]
        public void SetUp()
        {
            orderRepository = MockRepository.GenerateStub<IRepository<Order>>();
			mailingListRepository = MockRepository.GenerateStub<IRepository<MailingListSubscription>>();
            reportController = new ReportController(orderRepository, mailingListRepository);
        }

        [Test]
        public void Index_ShouldShowIndexView()
        {
            reportController.Index()
                .ReturnsViewResult()
                .ForView("Index");
        }

        [Test]
        public void Orders_ShouldReturnACsvFileOfOrders()
        {
            var orders = new List<Order>
            {
                CreateOrder(),
                CreateOrder()
            }.AsQueryable();

            orderRepository.Expect(or => or.GetAll()).Return(orders);

            ContentResult result = reportController.Orders()
                .ReturnsContentResult();

            var returnedOrderCsvArray = result.Content.Split(',', '\n');
            var expectedOrderCsvArray = expectedOrdersCsv.Split(',', '\n');

            Assert.That(returnedOrderCsvArray.Length, Is.EqualTo(11), "CSV file not in expected format");
            Assert.That(returnedOrderCsvArray[0], Is.EqualTo(expectedOrderCsvArray[0]));
            Assert.That(returnedOrderCsvArray[1], Is.EqualTo(expectedOrderCsvArray[1]));
            Assert.That(returnedOrderCsvArray[2], Is.EqualTo(expectedOrderCsvArray[2]));

            // Date compare fails for different language setups.
            // Assert.That(returnedOrderCsvArray[3], Is.EqualTo(expectedOrderCsvArray[3]));
            
            Assert.That(returnedOrderCsvArray[4], Is.EqualTo(expectedOrderCsvArray[4]));

            Assert.That(returnedOrderCsvArray[5], Is.EqualTo(expectedOrderCsvArray[5]));
            Assert.That(returnedOrderCsvArray[6], Is.EqualTo(expectedOrderCsvArray[6]));
            Assert.That(returnedOrderCsvArray[7], Is.EqualTo(expectedOrderCsvArray[7]));

            // Date compare fails for different language setups.
            // Assert.That(returnedOrderCsvArray[8], Is.EqualTo(expectedOrderCsvArray[8]));
            
            Assert.That(returnedOrderCsvArray[9], Is.EqualTo(expectedOrderCsvArray[9]));

            Assert.That(result.ContentType, Is.EqualTo("text/csv"));
        }


        public static Order CreateOrder()
        {
            var order = new Order
            {
                UseCardHolderContact = true,
                CardContact = new Contact
                {
                    Country = new Country
                    {
                        PostZone = new PostZone { Multiplier = 2.5M, FlatRate = new Money(10.00M), AskIfMaxWeight = false }
                    }
                },
                Email = "mike@mike.com",
                CreatedDate = new DateTime(2008, 10, 18),
                OrderStatus = new OrderStatus { Name = "Dispatched" },
                OrderLines =
                    {
                        new OrderLine
                        {
                            Price = new Money(12.33M),
                            Quantity = 1
                        }
                    }
            };
            return order;
        }


    	[Test]
    	public void MailingListSubscriptions_ShouldReturnASscFileOfSubscriptions()
    	{
			var subscriptions = new[]
    		{
    			new MailingListSubscription
    			{
    				Contact = new Contact()
    				{
    					Firstname = "Firstname",
						Lastname = "Lastname",
						Address1 = "Address1",
						Address2 = "Address2",
						Address3 = "Address3",
						Town = "Town",
						County = "County",
						Postcode = "Postcode",
                        Telephone = "01234567891",
						Country = new Country() { Name = "UK" },
    				},
					Email = "foo@bar.com"
    			},
				new MailingListSubscription
				{
					Contact = new Contact
					{
						Firstname = "Firstname2",
						Lastname = "Lastname2",
						Address1 = "Address12",
						Address2 = "Address22",
						Address3 = "Address32",
						Town = "Town2",
						County = "County2",
						Postcode = "Postcode2",
                        Telephone = "01234567892",
						Country = new Country() { Name = "UK" },

					},
					Email = "bar@foo.com"
				}
    		}.AsQueryable();

			mailingListRepository.Expect(x => x.GetAll()).Return(subscriptions);

			var result = reportController
				.MailingListSubscriptions()
				.ReturnsContentResult()
				.Content.Split(new[] { ',', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

			var expected = expectedSubscriptionsCsv.Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

			for(int i = 0; i < 22; i++ )
			{
				result[i].ShouldEqual(expected[i]);
			}

    	}

    	[Test]
    	public void MailingListSubscriptions_IgnoresOldAddressForSameEmail()
    	{
			var subscriptions = new[]
    		{
    			new MailingListSubscription
    			{
    				Email = "foo@bar.com", DateSubscribed = new DateTime(2009, 1, 1), Contact = new Contact() { Country = new Country(), }
    			},
				new MailingListSubscription
				{
					Email = "foo@bar.com", DateSubscribed = new DateTime(2009, 1, 2), Contact = new Contact() { Country = new Country()}
				},
				new MailingListSubscription
				{
					Email = "baz@blah.com", DateSubscribed = new DateTime(2009, 1, 1), Contact = new Contact() { Country = new Country()}
				}
    		};

			mailingListRepository.Expect(x => x.GetAll()).Return(subscriptions.AsQueryable());

			var result = reportController.MailingListSubscriptions().ReturnsContentResult()
				.Content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

			result.Count().ShouldEqual(2);
    	}

    	[Test]
    	public void MailingListEmails_ShouldReturnEmailAddressesSemiColonSeparated()
    	{
			var subscriptions = new[]
    		{
    			new MailingListSubscription() { Email = "foo@bar.com"},
				new MailingListSubscription { Email = "bar@foo.com"}
    		};

			mailingListRepository.Expect(x => x.GetAll()).Return(subscriptions.AsQueryable());

			var result = reportController.MailingListEmails().ReturnsContentResult().Content;
			string expected = "foo@bar.com;bar@foo.com";
			result.ShouldEqual(expected);
    	}

		const string expectedSubscriptionsCsv = @"
""Firstname"",""Lastname"",""Address1"",""Address2"",""Address3"",""Town"",""County"",""Postcode"",""UK"",""01234567891"",""foo@bar.com""
""Firstname2"",""Lastname2"",""Address12"",""Address22"",""Address32"",""Town2"",""County2"",""Postcode2"",""UK"",""01234567892"",""bar@foo.com""
";

        const string expectedOrdersCsv =
@"0,""mike@mike.com"",""Dispatched"",""18/10/2008x 00:00:00"",12.33
0,""mike@mike.com"",""Dispatched"",""18/10/2008x 00:00:00"",12.33
";
    }
}
