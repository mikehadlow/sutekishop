using System;
using Suteki.Common.Models;

namespace Suteki.Shop.Tests.Models.Builders
{
    public static class Default
    {
        public static PostZone PostZone()
        {
            return new PostZone
            {
                Name = "UK",
                Multiplier = 1.0M,
                AskIfMaxWeight = false,
                Position = 1,
                IsActive = true,
                FlatRate = new Money(10M)
            };
        }

        public static Postage Postage()
        {
            return new Postage
            {
                Name = "A",
                IsActive = true,
                MaxWeight = 1000,
                Position = 1,
                Price = new Money(0.45M)
            };
        }

        public static Country Country()
        {
            return new Country
            {
                Name = "UK",
                IsActive = true,
                Position = 1
            };
        }

        public static CardType CardType()
        {
            return new CardType
            {
                Name = "Visa / MasterCard",
                RequiredIssueNumber = false
            };
        }

        public static Card Card()
        {
            return new Card
            {
                ExpiryMonth = 6,
                ExpiryYear = 2015,
                Holder = "R WAKEMAN",
                IssueNumber = null,
                Number = "1111111111111117",
                SecurityCode = "123",
                StartMonth = 2,
                StartYear = 2010
            };
        }

        public static Contact Contact()
        {
            return new Contact
            {
                Address1 = "4 The Street",
                Address2 = "Little Place",
                Address3 = null,
                Firstname = "Steve",
                Lastname = "Howe",
                Postcode = "BN4 6SS",
                Telephone = "01732 456789",
                Town = "Hove"
            };
        }

        public static MailingListSubscription MailingListSubscription()
        {
            return new MailingListSubscription
            {
                Email = "mike@suteki.co.uk",
                DateSubscribed = new DateTime(2010, 4, 3)
            };
        }

        public static Order Order()
        {
            return new Order
            {
                Email = "mike@suteki.co.uk",
                AdditionalInformation = "Please leave in the cat flap",
                UseCardHolderContact = true,
                PayByTelephone = false,
                CreatedDate = new DateTime(2010, 4, 1),
                DispatchedDate = new DateTime(2010, 4, 3),
                Note = "Dispatched late because of rain",
                ContactMe = true
            };
        }

        public static OrderStatus OrderStatus()
        {
            return new OrderStatus
            {
                Name = "Created"
            };
        }

        public static Basket Basket()
        {
            return new Basket
            {
                OrderDate = new DateTime(2010, 4, 3)
            };
        }

        public static BasketItem BasketItem()
        {
            return new BasketItem
            {
                Quantity = 1
            };
        }

        public static User User()
        {
            return new User
            {
                Email = "mike@mike.com",
                Password = "some encrypted rubbish",
                IsEnabled = true
            };
        }

        public static Role Role()
        {
            return new Role
            {
                Name = "Administrator"
            };
        }

        public static Size Size()
        {
            return new Size
            {
                Name = "Large",
                IsActive = true,
                IsInStock = true
            };
        }

        public static Product Product()
        {
            return new Product
            {
                Name = "Blue Jeans",
                Description = "Our nicest Blue Jeans, for when price is no object.",
                Price = new Money(200M),
                Position = 1,
                Weight = 200,
                IsActive = true
            };
        }

        public static Review Review()
        {
            return new Review
            {
                Approved = true,
                Text = "The Review Text",
                Rating = 1,
                Reviewer = "John Paul Jones"
            };
        }

        public static ProductImage ProductImage()
        {
            return new ProductImage();
        }

        public static Image Image()
        {
            return new Image
            {
                FileName = new Guid("047265dc-e293-4d51-afcc-fce636fa3fe7"),
                Description = "A pretty tree"
            };
        }

        public static ProductCategory ProductCategory()
        {
            return new ProductCategory();
        }

        public static Category Category()
        {
            return new Category
            {
                Name = "Woolens",
                Position = 1,
                IsActive = true
            };
        }

        public static Menu Menu()
        {
            return new Menu
            {
                Name = "Help",
                Position = 1,
                IsActive = true
            };
        }

        public static TextContent TextContent()
        {
            return new TextContent
            {
                Name = "Getting the best fit",
                Text = "Here how you do it.",
                Position = 2,
                IsActive = true
            };
        }

        public static ActionContent ActionContent()
        {
            return new ActionContent
            {
                Name = "Home Page",
                Controller = "Home",
                Action = "Index",
                Position = 3,
                IsActive = true
            };
        }

        public static TopContent TopContent()
        {
            return new TopContent
            {
                Name = "Our Shop",
                Text = "Our shop is the best in world for Widgets",
                Position = 4,
                IsActive = true
            };
        }
    }
}