using FluentNHibernate.Mapping;
using Suteki.Common.NHibernate;

namespace Suteki.Shop.Maps
{
    public class OrderAdjustmentMap : ClassMap<OrderAdjustment>
    {
        public OrderAdjustmentMap()
        {
            Id(x => x.Id);
            Map(x => x.Amount).Money();
            Map(x => x.Description).Text();

            References(x => x.Order);
        }
    }
}