using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Id(x => x.Id);

            Map(x => x.Email);
            Map(x => x.AdditionalInformation).Text();
            Map(x => x.UseCardHolderContact);
            Map(x => x.PayByTelephone);
            Map(x => x.CreatedDate);
            Map(x => x.DispatchedDate);
            Map(x => x.Note).Text();
            Map(x => x.ContactMe);
            Map(x => x.TrackingNumber);

            References(x => x.Card).Cascade.All();
            References(x => x.CardContact).Cascade.All();
            References(x => x.DeliveryContact).Cascade.All();
            References(x => x.OrderStatus);
            References(x => x.ModifiedBy).Cascade.All();
            References(x => x.User).Cascade.All();

            HasMany(x => x.OrderLines).Cascade.AllDeleteOrphan();
            HasMany(x => x.Adjustments).Cascade.AllDeleteOrphan();

            Component(x => x.Postage).ColumnPrefix("Postage");
        }
    }
}