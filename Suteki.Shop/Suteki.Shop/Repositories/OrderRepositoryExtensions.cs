using System.Linq;

namespace Suteki.Shop.Repositories
{
    public static class OrderRepositoryExtensions
    {
        public static IQueryable<Order> ByCreatedDate(this IQueryable<Order> orders)
        {
            return orders.OrderByDescending(o => o.CreatedDate);
        }

        public static IQueryable<Order> ThatMatch(this IQueryable<Order> orders, OrderSearchCriteria criteria)
        {

			orders = orders.Where(x => x.OrderStatus.Id > 0); //Exclude 'Pending'

            if (criteria.OrderId != 0)
            {
                orders = orders.Where(o => o.Id == criteria.OrderId);
            }
            if (!string.IsNullOrEmpty(criteria.Email))
            {
                orders = orders.Where(o => o.Email == criteria.Email);
            }
            if (!string.IsNullOrEmpty(criteria.Postcode))
            {
                orders = orders.Where(o => o.CardContact.Postcode == criteria.Postcode);
            }
            if (!string.IsNullOrEmpty(criteria.Lastname))
            {
                orders = orders.Where(o => o.CardContact.Lastname == criteria.Lastname);
            }
            if (criteria.OrderStatusId != 0)
            {
                orders = orders.Where(o => o.OrderStatus.Id == criteria.OrderStatusId);
            }
            return orders;
        }
    }
}
