namespace Suteki.Common.Repositories
{
	public interface IUnitOfWorkManager
	{
		void Commit();
	}

    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        readonly ISessionManager sessionManager;

        public UnitOfWorkManager(ISessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
        }

        public void Commit()
        {
            var session = sessionManager.OpenSession();
            session.Flush();
        }
    }
}