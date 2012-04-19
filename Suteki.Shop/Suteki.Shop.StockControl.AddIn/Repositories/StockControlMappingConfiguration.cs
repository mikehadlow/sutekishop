using FluentNHibernate.Cfg;
using Suteki.Common.Repositories;
using Suteki.Shop.StockControl.AddIn.Models;

namespace Suteki.Shop.StockControl.AddIn.Repositories
{
    public class StockControlMappingConfiguration : IMappingConfigurationContributor
    {
        public void ConfigureMappings(MappingConfiguration mappingConfiguration)
        {
            mappingConfiguration.FluentMappings.AddFromAssemblyOf<StockItem>();
        }
    }
}