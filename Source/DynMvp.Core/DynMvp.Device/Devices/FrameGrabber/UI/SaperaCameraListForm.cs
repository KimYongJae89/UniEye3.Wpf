using DynMvp.Base;
using DynMvp.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class SaperaCameraListForm : Form
    {
        private int requiredNumCamera;
        public int RequiredNumCamera
        {
            set => requiredNumCamera = value;
        }
        public CameraConfiguration CameraConfiguration { get; set; }

        public SaperaCameraListForm()
        {
            InitializeComponent();

            columnDirectionType.Items.AddRange(Enum.GetNames(typeof(CameraInfoSapera.EScanDirectionType)));
            columnClientType.Items.AddRange(Enum.GetNames(typeof(CameraInfoSapera.EClientType)));
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
                if (cameraInfo is CameraInfoSapera cameraInfoGenTL)
                {
                    int idx = cameraInfoGrid.Rows.Add("Edit", i,
                        cameraInfoGenTL.Width, cameraInfoGenTL.Height,
                        "", cameraInfoGenTL.FrameNum, "",
                        cameraInfoGenTL.ClientType.ToString(), cameraInfoGenTL.ScanDirectionType.ToString(),
                        false, false, "");
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

                if (!(row.Tag is CameraInfoSapera cameraInfoGenTL))
                {
                    continue;
                }

                bool ok = true;
                CameraInfoSapera.EClientType clientType = CameraInfoSapera.EClientType.Master;
                CameraInfoSapera.EScanDirectionType directionType = CameraInfoSapera.EScanDirectionType.Forward;

                ok &= int.TryParse(row.Cells[2].Value.ToString(), out int width);
                ok &= int.TryParse(row.Cells[3].Value.ToString(), out int height);
                ok &= uint.TryParse(row.Cells[5].Value.ToString(), out uint frameNo);
                ok &= Enum.TryParse<CameraInfoSapera.EClientType>(row.Cells[7].Value.ToString(), out clientType);
                ok &= Enum.TryParse<CameraInfoSapera.EScanDirectionType>(row.Cells[8].Value.ToString(), out directionType);
                ok &= bool.TryParse(row.Cells[9].Value.ToString(), out bool useMilBuf);

                if (ok == false)
                {
                    return false;
                }

                cameraInfoGenTL.Width = width;
                cameraInfoGenTL.Height = height;
                cameraInfoGenTL.FrameNum = frameNo;
                cameraInfoGenTL.ClientType = clientType;
                //                cameraInfoGenTL.UseMilBuffer = useMilBuf;
                cameraInfoGenTL.ScanDirectionType = directionType;
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

            var cameraInfoGenTL = (CameraInfoSapera)cameraInfoGrid.Rows[e.RowIndex].Tag;

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
