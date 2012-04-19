using System.Linq;
using System.Web.Mvc;
using Castle.Core.Logging;
using Suteki.Common.Extensions;
using Suteki.Common.Models;
using Suteki.Common.Services;

namespace Suteki.Shop.Controllers
{
    public class TestController : ControllerBase
    {
        private readonly OrderController orderController;
        private readonly ILogger logger;
        private readonly IEmailSender emailSender;

        public TestController(OrderController orderController, ILogger logger, IEmailSender emailSender)
        {
            this.orderController = orderController;
            this.logger = logger;
            this.emailSender = emailSender;
        }

        public ActionResult Index()
        {
/*
            string html = this.CaptureActionHtml(orderController, "Print", c => (ViewResult)c.Item(8));

            logger.Info(html);

*/
            // do a redirect
            return RedirectToRoute(new {Controller = "Order", Action = "Item", Id = 8});
        }

        public ActionResult Email()
        {
            string toAddress = BaseControllerService.EmailAddress;
            emailSender.Send(toAddress, "Hello from Suteki Shop", "The email body", false);
            return Content("Email sent to {0}".With(toAddress));
        }

        public ActionResult UriInfo()
        {
            var url = System.Web.HttpContext.Current.Request.Url;
            var output = "AbsoluteUri: {0}, LocalPath: {1}".With(url.AbsoluteUri, url.LocalPath);
            return Content(output);
        }

        public ActionResult MoneyBindingTest(Money money)
        {
            if (ModelState.IsValid)
            {
                return Content(string.Format("Bound to Money value: {0}", money));
            }
            return Content(ModelState.SelectMany(ms => ms.Value.Errors).Aggregate("", (agg, error) => agg + error.ErrorMessage + " "));
        }
    }
}
