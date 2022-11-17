using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class VirtualCameraListForm : Form
    {
        private int requiredNumCamera;
        public int RequiredNumCamera
        {
            set => requiredNumCamera = value;
        }
        public CameraConfiguration CameraConfiguration { get; set; }

        private string folderPath;

        public VirtualCameraListForm()
        {
            InitializeComponent();

            detectAllButton.Text = StringManager.GetString(detectAllButton.Text);
            buttonMoveUp.Text = StringManager.GetString(buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);
        }

        private void VirtualCameraListForm_Load(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            cameraInfoGrid.Rows.Clear();
            for (int i = 0; i < requiredNumCamera; i++)
            {
                var row = new DataGridViewRow();
                row.CreateCells(cameraInfoGrid);

                if (CameraConfiguration.CameraInfoList.Count > i)
                {
                    CameraInfo cameraInfo = CameraConfiguration.CameraInfoList[i];
                    row.Cells[0].Value = cameraInfo.Index;
                    row.Cells[1].Value = cameraInfo.Width;
                    row.Cells[2].Value = cameraInfo.Height;
                    row.Cells[3].Value = cameraInfo.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                    row.Cells[4].Value = "Edit";
                    row.Tag = cameraInfo;
                }
                else
                {
                    row.Cells[0].Value = i;
                    row.Cells[1].Value = 0;
                    row.Cells[2].Value = 0;
                    row.Cells[3].Value = false;
                    row.Cells[4].Value = "Edit";
                }
                cameraInfoGrid.Rows.Add(row);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //cameraConfiguration.Clear();

            //foreach (DataGridViewRow row in cameraInfoGrid.Rows)
            //{
            //    if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null)
            //    {
            //        CameraInfoVirtual cameraInfo = new CameraInfoVirtual();
            //        cameraInfo.Index = int.Parse(row.Cells[0].Value.ToString());
            //        cameraInfo.Width = int.Parse(row.Cells[1].Value.ToString());
            //        cameraInfo.Height = int.Parse(row.Cells[2].Value.ToString());
            //        cameraInfo.PixelFormat = (bool.Parse(row.Cells[3].Value.ToString()) == true ? System.Drawing.Imaging.PixelFormat.Format24bppRgb : System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            //        cameraInfo.FolderPath = folderPath;
            //        cameraConfiguration.AddCameraInfo(cameraInfo);
            //    }
            //}

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void autoDetectButton_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(dialog.SelectedPath) == false)
                {
                    return;
                }

                uint cameraIndex = 0;

                foreach (DataGridViewRow row in cameraInfoGrid.Rows)
                {
                    string searchPattern = string.Format("*.bmp");

                    string[] imagePath = Directory.GetFiles(dialog.SelectedPath, searchPattern);

                    if (imagePath.Count() == 0)
                    {
                        continue;
                    }

                    folderPath = dialog.SelectedPath;

                    var defaultImage = new Bitmap(imagePath[0]);

                    row.Cells[1].Value = defaultImage.Width;
                    row.Cells[2].Value = defaultImage.Height;
                    row.Cells[3].Value = defaultImage.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                    cameraIndex++;

                    defaultImage.Dispose();
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

        private void cameraInfoGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var cameraInfo = (CameraInfoVirtual)cameraInfoGrid.Rows[e.RowIndex].Tag;
            ShowPropWindow(cameraInfo);
        }

        private void ShowPropWindow(CameraInfoVirtual cameraInfo)
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
