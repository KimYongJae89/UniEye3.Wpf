using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.MotionController.UI
{
    public partial class NewMotionForm : Form
    {
        public MotionType MotionType { get; set; }
        public string MotionName { get; set; }

        public NewMotionForm()
        {
            InitializeComponent();

            labelMotionName.Text = StringManager.GetString(labelMotionName.Text);
            labelMotionType.Text = StringManager.GetString(labelMotionType.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

            cmbMotionType.DataSource = Enum.GetNames(typeof(MotionType));
        }

        private void GrabberInfoForm_Load(object sender, EventArgs e)
        {
            cmbMotionType.SelectedIndex = (int)MotionType;
            txtMotionName.Text = MotionName;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            MotionType = (MotionType)cmbMotionType.SelectedIndex;
            MotionName = txtMotionName.Text;
        }

        private void cmbMotionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMotionName.Text))
            {
                txtMotionName.Text = ((MotionType)cmbMotionType.SelectedIndex).ToString();
            }
        }
    }
}
