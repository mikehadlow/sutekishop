using System;
using NHibernate;

namespace Suteki.Common.Repositories
{
    public class SessionManager : ISessionManager, IDisposable
    {
        private readonly ISessionFactory sessionFactory;
        private ISession session;
        private bool disposed = false;

        public SessionManager(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public ISession OpenSession()
        {
            return session ?? (session = sessionFactory.OpenSession());
        }

        public void Dispose()
        {
            if (disposed) return;

            if (session != null)
            {
                session.Close();
            }

            disposed = true;
        }
    }
}