using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

namespace Unieye.WPF.Base.Extensions
{
    public static class ResourceExtensions
    {
        public static ResourceManager _resLoader = new ResourceManager("Unieye.WPF.Base.Strings.Resources", Assembly.GetExecutingAssembly());

        public static string GetLocalized(this string resourceKey)
        {
            try
            {
                return _resLoader.GetString(resourceKey);
            }
            catch (Exception)
            {
            }

            return resourceKey;
        }
    }
}
