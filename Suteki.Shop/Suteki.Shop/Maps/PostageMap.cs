using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class PostageMap : ClassMap<Postage>
    {
        public PostageMap()
        {
            Id(x => x.Id);
            Map(x => x.IsActive);
            Map(x => x.MaxWeight);
            Map(x => x.Name);
            Map(x => x.Position);
            Map(x => x.Price).Money();
        }
    }
}