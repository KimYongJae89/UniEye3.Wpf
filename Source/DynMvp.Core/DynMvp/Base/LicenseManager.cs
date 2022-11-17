using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Base
{
    public class LicenseManager
    {
        public static bool IsAvailable(string taskName)
        {
            bool result = false;
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Planbss\License");
            if (registryKey != null)
            {
                result = Convert.ToBoolean(registryKey.GetValue(taskName).ToString());
            }

            return result;
        }
    }
}
