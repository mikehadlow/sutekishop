using System.Web.Mvc;
using Suteki.Common.Services;

namespace Suteki.Common.Controllers
{
    [HandleError]
    public abstract class ControllerBase : Controller
    {
        public IDebugWritingService DebugWritingService { get; set; }
    }
}