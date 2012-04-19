using System;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using Suteki.Common.Extensions;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.Tests.Maps
{
    [TestFixture]
    public class ContentMapTests : MapTestBase
    {
        int menu1Id = 0;

        [TestFixtureSetUp]
        public void SetUp()
        {
            var menu1 = new Menu
            {
                Name = "menu1",
                IsActive = true,
                Position = 1
            };

            var text = new TextContent
            {
                Name = "text1",
                Text = "some text",
                IsActive = true,
                Position = 2
            };

            var action = new ActionContent
            {
                Name = "My Action",
                Controller = "controller",
                Action = "action",
                IsActive = true,
                Position = 3
            };

            var menu2 = new Menu
            {
                Name = "menu2",
                IsActive = true,
                Position = 4
            };

            InSession(session =>
            {
                session.Save(menu1);
                session.Save(text);
                session.Save(action);
                session.Save(menu2);
            });

            menu1Id = menu1.Id;
        }

        [Test]
        public void Should_be_able_to_get_only_menus()
        {
            InSession(session =>
            {
                var menus = session.Query<Content>().Menus().AsEnumerable();
                // other tests might have inserted menus, but we should get at least our two
                Assert.That(menus.Count() >= 2);
            });
        }

        [Test]
        public void Get_returns_a_proxy_of_the_correct_type()
        {
            InSession(session =>
            {
                var content = session.Get<Content>(menu1Id);
                var menu = content as Menu;
                menu.ShouldNotBeNull("menu is null");
            });
        }

        [Test]
        public void Load_does_not_get_return_a_proxy_of_the_correct_type()
        {
            InSession(session =>
            {
                var content = session.Load<Content>(menu1Id);
                Console.WriteLine(content.GetType().Name);
                var menu = content as Menu;
                menu.ShouldBeNull();
            });
        }

        [Test]
        public void Type_is_not_correct_after_the_entity_loaded()
        {
            InSession(session =>
            {
                var content = session.Load<Content>(menu1Id);
                Console.WriteLine(content.Name);
                var menu = content as Menu;
                menu.ShouldBeNull();
            });
        }

        [Test]
        public void Use_CastAs_to_cast_lazy_loaded_entities()
        {
            InSession(session =>
            {
                var content = session.Load<Content>(menu1Id);
                Console.WriteLine(content.GetType().Name);
                var menu = content.CastAs<Menu>();
                menu.ShouldNotBeNull();
                menu.Name.ShouldEqual("menu1");
            });
        }
    }
}