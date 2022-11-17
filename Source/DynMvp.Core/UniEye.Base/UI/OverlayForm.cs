using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public partial class OverlayForm : Form
    {
        private DrawBox imageBox;

        public OverlayForm()
        {
            InitializeComponent();

            buttonCam1.Text = StringManager.GetString(buttonCam1.Text);
            buttonCam2.Text = StringManager.GetString(buttonCam2.Text);
            buttonSelectImage1.Text = StringManager.GetString(buttonSelectImage1.Text);
            buttonSelectImage2.Text = StringManager.GetString(buttonSelectImage2.Text);
            buttonSetCam1.Text = StringManager.GetString(buttonSetCam1.Text);
            buttonSetCam2.Text = StringManager.GetString(buttonSetCam2.Text);

            Initialize();
        }

        private void Initialize()
        {
            imageBox = new DrawBox();

            SuspendLayout();

            imageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            imageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            imageBox.Name = "imageBox";
            imageBox.TabIndex = 0;
            imageBox.TabStop = false;
            imageBox.OverlayMoveMode = true;
            imageBox.Enable = true;

            panelImage.Controls.Add(imageBox);

            ResumeLayout(false);

            ChangeVisibleControl();
        }

        private void buttonSelectImage2_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxImage2Path.Text = dialog.FileName;
                imageBox.OverlayImage = (Bitmap)ImageHelper.LoadImage(dialog.FileName);
                imageBox.Invalidate();
            }
        }

        private void buttonSelectImage1_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxImage1Path.Text = dialog.FileName;
                imageBox.UpdateImage((Bitmap)ImageHelper.LoadImage(dialog.FileName));
            }
        }

        private ImageD Grab(int index)
        {
            Camera camera = DeviceManager.Instance().CameraHandler.GetCamera(0);
            if (camera == null)
            {
                return null;
            }

            camera.GrabOnce();

            if (camera.WaitGrabDone(5000) == false)
            {
                return null;
            }

            return camera.GetGrabbedImage();
        }

        private void buttonCam1_Click(object sender, EventArgs e)
        {
            if (DeviceConfig.Instance().VirtualMode)
            {
                return;
            }

            imageBox.UpdateImage(Grab(0).ToBitmap());
            imageBox.Invalidate();
        }

        private void buttonCam2_Click(object sender, EventArgs e)
        {
            if (DeviceConfig.Instance().VirtualMode)
            {
                return;
            }

            imageBox.OverlayImage = Grab(1).ToBitmap();
            imageBox.Invalidate();
        }

        private void ChangeVisibleControl()
        {
            if (DeviceConfig.Instance().VirtualMode)
            {
                panelCam.Visible = false;
            }
        }

        public PointF GetOffset()
        {
            if (imageBox.OverlayImage == null)
            {
                return new PointF(0, 0);
            }

            return imageBox.OverlayPos;
        }

        private void buttonSetCam1_Click(object sender, EventArgs e)
        {
            //            Settings.Instance().CalibrationSettings.Calibration2 = GetOffset();
            //            Settings.Save();
        }

        private void buttonSetCam2_Click(object sender, EventArgs e)
        {
            //            Settings.Instance().CalibrationSettings.Calibration2 = GetOffset();
            //            Settings.Save();
        }
    }
}
