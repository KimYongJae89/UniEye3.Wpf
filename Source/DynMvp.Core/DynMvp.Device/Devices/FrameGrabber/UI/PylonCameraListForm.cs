using DynMvp.Base;
using DynMvp.UI;
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
    public partial class PylonCameraListForm : Form
    {
        private int requiredNumCamera;
        public int RequiredNumCamera
        {
            set => requiredNumCamera = value;
        }
        public CameraConfiguration CameraConfiguration { get; set; }

        public PylonCameraListForm()
        {
            InitializeComponent();

            autoDetectButton.Text = StringManager.GetString(autoDetectButton.Text);
            buttonMoveUp.Text = StringManager.GetString(buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

        }

        private void PylonCameraListForm_Load(object sender, EventArgs e)
        {
            foreach (CameraInfo cameraInfo in CameraConfiguration)
            {
                var cameraInfoPylon = (CameraInfoPylon)cameraInfo;

                int numBand = cameraInfo.GetNumBand();
                cameraInfoGrid.Rows.Add(cameraInfoPylon.Index, cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo, cameraInfoPylon.ModelName,
                                                cameraInfoPylon.Width, cameraInfoPylon.Height, numBand.ToString(), cameraInfoPylon.RotateFlipType.ToString());
            }

            int index = cameraInfoGrid.Rows.Count;
            while (cameraInfoGrid.Rows.Count < requiredNumCamera)
            {
                cameraInfoGrid.Rows.Add(index.ToString());
                index++;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            CameraConfiguration.Clear();

            foreach (DataGridViewRow row in cameraInfoGrid.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                {
                    var cameraInfoPylon = new CameraInfoPylon();
                    cameraInfoPylon.Index = int.Parse(row.Cells[0].Value.ToString());
                    cameraInfoPylon.DeviceUserId = row.Cells[1].Value.ToString();
                    cameraInfoPylon.IpAddress = row.Cells[2].Value.ToString();
                    cameraInfoPylon.SerialNo = row.Cells[3].Value.ToString();
                    cameraInfoPylon.ModelName = row.Cells[4].Value.ToString();
                    cameraInfoPylon.Width = int.Parse(row.Cells[5].Value.ToString());
                    cameraInfoPylon.Height = int.Parse(row.Cells[6].Value.ToString());

                    int numBand = int.Parse(row.Cells[7].Value.ToString());
                    cameraInfoPylon.SetNumBand(numBand);
                    //switch (numBand)
                    //{
                    //    case 1:
                    //        cameraInfoPylon.PixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                    //        break;
                    //    case 2:
                    //        cameraInfoPylon.PixelFormat = System.Drawing.Imaging.PixelFormat.Format16bppRgb565;
                    //        break;
                    //    case 3:
                    //        cameraInfoPylon.PixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                    //        break;
                    //    case 4:
                    //        cameraInfoPylon.PixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                    //        break;
                    //}

                    cameraInfoPylon.RotateFlipType = (RotateFlipType)Enum.Parse(typeof(RotateFlipType), row.Cells[8].Value.ToString());

                    CameraConfiguration.AddCameraInfo(cameraInfoPylon);
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

            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "5000");

            PylonC.NET.Pylon.Initialize();

            List<PylonC.NETSupportLibrary.DeviceEnumerator.Device> deviceList = PylonC.NETSupportLibrary.DeviceEnumerator.EnumerateDevices();

            LogHelper.Debug(LoggerType.StartUp, string.Format("{0} camera(s) are detected.", deviceList.Count));

            cameraInfoGrid.Rows.Clear();

            int index = 0;
            foreach (PylonC.NETSupportLibrary.DeviceEnumerator.Device device in deviceList)
            {
                GrabberPylon.GetFeature(device.Tooltip, out string deviceUserId, out string ipAddress, out string serialNo, out string modelName);

                var cameraInfoPylon = new CameraInfoPylon();
                cameraInfoPylon.DeviceIndex = device.Index;

                var cameraPylon = new CameraPylon();
                cameraPylon.Initialize(cameraInfoPylon);

                cameraInfoGrid.Rows.Add(index, deviceUserId, ipAddress, serialNo, modelName, cameraPylon.ImageSize.Width, cameraPylon.ImageSize.Height, cameraPylon.NumOfBand.ToString(), RotateFlipType.RotateNoneFlipNone.ToString());

                cameraPylon.Release();

                cameraInfoGrid.Rows[index].Cells[0].ToolTipText = device.Tooltip;
                cameraInfoGrid.Rows[index].Cells[1].ToolTipText = device.Tooltip;
                cameraInfoGrid.Rows[index].Cells[2].ToolTipText = device.Tooltip;

                index++;
            }

            if (cameraInfoGrid.Rows.Count < requiredNumCamera && requiredNumCamera > 0)
            {
                while (cameraInfoGrid.Rows.Count < requiredNumCamera)
                {
                    cameraInfoGrid.Rows.Add(index.ToString());
                    index++;
                }
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
