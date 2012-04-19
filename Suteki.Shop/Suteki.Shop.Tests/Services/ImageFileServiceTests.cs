using NUnit.Framework;
using Rhino.Mocks;
using Suteki.Shop.Services;
using System.IO;
using System.Reflection;

namespace Suteki.Shop.Tests.Services
{
    [TestFixture]
    public class ImageFileServiceTests
    {
        ImageFileService imageFileService;

        [Test]
        public void GetFullPath_ShouldReturnFullPage()
        {
            var imageFolderPath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "TestImages");
            const string filename = "myfile.jpg";

            imageFileService = MockRepository.GenerateStub<ImageFileService>();
            imageFileService.Expect(ifs => ifs.GetImageFolderPath()).Return(imageFolderPath);

            var path = imageFileService.GetFullPath(filename);

            var expectedPath = Path.Combine(imageFolderPath, filename);

            Assert.AreEqual(expectedPath, path);
        }
    }
}
