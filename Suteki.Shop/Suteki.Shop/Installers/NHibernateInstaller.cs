using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NHibernate;
using Suteki.Common.Repositories;

namespace Suteki.Shop.Installers
{
    public class NHibernateInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(

                // we just want a single session factory for the lifetime of the application, so create it as singleton.
                Component.For<ISessionFactory>()
                    .UsingFactoryMethod(k => k.Resolve<IConfigurationBuilder>().GetConfiguration().BuildSessionFactory())
                    .LifeStyle.Singleton,

                Component.For<IConfigurationBuilder>().ImplementedBy<FluentNHibernateConfigurationBuilder>()
                    .LifeStyle.Transient,

                // session manager is responsible for handing out sessions. It should last the lifetime of a 
                // web request.
                Component.For<ISessionManager>().ImplementedBy<SessionManager>().LifeStyle.PerWebRequest,

                // where a resolving component might last longer than a single request (ActionFilters for example)
                // get the session factory via ISessionManagerFactory
                Component.For<ISessionManagerFactory>().AsFactory()
                );
        }
    }
}