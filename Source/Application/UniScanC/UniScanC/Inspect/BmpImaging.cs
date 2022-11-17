using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace UniScanC.Inspect
{
    public static class BmpImaging
    {
        public static BitmapSource CreateBitmapSourceFromBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }

            if (Application.Current.Dispatcher == null)
            {
                return null; // Is it possible?
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    // You need to specify the image format to fill the stream. 
                    // I'm assuming it is PNG
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // Make sure to create the bitmap in the UI thread
                    if (InvokeRequired)
                    {
                        return (BitmapSource)Application.Current.Dispatcher.Invoke(
                            new Func<Stream, BitmapSource>(CreateBitmapSourceFromStream),
                            System.Windows.Threading.DispatcherPriority.Normal,
                            memoryStream);
                    }

                    return CreateBitmapSourceFromStream(memoryStream);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool InvokeRequired => System.Windows.Threading.Dispatcher.CurrentDispatcher != Application.Current.Dispatcher;

        private static BitmapSource CreateBitmapSourceFromStream(Stream stream)
        {
            var bitmapDecoder = BitmapDecoder.Create(
                stream,
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.OnLoad);

            // This will disconnect the stream from the image completely...
            var writable = new WriteableBitmap(bitmapDecoder.Frames.Single());
            writable.Freeze();

            return writable;
        }

        public static void SaveBitmapSource(BitmapSource bitmapSource, string path)
        {
            BitmapEncoder encoder = new PngBitmapEncoder(); // BmpBitmapEncoder는 대용량 저장시 Overflow Exception 발생.
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                encoder.Save(fs);
            }
        }
    }
}
