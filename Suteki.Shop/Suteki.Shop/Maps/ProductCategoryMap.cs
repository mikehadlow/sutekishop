using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class ProductCategoryMap : ClassMap<ProductCategory>
    {
        public ProductCategoryMap()
        {
            Id(x => x.Id);

            References(x => x.Product);
            References(x => x.Category);
        }
    }
}