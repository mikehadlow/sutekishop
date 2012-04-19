using FluentNHibernate.Cfg;
using Suteki.Common.Repositories;
using Suteki.Shop.Maps;

namespace Suteki.Shop.Repositories
{
    public class SutekiShopMappingConfiguration : IMappingConfigurationContributor
    {
        public void ConfigureMappings(MappingConfiguration mappingConfiguration)
        {
            mappingConfiguration.FluentMappings
                .AddFromAssembly(typeof (ProductMap).Assembly);
        }
    }
}