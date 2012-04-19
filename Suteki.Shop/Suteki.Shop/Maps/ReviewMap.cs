using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class ReviewMap : SubclassMap<Review>
    {
        public ReviewMap()
        {
//            Id(x => x.Id);
//
//            Map(x => x.Approved);
//            Map(x => x.Text).Text();
//            Map(x => x.Rating);
//            Map(x => x.Reviewer);

            References(x => x.Product);
        }
    }
}