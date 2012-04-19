using System.Web.Mvc;
using NHibernate;
using Suteki.Common.Repositories;

namespace Suteki.Common.Filters
{
	public class UnitOfWorkAttribute : FilterUsingAttribute
	{
		public UnitOfWorkAttribute() : base(typeof (UnitOfWorkFilter))
		{
		}
	}

	public class UnitOfWorkFilter : IActionFilter
	{
        // from MVC 3, ActionFilters are cached, so we can no longer rely on having a new instance 
        // of UnitOfWorkFilter per request. SessionManager has a PerWebRequest lifestyle, but in order
        // to allow for this lifestyle in a component with a longer than request lifestyle, we use a factory.
	    private readonly ISessionManagerFactory sessionManagerFactory;
	    private readonly IPerActionTransactionStore perActionTransactionStore;

	    public UnitOfWorkFilter(IPerActionTransactionStore perActionTransactionStore, ISessionManagerFactory sessionManagerFactory)
	    {
	        this.sessionManagerFactory = sessionManagerFactory;
	        this.perActionTransactionStore = perActionTransactionStore;
	    }

	    public void OnActionExecuting(ActionExecutingContext filterContext)
	    {
	        var sessionManager = sessionManagerFactory.Resolve();
	        try
	        {
                var sesion = sessionManager.OpenSession();
                perActionTransactionStore.StoreTransaction(filterContext, sesion.BeginTransaction());
            }
	        finally
	        {
	            sessionManagerFactory.Release(sessionManager);    
	        }
	    }

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
		    var transaction = perActionTransactionStore.RetrieveTransaction(filterContext);
            if (transaction == null) return;

		    var thereWereNoExceptions = filterContext.Exception == null || filterContext.ExceptionHandled;
            if (filterContext.Controller.ViewData.ModelState.IsValid && thereWereNoExceptions)
			{
                transaction.Commit();
			}
			else
			{
                transaction.Rollback();
			}
		}
	}

    public interface IPerActionTransactionStore
    {
        void StoreTransaction(ActionExecutingContext filterContext, ITransaction transaction);
        ITransaction RetrieveTransaction(ActionExecutedContext filterContext);
    }

    public class PerActionTransactionStore : IPerActionTransactionStore
    {
        private const string transactionToken = "__transaction__";

        public void StoreTransaction(ActionExecutingContext filterContext, ITransaction transaction)
        {
            var controllerActionName =
                transactionToken +
                filterContext.Controller.GetType().Name +
                "." +
                filterContext.ActionDescriptor.ActionName;
            filterContext.RequestContext.HttpContext.Items[controllerActionName] = transaction;
        }

        public ITransaction RetrieveTransaction(ActionExecutedContext filterContext)
        {
            var controllerActionName =
                transactionToken +
                filterContext.Controller.GetType().Name +
                "." +
                filterContext.ActionDescriptor.ActionName;

            return filterContext.RequestContext.HttpContext.Items[controllerActionName] as ITransaction;
        }
    }
}