using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.UI
{
    public partial class MelsecConnectionInfoForm : Form
    {
        public MelsecConnectionInfo MelsecConnectionInfo { get; set; }

        public MelsecConnectionInfoForm()
        {
            InitializeComponent();

            groupBoxSerialDevice.Text = StringManager.GetString(groupBoxSerialDevice.Text);
            labelIpAddress.Text = StringManager.GetString(labelIpAddress.Text);
            labelPortNo.Text = StringManager.GetString(labelPortNo.Text);
            labelStationNo.Text = StringManager.GetString(labelStationNo.Text);
            labelPcStatusAddress.Text = StringManager.GetString(labelPcStatusAddress.Text);
            labelPlcStatusAddress.Text = StringManager.GetString(labelPlcStatusAddress.Text);
            buttonOk.Text = StringManager.GetString(buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            MelsecConnectionInfo.IpAddress = ipAddress.Text;
            MelsecConnectionInfo.Port = Convert.ToInt32(portNo.Text);
            MelsecConnectionInfo.StationNumber = Convert.ToInt32(stationNumber.Text);
            MelsecConnectionInfo.PcStatusAddress = Convert.ToInt32(pcStatusAddress.Text);
            MelsecConnectionInfo.PlcStatusAddress = Convert.ToInt32(plcStatusAddress.Text);
        }

        private void MelsecConnectionInfoForm_Load(object sender, EventArgs e)
        {
            ipAddress.Text = MelsecConnectionInfo.IpAddress;
            portNo.Text = MelsecConnectionInfo.Port.ToString();
            stationNumber.Text = MelsecConnectionInfo.StationNumber.ToString();
            pcStatusAddress.Text = MelsecConnectionInfo.PcStatusAddress.ToString();
            plcStatusAddress.Text = MelsecConnectionInfo.PlcStatusAddress.ToString();
        }
    }
}
