using System;
using System.Web.Mvc;
using Suteki.Common.Extensions;
using Suteki.Common.Windsor;

namespace Suteki.Common.Filters
{
	/// <summary>
	/// Filter attribute that wraps an inner filter. The inner filter is constructed by the ServiceLocator for the current IoC container.
	/// Eg FilterUsing(typeof(MyFilter)) where MyFilter implements IActionFilter and is registered with the container.
	/// </summary>
	public class FilterUsingAttribute : FilterAttribute, IAuthorizationFilter, IActionFilter, IResultFilter
	{
		private readonly Type filterType;
		private object instantiatedFilter;

		public FilterUsingAttribute(Type filterType)
		{
			if(!IsFilterType(filterType))
			{
				throw new InvalidOperationException("Type '{0}' is not valid within the FilterUsing attribute as it is not a filter type.".With(filterType.Name));
			}
			this.filterType = filterType;
		}

		private bool IsFilterType(Type type)
		{
			return typeof(IAuthorizationFilter).IsAssignableFrom(type) 
			       || typeof(IActionFilter).IsAssignableFrom(type) 
			       || typeof(IResultFilter).IsAssignableFrom(type);
		}

		public Type FilterType
		{
			get { return filterType; }
		}

		private T GetFilter<T>() where T : class
		{
			if(instantiatedFilter == null)
			{
			    instantiatedFilter = IocContainer.Resolve(filterType);
			}

			return instantiatedFilter as T;
		}

		private void ExecuteFilterWhenItIs<TFilter>(Action<TFilter> action) where TFilter :class 
		{
			var filter = GetFilter<TFilter>();

			if(filter != null)
			{
				action(filter);
			}
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			ExecuteFilterWhenItIs<IAuthorizationFilter>(f => f.OnAuthorization(filterContext));
		}

		public void OnResultExecuting(ResultExecutingContext filterContext)
		{
			ExecuteFilterWhenItIs<IResultFilter>(f => f.OnResultExecuting(filterContext));
		}

		public void OnResultExecuted(ResultExecutedContext filterContext)
		{
			ExecuteFilterWhenItIs<IResultFilter>(f => f.OnResultExecuted(filterContext));
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
			ExecuteFilterWhenItIs<IActionFilter>(f => f.OnActionExecuting(filterContext));
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			ExecuteFilterWhenItIs<IActionFilter>(f => f.OnActionExecuted(filterContext));
		}
	}
}