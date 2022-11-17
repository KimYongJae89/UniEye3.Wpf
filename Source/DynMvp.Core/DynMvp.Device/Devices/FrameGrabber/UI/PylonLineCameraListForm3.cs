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
    public partial class PylonLineCameraListForm3 : Form
    {
        private int requiredNumCamera;
        public int RequiredNumCamera
        {
            set => requiredNumCamera = value;
        }
        public CameraConfiguration CameraConfiguration { get; set; }

        public PylonLineCameraListForm3()
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
            UpdateData();
        }

        private void UpdateData()
        {
            cameraInfoGrid.Rows.Clear();
            foreach (CameraInfo cameraInfo in CameraConfiguration)
            {
                var cameraInfoPylon = (CameraInfoPylonLine)cameraInfo;
                int i = cameraInfoGrid.Rows.Add(
                    cameraInfoPylon.Index,
                    cameraInfoPylon.DeviceUserId,
                    cameraInfoPylon.IpAddress,
                    cameraInfoPylon.SerialNo,
                    cameraInfoPylon.ModelName,
                    cameraInfoPylon.Width,
                    cameraInfoPylon.Height,
                    cameraInfo.GetNumBand().ToString(),
                    cameraInfoPylon.RotateFlipType.ToString(),
                    cameraInfoPylon.UseNativeBuffering,
                    "Edit");

                cameraInfoGrid.Rows[i].Tag = cameraInfoPylon;
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
                CameraInfoPylonLine cameraInfoPylon = (row.Tag as CameraInfoPylonLine) ?? new CameraInfoPylonLine();
                if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                {
                    cameraInfoPylon.Index = int.Parse(row.Cells[0].Value.ToString());
                    cameraInfoPylon.DeviceUserId = row.Cells[1].Value.ToString();
                    cameraInfoPylon.IpAddress = row.Cells[2].Value.ToString();
                    cameraInfoPylon.SerialNo = row.Cells[3].Value.ToString();
                    cameraInfoPylon.ModelName = row.Cells[4].Value.ToString();
                    cameraInfoPylon.Width = int.Parse(row.Cells[5].Value.ToString());
                    cameraInfoPylon.Height = int.Parse(row.Cells[6].Value.ToString());

                    int numBand = int.Parse(row.Cells[7].Value.ToString());
                    cameraInfoPylon.SetNumBand(numBand);
                    cameraInfoPylon.RotateFlipType = (RotateFlipType)Enum.Parse(typeof(RotateFlipType), row.Cells[8].Value.ToString());
                    cameraInfoPylon.UseNativeBuffering = Convert.ToBoolean(row.Cells[9].Value);
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
            IList<Basler.Pylon.ICameraInfo> deviceList = Basler.Pylon.CameraFinder.Enumerate();
            LogHelper.Debug(LoggerType.StartUp, string.Format("{0} camera(s) are detected.", deviceList.Count));

            cameraInfoGrid.Rows.Clear();
            CameraConfiguration.Clear();

            for (int i = 0; i < Math.Min(deviceList.Count, requiredNumCamera); i++)
            {
                Basler.Pylon.ICameraInfo device = deviceList[i];
                //  if (device.ContainsKey(Basler.Pylon.CameraInfoKey.DeviceIpAddress))
                //      cameraInfoPylon.IpAddress = device[Basler.Pylon.CameraInfoKey.DeviceIpAddress];

                string deviceUserId = device[Basler.Pylon.CameraInfoKey.FriendlyName];
                string ipAddress = device[Basler.Pylon.CameraInfoKey.DeviceIpAddress];
                string serialNo = device[Basler.Pylon.CameraInfoKey.SerialNumber];
                string modelName = device[Basler.Pylon.CameraInfoKey.ModelName];

                var cameraInfoPylon = new CameraInfoPylonLine();
                cameraInfoPylon.DeviceUserId = deviceUserId;
                cameraInfoPylon.IpAddress = ipAddress;
                cameraInfoPylon.SerialNo = serialNo;
                cameraInfoPylon.ModelName = modelName;

                var cameraPylon = new CameraPylonLine();
                cameraPylon.Initialize(cameraInfoPylon);

                int id = cameraInfoGrid.Rows.Add(
                    i,
                    deviceUserId,
                    cameraInfoPylon.IpAddress,
                    serialNo, modelName,
                    cameraPylon.ImageSize.Width,
                    cameraPylon.ImageSize.Height,
                    cameraPylon.NumOfBand.ToString(),
                    RotateFlipType.RotateNoneFlipNone.ToString(),
                    cameraInfoPylon.UseNativeBuffering,
                    "Edit"
                    );
                cameraInfoGrid.Rows[id].Tag = cameraInfoPylon;

                cameraPylon.Release();

                cameraInfoGrid.Rows[i].Cells[0].ToolTipText =
                    device[Basler.Pylon.CameraInfoKey.DeviceType];

                if (device.ContainsKey(Basler.Pylon.CameraInfoKey.DeviceVersion))
                {
                    cameraInfoGrid.Rows[i].Cells[1].ToolTipText =
                        device[Basler.Pylon.CameraInfoKey.DeviceVersion];
                }

                cameraInfoGrid.Rows[i].Cells[2].ToolTipText = "";
            }

            while (cameraInfoGrid.Rows.Count < requiredNumCamera)
            {
                cameraInfoGrid.Rows.Add((requiredNumCamera - cameraInfoGrid.Rows.Count).ToString());
            }
        }

        private void ButtonMoveUp_Click(object sender, EventArgs e)
        {
            UiHelper.MoveUp(cameraInfoGrid);
        }

        private void ButtonMoveDown_Click(object sender, EventArgs e)
        {
            UiHelper.MoveDown(cameraInfoGrid);
        }

        private void cameraInfoGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 10)
            {
                return;
            }

            var cameraInfo = (CameraInfoPylonLine)cameraInfoGrid.Rows[e.RowIndex].Tag;
            ShowPropWindow(cameraInfo);


        }

        private void cameraInfoGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var cameraInfo = (CameraInfoPylonLine)cameraInfoGrid.Rows[e.RowIndex].Tag;
            ShowPropWindow(cameraInfo);
        }

        private void ShowPropWindow(CameraInfoPylonLine cameraInfo)
        {
            var propertyGrid = new PropertyGrid();
            propertyGrid.SelectedObject = cameraInfo;
            propertyGrid.Dock = DockStyle.Fill;

            var form = new Form();
            form.Size = new Size(640, 640);
            form.Controls.Add(propertyGrid);
            form.ShowDialog();
            UpdateData();
        }
    }
}
