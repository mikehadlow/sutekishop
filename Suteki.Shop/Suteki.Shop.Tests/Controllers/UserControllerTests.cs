using System;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Common.TestHelpers;
using Suteki.Shop.Controllers;
using Suteki.Shop.Services;
using Suteki.Shop.Tests.Repositories;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            // you have to be an administrator to access the user controller
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin"), new[] {"Administrator"});

            userRepository = MockRepositoryBuilder.CreateUserRepository();
            
            var roleRepository = MockRepositoryBuilder.CreateRoleRepository();
        	userService = MockRepository.GenerateStub<IUserService>();
        	userService.Stub(x => x.HashPassword(Arg<string>.Is.Anything)).Do(new Func<string, string>(s => s + "HASHED"));

        	userController = new UserController(userRepository, roleRepository, userService);
            testContext = new ControllerTestContext(userController);


            // setup the querystring to return an empty name value collection by default
            testContext.TestContext.Request.Expect(r => r.QueryString).Return(new NameValueCollection());
        }

        #endregion

        private UserController userController;
        private ControllerTestContext testContext;

        private IRepository<User> userRepository;
    	private IUserService userService;

        private static void AssertUserEditViewDataIsCorrect(ViewResultBase result)
        {
            var viewData = result.ViewData.Model as ShopViewData;
            Assert.IsNotNull(viewData, "ViewData is not ShopViewData");

            // there should be some roles
            Assert.IsNotNull(viewData.Roles, "viewData.Roles is null");

            // add should have inserted a default user into the viewData
            Assert.IsNotNull(viewData.User, "viewData User is null");
        }

        [Test]
        public void Edit_ShouldSelectCorrectUser()
        {
            const int userId = 23;

            var user = new User
            {
                Id = userId,
                Email = "mike@mike.com",
                Password = "password",
                Role = new Role { Id = 2 }
            };

            userRepository.Stub(ur => ur.GetById(userId)).Return(user);

            var result = userController.Edit(userId) as ViewResult;

            AssertUserEditViewDataIsCorrect(result);
        }

        [Test]
        public void Index_ShouldShowAListOfUsers()
        {
            userController.Index()
                .ReturnsViewResult()
                .ForView("Index")
                .WithModel<ShopViewData>()
                .AssertNotNull(vd => vd.Users)
                .AssertAreEqual(3, vd => vd.Users.Count());
        }

        [Test]
        public void New_ShouldDisplayUserEditView()
        {
            var result = userController.New()
                .ReturnsViewResult()
                .ForView("Edit");

            AssertUserEditViewDataIsCorrect(result);
        }

    	[Test]
    	public void NewWithPost_ShouldInsertNewUser()
    	{
			const string password = "bl0gs";
			var user = new User 
			{
				Id = 0,
				Email = "blogs@blogs.com",
                Role = new Role { Id = 3 },
				IsEnabled = false
			};

			userController.New(user, password)
				.ReturnsRedirectToRouteResult()
				.ToAction("Index");

			userController.Message.ShouldNotBeNull();
			user.Password.ShouldEqual(password + "HASHED");
    	}

    	[Test]
    	public void NewWithPost_shouldReturnViewOnError()
    	{
			var user = new User 
			{
				Id = 0,
				Email = "blogs@blogs.com",
                Role = new Role { Id = 3 },
				IsEnabled = false
			};

			userController.New(user, null)
				.ReturnsViewResult()
				.ForView("Edit")
				.WithModel<ShopViewData>()
				.AssertAreSame(user, x => x.User);

			userController.ModelState.IsValid.ShouldBeFalse();
    	}

        [Test]
        public void EditWithPost_ShouldUpdateExistingUser()
        {
            const int userId = 34;
            const string email = "blogs@blogs.com";
            const string password = "bl0gs";
            const int roleId = 3;
            const bool isEnabled = false;

            // setup expectations on the userRepository
            var user = new User
            {
                Id = userId,
                Email = email,
                Password = "oldpassword",
                Role = new Role { Id = roleId },
                IsEnabled = isEnabled
            };

            // call Update
            var result = userController.Edit(user, password) as ViewResult;

            // Assertions
            Assert.IsNotNull(user, "user is null");
            Assert.AreEqual(email, user.Email);
            Assert.AreEqual(password + "HASHED", user.Password);
            Assert.AreEqual(roleId, user.Role.Id);
            Assert.AreEqual(isEnabled, user.IsEnabled);
            
            AssertUserEditViewDataIsCorrect(result);
        }

    	[Test]
    	public void EditWithPost_ShouldUpdateExistingUserButNotChangePassword()
    	{
			// setup expectations on the userRepository
    		const string oldpassword = "oldpassword";

    		var user = new User 
			{
				Id = 34,
				Email = "old@old.com",
				Password = oldpassword,
                Role = new Role { Id = 1 },
				IsEnabled = true
			};

			userController.Edit(user, null);
			user.Password.ShouldEqual(oldpassword);

    	}
    }
}