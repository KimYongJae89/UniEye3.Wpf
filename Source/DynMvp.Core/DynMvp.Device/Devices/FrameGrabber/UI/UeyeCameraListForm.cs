using DynMvp.Base;
using DynMvp.UI;
using PylonC.NETSupportLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class UeyeCameraListForm : Form
    {
        private int requiredNumCamera;
        public int RequiredNumCamera
        {
            set => requiredNumCamera = value;
        }
        public CameraConfiguration CameraConfiguration { get; set; }

        public UeyeCameraListForm()
        {
            InitializeComponent();

            autoDetectButton.Text = StringManager.GetString(autoDetectButton.Text);
            buttonMoveUp.Text = StringManager.GetString(buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);


        }

        private void UeyeCameraListForm_Load(object sender, EventArgs e)
        {
            foreach (CameraInfo cameraInfo in CameraConfiguration)
            {
                var cameraInfoUEye = (CameraInfoUEye)cameraInfo;
                //cameraInfoGrid.Rows.Add(cameraInfoUEye.Index, cameraInfoUEye.DeviceId, cameraInfoUEye.MirrorX, cameraInfoUEye.MirrorY);
                cameraInfoGrid.Rows.Add(cameraInfoUEye.Index, cameraInfoUEye.SerialNo, cameraInfoUEye.MirrorX, cameraInfoUEye.MirrorY);
                //cameraInfoGrid.Rows.Add(cameraInfoUEye.Index, cameraInfoUEye.SerialNo,cameraInfoUEye.DeviceId, cameraInfoUEye.MirrorX, cameraInfoUEye.MirrorY);
            }

            if (cameraInfoGrid.Rows.Count < requiredNumCamera)
            {
                cameraInfoGrid.Rows.Add();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            CameraConfiguration.Clear();

            foreach (DataGridViewRow row in cameraInfoGrid.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                {
                    var cameraInfoUEye = new CameraInfoUEye();
                    cameraInfoUEye.Index = int.Parse(row.Cells[0].Value.ToString());
                    //cameraInfoUEye.DeviceId = int.Parse(row.Cells[1].Value.ToString());
                    cameraInfoUEye.SerialNo = row.Cells[1].Value.ToString();

                    if (string.IsNullOrEmpty((row.Cells[2].Value.ToString())))
                    {
                        cameraInfoUEye.MirrorX = false;
                    }
                    else
                    {
                        cameraInfoUEye.MirrorX = bool.Parse(row.Cells[2].Value.ToString());
                    }

                    if (string.IsNullOrEmpty((row.Cells[3].Value.ToString())))
                    {
                        cameraInfoUEye.MirrorY = false;
                    }
                    else
                    {
                        cameraInfoUEye.MirrorY = bool.Parse(row.Cells[3].Value.ToString());
                    }

                    CameraConfiguration.AddCameraInfo(cameraInfoUEye);
                }
            }

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void autoDetectButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.StartUp, "Auto Detect Camera(s)");

            uEye.Info.Camera.GetCameraList(out uEye.Types.CameraInformation[] cameraList);

            if (requiredNumCamera > 0 && cameraList.Count() != requiredNumCamera)
            {
                throw new CameraInitializeFailException("Some camera device cann't be detected.(UEye)");
            }
            LogHelper.Debug(LoggerType.StartUp, string.Format("{0} camera(s) are detected.", cameraList.Count().ToString()));
            cameraInfoGrid.Rows.Clear();
            int idx = 0;
            foreach (uEye.Types.CameraInformation info in cameraList)
            {
                //cameraInfoGrid.Rows.Add(info.CameraID, info.SerialNumber, false, false);
                cameraInfoGrid.Rows.Add(idx++, info.SerialNumber, false, false);
            }
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            UiHelper.MoveUp(cameraInfoGrid);
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            UiHelper.MoveDown(cameraInfoGrid);
        }
    }
}
