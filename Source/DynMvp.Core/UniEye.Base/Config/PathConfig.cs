using DynMvp.Base;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml;

namespace UniEye.Base.Config
{
    public class PathConfig
    {
        public string Model { get; set; }
        public string Result { get; set; }
        public string RemoteResult { get; set; }
        public string Log { get; set; }
        public string Temp { get; set; }
        public string TitleBar { get; set; }
        public string ProductLogo { get; set; }
        public string CompanyLogo { get; set; }

        private static PathConfig instance;
        public static PathConfig Instance()
        {
            if (instance == null)
            {
                instance = new PathConfig();
            }

            return instance;
        }

        private PathConfig()
        {
            string curDir = BaseConfig.Instance().ConfigPath;

            RemoteResult = $"{curDir}\\..\\RemoteResult";
            Result = $"{curDir}\\..\\Result";
            Model = $"{curDir}\\..\\Model";
            Temp = $"{curDir}\\..\\Temp";
            Log = $"{curDir}\\..\\Log";
        }

        private void CreateDirectory()
        {
            if (Directory.Exists(Model) == false)
            {
                Directory.CreateDirectory(Model);
            }

            if (Directory.Exists(Result) == false)
            {
                Directory.CreateDirectory(Result);
            }

            if (Directory.Exists(Log) == false)
            {
                Directory.CreateDirectory(Log);
            }

            if (Directory.Exists(Log) == false)
            {
                Directory.CreateDirectory(Temp);
            }
        }

        public void Save()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "Path.cfg");

            string writeString = JsonConvert.SerializeObject(instance, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(cfgPath, writeString);
        }

        public void Load()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "Path.cfg");
            if (File.Exists(cfgPath) == false)
            {
                return;
            }

            string readString = File.ReadAllText(cfgPath);
            instance = JsonConvert.DeserializeObject<PathConfig>(readString);

            instance.CreateDirectory();
        }
    }
}
