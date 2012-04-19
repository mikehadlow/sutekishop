using NHibernate.Cfg;

namespace Suteki.Common.Repositories
{
    public interface IConfigurationBuilder
    {
        Configuration GetConfiguration();
    }
}