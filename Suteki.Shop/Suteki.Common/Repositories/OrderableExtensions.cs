using System.Linq;
using System.Collections.Generic;

namespace Suteki.Common.Repositories
{
    public static class OrderableExtensions
    {
        public static IQueryable<T> InOrder<T>(this IQueryable<T> items) where T : IOrderable
        {
            if (items == null) return null;
            return items.OrderBy(i => i.Position);
        }

        public static IEnumerable<T> InOrder<T>(this IEnumerable<T> items) where T : IOrderable
        {
            if (items == null) return null;
            return InOrder(items.AsQueryable());
        }

        public static T AtPosition<T>(this IEnumerable<T> items, int position) where T : IOrderable
        {
            return items.SingleOrDefault(i => i.Position == position);
        }

        public static T GetItemBefore<T>(this IEnumerable<T> items, int position) where T : IOrderable
        {
            if(!items.Any(i => i.Position < position)) return default(T);
            return items.SingleOrDefault(i1 => items.Where(i2 => i2.Position < position).Max(i3 => i3.Position) == i1.Position);
        }

        public static T GetItemAfter<T>(this IEnumerable<T> items, int position) where T : IOrderable
        {
            if (!items.Any(i => i.Position > position)) return default(T);
            return items.SingleOrDefault(i1 => items.Where(i2 => i2.Position > position).Min(i3 => i3.Position) == i1.Position);
        }

        public static int GetNextPosition<T>(this IEnumerable<T> items) where T : IOrderable
        {
            if (items.Count() == 0) return 1;
            return items.Max(i => i.Position) + 1;
        }
    }
}