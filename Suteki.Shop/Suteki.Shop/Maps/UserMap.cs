using FluentNHibernate.Mapping;

namespace Suteki.Shop.Maps
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);

            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.IsEnabled);

            References(x => x.Role);

            HasMany(x => x.Baskets).Cascade.SaveUpdate();
            HasMany(x => x.Orders);
        }
    }
}