using System.Collections.Specialized;
using System.Web;

namespace Suteki.Common.Services
{
    /// <summary>
    /// Provides HttpContext IoC stylie
    /// </summary>
    public class HttpContextService : IHttpContextService
    {
        public HttpContextBase Context
        {
            get
            {
                return new HttpContextWrapper(HttpContext.Current);
            }
        }

        public HttpRequestBase Request
        {
            get
            {
                return Context.Request;
            }
        }

        public HttpResponseBase Response
        {
            get
            {
                return Context.Response;
            }
        }

        public NameValueCollection FormOrQuerystring
        {
            get
            {
                if(Request.RequestType == "POST")
                {
                    return Request.Form;
                }
                return Request.QueryString;
            }
        }
    }
}