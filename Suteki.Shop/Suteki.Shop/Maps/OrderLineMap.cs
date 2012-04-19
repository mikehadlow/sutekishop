using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class OrderLineMap : ClassMap<OrderLine>
    {
        public OrderLineMap()
        {
            Id(x => x.Id);

            Map(x => x.ProductName).Length(510);
            Map(x => x.ProductUrlName);
            Map(x => x.Quantity);
            Map(x => x.Price).Money();
            Map(x => x.ProductId);
            Map(x => x.SizeName);

            References(x => x.Order);
        }
    }
}