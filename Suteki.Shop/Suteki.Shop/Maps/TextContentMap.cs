using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class TextContentMap : SubclassMap<TextContent>
    {
        public TextContentMap()
        {
            Map(x => x.Text).Text();
        }
    }
}