using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Devices
{
    public class LightConfig
    {
        public string[] LightDeviceNameList { get; set; } = null;

        public int NumLightType { get; set; } = 1;
        public int LightStableTime { get; set; } = 50;
        public LightParamSet LightParamSet { get; set; } = new LightParamSet();

        private LightParam liveLightParam = null;
        public int LiveExposureTimeUs { get; set; }

        private static LightConfig _instance;
        public static LightConfig Instance()
        {
            if (_instance == null)
            {
                _instance = new LightConfig();
            }

            return _instance;
        }

        private LightConfig()
        {

        }

        public void Initialize()
        {
            int numLight = DeviceConfig.Instance().GetNumLight();
            LightDeviceNameList = new string[numLight];
            LightParamSet.Initialize(numLight, NumLightType);
        }

        public void SetLiveLightParam(LightParam lightParam)
        {
            liveLightParam = lightParam;
        }

        public LightParam GetLiveLightParam()
        {
            if (liveLightParam == null)
            {
                liveLightParam = (LightParam)LightParamSet[0].Clone();
            }

            return liveLightParam;
        }

        public void Load()
        {
            string fileName = string.Format(@"{0}\LightConfig.xml", BaseConfig.Instance().ConfigPath);
            if (File.Exists(fileName) == false)
            {
                File.Create(fileName);
            }

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
            {
                return;
            }

            int numLight = DeviceConfig.Instance().GetNumLight();

            XmlElement element = xmlDocument["LightSettings"];

            XmlElement lightDeviceNamesElement = element["LightDeviceNames"];
            if (lightDeviceNamesElement != null)
            {
                for (int i = 0; i < numLight; i++)
                {
                    LightDeviceNameList[i] = XmlHelper.GetValue(lightDeviceNamesElement, string.Format("LightDeviceName{0}", i), string.Format("Light {0}", i + 1));
                }
            }

            XmlElement lightParamSetElement = element["LightParamSet"];
            if (lightParamSetElement != null)
            {
                LightParamSet.Load(lightParamSetElement);
            }
        }

        public void Save()
        {
            string fileName = string.Format(@"{0}\LightConfig.xml", BaseConfig.Instance().ConfigPath);

            int numLight = DeviceConfig.Instance().GetNumLight();

            var xmlDocument = new XmlDocument();
            XmlElement lightSettingsElement = xmlDocument.CreateElement("", "LightSettings", "");
            xmlDocument.AppendChild(lightSettingsElement);

            // Save Light Name
            XmlElement lightNamesElement = xmlDocument.CreateElement("", "LightNames", "");
            lightSettingsElement.AppendChild(lightNamesElement);

            if (LightDeviceNameList != null)
            {
                for (int i = 0; i < numLight; i++)
                {
                    XmlHelper.SetValue(lightNamesElement, string.Format("DeviceLightName{0}", i), LightDeviceNameList[i]);
                }
            }

            // Save Lignt Value
            if (LightParamSet != null)
            {
                XmlElement lightParamSetElement = xmlDocument.CreateElement("", "LightParamSet", "");
                lightSettingsElement.AppendChild(lightParamSetElement);

                LightParamSet.Save(lightParamSetElement);
            }

            XmlHelper.Save(xmlDocument, fileName);
        }
    }
}
