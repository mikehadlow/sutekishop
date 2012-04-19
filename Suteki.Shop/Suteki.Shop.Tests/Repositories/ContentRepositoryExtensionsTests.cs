using NHibernate.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Tests.Maps;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.Tests.Repositories
{
    [TestFixture]
    public class ContentRepositoryExtensionsTests : MapTestBase
    {
        Menu menu;

        [SetUp]
        public void SetUp()
        {
            menu = new Menu
            {
                Name = "Main", 
                IsActive = true, 
                Position = 1
            };
            var textContent = new TextContent
            {
                Name = "Text", 
                Text = "Some text", 
                IsActive = true, 
                Position = 2,
                ParentContent = menu
            };
            var actionContent = new ActionContent
            {
                Name = "Action",
                Controller = "HomeController",
                Action = "Index",
                IsActive = true,
                Position = 3,
                ParentContent = menu
            };
            var topContent = new TopContent
            {
                Name = "Top content",
                Text = "Some more text",
                IsActive = true,
                Position = 4,
                ParentContent = menu
            };

            InSession(session =>
            {
                session.Save(menu);
                session.Save(textContent);
                session.Save(actionContent);
                session.Save(topContent);
            });
        }

        [Test]
        public void DefaultText_should_return_textContent()
        {
            Content defaultText = null;
            InSession(session =>
            {
                defaultText = session.Query<Content>().DefaultText(menu);
            });

            defaultText.ShouldNotBeNull();
        }
    }
}