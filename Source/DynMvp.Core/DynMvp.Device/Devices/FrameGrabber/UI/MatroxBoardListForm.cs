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
    public partial class MatroxBoardListForm : Form
    {
        private int requiredNumCamera;
        public int RequiredNumCamera
        {
            set => requiredNumCamera = value;
        }
        public CameraConfiguration CameraConfiguration { get; set; }

        public MatroxBoardListForm()
        {
            InitializeComponent();

            buttonMoveUp.Text = StringManager.GetString(buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);
        }

        private void MatroxBoardListForm_Load(object sender, EventArgs e)
        {
            foreach (CameraInfo cameraInfo in CameraConfiguration)
            {
                if (cameraInfo is CameraInfoMil cameraInfoMil)
                {
                    int row = cameraInfoGrid.Rows.Add(
                        cameraInfoMil.SystemType.ToString(),
                        cameraInfoMil.SystemNum,
                        cameraInfoMil.DigitizerNum,
                        cameraInfoMil.CameraType.ToString(),
                        cameraInfoMil.DcfFilePath,
                        0
                        );
                    cameraInfoGrid.Rows[row].Tag = cameraInfoMil;
                }
                else if (cameraInfo is CameraInfoMilCXP cameraInfoMilCXP)
                {
                    int row = cameraInfoGrid.Rows.Add(
                        cameraInfoMilCXP.SystemType.ToString(),
                        cameraInfoMilCXP.SystemNum,
                        cameraInfoMilCXP.DigitizerNum,
                        cameraInfoMilCXP.CameraType.ToString(),
                        cameraInfoMilCXP.DcfFilePath,
                        cameraInfoMilCXP.Height
                        );
                    cameraInfoGrid.Rows[row].Tag = cameraInfoMilCXP;
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
                if (row.Tag is CameraInfoMil cameraInfoMil)
                {
                    if (cameraInfoMil == null)
                    {
                        cameraInfoMil = new CameraInfoMil();
                    }

                    cameraInfoMil.SystemType = (MilSystemType)Enum.Parse(typeof(MilSystemType), row.Cells[0].Value.ToString());
                    cameraInfoMil.SystemNum = uint.Parse(row.Cells[1].Value.ToString());
                    cameraInfoMil.DigitizerNum = uint.Parse(row.Cells[2].Value.ToString());
                    cameraInfoMil.CameraType = (CameraType)Enum.Parse(typeof(CameraType), row.Cells[3].Value.ToString());
                    cameraInfoMil.DcfFilePath = (row.Cells[4].Value?.ToString()) ?? "";
                    CameraConfiguration.AddCameraInfo(cameraInfoMil);
                }
                else if (row.Tag is CameraInfoMilCXP cameraInfoMilCXP)
                {
                    if (cameraInfoMilCXP == null)
                    {
                        cameraInfoMilCXP = new CameraInfoMilCXP();
                    }

                    cameraInfoMilCXP.SystemType = (MilSystemType)Enum.Parse(typeof(MilSystemType), row.Cells[0].Value.ToString());
                    cameraInfoMilCXP.SystemNum = uint.Parse(row.Cells[1].Value.ToString());
                    cameraInfoMilCXP.DigitizerNum = uint.Parse(row.Cells[2].Value.ToString());
                    cameraInfoMilCXP.CameraType = (CameraType)Enum.Parse(typeof(CameraType), row.Cells[3].Value.ToString());
                    cameraInfoMilCXP.DcfFilePath = (row.Cells[4].Value?.ToString()) ?? "";
                    cameraInfoMilCXP.Height = Convert.ToInt32(row.Cells[5].Value?.ToString());
                    CameraConfiguration.AddCameraInfo(cameraInfoMilCXP);
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
