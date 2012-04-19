using System;
using System.Web.Security;

namespace Suteki.Shop.Services
{
	public interface IFormsAuthentication
	{
		void SignOut();
		void SetAuthCookie(string email, bool createPersistentCookie);
		string HashPasswordForStoringInConfigFile(string password);
	}

	public class FormsAuthenticationWrapper : IFormsAuthentication
	{
		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}

		public void SetAuthCookie(string email, bool createPersistentCookie)
		{
			FormsAuthentication.SetAuthCookie(email, createPersistentCookie);
		}

		public string HashPasswordForStoringInConfigFile(string password)
		{
			return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
		}
	}
}