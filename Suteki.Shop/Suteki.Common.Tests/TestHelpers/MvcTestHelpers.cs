using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rhino.Mocks;

namespace Suteki.Common.Tests.TestHelpers
{
    public class MvcTestHelpers
    {
        public static HtmlHelper CreateMockHtmlHelper(TextWriter writer, RouteData routeData)
        {
            ViewContext viewContext;
            IViewDataContainer viewDataContainer = GetViewDataContainer(writer, routeData, out viewContext);

            return new HtmlHelper(viewContext, viewDataContainer);
        }

        public static HtmlHelper<TModel> CreateMockHtmlHelper<TModel>(TextWriter writer, RouteData routeData)
        {
            ViewContext viewContext;
            IViewDataContainer viewDataContainer = GetViewDataContainer(writer, routeData, out viewContext);

            return new HtmlHelper<TModel>(viewContext, viewDataContainer);
        }

        private static IViewDataContainer GetViewDataContainer(TextWriter writer, RouteData routeData, out ViewContext viewContext)
        {
            var mocks = new MockRepository();

            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            mocks.Record();

            var contextItems = new Hashtable();
            var form = new NameValueCollection();
            var httpContext = mocks.StrictMock<HttpContextBase>();
            var httpRequestBase = mocks.StrictMock<HttpRequestBase>();
            var httpResponseBase = mocks.StrictMock<HttpResponseBase>();
            var view = mocks.Stub<IView>();

            httpContext.Expect(context => context.Request).Return(httpRequestBase).Repeat.Any();
            httpContext.Expect(context => context.Response).Return(httpResponseBase).Repeat.Any();
            httpContext.Expect(context => context.Items).Return(contextItems).Repeat.Any();

            httpRequestBase.Expect(request => request.Form).Return(form).Repeat.Any();
            httpRequestBase.Expect(request => request.QueryString).Return(form).Repeat.Any();
            httpRequestBase.Expect(request => request.RequestType).Return("GET").Repeat.Any();

            httpResponseBase.Expect(response => response.Output).Return(writer).Repeat.Any();
            httpResponseBase.Expect(response => response.Write(null))
                .Callback<string>(s => { writer.Write(s); return true; }).Repeat.Any();

            if (routeData == null)
            {
                routeData = new RouteData();
                routeData.Values.Add("action", "Index");
                routeData.Values.Add("controller", "Home");
            }

            var controller = MockRepository.GenerateMock<ControllerBase>();

            var viewDataDictionary = new ViewDataDictionary();

            viewContext = new ViewContext(new ControllerContext(httpContext, routeData, controller), view, viewDataDictionary, new TempDataDictionary(), writer);

            var viewDataContainer = mocks.StrictMock<IViewDataContainer>();
            viewDataContainer.Expect(vdc => vdc.ViewData).Return(viewDataDictionary).Repeat.Any();

            mocks.ReplayAll();
            return viewDataContainer;
        }

        public static HtmlHelper CreateMockHtmlHelper(TextWriter writer)
        {
            return CreateMockHtmlHelper(writer, null);
        }

        public static HtmlHelper<TModel> CreateMockHtmlHelper<TModel>(TextWriter writer)
        {
            return CreateMockHtmlHelper<TModel>(writer, null);
        }

        public static HtmlHelper CreateMockHtmlHelper()
        {
            var writer = new StringWriter();
            return CreateMockHtmlHelper(writer, null);
        }

        public static HtmlHelper<TModel> CreateMockHtmlHelper<TModel>()
        {
            var writer = new StringWriter();
            return CreateMockHtmlHelper<TModel>(writer, null);
        }

    }
}