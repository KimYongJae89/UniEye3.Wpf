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
    public partial class MelsecSettingForm : Form
    {
        public MelsecInfo MelsecInfo { get; set; }

        public MelsecSettingForm(MelsecInfo melsecInfo)
        {
            InitializeComponent();

            labelIpAddress.Text = StringManager.GetString(labelIpAddress.Text);
            labelPortNo.Text = StringManager.GetString(labelPortNo.Text);
            btnOK.Text = StringManager.GetString(btnOK.Text);
            btnCancel.Text = StringManager.GetString(btnCancel.Text);

            MelsecInfo = melsecInfo;
        }

        private void TcpIpSettingForm_Load(object sender, EventArgs e)
        {
            ipAddress.Text = MelsecInfo.TcpIpInfo.IpAddress;
            nudPortNo.Value = MelsecInfo.TcpIpInfo.PortNo;

            nudNetworkNo.Value = MelsecInfo.NetworkNo;
            nudPlcNo.Value = MelsecInfo.PlcNo;
            nudModuleIoNo.Value = MelsecInfo.ModuleIoNo;
            nudModuleDeviceNo.Value = MelsecInfo.ModuleDeviceNo;
            nudCpuInspectorData.Value = MelsecInfo.CpuInspectorData;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            MelsecInfo.TcpIpInfo = new TcpIpInfo(ipAddress.Text.Replace(" ", ""), (int)nudPortNo.Value);

            MelsecInfo.NetworkNo = Convert.ToByte(nudNetworkNo.Value);
            MelsecInfo.PlcNo = Convert.ToByte(nudPlcNo.Value);
            MelsecInfo.ModuleIoNo = Convert.ToInt16(nudModuleIoNo.Value);
            MelsecInfo.ModuleDeviceNo = Convert.ToByte(nudModuleDeviceNo.Value);
            MelsecInfo.CpuInspectorData = Convert.ToInt16(nudCpuInspectorData.Value);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
