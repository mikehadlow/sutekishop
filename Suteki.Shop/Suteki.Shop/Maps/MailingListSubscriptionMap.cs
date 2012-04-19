using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class MailingListSubscriptionMap : ClassMap<MailingListSubscription>
    {
        public MailingListSubscriptionMap()
        {
            Id(x => x.Id);
            Map(x => x.Email);
            Map(x => x.DateSubscribed);

            References(x => x.Contact).Cascade.All();
        }
    }
}