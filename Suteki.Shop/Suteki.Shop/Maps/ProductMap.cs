using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Description).Text();
            Map(x => x.Price).Money();
            Map(x => x.Position);
            Map(x => x.Weight);
            Map(x => x.IsActive);
            Map(x => x.UrlName);

            HasMany(x => x.ProductCategories).Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.Sizes).Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.ProductImages).Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.Reviews).Cascade.AllDeleteOrphan().Inverse();
        }
    }
}