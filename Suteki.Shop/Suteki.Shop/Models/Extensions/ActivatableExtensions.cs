using Suteki.Common.Models;

namespace Suteki.Shop.Extensions
{
    public static class ActivatableExtensions
    {
        public static bool IsVisibleTo(this IActivatable activatable, User user)
        {
            return user.IsAdministrator ? true : activatable.IsActive;
        }
    }
}