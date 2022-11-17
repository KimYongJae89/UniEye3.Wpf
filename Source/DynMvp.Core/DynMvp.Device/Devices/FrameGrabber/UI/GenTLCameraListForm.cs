using DynMvp.Base;
using DynMvp.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class GenTLCameraListForm : Form
    {
        private int requiredNumCamera;
        public int RequiredNumCamera
        {
            set => requiredNumCamera = value;
        }
        public CameraConfiguration CameraConfiguration { get; set; }

        public GenTLCameraListForm()
        {
            InitializeComponent();

            columnDirectionType.Items.AddRange(Enum.GetNames(typeof(CameraInfoGenTL.EScanDirectionType)));
            columnClientType.Items.AddRange(Enum.GetNames(typeof(CameraInfoGenTL.EClientType)));
            buttonOK.Text = StringManager.GetString(GetType().FullName, buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(GetType().FullName, buttonCancel.Text);
        }

        private void GenTLCameraListForm_Load(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            cameraInfoGrid.Rows.Clear();
            for (int i = 0; i < CameraConfiguration.CameraInfoList.Count; i++)
            {
                CameraInfo cameraInfo = CameraConfiguration.CameraInfoList[i];
                if (cameraInfo is CameraInfoGenTL cameraInfoGenTL)
                {
                    int idx = cameraInfoGrid.Rows.Add("Edit", i, cameraInfoGenTL.Width, cameraInfoGenTL.Height, cameraInfoGenTL.ScanLength, cameraInfoGenTL.FrameNum, cameraInfoGenTL.OffsetX, cameraInfoGenTL.ClientType.ToString(), cameraInfoGenTL.DirectionType.ToString(), false, cameraInfoGenTL.BinningVertical, "");
                    cameraInfoGrid.Rows[idx].Tag = cameraInfoGenTL;

                    //DataGridViewButtonCell dataGridViewButtonCell = cameraInfoGrid.Rows[idx].Cells[10] as DataGridViewButtonCell;
                    //dataGridViewButtonCell.
                }
            }

            if (cameraInfoGrid.Rows.Count < requiredNumCamera)
            {
                cameraInfoGrid.Rows.Add();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (ApplyData() == false)
            {
                MessageForm.Show(null, "Plaease, Check the values");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ApplyData()
        {
            var newCameraConfiguration = new CameraConfiguration();

            foreach (DataGridViewRow row in cameraInfoGrid.Rows)
            {
                if (row.IsNewRow == true)
                {
                    continue;
                }

                if (!(row.Tag is CameraInfoGenTL cameraInfoGenTL))
                {
                    continue;
                }

                bool ok = true;
                CameraInfoGenTL.EClientType clientType = CameraInfoGenTL.EClientType.Master;
                CameraInfoGenTL.EScanDirectionType directionType = CameraInfoGenTL.EScanDirectionType.Forward;

                ok &= int.TryParse(row.Cells[2].Value.ToString(), out int width);
                ok &= int.TryParse(row.Cells[3].Value.ToString(), out int height);
                ok &= uint.TryParse(row.Cells[4].Value.ToString(), out uint scanLength);
                ok &= uint.TryParse(row.Cells[5].Value.ToString(), out uint frameNo);
                ok &= uint.TryParse(row.Cells[6].Value.ToString(), out uint offsetX);
                ok &= Enum.TryParse<CameraInfoGenTL.EClientType>(row.Cells[7].Value.ToString(), out clientType);
                ok &= Enum.TryParse<CameraInfoGenTL.EScanDirectionType>(row.Cells[8].Value.ToString(), out directionType);
                ok &= bool.TryParse(row.Cells[9].Value.ToString(), out bool useMilBuf);
                ok &= bool.TryParse(row.Cells[10].Value.ToString(), out bool binningVertical);

                if (ok == false)
                {
                    return false;
                }

                cameraInfoGenTL.Width = width;
                cameraInfoGenTL.Height = height;
                cameraInfoGenTL.ScanLength = scanLength;
                cameraInfoGenTL.FrameNum = frameNo;
                cameraInfoGenTL.OffsetX = offsetX;
                cameraInfoGenTL.ClientType = clientType;
                //                cameraInfoGenTL.UseMilBuffer = useMilBuf;
                cameraInfoGenTL.BinningVertical = binningVertical;
                cameraInfoGenTL.DirectionType = directionType;
                //                cameraInfoGenTL.VirtualImagePath = row.Cells[11].Value?.ToString();
                newCameraConfiguration.AddCameraInfo(cameraInfoGenTL);
            }

            CameraConfiguration.Clear();
            foreach (CameraInfo cameraInfo in newCameraConfiguration.CameraInfoList)
            {
                CameraConfiguration.AddCameraInfo(cameraInfo);
            }

            return true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cameraInfoGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0)
            {
                return;
            }

            var cameraInfoGenTL = (CameraInfoGenTL)cameraInfoGrid.Rows[e.RowIndex].Tag;

            var propertyGrid = new PropertyGrid();
            propertyGrid.SelectedObject = cameraInfoGenTL;
            propertyGrid.Dock = DockStyle.Fill;

            var form = new Form();
            form.Controls.Add(propertyGrid);
            form.ShowDialog();
            UpdateData();
        }
    }
}
