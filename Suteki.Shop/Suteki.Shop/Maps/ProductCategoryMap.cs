using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class ProductCategoryMap : ClassMap<ProductCategory>
    {
        public ProductCategoryMap()
        {
            Id(x => x.Id);

            Map(x => x.Position);

            References(x => x.Product);
            References(x => x.Category);
        }
    }
}