using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfoGenTL : CameraInfo
    {
        public enum EStageType { X64, X128, X192, X256 }
        public enum EClientType { Master, Slave }
        public enum EScanDirectionType { Forward, Reverse }
        public enum EAnalogGain { X1, X2, X3, X4 }
        [Category("CameraInfoGenTL"), Description("Image Length")]
        public uint ScanLength { get; set; }

        private EClientType clientType = EClientType.Master;
        [Category("CameraInfoGenTL"), Description("DF Client Type")]
        public EClientType ClientType
        {
            get => clientType;
            set => clientType = value;
        }

        private EScanDirectionType directionType;
        [Category("CameraInfoGenTL"), Description("Camera Scan Direction")]
        public EScanDirectionType DirectionType
        {
            get => directionType;
            set => directionType = value;
        }
        [Category("CameraInfoGenTL"), Description("Image Offset X > 0")]
        public uint OffsetX { get; set; } = 0;

        private float digitalGain = 1.0f;
        [Category("CameraInfoGenTL"), Description("Degital Gain")]
        public float DigitalGain
        {
            get => digitalGain;
            set => digitalGain = value;
        }

        private EAnalogGain analogGain = EAnalogGain.X1;
        [Category("CameraInfoGenTL"), Description("Analog Gain")]
        public EAnalogGain AnalogGain
        {
            get => analogGain;
            set => analogGain = value;
        }

        private EStageType stageType = EStageType.X64;
        [Category("CameraInfoGenTL"), Description("Stage Type")]
        public EStageType StageType
        {
            get => stageType;
            set => stageType = value;
        }
        [Category("CameraInfoGenTL"), Description("Binning Vertical")]
        public bool BinningVertical { get; set; } = false;

        public CameraInfoGenTL()
        {
            GrabberType = GrabberType.GenTL;
        }

        public CameraInfoGenTL(int width, int height, uint scanLength, uint frameNum, EClientType clientType, EScanDirectionType directionType, bool useMilBuffer = false)
        {
            GrabberType = GrabberType.GenTL;

            Width = width;
            Height = height;
            FrameNum = frameNum;
            ScanLength = scanLength;
            this.clientType = clientType;
            this.directionType = directionType;
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            FrameNum = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "FrameNum", "1"));
            ScanLength = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "ScanLength", "0"));
            ScanLength = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "ScanLength", "0"));
            OffsetX = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "OffsetX", "0"));
            BinningVertical = Convert.ToBoolean(XmlHelper.GetValue(cameraElement, "BinningVertical", "false"));
            Enum.TryParse(XmlHelper.GetValue(cameraElement, "StageType", EStageType.X64.ToString()), out stageType);
            Enum.TryParse(XmlHelper.GetValue(cameraElement, "ClientType", EClientType.Master.ToString()), out clientType);
            Enum.TryParse(XmlHelper.GetValue(cameraElement, "DirectionType", EScanDirectionType.Forward.ToString()), out directionType);
            Enum.TryParse(XmlHelper.GetValue(cameraElement, "AnalogGain", analogGain.ToString()), out analogGain);
            float.TryParse(XmlHelper.GetValue(cameraElement, "DigitalGain", digitalGain.ToString()), out digitalGain);
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "FrameNum", FrameNum.ToString());
            XmlHelper.SetValue(cameraElement, "ScanLength", ScanLength.ToString());
            XmlHelper.SetValue(cameraElement, "OffsetX", OffsetX.ToString());
            XmlHelper.SetValue(cameraElement, "BinningVertical", BinningVertical.ToString());
            XmlHelper.SetValue(cameraElement, "StageType", stageType.ToString());
            XmlHelper.SetValue(cameraElement, "ClientType", clientType.ToString());
            XmlHelper.SetValue(cameraElement, "DirectionType", directionType.ToString());
            XmlHelper.SetValue(cameraElement, "DigitalGain", digitalGain.ToString());
            XmlHelper.SetValue(cameraElement, "AnalogGain", analogGain.ToString());
        }
    }

    public class GrabberGenTL : Grabber
    {
        private static int cntOpenDriver = 0;

        public GrabberGenTL(GrabberInfo grabberInfo) : base(GrabberType.GenTL, grabberInfo)
        {

        }

        public override Camera CreateCamera()
        {
            return new CameraGenTL();
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            var form = new GenTLCameraListForm();
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public override bool Initialize()
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize MultiCam Camera Manager");

            cntOpenDriver++;
            return true;
        }

        public override void Release()
        {
            base.Release();

            cntOpenDriver--;
        }

        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {

        }
    }
}
