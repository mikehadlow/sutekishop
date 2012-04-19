using System;
using NUnit.Framework;

namespace Suteki.Shop.Tests.Models
{
    [TestFixture]
    public class ImageTests
    {
        [Test]
        public void GetExtendedName_ShouldCreateCorrectExtendedName()
        {
            string path = @"C:\somedirectory\subdir\myfilename.ext";
            string expected = @"C:\somedirectory\subdir\myfilename.thumb.ext";

            string result = Image.GetExtendedName(path, ImageNameExtension.Thumb);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ThumbFileName_ShouldReturnCorrectThumbName()
        {
            Guid filename = Guid.NewGuid();
            string thumbFilename = filename.ToString() + ".thumb.jpg";

            Image image = new Image { FileName = filename };
            Assert.AreEqual(thumbFilename, image.ThumbFileName);
        }

        [Test]
        public void MainFileName_ShouldReturnCorrectMainName()
        {
            Guid filename = Guid.NewGuid();
            string mainFilename = filename.ToString() + ".main.jpg";

            Image image = new Image { FileName = filename };
            Assert.AreEqual(mainFilename, image.MainFileName);
        }
    }
}
