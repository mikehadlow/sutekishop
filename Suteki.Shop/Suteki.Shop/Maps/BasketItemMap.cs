using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class BasketItemMap : ClassMap<BasketItem>
    {
        public BasketItemMap()
        {
            Id(x => x.Id);

            Map(x => x.Quantity);

            References(x => x.Basket);
            References(x => x.Size);
        }
    }
}