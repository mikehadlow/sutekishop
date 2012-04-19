using NUnit.Framework;
using Suteki.Common.Repositories;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.Tests.Repositories
{
    [TestFixture]
    public class UserRepositoryExtensionsTests
    {
        IRepository<User> userRepository;

        [SetUp]
        public void SetUp()
        {
            userRepository = MockRepositoryBuilder.CreateUserRepository();
        }

        [Test]
        public void WhereEmailIs_ShouldReturnCorrectUserFromList()
        {
            const string email = "Henry@suteki.co.uk";
            var user = userRepository.GetAll().WhereEmailIs(email);

            Assert.IsNotNull(user);
            Assert.AreEqual(email, user.Email);
        }

        [Test]
        public void ContainsUser_ShouldReturnTrueForExistingUser()
        {
            const string email = "Henry@suteki.co.uk";
            const string password = "6C80B78681161C8349552872CFA0739CF823E87B";

            Assert.IsTrue(userRepository.GetAll().ContainsUser(email, password));
        }

        [Test]
        public void ContainsUser_ShouldReturnFalseForIncorrectPassword()
        {
            const string email = "Henry@suteki.co.uk";
            const string password = "xyz";

            Assert.IsFalse(userRepository.GetAll().ContainsUser(email, password));
        }

    }
}
