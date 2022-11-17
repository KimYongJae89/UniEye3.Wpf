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

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class NewGrabberForm : Form
    {
        public GrabberType GrabberType { get; set; }
        public string GrabberName { get; set; }
        public int NumCamera { get; set; }

        public NewGrabberForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(labelName.Text);
            labelGrabberType.Text = StringManager.GetString(labelGrabberType.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

            cmbGrabberType.DataSource = Enum.GetNames(typeof(GrabberType));
        }

        private void GrabberInfoForm_Load(object sender, EventArgs e)
        {
            cmbGrabberType.SelectedIndex = (int)GrabberType;
            txtName.Text = GrabberName;
            txtNumCamera.Value = NumCamera;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            GrabberType = (GrabberType)cmbGrabberType.SelectedIndex;
            GrabberName = txtName.Text;
            NumCamera = (int)txtNumCamera.Value;
        }

        private void CmbGrabberType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.Text = ((GrabberType)cmbGrabberType.SelectedIndex).ToString();
            }
        }

        private void TxtNumCamera_ValueChanged(object sender, EventArgs e)
        {
            NumCamera = (int)txtNumCamera.Value;
        }
    }
}
