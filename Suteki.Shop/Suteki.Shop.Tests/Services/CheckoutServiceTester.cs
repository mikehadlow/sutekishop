using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Models;
using Suteki.Common.Repositories;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;

// ReSharper disable InconsistentNaming
namespace Suteki.Shop.Tests.Services
{
	[TestFixture]
	public class CheckoutServiceTester
	{
	    ICheckoutService checkoutService;
        IRepository<Basket> basketRepository;
	    IEncryptionService encryptionService;
        IPostageService postageService;
	    IUserService userService;

	    CheckoutViewData checkoutViewData;
	    Basket basket;
	    User user;

	    [SetUp]
	    public void SetUp()
	    {
            basketRepository = MockRepository.GenerateStub<IRepository<Basket>>();
            encryptionService = MockRepository.GenerateStub<IEncryptionService>();
	        postageService = MockRepository.GenerateStub<IPostageService>();
	        userService = MockRepository.GenerateStub<IUserService>();

            checkoutService = new CheckoutService(basketRepository, encryptionService, postageService, userService);

            checkoutViewData = GetCheckoutViewData();
            basket = CreateBasketWithId(7);
            basketRepository.Stub(r => r.GetById(7)).Return(basket);

            user = new User { Role = Role.Administrator };
	        userService.Stub(u => u.CurrentUser).Return(user);
	    }

	    [Test]
	    public void OrderFromCheckoutViewData_should_create_the_correct_order()
	    {
            var order = checkoutService.OrderFromCheckoutViewData(checkoutViewData, new ModelStateDictionary());

            VerifyOrderMatchesCheckoutViewData(order, checkoutViewData);
	    }

	    [Test]
	    public void OrderFromCheckoutViewdata_should_create_the_correct_orderLines()
	    {
            var order = checkoutService.OrderFromCheckoutViewData(checkoutViewData, new ModelStateDictionary());

            order.OrderLines.Count.ShouldEqual(basket.BasketItems.Count,
                "Number of OrderLines does not match number of BasketItems");

            foreach (var basketItem in basket.BasketItems)
            {
                var productName = basketItem.Size.Product.Name + " - " + basketItem.Size.Name;
                var orderLine = order.OrderLines
                    .SingleOrDefault(line => line.ProductName == productName);

                if (orderLine == null)
                {
                    Assert.Fail(string.Format("No OrderLine with name {0} found", productName));
                }

                orderLine.Price.ShouldEqual(basketItem.Size.Product.Price);
                orderLine.Quantity.ShouldEqual(basketItem.Quantity);
                orderLine.Total.ShouldEqual(basketItem.Total);
                orderLine.ProductUrlName.ShouldEqual(basketItem.Size.Product.UrlName);
                orderLine.ProductId.ShouldEqual(basketItem.Size.Product.Id);
                orderLine.SizeName.ShouldEqual(basketItem.Size.Name);
            }
        }

	    [Test]
	    public void OrderFromCheckoutViewData_should_calculate_the_correct_postage()
	    {
	        var postage = new PostageResult();

	        postageService.Stub(x => x.CalculatePostageFor(basket)).Return(postage);

            var order = checkoutService.OrderFromCheckoutViewData(checkoutViewData, new ModelStateDictionary());

	        order.Postage.ShouldBeTheSameAs(postage);
	    }

	    [Test]
	    public void OrderFromCheckoutViewData_should_not_insert_errors_in_ModelState()
	    {
	        var modelState = new ModelStateDictionary();

            var order = checkoutService.OrderFromCheckoutViewData(checkoutViewData, modelState);

            modelState.IsValid.ShouldBeTrue();
	    }

        private static CheckoutViewData GetCheckoutViewData()
        {
            return new CheckoutViewData
            {
                OrderId = 0,
                BasketId = 7,

                CardContactFirstName = "Jon",
                CardContactLastName = "Anderson",
                CardContactAddress1 = "5 Yes Avenue",
                CardContactAddress2 = "Close to the Edge",
                CardContactAddress3 = "Near Fragile",
                CardContactTown = "Brighton",
                CardContactCounty = "Sussex",
                CardContactPostcode = "BN3 6TT",
                CardContactCountry = new Country(),
                CardContactTelephone = "01273999555",

                Email = "Jon@yes.com",
                EmailConfirm = "Jon@yes.com",

                UseCardholderContact = false,

                DeliveryContactFirstName = "Jonx",
                DeliveryContactLastName = "Andersonx",
                DeliveryContactAddress1 = "5 Yes Avenuex",
                DeliveryContactAddress2 = "Close to the Edgx",
                DeliveryContactAddress3 = "Near Fragilex",
                DeliveryContactTown = "Brightonx",
                DeliveryContactCounty = "Sussexx",
                DeliveryContactPostcode = "BN3 6TTx",
                DeliveryContactCountry = new Country(),
                DeliveryContactTelephone = "01273999555x",

                AdditionalInformation = "some additional info",

                CardCardType = new CardType(),
                CardHolder = "Jon Anderson",
                CardNumber = "1111111111111117",
                CardExpiryMonth = 3,
                CardExpiryYear = 2012,
                CardStartMonth = 2,
                CardStartYear = 2009,
                CardIssueNumber = "3",
                CardSecurityCode = "123",

                PayByTelephone = false,
                ContactMe = true
            };
        }

