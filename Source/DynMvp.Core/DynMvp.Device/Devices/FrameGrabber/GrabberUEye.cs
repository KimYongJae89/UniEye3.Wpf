using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfoUEye : CameraInfo
    {
        public int CameraId { get; set; }
        public int DeviceId { get; set; } = -1;
        public string SerialNo { get; set; }

        public CameraInfoUEye()
        {
            GrabberType = GrabberType.uEye;

            CameraId = -1;
            //this.deviceId = -1;
            SerialNo = "";
        }

        public CameraInfoUEye(int cameraId, int deviceId)
        {
            GrabberType = GrabberType.uEye;

            CameraId = cameraId;
            //this.deviceId = deviceId;
            SerialNo = "";
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            CameraId = Convert.ToInt32(XmlHelper.GetValue(cameraElement, "CameraId", "-1"));
            DeviceId = Convert.ToInt32(XmlHelper.GetValue(cameraElement, "DeviceId", "-1"));
            SerialNo = Convert.ToString(XmlHelper.GetValue(cameraElement, "SerialNo", ""));
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "CameraId", CameraId.ToString());
            XmlHelper.SetValue(cameraElement, "DeviceId", DeviceId.ToString());
            XmlHelper.SetValue(cameraElement, "SerialNo", SerialNo.ToString());
        }
    }

    internal class GrabberUEye : Grabber
    {
        public GrabberUEye(GrabberInfo grabberInfo) : base(GrabberType.uEye, grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Pylon Device Handler Created");
        }

        public override Camera CreateCamera()
        {
            return new CameraUEye();
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            var form = new UeyeCameraListForm();
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public override bool Initialize()
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialzie camera(s)");

            return true;
        }

        public override void Release()
        {
            base.Release();
        }

        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {

        }
    }
}
