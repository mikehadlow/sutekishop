using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class OutfitImageMap : ClassMap<OutfitImage>
    {
        public OutfitImageMap()
        {
            Id(x => x.Id);

            Map(x => x.Position);

            References(x => x.Outfit);
            References(x => x.Image).Cascade.All();
        }
    }
}