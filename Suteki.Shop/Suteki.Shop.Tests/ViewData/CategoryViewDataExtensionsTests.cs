using NUnit.Framework;
using Suteki.Shop.Tests.Repositories;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Tests.ViewData
{
    [TestFixture]
    public class CategoryViewDataExtensionsTests
    {
        [Test]
        public void MapToViewData_should_map_category_entities_to_view_data()
        {
            var categories = MockRepositoryBuilder.CreateCategoryRepository().GetAll();
            var viewDataCategories = categories.MapToViewData();

            var rootCategory = viewDataCategories.GetRoot();

            Assert.That(rootCategory.Name, Is.EqualTo("root"));
            Assert.That(rootCategory.ChildCategories.Count, Is.EqualTo(2), "root has no children");

            var one = rootCategory.ChildCategories[0];
            var two = rootCategory.ChildCategories[1];
            Assert.That(one.Name, Is.EqualTo("one"));
            Assert.That(two.Name, Is.EqualTo("two"));

            Assert.That(one.ChildCategories.Count, Is.EqualTo(2), "one has no children");
            var oneOne = one.ChildCategories[0];
            var oneTwo = one.ChildCategories[1];
            Assert.That(oneOne.Name, Is.EqualTo("oneOne"));
            Assert.That(oneTwo.Name, Is.EqualTo("oneTwo"));

            Assert.That(oneTwo.ChildCategories.Count, Is.EqualTo(2), "oneTwo has no children");
            var oneTwoOne = oneTwo.ChildCategories[0];
            var oneTwoTwo = oneTwo.ChildCategories[1];
            Assert.That(oneTwoOne.Name, Is.EqualTo("oneTwoOne"));
            Assert.That(oneTwoTwo.Name, Is.EqualTo("oneTwoTwo"));
        }
    }
}