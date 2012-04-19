namespace Suteki.Shop.Services
{
    public interface IUserService
    {
        User CreateNewCustomer();
        User CurrentUser { get; }
        void SetAuthenticationCookie(string email);
        void SetContextUserTo(User user);
        void RemoveAuthenticationCookie();
    	string HashPassword(string password);
        bool Authenticate(string email, string password);
    }
}
