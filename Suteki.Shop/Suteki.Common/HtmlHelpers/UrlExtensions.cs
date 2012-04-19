using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Suteki.Common.HtmlHelpers
{
    public static class UrlExtensions
    {
        public static string Action<T>(this UrlHelper helper, Expression<Action<T>> action) where T : Controller
        {
            var routeValues = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(action);
            return helper.RouteUrl(routeValues);
        }

        // http://blog.veggerby.dk/2009/01/13/getting-an-absolute-url-from-aspnet-mvc/
        //
        public static Uri GetBaseUrl(this UrlHelper url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            if (url.RequestContext == null)
            {
                throw new ArgumentNullException("url.RequestContext");
            }
            if (url.RequestContext.HttpContext == null)
            {
                throw new ArgumentNullException("url.RequestContext.HttpContext");
            }

            var contextUri = new Uri(url.RequestContext.HttpContext.Request.Url, url.RequestContext.HttpContext.Request.RawUrl);
            var realmUri = new UriBuilder(contextUri) { Path = url.RequestContext.HttpContext.Request.ApplicationPath, Query = null, Fragment = null };
            return realmUri.Uri;
        }

        // http://blog.veggerby.dk/2009/01/13/getting-an-absolute-url-from-aspnet-mvc/
        //
        public static string ActionAbsolute(this UrlHelper url, string actionName, string controllerName)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            if (actionName == null)
            {
                throw new ArgumentNullException("actionName");
            }
            if (controllerName == null)
            {
                throw new ArgumentNullException("controllerName");
            }

            return new Uri(GetBaseUrl(url), url.Action(actionName, controllerName)).AbsoluteUri;
        }

        public static string ActionAbsolute<TController>(this UrlHelper urlHelper, Expression<Action<TController>> controllerExpression)
            where TController : Controller
        {
            if (urlHelper == null)
            {
                throw new ArgumentNullException("urlHelper");
            }
            if (controllerExpression == null)
            {
                throw new ArgumentNullException("controllerExpression");
            }

            return new Uri(GetBaseUrl(urlHelper), urlHelper.Action(controllerExpression)).AbsoluteUri;
        }

        public static string ContentAbsolute(this UrlHelper url, string contentPath)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }
            if (contentPath == null)
            {
                throw new ArgumentNullException("contentPath");
            }

            return new Uri(GetBaseUrl(url), url.Content(contentPath)).AbsoluteUri;
        }
    }
}