using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class ImageMap : ClassMap<Image>
    {
        public ImageMap()
        {
            Id(x => x.Id);

            Map(x => x.FileName);
            Map(x => x.Description).Text();
            Map(x => x.HasOriginal);

            HasMany(x => x.ProductImages).Inverse();
        }
    }
}