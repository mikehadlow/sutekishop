using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class BasketMap : ClassMap<Basket>
    {
        public BasketMap()
        {
            Id(x => x.Id);

            Map(x => x.OrderDate);

            References(x => x.Country);
            References(x => x.User);

            HasMany(x => x.BasketItems).Cascade.AllDeleteOrphan().Inverse();
        }
    }
}