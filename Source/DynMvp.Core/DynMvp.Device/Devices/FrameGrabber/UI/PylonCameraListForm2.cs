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
    public partial class PylonCameraListForm2 : Form
    {
        private int requiredNumCamera;
        public int RequiredNumCamera
        {
            set => requiredNumCamera = value;
        }
        public CameraConfiguration CameraConfiguration { get; set; }

        public PylonCameraListForm2()
        {
            InitializeComponent();

            //this.ColumnRotateFlipType.Items.Clear();
            //this.ColumnRotateFlipType.Items.AddRange(Enum.GetValues(typeof(RotateFlipType)));

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
                var cameraInfoPylon = (CameraInfoPylon2)cameraInfo;

                var row = new DataGridViewRow();
                row.CreateCells(cameraInfoGrid);
                FillRow(row, cameraInfoPylon);
                cameraInfoGrid.Rows.Add(row);
                //int numBand = cameraInfo.GetNumBand();
                //cameraInfoGrid.Rows.Add(cameraInfoPylon.Index, cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo, cameraInfoPylon.ModelName,
                //                                cameraInfoPylon.Width, cameraInfoPylon.Height, numBand.ToString(), cameraInfoPylon.RotateFlipType.ToString());
            }

            int index = cameraInfoGrid.Rows.Count;
            while (cameraInfoGrid.Rows.Count < requiredNumCamera)
            {
                cameraInfoGrid.Rows.Add(index.ToString());
                index++;
            }
        }

        private void FillRow(DataGridViewRow row, CameraInfoPylon2 cameraInfoPylon)
        {
            row.Cells[0].Value = cameraInfoPylon.Index;
            row.Cells[1].Value = cameraInfoPylon.DeviceUserId;
            row.Cells[2].Value = cameraInfoPylon.IpAddress;
            row.Cells[3].Value = cameraInfoPylon.SerialNo;
            row.Cells[4].Value = cameraInfoPylon.ModelName;
            row.Cells[5].Value = cameraInfoPylon.Width;
            row.Cells[6].Value = cameraInfoPylon.Height;
            row.Cells[7].Value = cameraInfoPylon.GetNumBand().ToString();
            row.Cells[8].Value = cameraInfoPylon.RotateFlipType.ToString();

            row.Tag = cameraInfoPylon;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            CameraConfiguration.Clear();

            foreach (DataGridViewRow row in cameraInfoGrid.Rows)
            {

                //if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                {
                    if (!(row.Tag is CameraInfoPylon2 cameraInfoPylon))
                    {
                        continue;
                    }

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
                    CameraConfiguration.AddCameraInfo(cameraInfoPylon);
                }
            }

            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AutoDetectButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.StartUp, "Auto Detect Camera(s)");

            //Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "5000"); // ??
            IList<Basler.Pylon.ICameraInfo> deviceList = Basler.Pylon.CameraFinder.Enumerate();
            LogHelper.Debug(LoggerType.StartUp, string.Format("{0} camera(s) are detected.", deviceList.Count));

            cameraInfoGrid.Rows.Clear();

            int index = 0;
            for (int i = 0; i < Math.Min(deviceList.Count, CameraConfiguration.CameraInfoList.Count); i++)
            //foreach (Basler.Pylon.ICameraInfo device in deviceList)
            {
                Basler.Pylon.ICameraInfo device = deviceList[i];
                var cameraInfoPylon = new CameraInfoPylon2();

                string deviceUserId = device[Basler.Pylon.CameraInfoKey.FriendlyName];

                if (device.ContainsKey(Basler.Pylon.CameraInfoKey.DeviceIpAddress))
                {
                    cameraInfoPylon.IpAddress = device[Basler.Pylon.CameraInfoKey.DeviceIpAddress];
                }

                string serialNo = device[Basler.Pylon.CameraInfoKey.SerialNumber];
                string modelName = device[Basler.Pylon.CameraInfoKey.ModelName];

                cameraInfoPylon.DeviceUserId = deviceUserId;
                cameraInfoPylon.SerialNo = serialNo;
                cameraInfoPylon.ModelName = modelName;
                cameraInfoPylon.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
                cameraInfoPylon.AutoDetectMode = true;

                {
                    var cameraPylon = new CameraPylon2();
                    cameraPylon.Initialize(cameraInfoPylon);
                    cameraInfoPylon.Width = cameraPylon.ImageSize.Width;
                    cameraInfoPylon.Height = cameraPylon.ImageSize.Height;
                    cameraInfoPylon.SetNumBand(cameraPylon.NumOfBand);
                    cameraPylon.Release();
                }

                var row = new DataGridViewRow();
                row.CreateCells(cameraInfoGrid);
                FillRow(row, cameraInfoPylon);
                cameraInfoGrid.Rows.Add(row);

                cameraInfoGrid.Rows[index].Cells[0].ToolTipText = device[Basler.Pylon.CameraInfoKey.DeviceType];
                if (device.ContainsKey(Basler.Pylon.CameraInfoKey.DeviceVersion))
                {
                    cameraInfoGrid.Rows[index].Cells[1].ToolTipText = device[Basler.Pylon.CameraInfoKey.DeviceVersion];
                }

                cameraInfoGrid.Rows[index].Cells[2].ToolTipText = "";

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

        private void ButtonMoveUp_Click(object sender, EventArgs e)
        {
            UiHelper.MoveUp(cameraInfoGrid);
        }

        private void ButtonMoveDown_Click(object sender, EventArgs e)
        {
            UiHelper.MoveDown(cameraInfoGrid);
        }

        private void CameraInfoGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow selectedRow = cameraInfoGrid.Rows[e.RowIndex];
            if (!(selectedRow.Tag is CameraInfoPylon2 cameraInfoPylon2))
            {
                return;
            }

            var form = new Form();
            form.Controls.Add(new PropertyGrid() { Dock = DockStyle.Fill, SelectedObject = cameraInfoPylon2/*, PropertySort = PropertySort.NoSort */});
            form.ShowDialog(this);

            FillRow(selectedRow, cameraInfoPylon2);
        }
    }
}
