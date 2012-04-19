using System.Linq;
using System.Collections.Generic;
using Suteki.Common.Models;

namespace Suteki.Shop.Repositories
{
    public static class ActivatableExtensions
    {
        public static IQueryable<T> Active<T>(this IQueryable<T> items) where T : IActivatable
        {
            if (items == null) return items;
            return items.Where(item => item.IsActive);
        }

        public static IEnumerable<T> Active<T>(this IEnumerable<T> items) where T : IActivatable
        {
            return items.AsQueryable().Active();
        }

        public static IQueryable<T> ActiveFor<T>(this IQueryable<T> items, User user) where T : IActivatable
        {
            if (user.IsAdministrator)
            {
                return items;
            }
            return items.Active();
        }

        public static IEnumerable<T> ActiveFor<T>(this IEnumerable<T> items, User user) where T : IActivatable
        {
            return items.AsQueryable().ActiveFor(user);
        }
    }
}
