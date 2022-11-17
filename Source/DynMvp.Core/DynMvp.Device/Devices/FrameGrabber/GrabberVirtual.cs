using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfoVirtual : CameraInfo
    {
        public int Interval { get; set; } = 500;
        public string FolderPath { get; set; } = @"D:\VirtualImage\";

        public CameraInfoVirtual()
        {
            GrabberType = GrabberType.Virtual;
        }

        public CameraInfoVirtual(int width, int height, int interval, string folderPath)
        {
            Width = width;
            Height = height;
            Interval = interval;
            FolderPath = folderPath;
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            FolderPath = XmlHelper.GetValue(cameraElement, "FolderPath", @"D:\VirtualImage\");

            if (!Path.HasExtension(FolderPath))
            {
                FolderPath = Path.Combine(FolderPath, "*.bmp");
            }

            Interval = Convert.ToInt32(XmlHelper.GetValue(cameraElement, "Interval", "500"));
            if (Interval < 0)
            {
                Interval = 500;
            }
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "FolderPath", FolderPath);
            XmlHelper.SetValue(cameraElement, "Interval", Interval);
        }
    }

    public class GrabberVirtual : Grabber
    {
        public GrabberVirtual(GrabberInfo grabberInfo) : base(GrabberType.Virtual, grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Virtual Camera Manager Created");
        }

        public override Camera CreateCamera()
        {
            return new CameraVirtual();
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            var form = new VirtualCameraListForm();
            form.RequiredNumCamera = numCamera;
            form.CameraConfiguration = cameraConfiguration;
            form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public override bool Initialize()
        {
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
