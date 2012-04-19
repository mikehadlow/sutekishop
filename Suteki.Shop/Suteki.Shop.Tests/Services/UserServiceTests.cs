using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Shop.Services;

namespace Suteki.Shop.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        IUserService userService;
        IRepository<User> userRepository;
    	IFormsAuthentication formsAuthentication;

    	[SetUp]
        public void SetUp()
        {
            userRepository = MockRepository.GenerateStub<IRepository<User>>();
        	formsAuthentication = MockRepository.GenerateStub<IFormsAuthentication>();
            userService = new UserService(userRepository, formsAuthentication);
        }

        [Test]
        public void CreateNewCustomer_ShouldReturnANewCustomerAddedToTheRepository()
        {
            userRepository.Expect(ur => ur.SaveOrUpdate(Arg<User>.Is.Anything));

            User user = userService.CreateNewCustomer();

            Assert.IsNotNull(user, "returned user is null");
            Assert.IsTrue(user.IsCustomer, "returned user is not a customer");
        }

    	[Test]
    	public void HashPassword_should_delegate_to_formsAuthentication()
    	{
    		formsAuthentication.Expect(x => x.HashPasswordForStoringInConfigFile("foo")).Return("bar");
    		userService.HashPassword("foo").ShouldEqual("bar");
    	}

    	[Test]
    	public void RemoveAuthenticationCookie_should_delegate_to_formsAuthentication()
    	{
    		userService.RemoveAuthenticationCookie();
			formsAuthentication.AssertWasCalled(x => x.SignOut());
    	}

    	[Test]
    	public void SetAuthenticationCookie_should_delegate_to_formsAuthentication()
    	{
    		userService.SetAuthenticationCookie("foo@foo.com");
			formsAuthentication.AssertWasCalled(x => x.SetAuthCookie("foo@foo.com", true));
    	}

        [Test]
        public void Authenticate_should_check_that_user_and_matching_password_exist_in_repository()
        {
            formsAuthentication.Stub(x => x.HashPasswordForStoringInConfigFile("foo")).Return("bar");

            var user = new User
            {
                Email = "foo@foo.com", 
                Password = "bar",
                IsEnabled = true
            };
            userRepository.Stub(r => r.GetAll()).Return(new[] {user}.AsQueryable());

            var isAuthenticated = userService.Authenticate("foo@foo.com", "foo");

            Assert.That(isAuthenticated, Is.True);
        }
    }
}
