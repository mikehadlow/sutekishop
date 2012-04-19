using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class SizeMap : ClassMap<Size>
    {
        public SizeMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.IsActive);
            Map(x => x.IsInStock);

            References(x => x.Product);

            HasMany(x => x.BasketItems);
        }
    }
}