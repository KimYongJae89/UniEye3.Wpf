using DynMvp.Base;
using DynMvp.Devices.BarcodeReader;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Daq;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices
{
    public class DeviceConfig
    {
        public DigitalIoInfoList DigitalIoInfoList { get; set; } = new DigitalIoInfoList();
        public GrabberInfoList GrabberInfoList { get; set; } = new GrabberInfoList();
        public MotionInfoList MotionInfoList { get; set; } = new MotionInfoList();
        public LightCtrlInfoList LightCtrlInfoList { get; set; } = new LightCtrlInfoList();
        public DaqChannelPropertyList DaqChannelPropertyList { get; set; } = new DaqChannelPropertyList();

        public int Version { get; set; }
        public string ConfigPath { get; set; }
        public bool VirtualMode { get; set; } = false;

        public bool UseBarcodeReader { get; set; } = false;
        public BarcodeReaderType BarcodeReaderType { get; set; } = BarcodeReaderType.Serial;
        public SerialPortInfo BarcodeReaderPortInfo { get; set; } = new SerialPortInfo();

        public bool UseTowerLamp { get; set; } = false;
        public int TowerLampUpdateInterval { get; set; } = 100;

        public bool UseRobotStage { get; set; } = false;
        public bool UseConveyorMotor { get; set; } = false;
        public bool UseConveyorSystem { get; set; } = false;

        public bool UseSoundBuzzer { get; set; } = false;
        public bool UseOpPanel { get; set; } = false;
        public bool UseRejectPusher { get; set; } = false;
        public bool UseDoorSensor { get; set; } = false;

        // LineScan 카메라 사용시, 라인 이송 속도
        public float LineSpeed { get; set; } = 1000;

        public int NumImageBuffer { get; set; } = 1;

        public int GrabTimeoutMs { get; set; } = 3000;
        public int MoveTimeoutMs { get; set; } = 100000;
        public int OriginTimeoutMs { get; set; } = 100000;

        public int LightStableTimeMs { get; set; } = 100;
        public int DefaultExposureTimeMs { get; set; } = 8;
        public int IoMonitorCheckInterval { get; set; } = 100;

        public bool UseAutoFocus { get; set; } = false;
        public int AutoFocusVender { get; set; } = 0;
        private SerialPortInfo AutoFocusSerialInfo { get; set; } = new SerialPortInfo();

        private static DeviceConfig instance = null;
        public static DeviceConfig Instance()
        {
            if (instance == null)
            {
                instance = new DeviceConfig();
            }

            return instance;
        }

        public void Save()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "Device.cfg");

            var jss = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string writeString = JsonConvert.SerializeObject(instance, Newtonsoft.Json.Formatting.Indented, jss);
            File.WriteAllText(cfgPath, writeString);
        }

        public void Load()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "Device.cfg");
            if (File.Exists(cfgPath) == false)
            {
                return;
            }

            string readString = File.ReadAllText(cfgPath);

            var jss = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            instance = JsonConvert.DeserializeObject<DeviceConfig>(readString, jss);

            if (instance.GrabberInfoList == null)
            {
                instance.GrabberInfoList = new GrabberInfoList();
            }

            if (instance.DigitalIoInfoList == null)
            {
                instance.DigitalIoInfoList = new DigitalIoInfoList();
            }

            if (instance.MotionInfoList == null)
            {
                instance.MotionInfoList = new MotionInfoList();
            }

            if (instance.LightCtrlInfoList == null)
            {
                instance.LightCtrlInfoList = new LightCtrlInfoList();
            }

            if (instance.DaqChannelPropertyList == null)
            {
                instance.DaqChannelPropertyList = new DaqChannelPropertyList();
            }
        }

        public int GetNumCamera()
        {
            return GrabberInfoList.GetNumCamera();
        }

        public int GetNumLight()
        {
            return LightCtrlInfoList == null ? 0 : LightCtrlInfoList.Sum(x => x.NumChannel);
        }

        /* 아래 코드 적용 가능성 있음.
         * 저장 기능 확인 후 제거
                private void LoadDaqChannelProperty(XmlElement machineElement)
                {
                    daqChannelPropertyList.Clear();

                    XmlNodeList daqChannelPropertyNodeList = machineElement.GetElementsByTagName("DaqChannelProperty");
                    if (daqChannelPropertyNodeList.Count > 0)
                    {
                        foreach (XmlNode daqChannelPropertyNode in daqChannelPropertyNodeList)
                        {
                            DaqChannelProperty daqChannelProperty = new DaqChannelProperty();
                            daqChannelProperty.LoadXml((XmlElement)daqChannelPropertyNode);

                            if (daqChannelProperty.DaqChannelType != DaqChannelType.None)
                                daqChannelPropertyList.Add(daqChannelProperty);
                        }
                    }
                }

                private void LoadDigitalIoInfo(XmlElement machineElement)
                {
                    digitalIoInfoList.Clear();

                    DigitalIoType digitalIoType = (DigitalIoType)Enum.Parse(typeof(DigitalIoType), XmlHelper.GetValue(machineElement, "DigitalIoType", "Adlink7432"));

                    //XmlNodeList digitalIoInfoNodeList = machineElement.GetElementsByTagName("DigitalIoInfoList");
                    XmlNodeList digitalIoInfoNodeList = machineElement.GetElementsByTagName("DigitalIoInfo");
                    if (digitalIoInfoNodeList.Count > 0)
                    {
                        foreach (XmlNode digitalIoInfoNode in digitalIoInfoNodeList)
                        {
                            DigitalIoType type = (DigitalIoType)Enum.Parse(typeof(DigitalIoType), XmlHelper.GetValue((XmlElement)digitalIoInfoNode, "Type", "Adlink7230"));

                            if (DigitalIoFactory.IsSlaveDevice(type))
                            {
                                SlaveDigitalIoInfo digitalIoInfo = new SlaveDigitalIoInfo();
                                digitalIoInfo.LoadXml((XmlElement)digitalIoInfoNode);

                                digitalIoInfoList.Add(digitalIoInfo);
                            }
                            else
                            {
                                DigitalIoInfo digitalIoInfo = new DigitalIoInfo();
                                digitalIoInfo.LoadXml((XmlElement)digitalIoInfoNode);

                                digitalIoInfoList.Add(digitalIoInfo);
                            }
                        }
                    }
                }

                private void LoadMotionInfo(XmlElement machineElement)
                {
                    motionInfoList.Clear();

                    XmlNodeList motionInfoNodeList = machineElement.GetElementsByTagName("MotionInfo");
                    if (motionInfoNodeList.Count > 0)
                    {
                        foreach (XmlNode motionInfoNode in motionInfoNodeList)
                        {
                            MotionType motionType = (MotionType)Enum.Parse(typeof(MotionType), XmlHelper.GetValue((XmlElement)motionInfoNode, "Type", "None"));

                            MotionInfo motionInfo = MotionInfoFactory.CreateMotionInfo(motionType);
                            motionInfo.LoadXml((XmlElement)motionInfoNode);

                            motionInfoList.Add(motionInfo);
                        }
                    }
                }

                private void LoadGrabberInfo(XmlElement machineElement)
                {
                    grabberInfoList.Clear();

                    GrabberType grabberType = (GrabberType)Enum.Parse(typeof(GrabberType), XmlHelper.GetValue(machineElement, "CameraManagerType", "Pylon"));
                    XmlNodeList grabberInfoNodeList = machineElement.GetElementsByTagName("GrabberInfo");
                    if (grabberInfoNodeList.Count > 0)
                    {
                        foreach (XmlNode grabberInfoNode in grabberInfoNodeList)
                        {
                            GrabberInfo grabberInfo = new GrabberInfo();
                            grabberInfo.LoadXml((XmlElement)grabberInfoNode);

                            grabberInfoList.Add(grabberInfo);
                        }
                    }
                }

                private void LoadLightInfo(XmlElement machineElement)
                {
                    lightCtrlInfoList.Clear();

                    XmlNodeList lightInfoNodeList = machineElement.GetElementsByTagName("LightInfo");
                    if (lightInfoNodeList.Count > 0)
                    {
                        foreach (XmlNode lightInfoNode in lightInfoNodeList)
                        {
                            LightCtrlType lightCtrlType;
                            string lightCtrlTypeStr = XmlHelper.GetValue((XmlElement)lightInfoNode, "LightCtrlType", "None");
                            try
                            {
                                lightCtrlType = (LightCtrlType)Enum.Parse(typeof(LightCtrlType), lightCtrlTypeStr);
                            }
                            catch (Exception)
                            {
                                ErrorManager.Instance().Report((int)ErrorSection.Light, (int)CommonError.InvalidType,
                                    ErrorLevel.Error, ErrorSection.Light.ToString(), CommonError.InvalidType.ToString(), "Light Type : " + lightCtrlTypeStr);
                                continue;
                            }

                            LightCtrlInfo lightCtrlInfo = LightCtrlInfoFactory.Create(lightCtrlType);
                            if (lightCtrlInfo != null)
                            {
                                lightCtrlInfo.LoadXml((XmlElement)lightInfoNode);
                                lightCtrlInfoList.Add(lightCtrlInfo);
                            }
                        }
                    }
                }*/
    }
}
