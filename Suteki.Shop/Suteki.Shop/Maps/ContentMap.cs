using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class ContentMap : ClassMap<Content>
    {
        public ContentMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Position);
            Map(x => x.IsActive);
            Map(x => x.UrlName);

            References(x => x.ParentContent);
            // References(x => x.ContentType);

            HasMany(x => x.Contents).KeyColumn("ParentContentId").Cascade.AllDeleteOrphan().Inverse();

            DiscriminateSubClassesOnColumn("ContentType");
        }
    }
}