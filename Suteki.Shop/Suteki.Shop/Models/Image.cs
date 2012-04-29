using System;
using System.Collections.Generic;
using System.IO;
using Suteki.Common.Models;

namespace Suteki.Shop
{
    public class Image : IEntity
    {
        public virtual int Id { get; set; }
        public virtual Guid FileName { get; set; }
        public virtual string Description { get; set; }
        public virtual bool HasOriginal { get; protected set; }

        public Image()
        {
            // backwards compatibility hack. Any newly created images will have the 
            // original image stored on disk, existing images from before this change
            // will not.
            HasOriginal = true;
        }

        IList<ProductImage> productImages = new List<ProductImage>();
        public virtual IList<ProductImage> ProductImages
        {
            get { return productImages; }
            set { productImages = value; }
        }
        
        public static string GetExtendedName(string path, ImageNameExtension imageNameExtension)
        {
            string extension = Enum.GetName(typeof(ImageNameExtension), imageNameExtension).ToLower();
            return string.Format("{0}.{1}{2}",
                Path.Combine(
                    Path.GetDirectoryName(path),
                    Path.GetFileNameWithoutExtension(path)),
                extension,
                Path.GetExtension(path));

        }

        public virtual string FileNameAsString
        {
            get
            {
                return FileName + ".jpg";
            }
        }

        public virtual string ThumbFileName
        {
            get
            {
                return GetExtendedName(FileNameAsString, ImageNameExtension.Thumb);
            }
        }

        public virtual string MainFileName
        {
            get
            {
                return GetExtendedName(FileNameAsString, ImageNameExtension.Main);
            }
        }

        public virtual string CategoryFileName
    	{
    		get
    		{
				return GetExtendedName(FileNameAsString, ImageNameExtension.Category);
    		}
    	}
    }

    public enum ImageNameExtension
    {
        Thumb,
        Main,
		Category
    }
}
