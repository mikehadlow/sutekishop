using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class TopContentMap : SubclassMap<TopContent>
    {
        public TopContentMap()
        {
            Map(x => x.Text).Text();
        }
    }
}