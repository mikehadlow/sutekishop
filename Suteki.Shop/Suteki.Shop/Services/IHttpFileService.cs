using System.Collections.Generic;

namespace Suteki.Shop.Services
{
    public interface IHttpFileService
    {
        IEnumerable<Image> GetUploadedImages(System.Web.HttpRequestBase httpRequest, params string[] imageDefinitionKeys);
    }
}
