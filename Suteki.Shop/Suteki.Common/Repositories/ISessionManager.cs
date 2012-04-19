using NHibernate;

namespace Suteki.Common.Repositories
{
    public interface ISessionManagerFactory
    {
        ISessionManager Resolve();
        void Release(ISessionManager sessionManager);
    }

    public interface ISessionManager
    {
        ISession OpenSession();
    }
}