using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class CardMap : ClassMap<Card>
    {
        public CardMap()
        {
            Id(x => x.Id);
            Map(x => x.ExpiryMonth);
            Map(x => x.ExpiryYear);
            Map(x => x.Holder);
            Map(x => x.IssueNumber);
            Map(x => x.Number).Length(500);
            Map(x => x.SecurityCode).Length(500);
            Map(x => x.StartMonth);
            Map(x => x.StartYear);

            References(x => x.CardType);
            HasMany(x => x.Orders);
        }
    }
}