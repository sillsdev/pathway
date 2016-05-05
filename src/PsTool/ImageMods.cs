// --------------------------------------------------------------------------------------------
// <copyright file="ImageMods.cs" from='2016' to='2016' company='SIL International'>
//      Copyright ( c ) 2016, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// from http://stackoverflow.com/questions/1922040/resize-an-image-c-sharp
// </remarks>
// --------------------------------------------------------------------------------------------
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace SIL.Tool
{
    public class ImageMods
    {
        public static float DestHorizontalResolution = float.Parse("96.0");
        public static float DestVerticalResolution = float.Parse("96.0");

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(DestHorizontalResolution, DestVerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static void ResizeImage(string fromFullPath, string toFullPath, float width, float height)
        {
            Image img = new Bitmap(fromFullPath);
            var scaleh =  img.Height / (height * DestVerticalResolution);
            var scalew = img.Width / (width * DestHorizontalResolution);
            var scale = scaleh > scalew ? scaleh : scalew;
            var newHeight = (int) (img.Height / scale);
            var newWidth = (int) (img.Width / scale);
            var result = ResizeImage(img, newWidth, newHeight);
            img.Dispose();
            if (File.Exists(toFullPath))
            {
                File.Delete(toFullPath);
            }
            result.Save(toFullPath);
            result.Dispose();
        }
    }
}
