using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class CardTypeMap : ClassMap<CardType>
    {
        public CardTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name);
            Map(x => x.RequiredIssueNumber);
            HasMany(x => x.Cards);
        }
    }
}