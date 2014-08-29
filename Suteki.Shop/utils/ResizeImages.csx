#r "System.Drawing.dll"

using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;

var productPhotosDirectory = @"Z:\Source\sutekishop\Suteki.Shop\Suteki.Shop\ProductPhotos\";

// ResizeSingleImage();
ResizeDirectory();

public void ResizeSingleImage() {
	var oldPath = @"c:\TEMP\testimage.jpg";
	var newPath = @"c:\TEMP\testimage.new.jpg";

	Resize(oldPath, newPath, 900, 900);

	Console.WriteLine("Resize complete.");
}

public void ResizeDirectory() {
	Console.WriteLine("Starting Image Resize for directory {0}", productPhotosDirectory);

	var regexMain = new Regex(@"^([A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12})\.main\.jpg$", RegexOptions.IgnoreCase);

	var directory = new DirectoryInfo(productPhotosDirectory);
	var count = 0;
	var largeCount = 0;

	foreach(var file in directory.GetFiles()) {
		var matchResult = regexMain.Match(file.Name);
		if(matchResult.Success) {
			var guid = matchResult.Groups[1].Value;

			var largePath = Path.Combine(productPhotosDirectory, guid + ".jpg");
			var mainPath = Path.Combine(productPhotosDirectory, guid + ".main.jpg");
			var thumbPath = Path.Combine(productPhotosDirectory, guid + ".thumb.jpg");
			var newThumbPath = thumbPath + ".new";
			var newMainPath = mainPath + ".new";

			if(File.Exists(largePath)) {
				largeCount++;
				Resize(largePath, newThumbPath, 164, 164);
				Resize(largePath, newMainPath, 450, 551);
			} else {
				Resize(mainPath, newThumbPath, 164, 164);
				Resize(mainPath, newMainPath, 450, 551);
			}

			File.Move(mainPath, mainPath + ".old");
			File.Move(thumbPath, thumbPath + ".old");

			File.Move(newMainPath, mainPath);
			File.Move(newThumbPath, thumbPath);

			count++;
		}
	}

	Console.WriteLine("Number of .main.jpg files found: {0}", count);
	Console.WriteLine("Number of matching .jpg files found: {0}", largeCount);
}

public void Resize(string oldPath, string newPath, int width, int height) {
	using(var oldImage = Image.FromFile(oldPath)) {

		var x = 0;
		var y = 0;
		var h = 0;
		var w = 0;

		var oldImageRatio = (float)oldImage.Width / (float)oldImage.Height;
		var targetRatio = (float)width / (float)height;

		if(oldImageRatio >= targetRatio) {
			// constrain by height
			h = oldImage.Height;
			w = (int)(oldImage.Height * targetRatio);
			x = (int)((oldImage.Width - w) / 2);
		} else {
			// constrain by width
			h = (int)(oldImage.Width / targetRatio);
			w = oldImage.Width;
			y = (oldImage.Height - h) / 2;
		}

		// Console.WriteLine("x {0}, y {1}, w {2}, h {3}", x, y, w, h);

		using (var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb)) {
			bitmap.SetResolution(oldImage.HorizontalResolution, oldImage.VerticalResolution);
			using (var graphics = Graphics.FromImage(bitmap))
			{
				graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(oldImage,
					new Rectangle(0, 0, width, height),
					new Rectangle(x, y, w, h),
					GraphicsUnit.Pixel);
			}

			bitmap.Save(newPath, ImageFormat.Jpeg);
		}
	}
}