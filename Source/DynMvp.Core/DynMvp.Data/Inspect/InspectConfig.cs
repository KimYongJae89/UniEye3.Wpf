using DynMvp.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Inspect
{
    public class InspectConfig
    {
        public bool SaveDebugImage { get; set; } = false;
        public bool SaveProbeImage { get; set; } = false;
        public bool SaveTargetImage { get; set; } = false;
        public bool SaveCameraImage { get; set; } = false;
        public bool SaveDefectImage { get; set; } = false;

        public int StepDelayMs { get; set; } = 0;
        public int ClipExtendSize { get; set; } = 0;
        public bool SaveResultFigure { get; set; } = false;

        public string ImageNameFormat { get; set; } = "Image_C{0:00}_S{1:000}_L{2:00}.bmp";

        private static InspectConfig instance = null;
        public static InspectConfig Instance()
        {
            if (instance == null)
            {
                instance = new InspectConfig();
            }

            return instance;
        }

        public void Save()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "Inspect.cfg");
            string writeString = JsonConvert.SerializeObject(instance, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(cfgPath, writeString);
        }

        public void Load()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "Inspect.cfg");
            if (File.Exists(cfgPath) == false)
            {
                return;
            }

            string readString = File.ReadAllText(cfgPath);
            instance = JsonConvert.DeserializeObject<InspectConfig>(readString);
        }
    }
}
