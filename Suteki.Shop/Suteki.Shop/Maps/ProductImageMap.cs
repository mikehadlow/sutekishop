using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class ProductImageMap : ClassMap<ProductImage>
    {
        public ProductImageMap()
        {
            Id(x => x.Id);

            Map(x => x.Position);

            References(x => x.Product);
            References(x => x.Image).Cascade.All();
        }
    }
}