using Suteki.Common.Repositories;

namespace Suteki.Shop.Services
{
    public interface IBaseControllerService
    {
        IRepository<Category> CategoryRepository { get; }
        string GoogleTrackingCode { get; set; }
        string ShopName { get; set; }
        string EmailAddress { get; set; }
        string SiteUrl { get; }
        string MetaDescription { get; set; }
        string Copyright { get; set; }
        string PhoneNumber { get; set; }
        string SiteCss { get; set; }
        string FacebookUserId { get; set; }
    }
}
