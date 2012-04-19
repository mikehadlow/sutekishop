using System.Collections.Specialized;
using System.Web;

namespace Suteki.Common.Services
{
    public interface IHttpContextService
    {
        HttpContextBase Context { get; }
        HttpRequestBase Request { get; }
        HttpResponseBase Response { get; }
        NameValueCollection FormOrQuerystring { get; }
    }
}