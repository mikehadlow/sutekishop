using System;
using NHibernate;
using Suteki.Common.Models;

namespace Suteki.Shop.Database
{
    public static class StaticData
    {
        public static void InsertRoles(ISession session)
        {
            var insertRole = CreateInsert<Role>(session);

            insertRole(Role.AdministratorId, "Administrator");
            insertRole(Role.OrderProcessorId, "Order Processor");
            insertRole(Role.CustomerId, "Customer");
            insertRole(Role.GuestId, "Guest");
        }

        public static void InsertAdministrator(ISession session)
        {
            var adminRole = session.Get<Role>(1);
            // password is 'admin'
            var admin = new User { Email = "admin@sutekishop.co.uk", Password = "D033E22AE348AEB5660FC2140AEC35850C4DA997", Role = adminRole, IsEnabled = true };
            session.Save(admin);
        }

        public static void InsertRootCategory(ISession session)
        {
            var root = new Category { Name = "- Root", Position = 1, IsActive = true };
            session.Save(root);
        }

        public static void InsertCardTypes(ISession session)
        {
            Action<int, string, bool> insertCardType = (id, name, requiresIssueNumber) =>
            {
                var cardType = new CardType {Id = id, Name = name, RequiredIssueNumber = requiresIssueNumber};
                session.Save(cardType);
            };

            insertCardType(CardType.VisaDeltaElectronId, "Visa / Delta / Electron", false);
            insertCardType(CardType.MasterCardEuroCardId, "Master Card / Euro Card", false);
            insertCardType(CardType.AmericanExpressId, "American Express", false);
            insertCardType(CardType.SwitchSoloMaestroId, "Switch / Solo / Maestro", true);
        }

        public static void InsertOrderStatus(ISession session)
        {
            var insertOrderStatus = CreateInsert<OrderStatus>(session);

            insertOrderStatus(OrderStatus.PendingId, "Pending");
            insertOrderStatus(OrderStatus.CreatedId, "Created");
            insertOrderStatus(OrderStatus.DispatchedId, "Dispatched");
            insertOrderStatus(OrderStatus.RejectedId, "Rejected");
        }

        public static void InsertContent(ISession session)
        {
            var mainMenu = new Menu
            {
                Name = "Main Menu",
                Position = 1,
                IsActive = true
            };
            session.Save(mainMenu);

            var home = new TopContent
            {
                Name = "Home",
                Text = 
@"<br/>
<h1>Welcome to Suteki Shop</h1>
<p>A .NET eCommerce application.</p>
<p>Please visit the <a href=""http://code.google.com/p/sutekishop/"">project web site</a> for more information.</p>",
                ParentContent = mainMenu,
                Position = 2,
                IsActive = true
            };
            session.Save(home);

            var shop = new ActionContent
            {
                Name = "Online Shop",
                Controller = "Home",
                Action = "Index",
                ParentContent = mainMenu,
                Position = 3,
                IsActive = true
            };
            session.Save(shop);

            var shopText = new TextContent
            {
                Name = "Shopfront",
                Text = "<h1>Wecome to our online shop</h1>",
                Position = 4,
                IsActive = true
            };
            session.Save(shopText);

            var mailingList = new ActionContent
            {
                Name = "Mailing List",
                Controller = "MailingList",
                Action = "Index",
                ParentContent = mainMenu,
                Position = 5,
                IsActive = true
            };
            session.Save(mailingList);

            var customerReviews = new ActionContent
            {
                Name = "Reviews",
                Controller = "Reviews",
                Action = "AllApproved",
                ParentContent = mainMenu,
                Position = 6,
                IsActive = true
            };
            session.Save(customerReviews);
        }

        public static void InsertPostZoneAndCountry(ISession session)
        {
            var postZone = new PostZone
            {
                Name = "United Kingdom",
                Multiplier = 1,
                AskIfMaxWeight = false,
                Position = 1,
                IsActive = true,
                FlatRate = new Money(10M)
            };
            session.Save(postZone);

            var uk = new Country
            {
                Name = "United Kingdom",
                Position = 1,
                PostZone = postZone,
                IsActive = true
            };
            session.Save(uk);
        }

        public static Action<int, string> CreateInsert<TModel>(ISession session) where TModel : INamedEntity, new()
        {
            return (id, name) =>
            {
                var entity = new TModel {Id = id, Name = name};
                session.Save(entity);
            };
        }
    }
}