using System;
using System.Globalization;
using System.IO;
using System.Threading;
using NUnit.Framework;
using Suteki.Common.Models;
using Suteki.Common.Services;
using Suteki.Shop.Services;
using Rhino.Mocks;

namespace Suteki.Shop.Tests.Services
{
    /// <summary>
    /// Test that the email templates render correctly.
    /// </summary>
    [TestFixture]
    public class EmailServiceTemplateTests
    {
        private IEmailService emailService;
        private MockEmailSender emailSender;

        private const string expectedOrderBodyPath = @"Templates\ExpectedOrderConfirmation.html";
        private const string expectedDispatchBodyPath = @"Templates\ExpectedDispatchNotification.html";

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-gb");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-gb");
		}

        [SetUp]
        public void SetUp()
        {
            var emailBuilder = new EmailBuilder(@"Templates");
            emailSender = new MockEmailSender();
            
            var baseControllerService = MockRepository.GenerateStub<IBaseControllerService>();
            baseControllerService.ShopName = "Suteki Shop";
            baseControllerService.EmailAddress = "info@sutekishop.co.uk";

            emailService = new EmailService(emailBuilder, emailSender, baseControllerService);
        }

        [Test]
        public void Should_render_order_confirmation_correctly()
        {
            var order = BuildOrder();
            emailService.SendOrderConfirmation(order);

            var renderedContents = emailSender.SentBody;
            var expectedContents = File.ReadAllText(expectedOrderBodyPath);

            //Console.WriteLine(renderedContents);
            renderedContents.ShouldEqual(expectedContents);
        }

        [Test]
        public void Should_render_dispatch_notification_correctly()
        {
            var order = BuildOrder();
            emailService.SendDispatchNotification(order);

            var renderedContents = emailSender.SentBody;
            var expectedContents = File.ReadAllText(expectedDispatchBodyPath);

            //Console.WriteLine(renderedContents);
            renderedContents.ShouldEqual(expectedContents);
        }

        private static Order BuildOrder()
        {
            var contact = new Contact
            {
                Firstname = "Joe",
                Lastname = "Blogs",
                Address1 = "4 The Street",
                Address2 = "Marlinspike",
                Address3 = "Near Arington",
                Town = "Brighton",
                County = "East Sussex",
                Postcode = "BN3 5SR",
                Country = new Country { Name = "UK" },
                Telephone = "01273 345678",
            };

            return new Order
            {
                Id = 98765,
                CreatedDate = new DateTime(2010, 8, 5),
                CardContact = contact,
                DeliveryContact = contact,
                Email = "joe@blogs.com",
                Card = new Card
                {
                    CardType = new CardType {Name = "Visa"},
                    Holder = "Joe Blogs"
                },
                AdditionalInformation = "some info",
                OrderLines =
                    {
                        new OrderLine
                        {
                            ProductName = "grey shirt - medium",
                            Quantity = 3,
                            Price = new Money(4.8M)
                        },
                        new OrderLine
                        {
                            ProductName = "blue trousers - small",
                            Quantity = 2,
                            Price = new Money(12.33M)
                        }
                    },
                Adjustments =
                    {
                        new OrderAdjustment
                        {
                            Description = "Reduction 'cos we're nice",
                            Amount = new Money(-3.40M)
                        }
                    },
                Postage = new PostageResult
                {
                    Phone = false,
                    Price = new Money(4.55M),
                    Description = "for United Kingdom"
                }
            };
        }
    }

    public class MockEmailSender : IEmailSender
    {
        public string SentBody { get; private set; }

        public void Send(string toAddress, string subject, string body)
        {
            SentBody = body;
        }

        public void Send(string[] toAddress, string subject, string body)
        {
            SentBody = body;
        }

        public void Send(string toAddress, string subject, string body, bool isBodyHtml)
        {
            SentBody = body;
        }

        public void Send(string[] toAddress, string subject, string body, bool isBodyHtml)
        {
            SentBody = body;
        }
    }
}