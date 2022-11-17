using Basler.Pylon;
using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfoPylon2 : CameraInfo
    {
        public CameraInfoPylon2()
        {
            GrabberType = GrabberType.Pylon2;
        }
        [Category("CameraBase"), Description("Device User Id"), ReadOnly(true)]
        public string DeviceUserId { get; set; } = "";

        [Category("CameraBase"), Description("Model Name"), ReadOnly(true)]
        public string ModelName { get; set; } = "";

        private Size maxSize = new Size(1, 1);
        [Category("CameraBase"), Description("Max Width,Height"), ReadOnly(true)]
        public Size MaxSize { get => maxSize; set => maxSize = value; }

        [Category("CameraBase"), Description("Serial No"), ReadOnly(true)]
        public string SerialNo { get; set; } = "";

        [Category("CameraBase"), Description("Ip Address"), ReadOnly(true)]
        public string IpAddress { get; set; } = "";

        //[Category("CameraInfoPylon2"), Description("AnalogControls")]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public AnalogControlsStruct AnalogControls { get; set; } = new AnalogControlsStruct();

        //[Category("CameraInfoPylon2"), Description("ImageFormatControls")]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public ImageFormatControls ImageFormatControls { get; set; } = new ImageFormatControls();

        //[Category("CameraInfoPylon2"), Description("AOIControls")]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public AOIControls AOIControls { get; set; } = new AOIControls(1000, 1000);

        [Category("CameraInfoPylon2"), Description("DIO1")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IoControlStruct DIO1 { get; set; } = new IoControlStruct("Line1");

        [Category("CameraInfoPylon2"), Description("DIO3")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IoControlStruct DIO3 { get; set; } = new IoControlStruct("Line3");

        [Category("CameraInfoPylon2"), Description("Frame Trigger")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TriggerStruct FrameStart { get; set; } = new TriggerStruct("FrameStart");

        [Category("CameraInfoPylon2"), Description("Frame Trigger")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TriggerStruct AcquisitionStart { get; set; } = new TriggerStruct("AcquisitionStart");

        //[Category("CameraInfoPylon2"), Description("Line Trigger")]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public TriggerStruct LineTrigger { get; set; } = new TriggerStruct("LineStart");

        //[Category("CameraInfoPylon2"), Description("FrequencyConverter")]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public FrequencyConverterStruct FrequencyConverter { get; set; } = new FrequencyConverterStruct();



        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            XmlHelper.GetValue(cameraElement, "MaxSize", ref maxSize);
            DeviceUserId = XmlHelper.GetValue(cameraElement, "DeviceUserId", "");
            IpAddress = XmlHelper.GetValue(cameraElement, "IpAddress", "");
            SerialNo = XmlHelper.GetValue(cameraElement, "SerialNo", "");
            ModelName = XmlHelper.GetValue(cameraElement, "ModelName", "");

            DIO1.Load(cameraElement, "DIO1");
            DIO3.Load(cameraElement, "DIO3");
            FrameStart.Load(cameraElement, "FrameTrigger");
            AcquisitionStart.Load(cameraElement, "AcquisitionStart");

            ExpouserMode = XmlHelper.GetValue(cameraElement, "ExpouserMode", ExpouserMode);

            //LineTrigger.Load(cameraElement, "LineTrigger");
            //FrequencyConverter.Load(cameraElement, "FrequencyConverter");
            //AnalogControls.Load(cameraElement, "AnalogControls");
            //ImageFormatControls.Load(cameraElement, "ImageFormatControls");
            //AOIControls.Load(cameraElement, "AOIControls");
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "MaxSize", MaxSize);
            XmlHelper.SetValue(cameraElement, "DeviceUserId", DeviceUserId);
            XmlHelper.SetValue(cameraElement, "IpAddress", IpAddress);
            XmlHelper.SetValue(cameraElement, "SerialNo", SerialNo);
            XmlHelper.SetValue(cameraElement, "ModelName", ModelName);

            DIO1.Save(cameraElement, "DIO1");
            DIO3.Save(cameraElement, "DIO3");
            FrameStart.Save(cameraElement, "FrameTrigger");
            AcquisitionStart.Save(cameraElement, "AcquisitionStart");

            XmlHelper.SetValue(cameraElement, "ExpouserMode", ExpouserMode);

            //LineTrigger.Save(cameraElement, "LineTrigger");
            //FrequencyConverter.Save(cameraElement, "FrequencyConverter");
            //AnalogControls.Save(cameraElement, "AnalogControls");
            //ImageFormatControls.Save(cameraElement, "ImageFormatControls");
            //AOIControls.Save(cameraElement, "AOIControls");
        }
    }


    public class GrabberPylon2 : Grabber
    {
        public GrabberPylon2(GrabberInfo grabberInfo) : base(GrabberType.Pylon2, grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Pylon Device Handler Created");
        }

        public IList<Basler.Pylon.ICameraInfo> DeviceList { get; private set; }

        public override Camera CreateCamera()
        {
            return new CameraPylon2();
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            var form = new PylonCameraListForm2
            {
                RequiredNumCamera = numCamera,
                CameraConfiguration = cameraConfiguration
            };
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        private Basler.Pylon.ICameraInfo GetDevice(CameraInfoPylon2 cameraInfo)
        {
            if (DeviceList == null)
            {
                throw new InvalidDeviceListException();
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

#if DEBUG
            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "5000");
#endif
            DeviceList = Basler.Pylon.CameraFinder.Enumerate(Basler.Pylon.DeviceType.GigE);
            return true;
        }

        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {
            if ((cameraInfo is CameraInfoPylon2) == false)
            {
                return;
            }

            var cameraInfoPylon = (CameraInfoPylon2)cameraInfo;
            try
            {
                ICameraInfo pylonDevice = GetDevice(cameraInfoPylon);
                if (pylonDevice == null)
                {
                    string messge = string.Format("Can't find camera. Device User Id : {0} / IP Address : {1} / SerialNo : {2}", cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo);

                    MessageBox.Show(messge);
                    LogHelper.Error(messge);

                    cameraInfoPylon.Enabled = false;

                    return;
                }
            }
            catch (InvalidIpAddressException)
            {
                string messge = string.Format("Can't find camera. Device User Id : {0} / IP Address : {1} / SerialNo : {2}", cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo);

                MessageBox.Show(messge);
                LogHelper.Error(messge);

                cameraInfoPylon.Enabled = false;
            }
        }
    }

    //public abstract class Struct
    //{
    //    public abstract void SaveXml(XmlElement xmlElement);
    //    public abstract void LoadXml(XmlElement xmlElement);
    //    //public abstract void ApplyCamera(Basler.Pylon.Camera camera);

    //    public string Name { get; private set; }

    //    public Struct(string name)
    //    {
    //        this.Name = name;
    //    }

    //    public void Load(XmlElement xmlElement, string key = "")
    //    {
    //        if (xmlElement == null)
    //            return;

    //        if (!string.IsNullOrEmpty(key))
    //        {
    //            Load(xmlElement[key]);
    //            return;
    //        }

    //        LoadXml(xmlElement);
    //    }

    //    public void Save(XmlElement xmlElement, string key = "")
    //    {
    //        if (xmlElement == null)
    //            return;

    //        if (!string.IsNullOrEmpty(key))
    //        {
    //            XmlElement subXmlElement = xmlElement.OwnerDocument.CreateElement(key);
    //            xmlElement.AppendChild(subXmlElement);
    //            Save(subXmlElement);
    //            return;
    //        }

    //        SaveXml(xmlElement);
    //    }
    //}

    //public class FrequencyConverterStruct : Struct
    //{
    //    public enum ESource { Line1, Line2, Line3 }
    //    public enum EAlignment { RisingEdge, FallingEdge }

    //    [Category("FrequencyConverter"), Description("Source")]
    //    public ESource Source { get; set; } = ESource.Line1;

    //    [Category("FrequencyConverter"), Description("Alignment")]
    //    public EAlignment Alignment { get; set; } = EAlignment.RisingEdge;

    //    [Category("FrequencyConverter"), Description("PreDivider")]
    //    public float PreDivider { get; set; } = 1;

    //    [Category("FrequencyConverter"), Description("Multiplier")]
    //    public float Multiplier { get; set; } = 1;

    //    [Category("FrequencyConverter"), Description("PostDivider")]
    //    public float PostDivider { get; set; } = 1;

    //    public FrequencyConverterStruct() : base("FrequencyConverter") { }

    //    public override void LoadXml(XmlElement xmlElement)
    //    {
    //        this.Source = XmlHelper.GetValue(xmlElement, "Source", this.Source);
    //        this.Alignment = XmlHelper.GetValue(xmlElement, "Alignment", this.Alignment);
    //        this.PreDivider = XmlHelper.GetValue(xmlElement, "PreDivider", this.PreDivider);
    //        this.Multiplier = XmlHelper.GetValue(xmlElement, "Multiplier", this.Multiplier);
    //        this.PostDivider = XmlHelper.GetValue(xmlElement, "PostDivider", this.PostDivider);
    //    }

    //    public override void SaveXml(XmlElement xmlElement)
    //    {
    //        XmlHelper.SetValue(xmlElement, "Source", this.Source);
    //        XmlHelper.SetValue(xmlElement, "Alignment", this.Alignment);
    //        XmlHelper.SetValue(xmlElement, "PreDivider", this.PreDivider);
    //        XmlHelper.SetValue(xmlElement, "Multiplier", this.Multiplier);
    //        XmlHelper.SetValue(xmlElement, "PostDivider", this.PostDivider);
    //    }
    //}

    //public class IoControlStruct : Struct
    //{
    //    public enum ELineMode { Input, Output }
    //    public enum EInverterMode { Off, On }
    //    public enum EOutputLineSource { Off, ExposureActive, FrameTriggerWait, Timer1Active, UserOutput1, UserOutput2, AcquisitionTriggerWait, SyncUserOutput2 }

    //    [Category("IoControl"), Description("LineMode")]
    //    public ELineMode LineMode { get; set; } = ELineMode.Input;

    //    [Category("IoControl"), Description("OutputLineSource")]
    //    public EOutputLineSource OutputLineSource { get; set; } = EOutputLineSource.Off;

    //    [Category("IoControl"), Description("Inverter")]
    //    public EInverterMode InverterMode { get; set; } = EInverterMode.Off;

    //    [Category("IoControl"), Description("DebouncerUs")]
    //    public float DebouncerUs { get; set; } = 0;

    //    public IoControlStruct(string name) : base(name) { }

    //    public override void SaveXml(XmlElement xmlElement)
    //    {
    //        XmlHelper.SetValue(xmlElement, "LineMode", this.LineMode);
    //        XmlHelper.SetValue(xmlElement, "OutputLineSource", this.OutputLineSource);
    //        XmlHelper.SetValue(xmlElement, "InverterMode", this.InverterMode);
    //        XmlHelper.SetValue(xmlElement, "DebouncerUs", this.DebouncerUs);
    //    }

    //    public override void LoadXml(XmlElement xmlElement)
    //    {
    //        this.LineMode = XmlHelper.GetValue(xmlElement, "LineMode", this.LineMode);
    //        this.OutputLineSource = XmlHelper.GetValue(xmlElement, "OutputLineSource", this.OutputLineSource);
    //        this.InverterMode = XmlHelper.GetValue(xmlElement, "InverterMode", this.InverterMode);
    //        this.DebouncerUs = XmlHelper.GetValue(xmlElement, "DebouncerUs", this.DebouncerUs);
    //    }
    //}

    //public class TriggerStruct : Struct
    //{
    //    public enum EMode { Off, On }
    //    public enum ESource { Software, Line1, Line2, Line3, FrequencyConverter }
    //    public enum EActivation { RisingEdge, FallingEdge, LevelHigh, LevelLow }

    //    [Category("Trigger"), Description("Mode")]
    //    public EMode Mode { get; set; } = EMode.Off;

    //    [Category("Trigger"), Description("Source")]
    //    public ESource Source { get; set; } = ESource.Line1;

    //    [Category("Trigger"), Description("Activation")]
    //    public EActivation Activation { get; set; } = EActivation.RisingEdge;

    //    public TriggerStruct(string name) : base(name) { }

    //    public override void LoadXml(XmlElement xmlElement)
    //    {
    //        this.Mode = XmlHelper.GetValue(xmlElement, "Mode", this.Mode);
    //        this.Source = XmlHelper.GetValue(xmlElement, "Source", this.Source);
    //        this.Activation = XmlHelper.GetValue(xmlElement, "Activation", this.Activation);
    //    }

    //    public override void SaveXml(XmlElement xmlElement)
    //    {
    //        XmlHelper.SetValue(xmlElement, "Mode", this.Mode);
    //        XmlHelper.SetValue(xmlElement, "Source", this.Source);
    //        XmlHelper.SetValue(xmlElement, "Activation", this.Activation);
    //    }
    //}

    #region Analog Controls
    public class AnalogControlsStruct : Struct
    {
        public AnalogControlsStruct() : base("AnalogControls") { }

        public enum EGainAuto { Off, Once, Continuous }
        [Category("AnalogControls"), Description("Sets the operation mode of the Gain Auto auto function.")]
        public EGainAuto GainAuto { get; set; } = EGainAuto.Off;

        [Category("AnalogControls"), Description("Sets the gain type to be adjusted.")]
        public string GainSelector { get; set; } = "All";

        [Category("AnalogControls"), Description("Value of the currently selected gain (raw value).")]
        public int GainRaw { get; set; } = 0;

        [Category("AnalogControls"), Description("Sets which sensor tap can be configured.")]
        public string BlackLevelSelector { get; set; } = "All";

        [Category("AnalogControls"), Description("Black level value to be applied to the currently selected sensor tap (raw value).")]
        public int BlackLevelRaw { get; set; } = 0;

        [Category("GammaEnable"), Description("Enables gamma correction.")]
        public bool GammaEnable { get; set; } = false;

        public enum EGammaSelector { User, sRGB }
        [Category("GammaEnable"), Description("Sets the type of gamma to be applied.")]
        public EGammaSelector GammaSelector { get; set; } = EGammaSelector.User;

        [Category("Gamma"), Description("Gamma correction to be applied.")]
        public float Gamma { get; set; } = 1.0F;

        [Category("DigitalShift"), Description("Digital shift to be applied.")]
        public int DigitalShift { get; set; } = 0;

        public override void LoadXml(XmlElement xmlElement)
        {
            GainAuto = XmlHelper.GetValue(xmlElement, "GainAuto", GainAuto);
            GainSelector = XmlHelper.GetValue(xmlElement, "GainSelector", GainSelector);
            GainRaw = XmlHelper.GetValue(xmlElement, "GainRaw", GainRaw);
            BlackLevelSelector = XmlHelper.GetValue(xmlElement, "BlackLevelSelector", BlackLevelSelector);
            BlackLevelRaw = XmlHelper.GetValue(xmlElement, "BlackLevelRaw", BlackLevelRaw);
            GammaEnable = XmlHelper.GetValue(xmlElement, "GammaEnable", GammaEnable);
            GammaSelector = XmlHelper.GetValue(xmlElement, "GammaSelector", GammaSelector);
            Gamma = XmlHelper.GetValue(xmlElement, "Gamma", Gamma);
            DigitalShift = XmlHelper.GetValue(xmlElement, "DigitalShift", DigitalShift);
        }
        public override void SaveXml(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "GainAuto", GainAuto);
            XmlHelper.SetValue(xmlElement, "GainSelector", GainSelector);
            XmlHelper.SetValue(xmlElement, "GainRaw", GainRaw);
            XmlHelper.SetValue(xmlElement, "BlackLevelSelector", BlackLevelSelector);
            XmlHelper.SetValue(xmlElement, "BlackLevelRaw", BlackLevelRaw);
            XmlHelper.SetValue(xmlElement, "GammaEnable", GammaEnable);
            XmlHelper.SetValue(xmlElement, "GammaEnable", GammaEnable);
            XmlHelper.SetValue(xmlElement, "GammaSelector", GammaSelector);
            XmlHelper.SetValue(xmlElement, "Gamma", Gamma);
            XmlHelper.SetValue(xmlElement, "DigitalShift", DigitalShift);
        }
    }
    #endregion

    #region Image Format Controls
    public class ImageFormatControls : Struct
    {
        public ImageFormatControls() : base("ImageFormatControls") { }

        public enum EPixelFormat { Mono8, Mono12, Mono12Packed }
        [Category("ImageFormatControls"), Description("Sets the format of the pixel data transmitted by the camera.")]
        public EPixelFormat PixelFormat { get; set; } = EPixelFormat.Mono8;

        [Category("ImageFormatControls"), Description("Enables horizontal flipping of the image.")]
        public bool ReverseX { get; set; } = false;

        [Category("ImageFormatControls"), Description("Enables vertical flipping of the image.")]
        public bool ReverseY { get; set; } = false;

        public enum ETestImageSelector { Off, Testimage1, Testimage2, Testimage3, Testimage4, Testimage5 }
        [Category("ImageFormatControls"), Description("Sets the test image to display.")]
        public ETestImageSelector TestImageSelector { get; set; } = ETestImageSelector.Off;

        [Category("ImageFormatControls"), Description("Holds all moving test images at their starting position.")]
        public bool TestImageResetAndHold { get; set; } = false;

        public override void LoadXml(XmlElement xmlElement)
        {
            PixelFormat = XmlHelper.GetValue(xmlElement, "PixelFormat", PixelFormat);
            ReverseX = XmlHelper.GetValue(xmlElement, "ReverseX", ReverseX);
            ReverseY = XmlHelper.GetValue(xmlElement, "ReverseY", ReverseY);
            TestImageSelector = XmlHelper.GetValue(xmlElement, "TestImageSelector", TestImageSelector);
            TestImageResetAndHold = XmlHelper.GetValue(xmlElement, "TestImageResetAndHold", TestImageResetAndHold);
        }

        public override void SaveXml(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "PixelFormat", PixelFormat);
            XmlHelper.SetValue(xmlElement, "ReverseX", ReverseX);
            XmlHelper.SetValue(xmlElement, "ReverseY", ReverseY);
            XmlHelper.SetValue(xmlElement, "TestImageSelector", TestImageSelector);
            XmlHelper.SetValue(xmlElement, "TestImageResetAndHold", TestImageResetAndHold);
        }
    }
    #endregion

    #region AOI Controls
    public class AOIControls : Struct
    {
        public AOIControls(int width, int height) : base("AOIControls")
        {
            Width = width;
            Height = height;
        }

        [Category("AOIControls"), Description("Width of the area of interest in pixels.")]
        public int Width { get; set; } = 1;

        [Category("AOIControls"), Description("Height of the area of interest in pixels.")]
        public int Height { get; set; } = 1;

        [Category("AOIControls"), Description("Horizontal offset from the left side of the sensor to the area of interest (in pixxels).")]
        public int XOffset { get; set; } = 0; //OffsetX

        [Category("AOIControls"), Description("Vertical offset from the top of the sensor to the area of interest (in pixxels).")]
        public int YOffset { get; set; } = 0; //OffsetY

        [Category("AOIControls"), Description("Enables horizontal centering of the image.")]
        public bool CenterX { get; set; } = false;

        [Category("AOIControls"), Description("Enables Vertical centering of the image.")]
        public bool CenterY { get; set; } = false;

        public enum EBinningHorizontalMode { Sum, Average }
        [Category("AOIControls"), Description("Sets the binning horizontal mode.")]
        public EBinningHorizontalMode BinningHorizontalMode { get; set; } = EBinningHorizontalMode.Sum;

        [Category("AOIControls"), Description("Number of adjacent horizontal pixels to be summed.")]
        public int BinningHorizontal { get; set; } = 1;

        public enum EBinningVerticalMode { Sum, Average }
        [Category("AOIControls"), Description("Sets the binning Vertical mode.")]
        public EBinningVerticalMode BinningVerticalMode { get; set; } = EBinningVerticalMode.Sum;

        [Category("AOIControls"), Description("Number of adjacent Vertical pixels to be summed.")]
        public int BinningVertical { get; set; } = 1;

        public override void LoadXml(XmlElement xmlElement)
        {
            Width = XmlHelper.GetValue(xmlElement, "Width", Width);
            Height = XmlHelper.GetValue(xmlElement, "Height", Height);
            XOffset = XmlHelper.GetValue(xmlElement, "XOffset", XOffset);
            YOffset = XmlHelper.GetValue(xmlElement, "YOffset", YOffset);
            CenterX = XmlHelper.GetValue(xmlElement, "CenterX", CenterX);
            CenterY = XmlHelper.GetValue(xmlElement, "CenterY", CenterY);
            BinningHorizontalMode = XmlHelper.GetValue(xmlElement, "BinningHorizontalMode", BinningHorizontalMode);
            BinningHorizontal = XmlHelper.GetValue(xmlElement, "BinningHorizontal", BinningHorizontal);
            BinningVerticalMode = XmlHelper.GetValue(xmlElement, "BinningVerticalMode", BinningVerticalMode);
            BinningVertical = XmlHelper.GetValue(xmlElement, "BinningVertical", BinningVertical);
        }
        public override void SaveXml(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Width", Width);
            XmlHelper.SetValue(xmlElement, "Height", Height);
            XmlHelper.SetValue(xmlElement, "XOffset", XOffset);
            XmlHelper.SetValue(xmlElement, "YOffset", YOffset);
            XmlHelper.SetValue(xmlElement, "CenterX", CenterX);
            XmlHelper.SetValue(xmlElement, "CenterY", CenterY);
            XmlHelper.SetValue(xmlElement, "BinningHorizontalMode", BinningHorizontalMode);
            XmlHelper.SetValue(xmlElement, "BinningHorizontal", BinningHorizontal);
            XmlHelper.SetValue(xmlElement, "BinningVerticalMode", BinningVerticalMode);
            XmlHelper.SetValue(xmlElement, "BinningVertical", BinningVertical);
        }
    }
    #endregion
}
