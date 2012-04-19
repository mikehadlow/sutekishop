using System.Web.Mvc;
using Suteki.Common.Extensions;
using Suteki.Shop.Services;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Controllers
{
    public class HeaderController : Controller
    {
        private readonly IBaseControllerService baseControllerService;

        public HeaderController(IBaseControllerService baseControllerService)
        {
            this.baseControllerService = baseControllerService;
        }

        public ViewResult Head()
        {
            return View("Head", GetHeaderViewData());
        }

        public ViewResult Header()
        {
            return View("Header", GetHeaderViewData());
        }

        public ViewResult Footer()
        {
            return View("Footer", GetHeaderViewData());
        }

        public ViewResult PageEnd()
        {
            return View("PageEnd", GetHeaderViewData());
        }

        private HeaderViewData GetHeaderViewData()
        {
            return new HeaderViewData
            {
                SiteUrl = baseControllerService.SiteUrl,
                Title = baseControllerService.ShopName,
                Email = baseControllerService.EmailAddress,
                Copyright = baseControllerService.Copyright,
                PhoneNumber = baseControllerService.PhoneNumber,
                GoogleTrackingCode = "\"{0}\"".With(baseControllerService.GoogleTrackingCode),
                SiteCss = baseControllerService.SiteCss
            };
        }
    }
}