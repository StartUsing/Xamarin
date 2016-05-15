using System;
using System.IO;
using Android.Graphics;
using Android.Util;

namespace XamarinTest.App.Droid.DeependencyService
{
    public class ImageServiceDroid : IImageService
    {
        public byte[] ResizeImage(byte[] bytes, float maxWidth, float maxHeight)
        {
 
            if (bytes!=null)
            {
                // First decode with inJustDecodeBounds=true to check dimensions
                var options = new BitmapFactory.Options()
                {
                    InJustDecodeBounds = false,
                    InPurgeable = true,
                };

                using (var image = BitmapFactory.DecodeByteArray(bytes,0,bytes.Length, options))
                {
                    if (image != null)
                    {
                        var sourceSize = Tuple.Create(image.Width, image.Height);

                        var maxResizeFactor = Math.Min(maxWidth / sourceSize.Item1, maxHeight / sourceSize.Item2);


                        byte[] writeBytes = new byte[] { };
                        if (maxResizeFactor > 0.9)
                        {
                            return bytes;
                        }
                        else
                        {
                            var width = (int)(maxResizeFactor * sourceSize.Item2);
                            var height = (int)(maxResizeFactor * sourceSize.Item1);
                           
                            using (var bitmapScaled = Bitmap.CreateScaledBitmap(image, height, width, true))
                            {
                 
                                using (Stream outStream = new MemoryStream())
                                {
                                    bitmapScaled.Compress(Bitmap.CompressFormat.Jpeg, 95, outStream);
                                    outStream.Seek(0, SeekOrigin.Begin);
                                    writeBytes = new byte[outStream.Length];
                                 
                                    outStream.Read(writeBytes, 0, writeBytes.Length);
                                    // 设置当前流的位置为流的开始
                                  

                                }
                                bitmapScaled.Recycle();
                            }
                        }

                        image.Recycle();
                        return writeBytes;
                    }
                    else
                        Log.Error("","Image scaling failed: ");
            
                }
            }
            return bytes;
        }
    }
}