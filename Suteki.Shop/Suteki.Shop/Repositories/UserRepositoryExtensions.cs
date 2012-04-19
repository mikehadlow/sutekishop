using System.Linq;

namespace Suteki.Shop.Repositories
{
    public static class UserRepositoryExtensions
    {
        public static User WhereEmailIs(this IQueryable<User> users, string email)
        {
            return users.Where(user => user.Email == email).SingleOrDefault();
        }

        public static bool ContainsUser(this IQueryable<User> users, string email, string password)
        {
            return users.GetUser(email, password) != null;
        }

        public static User GetUser(this IQueryable<User> users, string email, string password)
        {
            return users.SingleOrDefault(
                user =>
                    user.Email == email &&
                    user.Password == password &&
                    user.IsEnabled
                );
        }

        public static IQueryable<User> Editable(this IQueryable<User> users)
        {
            return users.Where(user => !(user.Role.Id == Role.CustomerId));
        }
    }
}
