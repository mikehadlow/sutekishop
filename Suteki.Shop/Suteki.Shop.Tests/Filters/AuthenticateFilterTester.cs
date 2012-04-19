using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using MvcContrib.TestHelper;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Repositories;
using Suteki.Shop.Filters;
using Suteki.Shop.Services;
using Suteki.Shop.Tests.Repositories;

namespace Suteki.Shop.Tests.Filters
{
	[TestFixture]
	public class AuthenticateFilterTester
	{
		private AuthenticateFilter filter;
		private AuthorizationContext context;
		private IPrincipal originalPrincipal;
		private IRepository<User> userRepository;
	    private IRepositoryFactory<User> userRepositoryFactory;
		private IFormsAuthentication formsAuth;

		[SetUp]
		public void Setup()
		{
			context = new AuthorizationContext
          	{
				HttpContext = new TestControllerBuilder().HttpContext
          	};

            userRepositoryFactory = MockRepository.GenerateStub<IRepositoryFactory<User>>();
			userRepository = MockRepositoryBuilder.CreateUserRepository();
		    userRepositoryFactory.Stub(x => x.Resolve()).Return(userRepository).Repeat.Any();

			formsAuth = MockRepository.GenerateStub<IFormsAuthentication>();
            filter = new AuthenticateFilter(formsAuth, userRepositoryFactory);
			
			originalPrincipal = Thread.CurrentPrincipal;
		}

		[TearDown]
		public void Teardown()
		{
			Thread.CurrentPrincipal = originalPrincipal;
		}

		protected User ValidUser
		{
			get { return userRepository.GetAll().First(); }
		}

		[Test]
		public void Current_user_should_be_set_to_guest_when_user_is_null()
		{
			context.HttpContext.User = null;
			filter.OnAuthorization(context);

			AssertGuest();
		}

		[Test]
		public void Current_user_should_be_set_to_guest_when_user_is_not_authenticated()
		{
			var user = new FakePrincipal() { IsAuthenticated = false };
			context.HttpContext.User = user;
			filter.OnAuthorization(context);

			AssertGuest();
		}

		[Test]
		public void Should_sign_out_when_user_is_null()
		{
			context.HttpContext.User = new FakePrincipal() { Name = "foo@bar" };
			filter.OnAuthorization(context);
			formsAuth.AssertWasCalled(x => x.SignOut());
		}

		[Test]
		public void Current_user_should_be_set_to_guest_when_user_cannot_be_found_from_repository()
		{
			var user = new FakePrincipal() { Name = "foo@bar" };
			Thread.CurrentPrincipal = context.HttpContext.User = user;
			filter.OnAuthorization(context);

			AssertGuest();
		}

		[Test]
		public void Should_replace_current_principal_when_user_is_valid()
		{
			context.HttpContext.User = new FakePrincipal { Name = ValidUser.Email };
			filter.OnAuthorization(context);
			context.HttpContext.User.ShouldBeTheSameAs(ValidUser);
			Thread.CurrentPrincipal.ShouldBeTheSameAs(ValidUser);
		}

		private void AssertGuest()
		{
			context.HttpContext.User.ShouldBe<User>();
			context.HttpContext.User.CastTo<User>().Email.ShouldEqual(User.Guest.Email);
			Thread.CurrentPrincipal.ShouldBe<User>();
			Thread.CurrentPrincipal.CastTo<User>().Email.ShouldEqual(User.Guest.Email);
		}

		private class FakePrincipal : IPrincipal, IIdentity
		{
			public FakePrincipal()
			{
				IsAuthenticated = true;
			}

			public bool IsInRole(string role)
			{
				throw new NotImplementedException();
			}

			public IIdentity Identity
			{
				get { return this; }
			}

			public string Name { get; set;}

			public string AuthenticationType
			{
				get { return "forms"; }
			}

			public bool IsAuthenticated{ get; set;}
		}
	}
}