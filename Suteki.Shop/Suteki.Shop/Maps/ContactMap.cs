using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class ContactMap : ClassMap<Contact>
    {
        public ContactMap()
        {
            Id(x => x.Id);

            Map(x => x.Address1);
            Map(x => x.Address2);
            Map(x => x.Address3);
            Map(x => x.Firstname);
            Map(x => x.Lastname);
            Map(x => x.Postcode);
            Map(x => x.Telephone);
            Map(x => x.Town);
            Map(x => x.County);

            References(x => x.Country);
        }
    }
}