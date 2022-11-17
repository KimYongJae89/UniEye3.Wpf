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
    public partial class NetworkMotionInfoForm : Form
    {
        public NetworkMotionInfo NetworkMotionInfo { get; set; }

        public NetworkMotionInfoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(labelName.Text);
            labelNumAxis.Text = StringManager.GetString(labelNumAxis.Text);
            labelPortNo.Text = StringManager.GetString(labelPortNo.Text);
            labelIpAddress.Text = StringManager.GetString(labelIpAddress.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

        }

        private void NetworkMotionInfoForm_Load(object sender, EventArgs e)
        {
            name.Text = NetworkMotionInfo.Name;
            numAxis.Value = NetworkMotionInfo.NumAxis;
            ipAddress.Text = NetworkMotionInfo.IpAddress;
            portNo.Value = NetworkMotionInfo.PortNo;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            NetworkMotionInfo.Name = name.Text;
            NetworkMotionInfo.NumAxis = (int)numAxis.Value;
            NetworkMotionInfo.IpAddress = ipAddress.Text;
            NetworkMotionInfo.PortNo = (byte)portNo.Value;
        }
    }
}
