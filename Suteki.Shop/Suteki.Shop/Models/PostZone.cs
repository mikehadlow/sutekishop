using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Suteki.Common;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class PostZone : IOrderable, IActivatable, IEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public virtual string Name { get; set; }

        public virtual decimal Multiplier { get; set; }
        public virtual bool AskIfMaxWeight { get; set; }
        public virtual int Position { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Money FlatRate { get; set; }

        IList<Country> countries = new List<Country>();
        public virtual IList<Country> Countries
        {
            get { return countries; }
            set { countries = value; }
        }
    }
}
