using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Recoding.Graphics;

namespace TCG.ImageManipulator
{
    public class ImageManager
    {
        public static byte[] CropImage(string OrigFileName, byte[] rawImageBytes, int x, int y, int width, int height)
        {
            BitmapImage OriginalImage = GetImage(rawImageBytes);
            var croppedImage = OriginalImage.Crop(x, y, width, height);
            return croppedImage.SaveAsBytes(OrigFileName);
        }

        public static byte[] ResizeImage(string OrigFileName, byte[] rawImageBytes,int width, int height)
        {
            BitmapImage OriginalImage = GetImage(rawImageBytes);
            var scalledImage = OriginalImage.Scale(width,height);
            return scalledImage.SaveAsBytes(OrigFileName);
        }

        private static BitmapImage GetImage(byte[] rawImageBytes)
        {
            BitmapImage imageSource = null;
            try
            {
                using (MemoryStream byteStream = new MemoryStream(rawImageBytes))
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.StreamSource = byteStream;
                    bi.EndInit();

                    byteStream.Close();

                    return bi;
                }
            }
            catch (System.Exception ex)
            {
            }

            return imageSource;
        }
    }
}
