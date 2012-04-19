using NUnit.Framework;
using Suteki.Common.TestHelpers;
using Suteki.Common.ViewData;
using Suteki.Shop.Controllers;
using Suteki.Shop.Services;
using Rhino.Mocks;

namespace Suteki.Shop.Tests.Controllers
{
    [TestFixture]
    public class LoginControllerTests
    {
        private IUserService userService;

        private LoginController loginController;

        private const string email = "Henry@suteki.co.uk";
    	private const string password = "henry1";

    	[SetUp]
        public void SetUp()
        {
            userService = MockRepository.GenerateStub<IUserService>();
            loginController = new LoginController(userService);
        }

        [Test]
        public void Index_ShouldDisplayIndexView()
        {
            const string view = "Index";

            loginController.Index()
                .ReturnsViewResult()
                .ForView(view);
        }

        [Test]
        public void Index_Post_should_authenticate_user()
        {
            loginController.Index(email, password, "some URL");
            userService.AssertWasCalled(s => s.Authenticate(email, password));
        }

        [Test]
        public void Index_Post_should_set_authentication_cookie()
        {
            userService.Stub(s => s.Authenticate(email, password)).Return(true);

            loginController.Index(email, password, null);

            userService.AssertWasCalled(s => s.SetAuthenticationCookie(email));
        }

        [Test]
        public void Index_Post_should_redirect_to_index_home_when_login_is_successful()
        {
            userService.Stub(s => s.Authenticate(email, password)).Return(true);

            loginController.Index(email, password, null)
                .ReturnsRedirectToRouteResult()
                .ToAction("Index")
                .ToController("Home");
        }

        [Test]
        public void Index_Post__should_show_error_message_if_authentication_fails()
        {
            userService.Stub(s => s.Authenticate(email, password)).Return(false);

            loginController.Index(email, password, null)
                .ReturnsViewResult()
                .ForView("") //view should match action
                .WithModel<IErrorViewData>()
                .AssertAreEqual("Unknown email or password", vd => vd.ErrorMessage);
        }

        [Test]
        public void Logout_ShouldLogUserOut()
        {
            loginController.Logout()
                .ReturnsRedirectToRouteResult()
                .ToAction("Index")
                .ToController("Home");

            userService.AssertWasCalled(c => c.RemoveAuthenticationCookie());
        }

    	[Test]
    	public void Should_redirect_to_returnurl()
    	{
    	    userService.Stub(s => s.Authenticate(email, password)).Return(true);

			loginController.Index(email, password, "/foo/bar")
				.ReturnsRedirect().Url.ShouldEqual("/foo/bar");

    	}
    }
}
