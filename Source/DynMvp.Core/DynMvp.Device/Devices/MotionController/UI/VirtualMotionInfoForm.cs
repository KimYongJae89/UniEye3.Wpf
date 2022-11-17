using DynMvp.Base;
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
    public partial class VirtualMotionInfoForm : Form
    {
        public VirtualMotionInfo VirtualMotionInfo { get; set; }

        public VirtualMotionInfoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(labelName.Text);
            labelNumAxis.Text = StringManager.GetString(labelNumAxis.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

        }

        private void VirtualMotionInfoForm_Load(object sender, EventArgs e)
        {
            name.Text = VirtualMotionInfo.Name;
            numAxis.Value = VirtualMotionInfo.NumAxis;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            VirtualMotionInfo.Name = name.Text;
            VirtualMotionInfo.NumAxis = (int)numAxis.Value;
        }
    }
}
