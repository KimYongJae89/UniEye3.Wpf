using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Unieye.WPF.Base.Extensions
{
    public static class ImageSourceExtensions
    {
        private const string FileExtension = ".png";

        public static async Task SaveAsync(this ImageSource imageSource, DirectoryInfo directoryInfo, string name)
        {
            string path = Path.Combine(directoryInfo.FullName, name + FileExtension);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await Task.Run(() =>
                {

                    BitmapEncoder encoder = new PngBitmapEncoder();

                    encoder.Frames.Add(BitmapFrame.Create(imageSource as BitmapSource));
                    encoder.Save(fileStream);
                });
            }
        }

        public static async Task<BitmapSource> ReadAsync(this ImageSource imageSource, DirectoryInfo directoryInfo, string name)
        {
            string path = Path.Combine(directoryInfo.FullName, name + FileExtension);

            if (File.Exists(path) == false)
            {
                return null;
            }

            return await Task.Run(() =>
            {
                var image = new BitmapImage(new Uri(path));
                image.Freeze();
                return image;
            });
        }

        public static void Save(this ImageSource imageSource, DirectoryInfo directoryInfo, string name)
        {
            string path = Path.Combine(directoryInfo.FullName, name + FileExtension);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(imageSource as BitmapSource));
                encoder.Save(fileStream);
            }
        }

        public static BitmapSource Read(this ImageSource imageSource, DirectoryInfo directoryInfo, string name)
        {
            string path = Path.Combine(directoryInfo.FullName, name + FileExtension);

            if (File.Exists(path) == false)
            {
                return null;
            }

            var image = new BitmapImage(new Uri(path));
            image.Freeze();

            return image;
        }
    }
}
