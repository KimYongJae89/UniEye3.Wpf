using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public partial class IoPortViewer : Form
    {
        public bool isClose = false;

        private bool intialized = false;

        private PortMap portMap;
        private DigitalIoHandler digitalIoHandler;
        private bool[] outputValueChecker;
        private bool[] inputValueChecker;

        private int oneTimeIndex = 0;
        private List<int> oneTimeArr = new List<int>();

        private List<int> virtualLongRunArr = new List<int>();
        private bool onVirtualSignalOn = false;

        public IoPortViewer()
        {
            InitializeComponent();

            inportTable.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(inportTable_CellMouseDown);
            inportTable.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(inportTable_CellMouseUp);

            labelDigitalIo.Text = StringManager.GetString(labelDigitalIo.Text);
            labelInput.Text = StringManager.GetString(labelInput.Text);
            labelOutput.Text = StringManager.GetString(labelOutput.Text);
            closeButton.Text = StringManager.GetString(closeButton.Text);

            digitalIoHandler = DeviceManager.Instance().DigitalIoHandler;
            portMap = DeviceManager.Instance().PortMap;

            int index = 1;
            foreach (IDigitalIo digitalIo in digitalIoHandler)
            {
                comboBoxDigitalIo.Items.Add(string.Format("{0} : {1}", index, digitalIo.GetName()));
                index++;
            }

            if (comboBoxDigitalIo.Items.Count != 0)
            {
                comboBoxDigitalIo.SelectedIndex = 0;
            }

            InitGroupCombo();

            oneTimeArr.Add(0);
            oneTimeArr.Add(0);
            oneTimeArr.Add(1);
            oneTimeArr.Add(1);

            virtualLongRunArr.Add(0);
            virtualLongRunArr.Add(0);
            virtualLongRunArr.Add(1);
            virtualLongRunArr.Add(1);

            intialized = true;
        }

        private void IoPortViewer_Load(object sender, EventArgs e)
        {
            InitInPortTable();
            InitOutPortTable();

            timer.Start();
        }

        private void InitGroupCombo()
        {
            comboBoxInPortGroup.Items.Clear();
            comboBoxOutPortGroup.Items.Clear();

            int deviceIndex = comboBoxDigitalIo.SelectedIndex;
            IDigitalIo digitalIo = digitalIoHandler.Get(deviceIndex);
            if (digitalIo == null)
            {
                return;
            }

            for (int i = 0; i < digitalIo.GetNumInPortGroup(); i++)
            {
                comboBoxInPortGroup.Items.Add(string.Format("Group {0} ", i));
            }
            comboBoxInPortGroup.SelectedIndex = 0;

            for (int i = 0; i < digitalIo.GetNumOutPortGroup(); i++)
            {
                comboBoxOutPortGroup.Items.Add(string.Format("Group {0} ", i));
            }
            comboBoxOutPortGroup.SelectedIndex = 0;
        }

        private void InitInPortTable()
        {
            inportTable.Rows.Clear();

            int deviceNo = comboBoxDigitalIo.SelectedIndex;
            IDigitalIo digitalIo = digitalIoHandler.Get(deviceNo);
            if (digitalIo == null)
            {
                return;
            }

            int groupNo = comboBoxInPortGroup.SelectedIndex;
            int numInPorts = digitalIo.GetNumInPort();

            int index = 0;
            List<IoPort> ioPortList = portMap.GetPorts(IoDirection.Input, deviceNo, groupNo);
            foreach (IoPort ioPort in ioPortList)
            {
                int rowIdx = inportTable.Rows.Add(index, ioPort.Name);
                inportTable.Rows[rowIdx].Cells[2].Value = global::DynMvp.Devices.Properties.Resources.led_off;
                index++;
            }
            inputValueChecker = new bool[index];
        }

        private void InitOutPortTable()
        {
            outportTable.Rows.Clear();

            int deviceNo = comboBoxDigitalIo.SelectedIndex;
            IDigitalIo digitalIo = digitalIoHandler.Get(deviceNo);
            if (digitalIo == null)
            {
                return;
            }

            int groupNo = comboBoxOutPortGroup.SelectedIndex;
            int numOutPorts = digitalIo.GetNumOutPort();

            int index = 0;
            List<IoPort> ioPortList = portMap.GetPorts(IoDirection.Output, deviceNo, groupNo);
            foreach (IoPort ioPort in ioPortList)
            {
                int rowIdx = outportTable.Rows.Add(index, ioPort.Name);
                outportTable.Rows[rowIdx].Cells[2].Value = global::DynMvp.Devices.Properties.Resources.Output_Off;
                index++;
            }
            outputValueChecker = new bool[index];
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                UpdatePortTable();
            }
        }

        private void UpdatePortTable()
        {
            int deviceIndex = comboBoxDigitalIo.SelectedIndex;
            IDigitalIo digitalIo = digitalIoHandler.Get(deviceIndex);
            if (digitalIo == null)
            {
                return;
            }

            int inPortGroupIndex = comboBoxInPortGroup.SelectedIndex;

            int numInPorts = digitalIo.GetNumInPort();
            uint inputValue = digitalIoHandler.ReadInputGroup(deviceIndex, inPortGroupIndex);
            for (int i = 0; i < inportTable.Rows.Count; i++)
            {
                bool value = ((inputValue >> i) & 0x1) == 1;

                if (inputValueChecker[i] != value)
                {
                    if (value)
                    {
                        inportTable.Rows[i].Cells[2].Value = global::DynMvp.Devices.Properties.Resources.led_on;
                    }
                    else
                    {
                        inportTable.Rows[i].Cells[2].Value = global::DynMvp.Devices.Properties.Resources.led_off;
                    }
                }
                inputValueChecker[i] = value;
            }

            int outPortGroupIndex = comboBoxOutPortGroup.SelectedIndex;

            int numOutPorts = digitalIo.GetNumOutPort();
            uint outputValue = digitalIoHandler.ReadOutputGroup(deviceIndex, outPortGroupIndex);
            for (int i = 0; i < outportTable.Rows.Count; i++)
            {
                bool value = ((outputValue >> i) & 0x1) == 1;
                if (outputValueChecker[i] != value)
                {
                    if (value)
                    {
                        outportTable.Rows[i].Cells[2].Value = global::DynMvp.Devices.Properties.Resources.Output_On;
                    }
                    else
                    {
                        outportTable.Rows[i].Cells[2].Value = global::DynMvp.Devices.Properties.Resources.Output_Off;
                    }
                }
                outputValueChecker[i] = value;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void outportTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                int index = e.RowIndex;

                bool value = outputValueChecker[index];

                string portName = "";
                object cellValue = outportTable.Rows[e.RowIndex].Cells[1].Value;
                if (cellValue != null)
                {
                    portName = cellValue.ToString();
                }
                var ioPort = new IoPort(IoDirection.Output, "Temp", "Temp", e.RowIndex, comboBoxOutPortGroup.SelectedIndex, comboBoxDigitalIo.SelectedIndex);
                LogHelper.Debug(LoggerType.Device, string.Format("Write Output : {0} -> {1}", e.RowIndex, (!value).ToString()));
                digitalIoHandler.WriteOutput(ioPort, !value);
            }
        }

        private void IoPortViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modal == false)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void comboBoxDigitalIo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (intialized)
            {
                InitGroupCombo();
            }
        }

        private void inportTable_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                int index = e.RowIndex;

                bool value = inputValueChecker[index];

                string portName = "";
                object cellValue = inportTable.Rows[e.RowIndex].Cells[1].Value;
                if (cellValue != null)
                {
                    portName = cellValue.ToString();
                }
                var ioPort = new IoPort(IoDirection.Input, "Temp", "Temp", e.RowIndex, comboBoxOutPortGroup.SelectedIndex, comboBoxDigitalIo.SelectedIndex);
                LogHelper.Debug(LoggerType.Device, string.Format("Virtual Input : {0} -> {1}", e.RowIndex, (!value).ToString()));
                digitalIoHandler.WriteInput(ioPort, !value);
            }
        }

        private void inportTable_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void comboBoxInPortGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (intialized)
            {
                InitInPortTable();
            }
        }

        private void comboBoxOutPortGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (intialized)
            {
                InitOutPortTable();
            }
        }

        private void buttonLongRun_Click(object sender, EventArgs e)
        {
            if (onVirtualSignalOn == false)
            {
                IoPort ioPort = portMap.GetPort("Vision Ready");
                digitalIoHandler.WriteOutput(ioPort, true);

                for (int i = 0; i < 16; i++)
                {
                    if (inputValueChecker[i] == false)
                    {
                        inputValueChecker[i] = true;
                    }

                    SetInPortSignal(i);
                    UpdatePortTable();
                }
                onVirtualSignalOn = true;
                buttonLongRun.Text = "Long Run Off";
                timerLongRun.Interval = 300;
                timer.Stop();

                timerLongRun.Start();
            }
            else
            {
                IoPort ioPort = portMap.GetPort("Vision Ready");
                digitalIoHandler.WriteOutput(ioPort, false);

                timerLongRun.Stop();

                onVirtualSignalOn = false;
                buttonLongRun.Text = "Long Run";

                for (int i = 0; i < 16; i++)
                {
                    if (inputValueChecker[i] == false)
                    {
                        inputValueChecker[i] = true;
                    }

                    SetInPortSignal(i);
                    UpdatePortTable();
                }

                timer.Start();
            }
        }

        private void buttonOneTimeRun_Click(object sender, EventArgs e)
        {
            Thread.Sleep(100);
            buttonOneTimeRun.Enabled = false;
            for (int i = 0; i < 16; i++)
            {
                if (inputValueChecker[i] == false)
                {
                    inputValueChecker[i] = true;
                }

                SetInPortSignal(i);
                UpdatePortTable();
            }
            timer.Stop();

            timer2.Start();
        }

        private void SetInPortSignal(int rowIndex)
        {
            int index = rowIndex;

            bool value = inputValueChecker[index];

            string portName = "";
            object cellValue = inportTable.Rows[rowIndex].Cells[1].Value;
            if (cellValue != null)
            {
                portName = cellValue.ToString();
            }
            var ioPort = new IoPort(IoDirection.Input, portName);
            ioPort.Set(rowIndex, comboBoxDigitalIo.SelectedIndex);

            digitalIoHandler.WriteInput(ioPort, !value);
        }

        private void timerLongRun_Tick(object sender, EventArgs e)
        {
            SetInPortSignal(oneTimeArr[oneTimeIndex]);
            UpdatePortTable();

            oneTimeIndex++;
            if (oneTimeIndex >= 2)
            {
                timerLongRun.Stop();
                timer4.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            IoPort ioPort = portMap.GetPort("Complete");
            if (digitalIoHandler.ReadOutput(ioPort) == true)
            {
                timer3.Start();
                timer1.Stop();
            }
            else
            {
                return;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            SetInPortSignal(oneTimeArr[oneTimeIndex]);
            UpdatePortTable();

            oneTimeIndex++;
            if (oneTimeIndex >= 2)
            {
                timer2.Stop();
                timer1.Start();
            }

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            SetInPortSignal(oneTimeArr[oneTimeIndex]);
            UpdatePortTable();

            oneTimeIndex++;
            if (oneTimeIndex >= 4)
            {
                oneTimeIndex = 0;

                IoPort ioPort = portMap.GetPort("Complete");
                digitalIoHandler.WriteOutput(ioPort, !digitalIoHandler.ReadOutput(ioPort));

                buttonOneTimeRun.Enabled = true;

                timer.Start();
                timer3.Stop();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            IoPort ioPort = portMap.GetPort("Complete");
            if (digitalIoHandler.ReadOutput(ioPort) == true)
            {
                timer5.Start();
                timer4.Stop();
            }
            else
            {
                return;
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            SetInPortSignal(oneTimeArr[oneTimeIndex]);
            UpdatePortTable();

            oneTimeIndex++;
            if (oneTimeIndex >= 4)
            {
                oneTimeIndex = 0;

                IoPort ioPort = portMap.GetPort("Complete");
                digitalIoHandler.WriteOutput(ioPort, false);

                timerLongRun.Start();
                timer5.Stop();
            }
        }
    }
}
