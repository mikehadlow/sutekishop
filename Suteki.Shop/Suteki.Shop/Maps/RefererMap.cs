using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class RefererMap : ClassMap<Referer>
    {
        public RefererMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Position);
            Map(x => x.IsActive);

            HasMany(x => x.Orders);
        }
    }
}