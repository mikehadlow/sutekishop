using System.Web.Mvc;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Common.Filters;
using Suteki.Common.Repositories;
using Suteki.Common.Tests.TestHelpers;

namespace Suteki.Common.Tests.Filters
{
	[TestFixture]
	public class UnitOfWorkFilterTester
	{
	    ISessionManagerFactory sessionManagerFactory;
	    ISessionManager sessionManager;
	    ISession session;
	    ITransaction transaction;
		UnitOfWorkFilter filter;

		[SetUp]
		public void Setup()
		{
		    sessionManagerFactory = MockRepository.GenerateStub<ISessionManagerFactory>();
		    sessionManager = MockRepository.GenerateStub<ISessionManager>();
		    session = MockRepository.GenerateStub<ISession>();
		    transaction = MockRepository.GenerateStub<ITransaction>();

		    sessionManagerFactory.Stub(x => x.Resolve()).Return(sessionManager).Repeat.Any();
		    sessionManager.Stub(s => s.OpenSession()).Return(session).Repeat.Any();
            session.Stub(s => s.BeginTransaction()).Return(transaction).Repeat.Any();

		    var perActionTransactionStore = new MockPerActionTransactionStore();

			filter = new UnitOfWorkFilter(perActionTransactionStore, sessionManagerFactory);
		}

	    [Test]
	    public void Transaction_should_be_started_when_action_is_run()
	    {
            filter.OnActionExecuting(new ActionExecutingContext { Controller = new TestController() });
            session.AssertWasCalled(s => s.BeginTransaction());
	    }

		[Test]
		public void Transaction_should_be_commited_when_action_completes()
		{
		    var controller = new TestController();
            filter.OnActionExecuting(new ActionExecutingContext { Controller = controller });
			filter.OnActionExecuted(new ActionExecutedContext { Controller = controller });
			transaction.AssertWasCalled(t => t.Commit());
		}

		[Test]
		public void Transaction_should_be_rolled_back_if_there_are_errors_in_modelstate()
		{
			var controller = new TestController();
			controller.ModelState.AddModelError("foo", "bar");
            filter.OnActionExecuting(new ActionExecutingContext { Controller = controller });
			filter.OnActionExecuted(new ActionExecutedContext { Controller = controller });
			transaction.AssertWasCalled(t => t.Rollback());
		}
	}

    public class MockPerActionTransactionStore : IPerActionTransactionStore
    {
        private ITransaction transaction;

        public void StoreTransaction(ActionExecutingContext filterContext, ITransaction transaction)
        {
            this.transaction = transaction;
        }

        public ITransaction RetrieveTransaction(ActionExecutedContext filterContext)
        {
            return transaction;
        }
    }
}