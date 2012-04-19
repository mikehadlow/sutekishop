using System;
using System.Web.Mvc;

namespace Suteki.Shop.Tests
{
    public class MockViewEngine : IViewEngine
    {
        public ViewContext ViewContext { get; private set; }

        public void RenderView(ViewContext viewContext)
        {
            ViewContext = viewContext;
        }

    	public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
    	{
    		throw new NotImplementedException();
    	}

    	public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
    	{
    		throw new NotImplementedException();
    	}

    	public void ReleaseView(ControllerContext controllerContext, IView view)
    	{
    		throw new NotImplementedException();
    	}
    }
}
