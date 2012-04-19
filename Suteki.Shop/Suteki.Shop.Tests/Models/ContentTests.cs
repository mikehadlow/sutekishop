using NUnit.Framework;

namespace Suteki.Shop.Tests.Models
{
    [TestFixture]
    public class ContentTests
    {
        Menu mainMenu;
        Menu subMenu;

        [SetUp]
        public void SetUp()
        {
            mainMenu = new Menu
            {
                Id = Menu.MainMenuId
            };

            subMenu = new Menu
            {
                ParentContent = mainMenu
            };
        }

        [Test]
        public void TextContent_SubMenu_ShouldReturnItsSubMenu()
        {
            var textContent = new TextContent
            {
                ParentContent = subMenu
            };

            Assert.That(textContent.SubMenu, Is.SameAs(subMenu));
        }

        [Test]
        public void TextContentWithoutSubMenu_SubMenu_ShouldReturnNull()
        {

            var textContent = new TextContent
            {
                ParentContent = mainMenu
            };

            Assert.That(textContent.SubMenu, Is.Null);
        }

        [Test]
        public void Menu_SubMenu_ShouldReturnItselfIfItIsASubMenu()
        {
            Assert.That(subMenu.SubMenu, Is.SameAs(subMenu));
        }

        [Test]
        public void MainMenu_SubMenu_ShouldReturnNull()
        {
            Assert.That(mainMenu.SubMenu, Is.Null);
        }

        [Test]
        public void SubSubMenu_SubMenu_ShouldReturnSubMenu()
        {
            var subSubMenu = new Menu
            {
                ParentContent = subMenu
            };

            Assert.That(subSubMenu.SubMenu, Is.SameAs(subMenu));
        }

		[Test]
        public void TextContentWithoutMenuShouldReturnNull()
        {
            var textContent = new TextContent
            {
                Text = "Hello World"
            };

            textContent.SubMenu.ShouldBeNull();
        }
    }
}
