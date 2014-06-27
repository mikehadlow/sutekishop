using Suteki.Common.Filters;
using Suteki.Shop.Filters;

namespace Suteki.Shop.Controllers
{
    [AdministratorsOnly, UnitOfWork]
    public class RefererController : ShopScaffoldController<Referer>
    {
         
    }
}