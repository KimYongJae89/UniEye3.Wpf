using DynMvp.Base;
using DynMvp.Devices.Dio;
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
    public partial class PortMapBuilderForm : Form
    {
        private DigitalIoHandler digitalIoHandler;
        private PortMap portMap;

        public PortMapBuilderForm(DigitalIoHandler digitalIoHandler, PortMap portMap)
        {
            InitializeComponent();

            labelInput.Text = StringManager.GetString(labelInput.Text);
            labelOutput.Text = StringManager.GetString(labelOutput.Text);
            buttonOk.Text = StringManager.GetString(buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

            this.digitalIoHandler = digitalIoHandler;
            this.portMap = portMap;
        }

        private void PortMapBuilderForm_Load(object sender, EventArgs e)
        {
            columnInputDeviceNo.Items.Add(0);
            columnOutputDeviceNo.Items.Add(0);

            IDigitalIo digitalIo = digitalIoHandler.Get(0);
            if (digitalIo != null)
            {
                var inputPortComboData = new List<string>();

                inputPortComboData.Add("None");

                int numInPort = digitalIo.GetNumInPort();
                for (int inputPort = 0; inputPort < numInPort; inputPort++)
                {
                    inputPortComboData.Add(inputPort.ToString());
                }

                columnInputPortNo.DataSource = inputPortComboData;

                var outputPortComboData = new List<string>();

                outputPortComboData.Add("None");

                int numOutPort = digitalIo.GetNumOutPort();
                for (int outputPort = 0; outputPort < numOutPort; outputPort++)
                {
                    outputPortComboData.Add(outputPort.ToString());
                }

                columnOutputPortNo.DataSource = outputPortComboData;

                List<IoPort> inPortList = portMap.GetPorts(IoDirection.Input);
                BuildPortTable(inPortList, inportTable);

                List<IoPort> outPortList = portMap.GetPorts(IoDirection.Input);
                BuildPortTable(outPortList, outportTable);
            }
        }

        private void BuildPortTable(List<IoPort> portList, DataGridView portView)
        {
            foreach (IoPort ioPort in portList)
            {
                string portNoString;
                if (ioPort.PortNo == -1)
                {
                    portNoString = "None";
                }
                else
                {
                    portNoString = ioPort.PortNo.ToString();
                }

                int rowIndex = portView.Rows.Add(ioPort.Name, 0, portNoString);
                portView.Rows[rowIndex].Tag = ioPort;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            var portViews = new DataGridView[] { inportTable, outportTable };

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < portViews[i].Rows.Count; j++)
                {
                    if (portViews[i].Rows[j].Cells[0].Value == null)
                    {
                        continue;
                    }

                    string name = portViews[i].Rows[j].Cells[0].Value.ToString();
                    if (string.IsNullOrEmpty(name))
                    {
                        continue;
                    }
                    int portNo = IoPort.UNUSED_PORT_NO;
                    if (portViews[i].Rows[j].Cells[2].Value.ToString() != "None")
                    {
                        portNo = Convert.ToInt32(portViews[i].Rows[j].Cells[2].Value);
                    }

                    int deviceNo = Convert.ToInt32(portViews[i].Rows[j].Cells[1].Value);

                    var ioPort = (IoPort)portViews[i].Rows[j].Tag;
                    ioPort.Set(portNo, deviceNo);
                }
            }

            portMap.Save();
        }
    }
}
