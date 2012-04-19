using System;
using System.Collections.Generic;
using System.Linq;

namespace Suteki.Shop.Repositories
{
    public static class BasketRepositoryExtensions
    {
        public static Basket CurrentBasket(this IQueryable<Basket> baskets)
        {
            if (baskets.Count() == 0) throw new ApplicationException("There is no current basket");
            return baskets.OrderBy(b => b.Id).Last();
        }

        public static Basket CurrentBasket(this IEnumerable<Basket> baskets)
        {
            return baskets.AsQueryable().CurrentBasket();
        }
    }
}
