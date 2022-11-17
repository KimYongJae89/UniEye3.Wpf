using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;
using Euresys.MultiCam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public enum EdgeStartPos
    {
        Left, Middle, Right
    }

    public class CameraInfoMultiCam : CameraInfo
    {
        public EuresysBoardType BoardType { get; private set; }
        public uint BoardId { get; private set; }
        public uint ConnectorId { get; private set; }
        public CameraType CameraType { get; private set; }
        public uint SurfaceNum { get; private set; }
        public uint PageLength { get; set; }
        public EdgeStartPos EdgeStartPos { get; set; }
        public uint ROIStartPos { get; set; }
        public uint ROIWidth { get; set; }

        public CameraInfoMultiCam()
        {
            GrabberType = GrabberType.MultiCam;
        }

        public CameraInfoMultiCam(EuresysBoardType boardType, uint boardId, uint connectorId, CameraType cameraType, uint surfaceNum, uint pageLength, EdgeStartPos edgeStartPos, uint roiStartPos, uint roiWidth)
        {
            GrabberType = GrabberType.MultiCam;

            BoardType = boardType;
            BoardId = boardId;
            ConnectorId = connectorId;
            CameraType = cameraType;
            SurfaceNum = surfaceNum;
            PageLength = pageLength;
            EdgeStartPos = edgeStartPos;
            ROIStartPos = roiStartPos;
            ROIWidth = roiWidth;
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            BoardType = (EuresysBoardType)Enum.Parse(typeof(EuresysBoardType), XmlHelper.GetValue(cameraElement, "BoardType", "GrabLink_Base"));
            BoardId = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "BoardId", "0"));
            ConnectorId = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "ConnectorId", "0"));
            CameraType = (CameraType)Enum.Parse(typeof(CameraType), XmlHelper.GetValue(cameraElement, "CameraType", "Jai_GO_5000"));
            SurfaceNum = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "SurfaceNum", "1"));
            PageLength = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "PageLength", "0"));
            EdgeStartPos = (EdgeStartPos)Enum.Parse(typeof(EdgeStartPos), XmlHelper.GetValue(cameraElement, "EdgeStartPos", "Middle"));
            ROIStartPos = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "ROIStartPos", "0"));
            ROIWidth = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "ROIWidth", "0"));
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "BoardType", BoardType.ToString());
            XmlHelper.SetValue(cameraElement, "BoardId", BoardId.ToString());
            XmlHelper.SetValue(cameraElement, "ConnectorId", ConnectorId.ToString());
            XmlHelper.SetValue(cameraElement, "CameraType", CameraType.ToString());
            XmlHelper.SetValue(cameraElement, "SurfaceNum", SurfaceNum.ToString());
            XmlHelper.SetValue(cameraElement, "PageLength", PageLength.ToString());
            XmlHelper.SetValue(cameraElement, "EdgeStartPos", EdgeStartPos.ToString());
            XmlHelper.SetValue(cameraElement, "ROIStartPos", ROIStartPos.ToString());
            XmlHelper.SetValue(cameraElement, "ROIWidth", ROIWidth.ToString());
        }
    }

    public class GrabberMultiCam : Grabber
    {
        private static int cntOpenDriver = 0;

        public GrabberMultiCam(GrabberInfo grabberInfo) : base(GrabberType.MultiCam, grabberInfo)
        {
        }

        public override Camera CreateCamera()
        {
            return new CameraMultiCam();
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            var form = new EuresysBoardListForm();
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public override bool Initialize()
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize MultiCam Camera Manager");

            // Open MultiCam driver
            if (cntOpenDriver == 0)
            {
                LogHelper.Debug(LoggerType.StartUp, "Open MultiCam Driver");
                MC.OpenDriver();
            }

            cntOpenDriver++;

            // Enable error logging
            MC.SetParam(MC.CONFIGURATION, "ErrorLog", "error.log");

            return true;
        }

        public override void Release()
        {
            base.Release();

            cntOpenDriver--;

            if (cntOpenDriver == 0)
            {
                LogHelper.Debug(LoggerType.Shutdown, "Release MultiCam Driver");
                MC.CloseDriver();
            }
        }

        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {

        }
    }
}
