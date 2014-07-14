using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class OutfitMap : ClassMap<Outfit>
    {
        public OutfitMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Description).Text();
            Map(x => x.Position);
            Map(x => x.IsActive);
            Map(x => x.UrlName);

            HasMany(x => x.OutfitImages).Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.OutfitProducts).Cascade.AllDeleteOrphan().Inverse();
        }
    }
}