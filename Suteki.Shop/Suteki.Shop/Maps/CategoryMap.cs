using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Position);
            Map(x => x.IsActive);
            Map(x => x.UrlName);

            References(x => x.Parent);
            References(x => x.Image).Cascade.SaveUpdate();
            
            HasMany(x => x.Categories).KeyColumn("ParentId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.ProductCategories);
        }
    }
}