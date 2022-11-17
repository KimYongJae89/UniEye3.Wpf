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
    public partial class EuresysBoardListForm : Form
    {
        private int requiredNumCamera;
        public int RequiredNumCamera
        {
            set => requiredNumCamera = value;
        }
        public CameraConfiguration CameraConfiguration { get; set; }

        public EuresysBoardListForm()
        {
            InitializeComponent();

            buttonMoveUp.Text = StringManager.GetString(buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);
        }

        private void EuresysBoardListForm_Load(object sender, EventArgs e)
        {
            foreach (CameraInfo cameraInfo in CameraConfiguration)
            {
                if (cameraInfo is CameraInfoMultiCam cameraInfoMultiCam)
                {
                    cameraInfoGrid.Rows.Add(
                        cameraInfoMultiCam.BoardType.ToString(),
                        cameraInfoMultiCam.BoardId,
                        cameraInfoMultiCam.ConnectorId,
                        cameraInfoMultiCam.CameraType.ToString(),
                        cameraInfoMultiCam.SurfaceNum.ToString(),
                        cameraInfoMultiCam.PageLength.ToString(),
                        cameraInfoMultiCam.EdgeStartPos.ToString(),
                        cameraInfoMultiCam.ROIStartPos.ToString(),
                        cameraInfoMultiCam.ROIWidth.ToString(),
                        "False");
                }
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
                if (row.Cells[0].Value != null &&
                    row.Cells[1].Value != null &&
                    row.Cells[2].Value != null &&
                    row.Cells[3].Value != null &&
                    row.Cells[4].Value != null &&
                    row.Cells[5].Value != null &&
                    row.Cells[6].Value != null &&
                    row.Cells[7].Value != null &&
                    row.Cells[8].Value != null)
                {
                    var cameraInfoMultiCam = new CameraInfoMultiCam(
                                    (EuresysBoardType)Enum.Parse(typeof(EuresysBoardType), row.Cells[0].Value.ToString()),
                                    uint.Parse(row.Cells[1].Value.ToString()),
                                    uint.Parse(row.Cells[2].Value.ToString()),
                                    (CameraType)Enum.Parse(typeof(CameraType), row.Cells[3].Value.ToString()),
                                    uint.Parse(row.Cells[4].Value.ToString()),
                                    uint.Parse(row.Cells[5].Value.ToString()),
                                    (EdgeStartPos)Enum.Parse(typeof(EdgeStartPos), row.Cells[6].Value.ToString()),
                                    uint.Parse(row.Cells[7].Value.ToString()),
                                    uint.Parse(row.Cells[8].Value.ToString()));

                    // cameraInfoMultiCam.UseNativeBuffering = bool.Parse(row.Cells[6].Value.ToString()); 
                    CameraConfiguration.AddCameraInfo(cameraInfoMultiCam);
                }
            }

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
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
