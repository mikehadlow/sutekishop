using System.Collections.Generic;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class OrderStatus : INamedEntity
    {
        // these constants must match those in the database
        public static int PendingId { get { return 0; } }
        public static int CreatedId { get { return 1; } }
        public static int DispatchedId { get { return 2; } }
        public static int RejectedId { get { return 3; } }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        IList<Order> orders = new List<Order>();
        public virtual IList<Order> Orders
        {
            get { return orders; }
            set { orders = value; }
        }

        // TODO: Will NH work with these??
        public static OrderStatus Dispatched
        {
            get 
            { 
                return new OrderStatus { Id = DispatchedId };
            }
        }

        public static OrderStatus Created
        {
            get 
            { 
                return new OrderStatus { Id = CreatedId };
            }
        }
        
        public static OrderStatus Rejected
        {
            get 
            { 
                return new OrderStatus { Id = RejectedId };
            }
        }

        public static OrderStatus Pending
        {
            get
            {
                return new OrderStatus {Id = PendingId};
            }
        }
    }
}
