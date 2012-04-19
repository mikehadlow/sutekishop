using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Suteki.Common;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class Country : IOrderable, IActivatable, INamedEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public virtual string Name { get; set; }

        public virtual int Position { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual PostZone PostZone { get; set; }

        IList<Contact> contacts = new List<Contact>();
        public virtual IList<Contact> Contacts
        {
            get { return contacts; }
            set { contacts = value; }
        }

        IList<Basket> baskets = new List<Basket>();
        public virtual IList<Basket> Baskets
        {
            get { return baskets; }
            set { baskets = value; }
        }

        // TODO: will this work with NH?
        public static Country UK
        {
            get
            {
                return new Country
                {
                    Id = 1, // assuming 1 == UK
                };
            }
        }
    }
}
