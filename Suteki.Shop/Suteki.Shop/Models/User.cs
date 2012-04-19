using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Security.Principal;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class User : IPrincipal, IEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual Role Role { get; set; }

        IList<Order> orders = new List<Order>();
        public virtual IList<Order> Orders
        {
            get { return orders; }
            set { orders = value; }
        }

        IList<Basket> baskets = new List<Basket>();
        public virtual IList<Basket> Baskets
        {
            get { return baskets; }
            set { baskets = value; }
        }

        public virtual void AddBasket(Basket basket)
        {
            basket.User = this;
            baskets.Add(basket);
        }
        
        public static User Guest
        {
            get
            {
                return new User { Email = "Guest@guest.com", Role = Role.Guest };
            }
        }

    	public static User DefaultUser
    	{
			get { return new User { Email = "", Password = "", Role = Role.Administrator, IsEnabled = true }; }
    	}

        public virtual string PublicIdentity
        {
            get
            {
                if (CanLogin) return Email;
                return "Guest";
            }
        }

        public virtual bool CanLogin { get { return IsAdministrator || IsOrderProcessor; } }

        public virtual IIdentity Identity
        {
            get
            {
                var isAuthenticated = Role.Name != Role.Guest.Name;
                return new Identity(isAuthenticated, Email);
            }
        }

        public virtual bool IsInRole(string role)
        {
            return Role.Name == role;
        }

        public virtual bool IsAdministrator { get { return Role.Id == Role.AdministratorId; } }
        public virtual bool IsOrderProcessor { get { return Role.Id == Role.OrderProcessorId; } }
        public virtual bool IsCustomer { get { return Role.Id == Role.CustomerId; } }

        public virtual void EnsureCanView(IAmOwnedBy amOwnedBy)
        {
            if (!IsAdministrator)
            {
                if (amOwnedBy.User.Id != Id)
                {
                    throw new ApplicationException("You are attempting to view an item that is not owned by you");
                }
            }
        }
    }

    /// <summary>
    /// Simple implementation of IIdentity
    /// </summary>
    public class Identity : IIdentity
    {
        readonly bool isAuthenticated;
        readonly string name;

        public Identity(bool isAuthenticated, string name)
        {
            this.isAuthenticated = isAuthenticated;
            this.name = name;
        }

        public string AuthenticationType
        {
            get { return "Forms"; }
        }

        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
        }

        public string Name
        {
            get { return name; }
        }
    }
}
