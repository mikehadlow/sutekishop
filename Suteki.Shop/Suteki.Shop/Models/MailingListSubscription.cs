using System;
using System.ComponentModel.DataAnnotations;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class MailingListSubscription : IEntity
	{
        public MailingListSubscription()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            DateSubscribed = DateTime.Now;
        }

        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(250, ErrorMessage = "Email cannot be more than 250 characters")]
        public virtual string Email { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual DateTime DateSubscribed { get; set; }
	}
}