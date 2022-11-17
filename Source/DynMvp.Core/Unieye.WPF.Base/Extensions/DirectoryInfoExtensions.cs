using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Unieye.WPF.Base.Helpers;

namespace Unieye.WPF.Base.Extensions
{
    public static class DirectoryInfoExtensions
    {
        private const string FileExtension = ".json";

        public static async Task SaveAsync<T>(this DirectoryInfo directoryInfo, string name, T content, params JsonConverter[] converters)
        {
            string path = Path.Combine(directoryInfo.FullName, name + FileExtension);

            string fileContent = await Json.StringifyAsync(content, converters);

            await Task.Run(() => File.WriteAllText(path, fileContent));
        }

        public static async Task<T> ReadAsync<T>(this DirectoryInfo directoryInfo, string name, params JsonConverter[] converters)
        {
            string path = Path.Combine(directoryInfo.FullName, name + FileExtension);

            if (!File.Exists(path))
            {
                return default(T);
            }

            string fileContent = await Task<string>.Run(() => File.ReadAllText(path));
            return await Json.ToObjectAsync<T>(fileContent, converters);
        }

        public static async Task RemoveAsync(this DirectoryInfo directoryInfo, string name)
        {
            string path = Path.Combine(directoryInfo.FullName, name + FileExtension);

            if (File.Exists(path))
            {
                await Task.Run(() =>
                {
                    using (new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose)) { };
                });
            }
        }
    }
}
