using System.Collections;
using System.Collections.Generic;
using Suteki.Common;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class Referer : INamedEntity, IOrderable, IActivatable
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Position { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual IList<Order> Orders { get; set; }

        public Referer()
        {
            Orders = new List<Order>();
        }
    }
}