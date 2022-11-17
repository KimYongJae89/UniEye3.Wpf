using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public partial class SerialMotionInfoForm : Form
    {
        public SerialMotionInfo SerialMotionInfo { get; set; }

        public SerialMotionInfoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(labelName.Text);
            groupBoxProperty.Text = StringManager.GetString(groupBoxProperty.Text);
            labelPortNo.Text = StringManager.GetString(labelPortNo.Text);
            labelBaudRate.Text = StringManager.GetString(labelBaudRate.Text);
            labelDataBits.Text = StringManager.GetString(labelDataBits.Text);
            labelParity.Text = StringManager.GetString(labelParity.Text);
            label1.Text = StringManager.GetString(label1.Text);
            btnOK.Text = StringManager.GetString(btnOK.Text);
            btnCancel.Text = StringManager.GetString(btnCancel.Text);

        }

        private void SerialMotionInfoForm_Load(object sender, EventArgs e)
        {
            SerialPortManager.FillComboAllPort(comboPortName);

            txtName.Text = SerialMotionInfo.Name;

            SerialPortInfo serialPortInfo = SerialMotionInfo.SerialPortInfo;

            comboPortName.Text = serialPortInfo.PortName;
            baudRate.Text = serialPortInfo.BaudRate.ToString();
            parity.Text = serialPortInfo.Parity.ToString();

            var stopBitsValues = new StopBits[] { StopBits.One, StopBits.OnePointFive, StopBits.Two };
            stopBits.SelectedIndex = stopBitsValues.ToList().IndexOf(serialPortInfo.StopBits);

            dataBits.Text = serialPortInfo.DataBits.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SerialMotionInfo.Name = txtName.Text;

            SerialPortInfo serialPortInfo = SerialMotionInfo.SerialPortInfo;

            serialPortInfo.PortName = comboPortName.Text;
            serialPortInfo.BaudRate = Convert.ToInt32(baudRate.Text);
            serialPortInfo.Parity = (Parity)Enum.Parse(typeof(Parity), parity.Text);
            serialPortInfo.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopBits.Text);
            serialPortInfo.DataBits = Convert.ToInt32(dataBits.Text);

            var stopBitsValues = new StopBits[] { StopBits.One, StopBits.OnePointFive, StopBits.Two };
            serialPortInfo.StopBits = stopBitsValues[stopBits.SelectedIndex];

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
