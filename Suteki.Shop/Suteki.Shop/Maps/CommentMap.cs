using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class CommentMap : ClassMap<IComment>
    {
        public CommentMap()
        {
            Id(x => x.Id);

            Map(x => x.Approved);
            Map(x => x.Text).Text();
            Map(x => x.Reviewer);
            Map(x => x.Answer);
        }
    }

    public class CommentSubclassMap : SubclassMap<Comment>
    {
    }
}