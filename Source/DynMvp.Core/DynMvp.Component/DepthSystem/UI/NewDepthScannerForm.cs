using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Component.DepthSystem.UI
{
    public partial class NewDepthScannerForm : Form
    {
        public DepthScannerType DepthScannerType { get; set; }
        public string DepthScannerName { get; set; }

        public NewDepthScannerForm()
        {
            InitializeComponent();

            cmbDepthScannerType.DataSource = Enum.GetNames(typeof(DepthScannerType));

            labelName.Text = StringManager.GetString(labelName.Text);
            labelGrabberType.Text = StringManager.GetString(labelGrabberType.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

        }

        private void GrabberInfoForm_Load(object sender, EventArgs e)
        {
            txtName.Text = DepthScannerName;
            cmbDepthScannerType.SelectedIndex = (int)DepthScannerType;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DepthScannerName = txtName.Text;
            DepthScannerType = (DepthScannerType)cmbDepthScannerType.SelectedIndex;
        }

        private void cmbDepthScannerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.Text = ((DepthScannerType)cmbDepthScannerType.SelectedIndex).ToString();
            }
        }
    }
}
