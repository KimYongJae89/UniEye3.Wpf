using DynMvp.Base;
using DynMvp.Devices.Daq;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.Daq.UI
{
    public partial class NewDaqChannelForm : Form
    {
        public string DaqChannelName { get; set; }
        public DaqChannelType DaqChannelType { get; set; }

        public NewDaqChannelForm()
        {
            InitializeComponent();

            labelMotionName.Text = StringManager.GetString(labelMotionName.Text);
            labelDaqChannelType.Text = StringManager.GetString(labelDaqChannelType.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

            cmbDaqChannelType.DataSource = Enum.GetNames(typeof(DaqChannelType));
        }

        private void GrabberInfoForm_Load(object sender, EventArgs e)
        {
            cmbDaqChannelType.SelectedIndex = (int)DaqChannelType;
            txtName.Text = DaqChannelName;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DaqChannelName = txtName.Text;
            DaqChannelType = (DaqChannelType)cmbDaqChannelType.SelectedIndex;
        }
    }
}
