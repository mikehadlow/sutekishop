using System;
using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Shop.Services;
using System.IO;
using System.Reflection;

namespace Suteki.Shop.Tests.Services
{
    [TestFixture]
    public class ImageServiceTests
    {
        ImageService imageService;

        /// <summary>
        /// This test fails when using the resharper test runner because the file can't be found in
        /// _extectuing_assembly_path\TestImages
        /// It works fine with TestDriven.NET which I prefer to use :P
        /// </summary>
        [Test]
        public void CreateSizedImages_ShouldCreateImagesOfTheCorrectSize()
        {
            var image = new Image { FileName = new Guid("46af1390-4cff-4741-a1d1-3c87b425bac9") };

            var imageFolderPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImages");

            var imageFileService = MockRepository.GenerateStub<ImageFileService>();
            imageFileService.Expect(ifs => ifs.GetImageFolderPath()).Return(imageFolderPath);

			var imageDefs = new[]
			{
				new ImageDefinition(ImageDefinition.ProductImage, 500, 500, ImageNameExtension.Main),
				new ImageDefinition(ImageDefinition.ProductThumbnail, 100, 100, ImageNameExtension.Thumb)
			};

            imageService = new ImageService(imageFileService, imageDefs);

            var jpgMainPath = Path.Combine(imageFolderPath, image.MainFileName);
            var jpgThumbPath = Path.Combine(imageFolderPath, image.ThumbFileName);

            imageService.CreateSizedImages(image, ImageDefinition.ProductImage, ImageDefinition.ProductThumbnail);

            Assert.IsTrue(File.Exists(jpgMainPath));
            Assert.IsTrue(File.Exists(jpgThumbPath));

            Console.WriteLine(jpgMainPath);

            //File.Delete(jpgMainPath);
            //File.Delete(jpgThumbPath);
        }


        public void GuidGenerator()
        {
            Console.WriteLine(Guid.NewGuid().ToString());
        }
    }
}
