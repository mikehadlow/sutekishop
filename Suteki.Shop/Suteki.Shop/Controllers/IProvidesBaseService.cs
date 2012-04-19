using Suteki.Shop.Services;

namespace Suteki.Shop.Controllers
{
    public interface IProvidesBaseService
    {
        IBaseControllerService BaseControllerService { get; set; }
    }
}
