using Authentication.Core;
using Authentication.Core.Sources;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Inspect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Translation.Helpers;

namespace UniEye.Base.Util
{
    public static class ApplicationHelper
    {
        public static void BeginInvokeIfRequired(this ISynchronizeInvoke obj, MethodInvoker action)
        {
            if (obj.InvokeRequired)
            {
                obj.BeginInvoke(action, new object[0]);
            }
            else
            {
                action();
            }
        }

        public static bool CheckLicense()
        {
            var haspHelper = new HaspHelper();
            try
            {
                if (haspHelper.CheckLicense() == false)
                {
                    return false;
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("can't find hasp_net_windows.dll");
            }

            return true;
        }

        public static void InitAuthentication()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init User.");
            string userPath = Path.Combine(BaseConfig.Instance().ConfigPath, "User.json");
            UserHandler.Instance.Initialize(new UserJsonSource(userPath));
        }

        public static void LoadStyleLibrary()
        {
            // Load the style library
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string styleLibraryPath = new List<string>(assembly.GetManifestResourceNames()).Find(i => i.EndsWith(".isl"));
            if (string.IsNullOrEmpty(styleLibraryPath) == false)
            {
                using (Stream stream = assembly.GetManifestResourceStream(styleLibraryPath))
                {
                    if (stream != null)
                    {
                        Infragistics.Win.AppStyling.StyleManager.Load(stream);
                    }
                }
            }
        }

        public static void InitStringTable()
        {
            string localeCode = UiConfig.Instance().GetLocaleCode();
            string configPath = BaseConfig.Instance().ConfigPath;

            var stringTable = new StringTable("UniEyeTouch");

            if (localeCode == "")
            {
                StringManager.Load($"{configPath}\\StringTable.xml");
            }
            else
            {
                string stringTablepath = $"{configPath}\\StringTable_{localeCode}.xml";
                StringManager.Load(stringTablepath);
                LogHelper.Debug(LoggerType.Error, string.Format("Load StringTable : {0}", stringTablepath));
            }

            //StringManager.AddStringTable(stringTable);
        }

        public static void LoadSettings()
        {
            BaseConfig.Instance().Init();
            OperationConfig.Instance().Load();
            UiConfig.Instance().Load();
            TranslationHelper.Instance.CurrentCultureInfo = UiConfig.Instance().LoadLanguage();
            DeviceConfig.Instance().Load();
            PathConfig.Instance().Load();
            InspectConfig.Instance().Load();
        }

        public static bool InitLogSystem()
        {
            string debugLogPath = Path.Combine(PathConfig.Instance().Log, "debug.log");
            if (File.Exists(debugLogPath) == true)
            {
                string[] logLines = File.ReadAllLines(debugLogPath);

                if (logLines.Last().Contains("Program is closed normally") == false)
                {
                    string bakFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "-debug.log";
                    try
                    {
                        File.Copy(debugLogPath, Path.Combine(Path.GetDirectoryName(debugLogPath), bakFileName));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error on copy debug log.\n" + e.Message);
                    }
                }
            }

            string logConfigFile = string.Format("{0}\\log4net.xml", BaseConfig.Instance().ConfigPath);
            if (File.Exists(logConfigFile) == false)
            {
                //MessageForm.Show(null, "Can't find log configuration file.\n\n" + logConfigFile);
                return false;
            }
            LogHelper.InitializeLogSystem(logConfigFile);

            return true;
        }
    }
}
