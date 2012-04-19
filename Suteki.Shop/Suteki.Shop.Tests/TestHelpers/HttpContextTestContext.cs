using Rhino.Mocks;
using System.Web;

namespace Suteki.Shop.Tests
{
	//TODO: Replace this with the TestControllerBuilder
    public class HttpContextTestContext
    {
        public HttpContextBase Context { get; private set; }
        public HttpRequestBase Request { get; private set; }
        public HttpResponseBase Response { get; private set; }
        public HttpSessionStateBase Session { get; private set; }
        public HttpServerUtilityBase Server { get; private set; }

        public HttpContextTestContext()
        {
            Context = MockRepository.GenerateStub<HttpContextBase>();
            Request = MockRepository.GenerateStub<HttpRequestBase>();
            Response = MockRepository.GenerateStub<HttpResponseBase>();
            Session = MockRepository.GenerateStub<HttpSessionStateBase>();
            Server = MockRepository.GenerateStub<HttpServerUtilityBase>();

            Context.Expect(c => c.Request).Return(Request);
            Context.Expect(c => c.Response).Return(Response);
            Context.Expect(c => c.Session).Return(Session);
            Context.Expect(c => c.Server).Return(Server);
        }
    }
}
