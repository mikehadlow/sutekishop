using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Shop.Repositories;
using Suteki.Shop.Services;

namespace Suteki.Shop.Filters
{
	public class AuthenticateAttribute : FilterUsingAttribute
	{
		public AuthenticateAttribute() : base(typeof(AuthenticateFilter))
		{
			Order = 0;
		}
	}

	public class AuthenticateFilter : IAuthorizationFilter
	{
	    private readonly IRepositoryFactory<User> userRepositoryFactory;
		private readonly IFormsAuthentication formsAuth;

		public AuthenticateFilter(IFormsAuthentication formsAuth, IRepositoryFactory<User> userRepositoryFactory)
		{
		    this.userRepositoryFactory = userRepositoryFactory;
		    this.formsAuth = formsAuth;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
		    var userRepository = userRepositoryFactory.Resolve();
		    try
		    {
                var context = filterContext.HttpContext;

                if (context.User != null && context.User.Identity.IsAuthenticated)
                {
                    var email = context.User.Identity.Name;
                    var user = userRepository.GetAll().WhereEmailIs(email);

                    if (user == null)
                    {
                        formsAuth.SignOut();
                    }
                    else
                    {
                        AuthenticateAs(context, user);
                        return;
                    }
                }

                AuthenticateAs(context, User.Guest);
            }
		    finally
		    {
		        userRepositoryFactory.Release(userRepository);
		    }

		}

		private void AuthenticateAs(HttpContextBase context, User user)
		{
			Thread.CurrentPrincipal = context.User = user;				
		}
	}
}