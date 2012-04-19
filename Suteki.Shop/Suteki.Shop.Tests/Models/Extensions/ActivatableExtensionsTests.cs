// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Suteki.Common.Models;
using Suteki.Shop.Extensions;

namespace Suteki.Shop.Tests.Models.Extensions
{
    [TestFixture]
    public class ActivatableExtensionsTests
    {
        private ActivatableThing thing;
        private User guest;
        private User admin;

        [SetUp]
        public void SetUp()
        {
            thing = new ActivatableThing();

            guest = User.Guest;
            admin = new User {Role = Role.Administrator};
        }

        [Test]
        public void IsVisible_should_false_when_IsActive_is_false_and_user_is_not_admin()
        {
            thing.IsActive = false;
            thing.IsVisibleTo(guest).ShouldBeFalse();
        }

        [Test]
        public void IsVisible_should_be_true_when_IsActive_is_true_and_user_is_not_admin()
        {
            thing.IsActive = true;
            thing.IsVisibleTo(guest).ShouldBeTrue();
        }

        [Test]
        public void IsVisible_should_be_true_when_IsActive_is_false_but_user_is_admin()
        {
            thing.IsActive = false;
            thing.IsVisibleTo(admin).ShouldBeTrue();
        }

        [Test]
        public void IsVisible_should_be_true_when_IsActive_is_true_and_user_is_admin()
        {
            thing.IsActive = true;
            thing.IsVisibleTo(admin).ShouldBeTrue();
        }
    }

    public class ActivatableThing : IActivatable
    {
        public bool IsActive { get; set; }
    }
}
// ReSharper restore InconsistentNaming