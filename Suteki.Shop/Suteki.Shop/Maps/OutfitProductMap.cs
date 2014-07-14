using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class OutfitProductMap : ClassMap<OutfitProduct>
    {
        public OutfitProductMap()
        {
            Id(x => x.Id);

            Map(x => x.Position);

            References(x => x.Outfit);
            References(x => x.Product);
        }
    }
}