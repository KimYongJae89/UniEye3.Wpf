using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public partial class FinsSettingForm : Form
    {
        public FinsInfo FinsInfo { get; set; }

        public FinsSettingForm()
        {
            InitializeComponent();

            labelIpAddress.Text = StringManager.GetString(labelIpAddress.Text);
            labelPortNo.Text = StringManager.GetString(labelPortNo.Text);
            labelNetworkNo.Text = StringManager.GetString(labelNetworkNo.Text);

            labelPlcStateAddress.Text = StringManager.GetString(labelPlcStateAddress.Text);
            labelPcStateAddress.Text = StringManager.GetString(labelPcStateAddress.Text);
            labelResultAddress.Text = StringManager.GetString(labelResultAddress.Text);

            btnOK.Text = StringManager.GetString(btnOK.Text);
            btnCancel.Text = StringManager.GetString(btnCancel.Text);

        }

        private void FinsSettingForm_Load(object sender, EventArgs e)
        {
            ipAddress.Text = FinsInfo.IpAddress;
            portNo.Value = FinsInfo.PortNo;
            networkNo.Value = FinsInfo.NetworkNo;
            pcStateAddress.Value = FinsInfo.PcStateAddress;
            plcStateAddress.Value = FinsInfo.PlcStateAddress;
            resultAddress.Value = FinsInfo.ResultAddress;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            FinsInfo.IpAddress = ipAddress.Text.Replace(" ", "");
            FinsInfo.PortNo = (int)portNo.Value;
            FinsInfo.NetworkNo = (int)networkNo.Value;
            FinsInfo.PcStateAddress = (int)pcStateAddress.Value;
            FinsInfo.PlcStateAddress = (int)plcStateAddress.Value;
            FinsInfo.ResultAddress = (int)resultAddress.Value;
        }
    }
}
