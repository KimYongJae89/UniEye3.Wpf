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
    public partial class TcpIpSettingForm : Form
    {
        public TcpIpInfo TcpIpInfo { get; set; }

        public TcpIpSettingForm(TcpIpInfo tcpIpInfo)
        {
            InitializeComponent();

            labelIpAddress.Text = StringManager.GetString(labelIpAddress.Text);
            labelPortNo.Text = StringManager.GetString(labelPortNo.Text);

            btnOK.Text = StringManager.GetString(btnOK.Text);
            btnCancel.Text = StringManager.GetString(btnCancel.Text);

            TcpIpInfo = tcpIpInfo;
        }

        private void TcpIpSettingForm_Load(object sender, EventArgs e)
        {
            ipAddress.Text = TcpIpInfo.IpAddress;
            nudPortNo.Value = TcpIpInfo.PortNo;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TcpIpInfo.IpAddress = ipAddress.Text.Replace(" ", "");
            TcpIpInfo.PortNo = (int)nudPortNo.Value;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}
