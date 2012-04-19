using System;

namespace Suteki.Shop.Services
{
    public interface IImageService
    {
        void CreateSizedImages(Image image, params string[] imageDefinitionKeys);
    }
}
