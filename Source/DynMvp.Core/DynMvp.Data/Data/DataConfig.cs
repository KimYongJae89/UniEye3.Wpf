using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Data
{
    internal class DataConfig
    {
        private static DataConfig instance = null;

        private static DataConfig Instance()
        {
            if (instance == null)
            {
                instance = new DataConfig();
            }

            return instance;
        }

        public static void Save(string configDir)
        {
            string cfgPath = Path.Combine(configDir, "Data.cfg");

            string writeString = JsonConvert.SerializeObject(instance, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(cfgPath, writeString);
        }

        public void Load(string configDir)
        {
            string cfgPath = Path.Combine(configDir, "Data.cfg");
            if (File.Exists(cfgPath) == false)
            {
                return;
            }

            string readString = File.ReadAllText(cfgPath);
            instance = JsonConvert.DeserializeObject<DataConfig>(readString);
        }
    }
}
