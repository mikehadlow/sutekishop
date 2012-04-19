using System;
using System.Web;
using Suteki.Common.Repositories;
using Suteki.Shop.Repositories;

namespace Suteki.Shop.Services
{
    public class UserService : IUserService
    {
        readonly IRepository<User> userRepository;
		readonly IFormsAuthentication formsAuth;

        public UserService(IRepository<User> userRepository, IFormsAuthentication formsAuth)
        {
        	this.userRepository = userRepository;
        	this.formsAuth = formsAuth;
        }

    	public User CreateNewCustomer()
        {
            // TODO: Will Role.Customer get saved correctly by NH?
            var user = new User
            {
                Email = Guid.NewGuid().ToString(),
                Password = "",
                Role = Role.Customer
            };

            userRepository.SaveOrUpdate(user);
            return user;
        }

        public virtual User CurrentUser
        {
            get
            {
                var user = HttpContext.Current.User as User;
                if (user == null) throw new ApplicationException("HttpContext.User is not a Suteki.Shop.User");
                return user;
            }
        }

        public virtual void SetAuthenticationCookie(string email)
        {
        	formsAuth.SetAuthCookie(email, true);
        }

        public virtual void SetContextUserTo(User user)
        {
            System.Threading.Thread.CurrentPrincipal = HttpContext.Current.User = user;
        }

        public virtual void RemoveAuthenticationCookie()
        {
			formsAuth.SignOut();
        }

    	public string HashPassword(string password)
    	{
    		return formsAuth.HashPasswordForStoringInConfigFile(password);
    	}

        public bool Authenticate(string email, string password)
        {
            return userRepository.GetAll().ContainsUser(email, HashPassword(password));
        }
    }
}
