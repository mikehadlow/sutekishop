using System.Web;
using System.Web.Routing;

namespace Suteki.Common.HtmlHelpers
{
    public static class RequestExtensions
    {
        public static RouteValueDictionary GetRequestValues(this HttpRequestBase request, params string[] excludes)
        {
            var values = new RouteValueDictionary();
            var requestValues = request.RequestType == "GET" ? request.QueryString : request.Form;
            foreach (var key in requestValues.AllKeys)
            {
                bool skip = false;
                foreach (var exclude in excludes)
                {
                    if (key == exclude) skip = true;
                }
                if (skip) continue;

                values.Add(key, requestValues[key]);
            }
            return values;
        }
    }
}