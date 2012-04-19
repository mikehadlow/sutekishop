using NHibernate.Cfg;
using Suteki.Common.Extensions;

namespace Suteki.Shop.Database
{
    public class StaticDataGenerator
    {
        private readonly Configuration configuration;

        public StaticDataGenerator(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public void Insert()
        {
            var sessionFactory = configuration.BuildSessionFactory();
            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.ApplyTo(
                    StaticData.InsertRoles,
                    StaticData.InsertAdministrator,
                    StaticData.InsertRootCategory,
                    StaticData.InsertCardTypes,
                    StaticData.InsertOrderStatus,
                    StaticData.InsertContent,
                    StaticData.InsertPostZoneAndCountry
                    );

                transaction.Commit();
            }
        }

    }
}