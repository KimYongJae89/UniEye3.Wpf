using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.Spectrometer;
using DynMvp.Devices.Spectrometer.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UniEye.Base.Config;
using UniScanC.Models;

namespace WPF.ThicknessMeasure.Override
{
    public enum SystemType
    {
        InLine = 0, StandAlone = 1
    }

    public enum HeadType
    {
        OneHead = 0, TwoHead = 1
    }

    public enum MotionPosition
    {
        STA, END, BAK, REF, MS
    }

    // 필요한 운영 환경 변수 추가하면 됨.
    public class SystemConfig
    {
        #region 속성
        private static SystemConfig _instance;
        public static SystemConfig Instance => _instance ?? (_instance = new SystemConfig());

        public int SelectedWidth { get; set; } = 0;

        // Model Param
        public Dictionary<string, List<LayerParam>> LayerParamList { get; set; } = new Dictionary<string, List<LayerParam>>();

        // Scan Width
        public List<ScanWidth> ScanWidthList { get; set; } = new List<ScanWidth>();

        // Spectrometer
        public SpectrometerProperty SpectrometerProperty { get; set; } = new SpectrometerProperty();

        public bool UseRefraction { get; set; } = false;

        public string ReflectionSpectrometer { get; set; } = string.Empty;

        public float AngleValue { get; set; } = 0;

        // Machine
        public MelsecInfo MelsecInfo { get; set; } = new MelsecInfo();

        public TcpIpInfo DataBaseTcpIpInfo { get; set; } = new TcpIpInfo();

        public TcpIpInfo MQTTTcpIpInfo { get; set; } = new TcpIpInfo();

        public AxisPosition ReferencePos { get; set; } = new AxisPosition(1);

        public AxisPosition BackGroundPos { get; set; } = new AxisPosition(1);

        public AxisPosition MasterSamplePos { get; set; } = new AxisPosition(1);

        public double HomeStartSpeed { get; set; } = 100;

        public double HomeEndSpeed { get; set; } = 10;

        public double JogSpeed { get; set; } = 100;

        public double MeasureSecond { get; set; } = 5;

        public double MovingSpeed { get; set; } = 200;

        public float SensorOffset { get; set; } = 0;

        public float PositionOffset { get; set; } = 0;

        // PortMap Out
        public int TowerLampRed { get; set; } = 16;

        public int TowerLampYellow { get; set; } = 17;

        public int TowerLampGreen { get; set; } = 18;

        public int TowerLampBuzzer { get; set; } = 19;

        public int HalogenLampVIS { get; set; } = 20;

        public int HalogenLampNIR { get; set; } = 21;

        // PortMap In
        public int EmergencyFront { get; set; } = 17;

        public int EmergencyBack { get; set; } = 18;

        public int AreaSensor { get; set; } = 19;

        // Sample
        public double Sample { get; set; } = 0;

        public double SampleRange { get; set; } = 0;

        public int MovingAverageCount { get; set; } = 0;

        // System Setting
        public SystemType SystemType { get; set; } = new SystemType();

        public HeadType HeadType { get; set; } = new HeadType();

        public string ResultPath { get; set; } = PathConfig.Instance().Result;

        public string LastLotNo { get; set; } = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        #endregion

        #region 메서드
        private void CheckInitSettings()
        {
            if (LayerParamList.Count == 0)
            {
                LayerParamList.Add("Layer1", new List<LayerParam>());
                LayerParamList["Layer1"].Add(new LayerParam());
            }

            if (ScanWidthList.Count == 0)
            {
                ScanWidthList.Add(new ScanWidth());
            }
        }

        private void SortList()
        {
            SpectrometerProperty.LayerNameList.Sort();

            foreach (KeyValuePair<string, List<LayerParam>> pair in LayerParamList)
            {
                LayerParamList[pair.Key].Sort((x, y) => x.ParamName.CompareTo(y.ParamName));
            }

            ScanWidthList.Sort((x, y) => x.Name.CompareTo(y.Name));
        }

        public void Load()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "SystemConfig.cfg");

            if (File.Exists(cfgPath) == true)
            {
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                string readString = File.ReadAllText(cfgPath);
                _instance = JsonConvert.DeserializeObject<SystemConfig>(readString, settings);
            }
            else
            {
                using (File.Create(cfgPath))
                {
                    _instance = new SystemConfig();
                }

                Save();
            }

            CheckInitSettings();
        }

        public void Save()
        {
            SortList();

            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "SystemConfig.cfg");
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string writeString = JsonConvert.SerializeObject(_instance, Newtonsoft.Json.Formatting.Indented, settings);
            File.WriteAllText(cfgPath, writeString);
        }
        #endregion
    }
}
