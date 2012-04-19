using System;
namespace Suteki.Shop.Services
{
    public interface IImageFileService
    {
        string GetFullPath(string filename);
        string GetRelativeUrl(string filename);
    }
}