        private void VerifyOrderMatchesCheckoutViewData(Order order, CheckoutViewData checkoutViewData)
        {
            order.CardContact.Firstname.ShouldEqual(checkoutViewData.CardContactFirstName);
            order.CardContact.Lastname.ShouldEqual(checkoutViewData.CardContactLastName);
            order.CardContact.Address1.ShouldEqual(checkoutViewData.CardContactAddress1);
            order.CardContact.Address2.ShouldEqual(checkoutViewData.CardContactAddress2);
            order.CardContact.Address3.ShouldEqual(checkoutViewData.CardContactAddress3);
            order.CardContact.Town.ShouldEqual(checkoutViewData.CardContactTown);
            order.CardContact.County.ShouldEqual(checkoutViewData.CardContactCounty);
            order.CardContact.Postcode.ShouldEqual(checkoutViewData.CardContactPostcode);
            order.CardContact.Country.ShouldEqual(checkoutViewData.CardContactCountry);
            order.CardContact.Telephone.ShouldEqual(checkoutViewData.CardContactTelephone);

            order.Email.ShouldEqual(checkoutViewData.Email);

            order.UseCardHolderContact.ShouldEqual(checkoutViewData.UseCardholderContact);

            order.DeliveryContact.Firstname.ShouldEqual(checkoutViewData.DeliveryContactFirstName);
            order.DeliveryContact.Lastname.ShouldEqual(checkoutViewData.DeliveryContactLastName);
            order.DeliveryContact.Address1.ShouldEqual(checkoutViewData.DeliveryContactAddress1);
            order.DeliveryContact.Address2.ShouldEqual(checkoutViewData.DeliveryContactAddress2);
            order.DeliveryContact.Address3.ShouldEqual(checkoutViewData.DeliveryContactAddress3);
            order.DeliveryContact.Town.ShouldEqual(checkoutViewData.DeliveryContactTown);
            order.DeliveryContact.County.ShouldEqual(checkoutViewData.DeliveryContactCounty);
            order.DeliveryContact.Postcode.ShouldEqual(checkoutViewData.DeliveryContactPostcode);
            order.DeliveryContact.Country.ShouldEqual(checkoutViewData.DeliveryContactCountry);
            order.DeliveryContact.Telephone.ShouldEqual(checkoutViewData.DeliveryContactTelephone);

            order.AdditionalInformation.ShouldEqual(checkoutViewData.AdditionalInformation);

            order.Card.CardType.ShouldEqual(checkoutViewData.CardCardType);
            order.Card.Holder.ShouldEqual(checkoutViewData.CardHolder);
            order.Card.Number.ShouldEqual(checkoutViewData.CardNumber);
            order.Card.ExpiryMonth.ShouldEqual(checkoutViewData.CardExpiryMonth);
            order.Card.ExpiryYear.ShouldEqual(checkoutViewData.CardExpiryYear);
            order.Card.StartMonth.ShouldEqual(checkoutViewData.CardStartMonth);
            order.Card.StartYear.ShouldEqual(checkoutViewData.CardStartYear);
            order.Card.IssueNumber.ShouldEqual(checkoutViewData.CardIssueNumber);
            order.Card.SecurityCode.ShouldEqual(checkoutViewData.CardSecurityCode);

            order.PayByTelephone.ShouldEqual(checkoutViewData.PayByTelephone);
            order.ContactMe.ShouldEqual(checkoutViewData.ContactMe);

            order.User.ShouldBeTheSameAs(user);
        }

        private static Basket CreateBasketWithId(int id)
        {
            var widget = new Size
            {
                Id = 1,
                IsActive = true,
                IsInStock = true,
                Name = "Large",
                Product = new Product
                {
                    Id = 101,
                    Name = "Widget",
                    Price = new Money(12.34M)
                }
            };

            var gadget = new Size
            {
                Id = 2,
                IsActive = true,
                IsInStock = true,
                Name = "Medium",
                Product = new Product
                {
                    Id = 222,
                    Name = "Gadget",
                    Price = new Money(4.59M)
                }
            };

            return new Basket
            {
                Id = id,
                Country = new Country(),
                BasketItems =
                    {
                        new BasketItem { Id = 1, Quantity = 2, Size = widget },
                        new BasketItem { Id = 2, Quantity = 1, Size = gadget }
                    }
            };
        }
	}
}
// ReSharper restore InconsistentNaming