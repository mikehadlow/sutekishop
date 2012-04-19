using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class PostZoneMap : ClassMap<PostZone>
    {
        public PostZoneMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Multiplier);
            Map(x => x.AskIfMaxWeight);
            Map(x => x.Position);
            Map(x => x.IsActive);
            Map(x => x.FlatRate).Money();
            HasMany(x => x.Countries);
        }
    }
}