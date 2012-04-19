using System;
using System.Web;
using System.Web.Routing;

namespace Suteki.Common.Utils
{
    public interface IUriBuilder
    {
        string CreateUriFromRouteValues(object values);
    }

    public class UriBuilder : IUriBuilder
    {
        private readonly Func<RouteCollection> getRouteCollection;
        private readonly Func<HttpContextBase> getHttpContext;

        public UriBuilder(Func<RouteCollection> getRouteCollection, Func<HttpContextBase> getHttpContext)
        {
            this.getRouteCollection = getRouteCollection;
            this.getHttpContext = getHttpContext;
        }

        public string CreateUriFromRouteValues(object values)
        {
            var routeValues = new RouteValueDictionary(values);

            var routeData = new RouteData();
            var requestContext = new RequestContext(getHttpContext(), routeData);
            var virtualPathData = getRouteCollection().GetVirtualPath(requestContext, routeValues);
            if (virtualPathData == null)
            {
                throw new ApplicationException("virtualPathData is null");
            }
            return virtualPathData.VirtualPath;
        }
    }
}