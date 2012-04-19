using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Suteki.Shop.ViewData;

namespace Suteki.Shop.Controllers
{
    public class RsdController : ControllerBase
    {
        public ActionResult Index()
        {
            RsdViewData viewData = new RsdViewData
            {
                SiteUrl = base.BaseControllerService.SiteUrl
            };
            return View("Index", viewData);
        }
    }
}
