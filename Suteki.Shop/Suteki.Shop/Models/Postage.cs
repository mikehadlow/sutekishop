using System.ComponentModel.DataAnnotations;
using Suteki.Common;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class Postage : IOrderable, IActivatable, IEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public virtual string Name { get; set; }

        public virtual int MaxWeight { get; set; }
        public virtual Money Price { get; set; }
        public virtual int Position { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
