using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    public class CameraPanel : CanvasPanel
    {
        private Camera linkedCamera;
        private Calibration calibration;

        public ImageUpdatedDelegate ImageUpdated;

        public CameraPanel()
        {
            ChildMouseClick += cameraPanel_MouseClicked;
        }

        ~CameraPanel()
        {

        }

        private void cameraPanel_MouseClicked(UserControl control, PointF point, MouseButtons button, ref bool processingCancelled)
        {

        }

        public void TurnOnMeasure(Calibration calibration)
        {
            DragMode = DragMode.Measure;
            this.calibration = calibration;
        }

        public void TurnOffMeasure()
        {
            DragMode = DragMode.Select;
        }

        public void ClearMeasure()
        {
            TempFigures.Clear();
            Invalidate(true);
        }

        public void SetCamera(Camera camera)
        {
            if (camera != null)
            {
                linkedCamera = camera;

                if (linkedCamera != null)
                {
                    linkedCamera.ImageGrabbed += camera_ImageGrabbed;
                }
            }
            else
            {
                if (linkedCamera != null)
                {
                    linkedCamera.ImageGrabbed -= camera_ImageGrabbed;
                }

                linkedCamera = null;
            }
        }

        private void camera_ImageGrabbed(Camera camera)
        {
            if (Visible == false)
            {
                return;
            }

            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Grab, "Start Invoke ImageGrabbed");
                BeginInvoke(new CameraEventDelegate(camera_ImageGrabbed), camera);
                return;
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("Start ImageGrabbed : Camera - {0}", camera.Index));

            var imageD = (Image2D)camera.GetGrabbedImage();

            if (imageD.ImageData != null)
            {
                UpdateImage(imageD.ToBitmap());

                ImageUpdated?.Invoke();

                Invalidate(true);
            }

            LogHelper.Debug(LoggerType.Grab, string.Format("End UpdateImage : Camera - {0}", camera.Index));
        }

        public override void Measure()
        {

        }
    }
}
