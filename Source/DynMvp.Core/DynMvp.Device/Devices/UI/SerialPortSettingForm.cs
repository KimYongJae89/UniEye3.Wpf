using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
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
    public partial class SerialPortSettingForm : Form
    {
        private SerialPortEx serialPortEx = new SerialPortEx();
        public SerialPortInfo SerialPortInfo { get; set; }
        public bool EnablePortNo { get; set; } = false;

        private bool onUpdate;

        public string PortName { get; set; } = "";

        public SerialPortSettingForm()
        {
            InitializeComponent();

            groupBoxProperty.Text = StringManager.GetString(groupBoxProperty.Text);
            labelName.Text = StringManager.GetString(labelName.Text);
            labelPortNo.Text = StringManager.GetString(labelPortNo.Text);
            labelBaudRate.Text = StringManager.GetString(labelBaudRate.Text);
            labelDataBits.Text = StringManager.GetString(labelDataBits.Text);
            labelParity.Text = StringManager.GetString(labelParity.Text);
            labelStopBits.Text = StringManager.GetString(labelStopBits.Text);
            labelHandshake.Text = StringManager.GetString(labelHandshake.Text);
            checkBoxRtsEnable.Text = StringManager.GetString(checkBoxRtsEnable.Text);
            checkBoxDtrEnable.Text = StringManager.GetString(checkBoxDtrEnable.Text);
            btnOK.Text = StringManager.GetString(btnOK.Text);
            btnCancel.Text = StringManager.GetString(btnCancel.Text);

            serialPortEx.PacketReceived = serialPortEx_PacketReceived;
            serialPortEx.PacketTransmitted = serialPortEx_PacketTransmitted;
        }

        private void serialPortEx_PacketTransmitted(byte[] dataByte)
        {
            AddGridViewTest("Tx", Encoding.Default.GetString(dataByte));
        }

        private void serialPortEx_PacketReceived(byte[] dataByte)
        {
            AddGridViewTest("Rx", Encoding.Default.GetString(dataByte));
        }

        private delegate void AddGridViewTestDelegate(string v1, string v2);
        private void AddGridViewTest(string v1, string v2)
        {
            if (InvokeRequired)
            {
                Invoke(new AddGridViewTestDelegate(AddGridViewTest), v1, v2);
                return;
            }

            dataGridViewTest.Rows.Insert(0, v1, v2);
        }

        private void SerialPortSettingForm_Load(object sender, EventArgs e)
        {
            UpdatePortCombo();

            UpdateData();
            try
            {
                serialPortEx.Open("SerialPort", SerialPortInfo);
                serialPortEx.StartListening();
            }
            catch (InvalidResourceException)
            {

            }
        }

        private void UpdatePortCombo()
        {
            onUpdate = true;

            SerialPortManager.FillComboAllPort(comboPortName);
            if (string.IsNullOrEmpty(PortName))
            {
                comboPortName.Text = SerialPortInfo.PortName;
            }
            else
            {
                comboPortName.Text = PortName;
                SerialPortInfo.PortName = comboPortName.Text;
            }

            onUpdate = false;
        }

        private void UpdateData()
        {
            onUpdate = true;

            textName.Text = SerialPortInfo.Name;

            //comboPortName.Enabled = enablePortNo;

            baudRate.Text = SerialPortInfo.BaudRate.ToString();
            parity.Text = SerialPortInfo.Parity.ToString();

            var stopBitsValues = new StopBits[] { StopBits.One, StopBits.OnePointFive, StopBits.Two };
            stopBits.SelectedIndex = stopBitsValues.ToList().IndexOf(SerialPortInfo.StopBits);

            dataBits.Text = SerialPortInfo.DataBits.ToString();

            var handShakes = new Handshake[] { Handshake.None, Handshake.RequestToSend, Handshake.RequestToSendXOnXOff, Handshake.XOnXOff };
            comboBoxHandshake.SelectedIndex = handShakes.ToList().IndexOf(SerialPortInfo.Handshake);

            checkBoxRtsEnable.Checked = SerialPortInfo.RtsEnable;
            checkBoxDtrEnable.Checked = SerialPortInfo.DtrEnable;

            onUpdate = false;
        }

        private void Apply(SerialPortInfo serialPortInfo)
        {
            serialPortInfo.Name = textName.Text;
            serialPortInfo.PortName = comboPortName.Text;
            serialPortInfo.BaudRate = Convert.ToInt32(baudRate.Text);
            serialPortInfo.Parity = (Parity)Enum.Parse(typeof(Parity), parity.Text);
            serialPortInfo.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopBits.Text);
            serialPortInfo.DataBits = Convert.ToInt32(dataBits.Text);

            var stopBitsValues = new StopBits[] { StopBits.One, StopBits.OnePointFive, StopBits.Two };
            serialPortInfo.StopBits = stopBitsValues[stopBits.SelectedIndex];

            var handShakes = new Handshake[] { Handshake.None, Handshake.RequestToSend, Handshake.RequestToSendXOnXOff, Handshake.XOnXOff };
            serialPortInfo.Handshake = handShakes[comboBoxHandshake.SelectedIndex];

            serialPortInfo.RtsEnable = checkBoxRtsEnable.Checked;
            serialPortInfo.DtrEnable = checkBoxDtrEnable.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Apply(SerialPortInfo);

            DialogResult = DialogResult.OK;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (onUpdate)
            {
                return;
            }

            if (serialPortEx.IsOpen)
            {
                serialPortEx.Close();
            }

            Apply(SerialPortInfo);

            serialPortEx.Open("SerialPort", SerialPortInfo);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (onUpdate)
            {
                return;
            }

            if (serialPortEx.IsOpen == false)
            {
                return;
            }

            string writeString = textBoxSend.Text;

            //System.Text.Encoding encoding = System.Text.Encoding.ASCII;
            //byte[] asciiBytes = encoding.GetBytes(writeString);

            serialPortEx.WritePacket(writeString);
        }

        private void SerialPortSettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPortEx.IsOpen)
            {
                serialPortEx.StopListening();
                serialPortEx.Close();
            }
        }
    }
}
