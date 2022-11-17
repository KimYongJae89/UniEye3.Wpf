using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfo
    {
        protected int index;
        public int Index
        {
            get => index;
            set => index = value;
        }

        protected GrabberType grabberType;
        public GrabberType GrabberType
        {
            get => grabberType;
            set => grabberType = value;
        }

        protected bool enabled;
        public bool Enabled
        {
            get => enabled;
            set => enabled = value;
        }

        protected int width = 1000;
        public int Width
        {
            get => width;
            set => width = value;
        }

        protected int height = 1000;
        public int Height
        {
            get => height;
            set => height = value;
        }

        public Size Size => new Size(Width, Height);

        protected int exposureTimeUs = 1000;
        public int ExposureTimeUs
        {
            get => exposureTimeUs;
            set => exposureTimeUs = value;
        }

        protected ExpouserMode expouserMode = ExpouserMode.Timed;
        public ExpouserMode ExpouserMode
        {
            get => expouserMode;
            set => expouserMode = value;
        }

        protected ExpouserAuto expouserAuto = ExpouserAuto.Off;
        public ExpouserAuto ExpouserAuto
        {
            get => expouserAuto;
            set => expouserAuto = value;
        }

        protected bool mirrorX;
        public bool MirrorX
        {
            get => mirrorX;
            set => mirrorX = value;
        }

        protected bool mirrorY;
        public bool MirrorY
        {
            get => mirrorY;
            set => mirrorY = value;
        }

        protected PixelFormat pixelFormat = PixelFormat.Format8bppIndexed;
        public PixelFormat PixelFormat
        {
            get => pixelFormat;
            set => pixelFormat = value;
        }

        protected bool bayerCamera;
        public bool BayerCamera
        {
            get => bayerCamera;
            set => bayerCamera = value;
        }

        protected float[] whiteBalanceCoefficient;
        public float[] WhiteBalanceCoefficient
        {
            get => whiteBalanceCoefficient;
            set => whiteBalanceCoefficient = value;
        }

        protected BayerType bayerType;
        internal BayerType BayerType
        {
            get => bayerType;
            set => bayerType = value;
        }

        protected RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;
        public RotateFlipType RotateFlipType
        {
            get => rotateFlipType;
            set => rotateFlipType = value;
        }

        protected CustomBufferType customBufferType;
        public CustomBufferType CustomBufferType
        {
            get => customBufferType;
            set => customBufferType = value;
        }

        protected bool autoDetectMode = false;
        public bool AutoDetectMode
        {
            get => autoDetectMode;
            set => autoDetectMode = value;
        }


        protected uint frameNum;
        [CategoryAttribute("CameraInfo"), DescriptionAttribute("Frame Buffer Count")]
        public uint FrameNum
        {
            get => frameNum;
            set => frameNum = value;
        }

        protected bool useNativeBuffering;
        [Category("CameraInfo"), Description("Use image pointer instead of data")]
        public virtual bool UseNativeBuffering
        {
            get => useNativeBuffering;
            set => useNativeBuffering = value;
        }

        [Category("CameraInfo"), Description("Scan Type")]
        public bool IsLineScan { get; set; }

        public CameraInfo()
        {
            grabberType = GrabberType.Virtual;
        }

        public CameraInfo(int index, int width, int height, PixelFormat pixelFormat)
        {
            grabberType = GrabberType.Virtual;

            this.index = index;
            this.width = width;
            this.height = height;
            this.pixelFormat = pixelFormat;
        }

        public static CameraInfo Create(GrabberType grabberType)
        {
            CameraInfo cameraInfo;
            switch (grabberType)
            {
                case GrabberType.MultiCam:
                    cameraInfo = new CameraInfoMultiCam(); break;
                case GrabberType.Pylon:
                    cameraInfo = new CameraInfoPylon(); break;
                case GrabberType.Pylon2:
                    cameraInfo = new CameraInfoPylon2(); break;
                case GrabberType.PylonLine:
                    cameraInfo = new CameraInfoPylonLine(); break;
                case GrabberType.uEye:
                    cameraInfo = new CameraInfoUEye(); break;
                case GrabberType.MIL:
                    cameraInfo = new CameraInfoMil(); break;
                case GrabberType.MILCXP:
                    cameraInfo = new CameraInfoMilCXP(); break;
                case GrabberType.GenTL:
                    cameraInfo = new CameraInfoGenTL(); break;
                case GrabberType.Sapera:
                    cameraInfo = new CameraInfoSapera(); break;
                default:
                case GrabberType.Virtual:
                    cameraInfo = new CameraInfoVirtual(); break;
            }

            return cameraInfo;
        }

        public int GetNumBand()
        {
            switch (pixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    return 1;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    return 3;
            }

            Debug.Assert(false);

            return 0;
        }

        public void SetNumBand(int numBand)
        {
            switch (numBand)
            {
                case 1:
                    pixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                    break;
                case 3:
                    pixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }
        }

        public virtual void LoadXml(XmlElement cameraElement)
        {
            grabberType = (GrabberType)Enum.Parse(typeof(GrabberType), XmlHelper.GetValue(cameraElement, "Type", "Pylon"));
            enabled = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "Enabled", "True"));
            width = Convert.ToInt32(XmlHelper.GetValue(cameraElement, "Width", "1000"));
            height = Convert.ToInt32(XmlHelper.GetValue(cameraElement, "Height", "1000"));
            exposureTimeUs = Convert.ToInt32(XmlHelper.GetValue(cameraElement, "ExposureTimeUs", "50"));
            expouserMode = (ExpouserMode)Enum.Parse(typeof(ExpouserMode), XmlHelper.GetValue(cameraElement, "ExpouserMode", "Timed"));
            expouserAuto = (ExpouserAuto)Enum.Parse(typeof(ExpouserAuto), XmlHelper.GetValue(cameraElement, "ExpouserAuto", "Off"));
            bayerCamera = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "BayerCamera", "False"));
            pixelFormat = (PixelFormat)Enum.Parse(typeof(PixelFormat), XmlHelper.GetValue(cameraElement, "PixelFormat", PixelFormat.Format8bppIndexed.ToString()));
            mirrorX = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "MirrorX", "False"));
            mirrorY = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "MirrorY", "False"));
            rotateFlipType = (RotateFlipType)Enum.Parse(typeof(RotateFlipType), XmlHelper.GetValue(cameraElement, "RotateFlipType", "RotateNoneFlipNone"));
            customBufferType = (CustomBufferType)Enum.Parse(typeof(CustomBufferType), XmlHelper.GetValue(cameraElement, "CustomBufferType", CustomBufferType.None.ToString()));
            customBufferType = (CustomBufferType)Enum.Parse(typeof(CustomBufferType), XmlHelper.GetValue(cameraElement, "CustomBufferType", CustomBufferType.None.ToString()));
            frameNum = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "FrameNum", "5"));
            IsLineScan = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "IsLineScan", "false"));
            useNativeBuffering = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "UseNativeBuffering", "False"));
        }

        public virtual void SaveXml(XmlElement cameraElement)
        {
            XmlHelper.SetValue(cameraElement, "Type", grabberType.ToString());
            XmlHelper.SetValue(cameraElement, "Enabled", enabled.ToString());
            XmlHelper.SetValue(cameraElement, "Width", width.ToString());
            XmlHelper.SetValue(cameraElement, "Height", height.ToString());
            XmlHelper.SetValue(cameraElement, "ExposureTimeUs", exposureTimeUs.ToString());
            XmlHelper.SetValue(cameraElement, "ExpouserMode", ExpouserMode.ToString());
            XmlHelper.SetValue(cameraElement, "ExpouserAuto", ExpouserAuto.ToString());
            XmlHelper.SetValue(cameraElement, "BayerCamera", bayerCamera.ToString());
            XmlHelper.SetValue(cameraElement, "PixelFormat", pixelFormat.ToString());
            XmlHelper.SetValue(cameraElement, "MirrorX", mirrorX.ToString());
            XmlHelper.SetValue(cameraElement, "MirrorY", mirrorY.ToString());
            XmlHelper.SetValue(cameraElement, "RotateFlipType", rotateFlipType.ToString());
            XmlHelper.SetValue(cameraElement, "CustomBufferType", customBufferType.ToString());
            XmlHelper.SetValue(cameraElement, "FrameNum", frameNum.ToString());
            XmlHelper.SetValue(cameraElement, "IsLineScan", IsLineScan.ToString());
            XmlHelper.SetValue(cameraElement, "UseNativeBuffering", useNativeBuffering.ToString());
        }
    }

    public class CameraConfiguration
    {
        public int RequiredCameras { get; set; }

        public IEnumerator<CameraInfo> GetEnumerator()
        {
            return CameraInfoList.GetEnumerator();
        }
        public List<CameraInfo> CameraInfoList { get; } = new List<CameraInfo>();

        public void Clear()
        {
            CameraInfoList.Clear();
        }

        public void AddCameraInfo(CameraInfo cameraInfo)
        {
            cameraInfo.Index = CameraInfoList.Count();
            cameraInfo.Enabled = true;
            CameraInfoList.Add(cameraInfo);
        }

        public bool LoadCameraConfiguration(string fileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Load Camera Configuration");

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
            {
                return false;
            }

            int index = 0;

            XmlElement cameraListElement = xmlDocument.DocumentElement;
            foreach (XmlElement cameraElement in cameraListElement)
            {
                if (cameraElement.Name == "Camera")
                {
                    var grabberType = (GrabberType)Enum.Parse(typeof(GrabberType), XmlHelper.GetValue(cameraElement, "Type", "Pylon"));

                    var cameraInfo = CameraInfo.Create(grabberType);
                    cameraInfo.LoadXml(cameraElement);

                    cameraInfo.Index = index++;

                    CameraInfoList.Add(cameraInfo);
                }
            }

            return true;
        }

        public void SaveCameraConfiguration(string fileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Save Camera Configuration");

            var xmlDocument = new XmlDocument();

            XmlElement cameraListElement = xmlDocument.CreateElement("", "CameraList", "");
            xmlDocument.AppendChild(cameraListElement);

            foreach (CameraInfo cameraInfo in CameraInfoList)
            {
                XmlElement cameraElement = xmlDocument.CreateElement("", "Camera", "");
                cameraListElement.AppendChild(cameraElement);

                cameraInfo.SaveXml(cameraElement);
            }

            XmlHelper.Save(xmlDocument, fileName);
        }
    }
}
