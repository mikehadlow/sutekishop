using System.Collections.Generic;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class Role : INamedEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        IList<User> users = new List<User>();
        public virtual IList<User> Users
        {
            get { return users; }
            set { users = value; }
        }

        public override string ToString()
        {
            return Name;
        }

        public const int AdministratorId = 1;
        public const int OrderProcessorId = 2;
        public const int CustomerId = 3;
        public const int GuestId = 4;

        // allowed roles. These must match the data in table Role
        // TODO: will this work with NH?
        public static Role Administrator { get { return new Role { Id = AdministratorId, Name = "Administrator" }; } }
        public static Role OrderProcessor { get { return new Role { Id = OrderProcessorId, Name = "Order Processor" }; } }
        public static Role Customer { get { return new Role { Id = CustomerId, Name = "Customer" }; } }
        public static Role Guest { get { return new Role { Id = GuestId, Name = "Guest" }; } }

        public virtual bool IsAdministrator { get { return Name == Administrator.Name; } }
        public virtual bool IsOrderProcessor { get { return Name == OrderProcessor.Name; } }
        public virtual bool IsCustomer { get { return Name == Customer.Name; } }
        public virtual bool IsGuest { get { return Name == Guest.Name; } }
    }
}
