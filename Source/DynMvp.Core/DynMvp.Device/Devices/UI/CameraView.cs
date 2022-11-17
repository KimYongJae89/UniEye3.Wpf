using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public partial class CameraView : PictureBox
    {
        public CameraEventDelegate ImageGrabbed;
        public Camera LinkedCamera { get; private set; } = null;
        public bool LockImageUpdate { get; set; } = false;

        public CameraView()
        {
            InitializeComponent();
        }

        public void SetCamara(Camera camera)
        {
            if (camera != null)
            {
                LinkedCamera = camera;

                if (LinkedCamera != null)
                {
                    LinkedCamera.ImageGrabbed += camera_ImageGrabbed;
                }
            }
            else
            {
                LinkedCamera.ImageGrabbed -= camera_ImageGrabbed;
                LinkedCamera = null;
            }
        }

        public void camera_ImageGrabbed(Camera camera)
        {
            if (LockImageUpdate == true)
            {
                return;
            }

            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Grab, "Start UpdateImage Invoke");
                Invoke(new CameraEventDelegate(camera_ImageGrabbed), camera);
                return;
            }

            LogHelper.Debug(LoggerType.Grab, "Start UpdateImage");

            var image2d = (Image2D)camera.GetGrabbedImage();

            LogHelper.Debug(LoggerType.Grab, "Set Bitmp");
            Image = image2d.ToBitmap();

            ImageGrabbed?.Invoke(camera);
        }

        public void EnableMeasureMode(float scaleX, float scaleY)
        {

        }
    }
}
