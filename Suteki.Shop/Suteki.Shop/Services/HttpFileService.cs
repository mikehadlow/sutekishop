using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using Suteki.Common.Validation;

namespace Suteki.Shop.Services
{
    public class HttpFileService : IHttpFileService
    {
        readonly IImageFileService imageFileService;
        readonly IImageService imageService;

        public HttpFileService(IImageFileService imageFileService, IImageService imageService)
        {
            this.imageFileService = imageFileService;
            this.imageService = imageService;
        }

        public IEnumerable<Image> GetUploadedImages(HttpRequestBase request, params string[] imageDefinitionKeys)
        {
            var images = new List<Image>();

            foreach (string inputTagName in request.Files)
            {
                var file = request.Files[inputTagName];
                if (file.ContentLength > 0)
                {
                    // upload the image to filesystem
                    if (IsNotImage(file))
                    {
                        throw new ValidationException(string.Format("File '{0}' is not an image file (*.jpg)", file.FileName));
                    }

                    var image = new Image
                    {
                        FileName = Guid.NewGuid(),
                        Description = Path.GetFileName(file.FileName)
                    };

                    file.SaveAs(imageFileService.GetFullPath(image.FileNameAsString));

                    // convert the image to main and thumb sizes
                    imageService.CreateSizedImages(image, imageDefinitionKeys);

                    File.Delete(imageFileService.GetFullPath(image.FileNameAsString));

                    images.Add(image);
                }
            }

            return images;
        }

        private static bool IsNotImage(HttpPostedFileBase file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            return (extension != ".jpg");
        }
    }
}
