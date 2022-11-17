using Basler.Pylon;
using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace DynMvp.Devices.FrameGrabber
{
    public enum TrigerSourceType { Software, Line1, Line2, Line3, FrequencyConverter }
    public enum TrigerActivation { RisingEdge, FallingEdge, LevelHigh, LevelLow }


    public abstract class Struct
    {
        public abstract void SaveXml(XmlElement xmlElement);
        public abstract void LoadXml(XmlElement xmlElement);
        //public abstract void ApplyCamera(Basler.Pylon.Camera camera);

        [Category("Struct"), Description("Name"), Browsable(false)]
        public string Name { get; private set; }

        public Struct(string name)
        {
            Name = name;
        }

        public void Load(XmlElement xmlElement, string key = "")
        {
            if (xmlElement == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(key))
            {
                Load(xmlElement[key]);
                return;
            }

            LoadXml(xmlElement);
        }

        public void Save(XmlElement xmlElement, string key = "")
        {
            if (xmlElement == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subXmlElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subXmlElement);
                Save(subXmlElement);
                return;
            }

            SaveXml(xmlElement);
        }
    }

    public class FrequencyConverterStruct : Struct
    {
        public enum ESource { Line1, Line2, Line3 }
        public enum EAlignment { RisingEdge, FallingEdge }

        [Category("FrequencyConverter"), Description("Source")]
        public ESource Source { get; set; } = ESource.Line1;

        [Category("FrequencyConverter"), Description("Alignment")]
        public EAlignment Alignment { get; set; } = EAlignment.RisingEdge;

        [Category("FrequencyConverter"), Description("PreDivider")]
        public int PreDivider { get; set; } = 1;

        [Category("FrequencyConverter"), Description("Multiplier")]
        public int Multiplier { get; set; } = 1;

        [Category("FrequencyConverter"), Description("PostDivider")]
        public int PostDivider { get; set; } = 1;

        public FrequencyConverterStruct() : base("FrequencyConverter") { }

        public override void LoadXml(XmlElement xmlElement)
        {
            Source = XmlHelper.GetValue(xmlElement, "Source", Source);
            Alignment = XmlHelper.GetValue(xmlElement, "Alignment", Alignment);
            PreDivider = XmlHelper.GetValue(xmlElement, "PreDivider", PreDivider);
            Multiplier = XmlHelper.GetValue(xmlElement, "Multiplier", Multiplier);
            PostDivider = XmlHelper.GetValue(xmlElement, "PostDivider", PostDivider);
        }

        public override void SaveXml(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Source", Source);
            XmlHelper.SetValue(xmlElement, "Alignment", Alignment);
            XmlHelper.SetValue(xmlElement, "PreDivider", PreDivider);
            XmlHelper.SetValue(xmlElement, "Multiplier", Multiplier);
            XmlHelper.SetValue(xmlElement, "PostDivider", PostDivider);
        }
    }

    public class IoControlStruct : Struct
    {
        public enum ELineMode { Input, Output }
        public enum EOutputLineSource { Off, ExposureActive, FrameTriggerWait, Timer1Active, UserOutput1, UserOutput2, AcquisitionTriggerWait, SyncUserOutput2 }

        [Category("IoControl"), Description("LineMode")]
        public ELineMode LineMode { get; set; } = ELineMode.Input;

        [Category("IoControl"), Description("OutputLineSource")]
        public EOutputLineSource OutputLineSource { get; set; } = EOutputLineSource.Off;

        [Category("IoControl"), Description("Inverter")]
        public bool InverterMode { get; set; } = false;

        [Category("IoControl"), Description("DebouncerUs")]
        public float DebouncerUs { get; set; } = 0;

        public IoControlStruct(string name) : base(name) { }

        public override void SaveXml(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "LineMode", LineMode);
            XmlHelper.SetValue(xmlElement, "OutputLineSource", OutputLineSource);
            XmlHelper.SetValue(xmlElement, "InverterMode", InverterMode);
            XmlHelper.SetValue(xmlElement, "DebouncerUs", DebouncerUs);
        }

        public override void LoadXml(XmlElement xmlElement)
        {
            LineMode = XmlHelper.GetValue(xmlElement, "LineMode", LineMode);
            OutputLineSource = XmlHelper.GetValue(xmlElement, "OutputLineSource", OutputLineSource);
            InverterMode = XmlHelper.GetValue(xmlElement, "InverterMode", InverterMode);
            DebouncerUs = XmlHelper.GetValue(xmlElement, "DebouncerUs", DebouncerUs);
        }
    }

    public class TriggerStruct : Struct
    {
        public enum EMode { Off, On }
        public enum ESource { Software, Line1, Line2, Line3, FrequencyConverter }
        public enum EActivation { RisingEdge, FallingEdge, LevelHigh, LevelLow }

        [Category("Trigger"), Description("Mode")]
        public EMode Mode { get; set; } = EMode.Off;

        [Category("Trigger"), Description("Source")]
        public ESource Source { get; set; } = ESource.Line1;

        [Category("Trigger"), Description("Activation")]
        public EActivation Activation { get; set; } = EActivation.RisingEdge;

        public TriggerStruct(string name) : base(name) { }
        public TriggerStruct(string name, EMode mode) : base(name) { Mode = mode; }

        public override void LoadXml(XmlElement xmlElement)
        {
            Mode = XmlHelper.GetValue(xmlElement, "Mode", Mode);
            Source = XmlHelper.GetValue(xmlElement, "Source", Source);
            Activation = XmlHelper.GetValue(xmlElement, "Activation", Activation);
        }

        public override void SaveXml(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Mode", Mode);
            XmlHelper.SetValue(xmlElement, "Source", Source);
            XmlHelper.SetValue(xmlElement, "Activation", Activation);
        }
    }


    public class CameraInfoPylonLine : CameraInfo
    {
        [CategoryAttribute("CameraInfoPylonLine"), DescriptionAttribute("Device ID")]
        public string DeviceUserId { get; set; }

        [CategoryAttribute("CameraInfoPylonLine"), DisplayNameAttribute("Update Device Feature"), DescriptionAttribute("Ignore ImageSize Settings in Pylon Viewer")]
        public bool UpdateDeviceFeature { get; set; }

        [CategoryAttribute("CameraInfoPylonLine"), DescriptionAttribute("IP Address")]
        public string IpAddress { get; set; }

        [CategoryAttribute("CameraInfoPylonLine"), DescriptionAttribute("Serial No")]
        public string SerialNo { get; set; }

        [CategoryAttribute("CameraInfoPylonLine"), DescriptionAttribute("Device Index")]
        public uint DeviceIndex { get; set; }

        [CategoryAttribute("CameraInfoPylonLine"), DescriptionAttribute("Model Name")]
        public string ModelName { get; set; }

        [CategoryAttribute("CameraInfoPylonLine"), DescriptionAttribute("Line Trigger")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TriggerStruct LineTriggerStruct { get; set; } = new TriggerStruct("LineStart");

        [CategoryAttribute("CameraInfoPylonLine"), DescriptionAttribute("Frame Trigger")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TriggerStruct FrameTriggerStruct { get; set; } = new TriggerStruct("FrameStart");

        [Category("CameraInfoPylonLine"), Description("DIO1")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IoControlStruct DIO1 { get; set; } = new IoControlStruct("Line1");

        [Category("CameraInfoPylonLine"), Description("DIO2")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IoControlStruct DIO2 { get; set; } = new IoControlStruct("Line2");

        [Category("CameraInfoPylonLine"), Description("DIO3")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IoControlStruct DIO3 { get; set; } = new IoControlStruct("Line3");

        [Category("CameraInfoPylonLine"), Description("FrequencyConverter")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public FrequencyConverterStruct FrequencyConverter { get; set; } = new FrequencyConverterStruct();

        [CategoryAttribute("CameraInfoPylonLine"), DescriptionAttribute("Trigger Partial Closing Frame(Only LevelHigh or LevelLow)")]
        public bool TriggerPartialClosingFrame { get; set; }

        [CategoryAttribute("CameraInfoPylonLine"), DescriptionAttribute("Use Chunk-Mode")]
        public bool UseChunkMode { get; set; }

        public CameraInfoPylonLine()
        {
            GrabberType = GrabberType.PylonLine;
            IsLineScan = true;

            DeviceUserId = "";
            IpAddress = "";
            SerialNo = "";
            DeviceIndex = 0;
            ModelName = "";
            UpdateDeviceFeature = false;

            LineTriggerStruct.Mode = TriggerStruct.EMode.Off;
            FrameTriggerStruct.Mode = TriggerStruct.EMode.Off;
            UseChunkMode = false;
            //this.useLineTrigger = false;
            //this.useFrameTrigger = false;
            //this.frameTriggerSourceType = TrigerSourceType.Line2;
            //this.frameTriggerActivation = TrigerActivation.LevelHigh;
        }

        public CameraInfoPylonLine(string deviceUserId, string ipAddress, string serialNo) : this()
        {
            GrabberType = GrabberType.PylonLine;
            IsLineScan = true;

            IpAddress = ipAddress;
            SerialNo = serialNo;
            DeviceUserId = deviceUserId;
            DeviceIndex = 0;
            ModelName = "";
            UpdateDeviceFeature = true; //세팅시 UI 갱신을 위해..

            //this.useLineTrigger = false;
            //this.lineTriggerSourceType = TrigerSourceType.Line1;
            //this.useFrameTrigger = false;
            //this.frameTriggerSourceType = TrigerSourceType.Line2;
            //this.frameTriggerActivation = TrigerActivation.LevelHigh;
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            DeviceUserId = XmlHelper.GetValue(cameraElement, "DeviceUserId", "");
            IpAddress = XmlHelper.GetValue(cameraElement, "IpAddress", "");
            SerialNo = XmlHelper.GetValue(cameraElement, "SerialNo", "");
            ModelName = XmlHelper.GetValue(cameraElement, "ModelName", "");
            UpdateDeviceFeature = XmlHelper.GetValue(cameraElement, "UpdateDeviceFeature", UpdateDeviceFeature);
            UseChunkMode = XmlHelper.GetValue(cameraElement, "UseChunkMode", false);
            TriggerPartialClosingFrame = XmlHelper.GetValue(cameraElement, "TriggerPartialClosingFrame", false);

            LineTriggerStruct.Load(cameraElement, "LineTriggerStruct");
            FrameTriggerStruct.Load(cameraElement, "FrameTriggerStruct");
            FrequencyConverter.Load(cameraElement, "FrequencyConverter");
            DIO1.Load(cameraElement, "DIO1");
            DIO2.Load(cameraElement, "DIO2");
            DIO3.Load(cameraElement, "DIO3");

            // 기존 코드 호환을 위함.
            if (XmlHelper.Exist(cameraElement, "UseLineTrigger"))
            {
                LineTriggerStruct.Mode = XmlHelper.GetValue(cameraElement, "UseLineTrigger", false) ? TriggerStruct.EMode.On : TriggerStruct.EMode.Off;
            }

            if (XmlHelper.Exist(cameraElement, "LineTriggerSourceType"))
            {
                LineTriggerStruct.Source = XmlHelper.GetValue(cameraElement, "LineTriggerSourceType", TriggerStruct.ESource.Line2);
            }

            if (XmlHelper.Exist(cameraElement, "UseFrameTrigger"))
            {
                FrameTriggerStruct.Mode = XmlHelper.GetValue(cameraElement, "UseFrameTrigger", false) ? TriggerStruct.EMode.On : TriggerStruct.EMode.Off;
            }

            if (XmlHelper.Exist(cameraElement, "FrameTriggerSourceType"))
            {
                FrameTriggerStruct.Source = XmlHelper.GetValue(cameraElement, "FrameTriggerSourceType", TriggerStruct.ESource.Line2);
            }

            if (XmlHelper.Exist(cameraElement, "FrameTriggerActivation"))
            {
                FrameTriggerStruct.Activation = XmlHelper.GetValue(cameraElement, "FrameTriggerActivation", TriggerStruct.EActivation.LevelHigh);
            }
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "DeviceUserId", DeviceUserId);
            XmlHelper.SetValue(cameraElement, "IpAddress", IpAddress);
            XmlHelper.SetValue(cameraElement, "SerialNo", SerialNo);
            XmlHelper.SetValue(cameraElement, "ModelName", ModelName);
            XmlHelper.SetValue(cameraElement, "UpdateDeviceFeature", UpdateDeviceFeature);
            XmlHelper.SetValue(cameraElement, "TriggerPartialClosingFrame", TriggerPartialClosingFrame);
            XmlHelper.SetValue(cameraElement, "UseChunkMode", UseChunkMode);

            LineTriggerStruct.Save(cameraElement, "LineTriggerStruct");
            FrameTriggerStruct.Save(cameraElement, "FrameTriggerStruct");
            FrequencyConverter.Save(cameraElement, "FrequencyConverter");
            DIO1.Save(cameraElement, "DIO1");
            DIO2.Save(cameraElement, "DIO2");
            DIO3.Save(cameraElement, "DIO3");

            //XmlHelper.SetValue(cameraElement, "UseLineTrigger", useLineTrigger);
            //XmlHelper.SetValue(cameraElement, "LineTriggerSourceType", lineTriggerSourceType.ToString());

            //XmlHelper.SetValue(cameraElement, "UseFrameTrigger", useFrameTrigger);
            //XmlHelper.SetValue(cameraElement, "FrameTriggerSourceType", frameTriggerSourceType.ToString());
            //XmlHelper.SetValue(cameraElement, "FrameTriggerActivation", frameTriggerActivation.ToString());
        }
    }

    public class GrabberPylonLine : Grabber
    {
        public GrabberPylonLine(GrabberInfo grabberInfo) : base(GrabberType.PylonLine, grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "PylonLine Device Handler Created");
        }

        public IList<Basler.Pylon.ICameraInfo> DeviceList { get; private set; }
        public override Camera CreateCamera()
        {
            return new CameraPylonLine();
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            var form = new PylonLineCameraListForm3();
            form.RequiredNumCamera = numCamera;
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        private Basler.Pylon.ICameraInfo GetDevice(CameraInfoPylonLine cameraInfo)
        {
            if (DeviceList == null)
            {
                return null;
            }

            foreach (ICameraInfo device in DeviceList)
            {
                string deviceUserId = device[Basler.Pylon.CameraInfoKey.FriendlyName];
                string ipAddress = device[Basler.Pylon.CameraInfoKey.DeviceIpAddress];
                string serialNo = device[Basler.Pylon.CameraInfoKey.SerialNumber];
                string modelName = device[Basler.Pylon.CameraInfoKey.ModelName];

                if (!string.IsNullOrEmpty(deviceUserId) && cameraInfo.DeviceUserId == deviceUserId)
                {
                    return device;
                }
                else if (!string.IsNullOrEmpty(ipAddress) && cameraInfo.IpAddress == ipAddress)
                {
                    return device;
                }
                else if (!string.IsNullOrEmpty(serialNo) && cameraInfo.SerialNo == serialNo)
                {
                    return device;
                }
            }

            return null;
        }

        public override bool Initialize()
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialzie camera(s)");

            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "5000"); // ??
            DeviceList = Basler.Pylon.CameraFinder.Enumerate();
            return true;
        }

        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {
            if ((cameraInfo is CameraInfoPylonLine) == false)
            {
                return;
            }

            var cameraInfoPylonLine = (CameraInfoPylonLine)cameraInfo;
            ICameraInfo pylonDevice = GetDevice(cameraInfoPylonLine);
            if (pylonDevice == null)
            {
                //var message = "Can't find camera. Device User Id : {0} / IP Address : {1} / SerialNo : {2}";
                string[] args = new string[] { cameraInfoPylonLine.DeviceUserId, cameraInfoPylonLine.IpAddress, cameraInfoPylonLine.SerialNo };
                //throw new AlarmException(ErrorCodeGrabber.Instance.FailToInitialize, ErrorLevel.Fatal, this.name, message, args, "");
            }

            cameraInfoPylonLine.DeviceIndex = (uint)cameraInfo.Index;//uint.Parse(pylonDevice[Basler.Pylon.CameraInfoKey.DeviceIdx]);
        }
    }
}
