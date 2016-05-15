using System;
using System.IO;
using CoreGraphics;
using UIKit;
using System.Drawing;

namespace XamarinTest.App.iOS.DependencyService
{
    public class ImageServiceIOS : IImageService
    {
        public void ResizeImage(string sourceFile, string targetFile, float maxWidth, float maxHeight)
        {
            if (File.Exists(sourceFile) && !File.Exists(targetFile))
            {
                using (UIImage sourceImage = UIImage.FromFile(sourceFile))
                {
                    var sourceSize = sourceImage.Size;
                    var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

                    if (!Directory.Exists(Path.GetDirectoryName(targetFile)))
                        Directory.CreateDirectory(Path.GetDirectoryName(targetFile));

                    if (maxResizeFactor > 0.9)
                    {
                        File.Copy(sourceFile, targetFile);
                    }
                    else
                    {
                        var width = maxResizeFactor * sourceSize.Width;
                        var height = maxResizeFactor * sourceSize.Height;

                        UIGraphics.BeginImageContextWithOptions(new CGSize((float)width, (float)height), true, 1.0f);
                        //  UIGraphics.GetCurrentContext().RotateCTM(90 / Math.PI);
                        sourceImage.Draw(new CGRect(0, 0, (float)width, (float)height));

                        var resultImage = UIGraphics.GetImageFromCurrentImageContext();
                        UIGraphics.EndImageContext();


                        if (targetFile.ToLower().EndsWith("png"))
                            resultImage.AsPNG().Save(targetFile, true);
                        else
                            resultImage.AsJPEG().Save(targetFile, true);
                    }
                }
            }
        }

        public byte[] ResizeImage(byte[] bytes, float maxWidth, float maxHeight)
        {
			return ResizeImage(bytes,maxWidth,maxHeight,70);
        }

	    byte[] ResizeImage(byte[] imageData, float width, float height, int quality)
		{
			UIImage originalImage = ImageFromByteArray(imageData);


			float oldWidth = (float)originalImage.Size.Width;
			float oldHeight = (float)originalImage.Size.Height;
			float scaleFactor = 0f;

			if (oldWidth > oldHeight)
			{
				scaleFactor = width / oldWidth;
			}
			else
			{
				scaleFactor = height / oldHeight;
			}

			float newHeight = oldHeight * scaleFactor;
			float newWidth = oldWidth * scaleFactor;

			//create a 24bit RGB image
			using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
				(int)newWidth, (int)newHeight, 8,
				(int)(4 * newWidth), CGColorSpace.CreateDeviceRGB(),
				CGImageAlphaInfo.PremultipliedFirst))
			{

				RectangleF imageRect = new RectangleF(0, 0, newWidth, newHeight);

				// draw the image
				context.DrawImage(imageRect, originalImage.CGImage);

				UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage());

				// save the image as a jpeg
				return resizedImage.AsJPEG((float)quality).ToArray();
			}
		}

		public static UIKit.UIImage ImageFromByteArray(byte[] data)
		{
			if (data == null)
			{
				return null;
			}

			UIKit.UIImage image;
			try
			{
				image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
			}
			catch (Exception e)
			{
				Console.WriteLine("Image load failed: " + e.Message);
				return null;
			}
			return image;
		}
    }
}