using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class CountryMap : ClassMap<Country>
    {
        public CountryMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.IsActive);
            Map(x => x.Position);

            References(x => x.PostZone);

            HasMany(x => x.Baskets);
            HasMany(x => x.Contacts);
        }
    }
}