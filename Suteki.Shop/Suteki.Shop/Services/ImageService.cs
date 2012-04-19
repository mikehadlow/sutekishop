using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Suteki.Common.Extensions;

namespace Suteki.Shop.Services
{
    public class ImageService : IImageService
    {
        readonly ImageDefinition[] imageDefinitions;
        readonly IImageFileService imageFileService;

        public ImageService(IImageFileService imageFileService, ImageDefinition[] imageDefinitions)
        {
            this.imageFileService = imageFileService;
        	this.imageDefinitions = imageDefinitions;
        }

        /// <summary>
        /// Creates a main and thumnail image from the given image of the taget size
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imageDefinitionKeys"></param>
        public void CreateSizedImages(Image image, params string[] imageDefinitionKeys)
        {
            var imagePath = imageFileService.GetFullPath(image.FileNameAsString);

            using (var gdiImage = System.Drawing.Image.FromFile(imagePath))
            {
				foreach(var imageDefinitionKey in imageDefinitionKeys)
				{
					var key = imageDefinitionKey;
					var imageDef = imageDefinitions.SingleOrDefault(x => x.Key == key);
					if(imageDef == null)
					{
						throw new InvalidOperationException("Could not find image definition with the key '{0}'".With(imageDefinitionKey));
					}

					CreateSizedImage(gdiImage, imageDef.Size, Image.GetExtendedName(imagePath, imageDef.Extension));
				}
            }
        }

        private void CreateSizedImage(System.Drawing.Image image, ImageSize targetSize, string targetPath)
        {
            var imageComparer = new ImageComparer(image, targetSize);
            ImageSize newImageSize = null;

            newImageSize = imageComparer.IsLandscape ? imageComparer.LandscapeSize : imageComparer.PortraitSize;

            using (var bitmap = new Bitmap(newImageSize.Width, newImageSize.Height, PixelFormat.Format24bppRgb))
            {
                bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(image,
                        new Rectangle(0, 0, newImageSize.Width, newImageSize.Height),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);
                }

                bitmap.Save(targetPath, ImageFormat.Jpeg);
            }
        }
    }

    public class ImageSize
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public ImageSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    public class ImageComparer
    {
        readonly System.Drawing.Image sourceImage;
        readonly ImageSize targetSize;

        public ImageComparer(System.Drawing.Image sourceImage, ImageSize targetSize)
        {
            this.sourceImage = sourceImage;
            this.targetSize = targetSize;
        }

        public float WidthRatio { get { return (float)targetSize.Width / (float)sourceImage.Width; } }
        public float HeightRatio { get { return (float)targetSize.Height / (float)sourceImage.Height; } }
        public bool IsLandscape { get { return HeightRatio >= WidthRatio; } }
        
        public ImageSize LandscapeSize 
        { 
            get 
            { 
                return new ImageSize((int)(sourceImage.Width * WidthRatio), (int)(sourceImage.Height * WidthRatio)); 
            } 
        }

        public ImageSize PortraitSize
        {
            get
            {
                return new ImageSize((int)(sourceImage.Width * HeightRatio), (int)(sourceImage.Height * HeightRatio));
            }
        }
    }

	public class ImageDefinition
	{

		public const string ProductImage = "product.image";
		public const string ProductThumbnail = "product.thumbnail.image";
		public const string CategoryImage = "category.image";

		public ImageDefinition(string key, int width, int height, ImageNameExtension imageNameExtension)
		{
			Key = key;
			Size = new ImageSize(width, height);
			Extension = imageNameExtension;
		}

		public string Key { get; private set; }
		public ImageSize Size { get; private set; }
		public ImageNameExtension Extension { get; private set; }
	}
}
