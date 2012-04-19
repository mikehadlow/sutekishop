using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate.Cfg;

namespace Suteki.Common.Repositories
{
    public interface IMappingConfigurationContributor
    {
        void ConfigureMappings(MappingConfiguration mappingConfiguration);
    }

    public class FluentNHibernateConfigurationBuilder : IConfigurationBuilder
    {
        private const string conectionStringKey = "SutekiShopConnectionString";
        private readonly IMappingConfigurationContributor[] configurationContributors;

        public FluentNHibernateConfigurationBuilder(IMappingConfigurationContributor[] configurationContributors)
        {
            this.configurationContributors = configurationContributors;
        }

        public Configuration GetConfiguration()
        {
            return BuildConfiguration(MsSqlConfiguration.MsSql2005.ConnectionString(c => 
                c.FromConnectionStringWithKey(conectionStringKey)));
        }

        public Configuration BuildConfiguration(IPersistenceConfigurer persistenceConfigurer)
        {
            return Fluently.Configure()
                .Database(persistenceConfigurer)
                .Mappings(ConfigureMappings)
                .BuildConfiguration();
        }

        private void ConfigureMappings(MappingConfiguration mappingConfiguration)
        {
            ConfigureMappings(mappingConfiguration, configurationContributors);
        }

        public static void ConfigureMappings(MappingConfiguration mappingConfiguration, params IMappingConfigurationContributor[] configurationContributors)
        {
            mappingConfiguration.FluentMappings
                .Conventions.Add(
                    ForeignKey.EndsWith("Id"),
                    PrimaryKey.Name.Is(x => x.EntityType.Name + "Id"),
                    DefaultCascade.None());

            foreach (var configurationContributor in configurationContributors)
            {
                configurationContributor.ConfigureMappings(mappingConfiguration);
            }
        }
    }
}