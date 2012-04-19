using System;
using System.Web;

namespace Suteki.Shop.StockControl.AddIn
{
    public delegate DateTime Now();

    public delegate string CurrentUser();

    public static class SimpleServices
    {
        public static string CurrentUser()
        {
            if (HttpContext.Current == null)
            {
                throw new StockControlException("Can only get current user in the context of a web application");
            }
            return HttpContext.Current.User.Identity.Name;
        }
    }
}