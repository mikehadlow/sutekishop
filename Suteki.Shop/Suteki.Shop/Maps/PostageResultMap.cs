using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class PostageResultMap : ComponentMap<PostageResult>
    {
        public PostageResultMap()
        {
            Map(x => x.Description);
            Map(x => x.Phone);
            Map(x => x.Price).Money();
        }
    }
}