using System.ComponentModel.DataAnnotations;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class OrderAdjustment : IEntity
    {
        public virtual int Id { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public virtual Money Amount { get; set; }
    }
}