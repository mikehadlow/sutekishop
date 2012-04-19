using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class OrderStatusMap : ClassMap<OrderStatus>
    {
        public OrderStatusMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name);

            HasMany(x => x.Orders);
        }
    }
}