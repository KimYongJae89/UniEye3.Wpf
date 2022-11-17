using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.Dio.UI
{
    public partial class SlaveDigitalIoInfoForm : Form
    {
        public SlaveDigitalIoInfo SlaveDigitalIoInfo { get; set; }
        public MotionInfoList MotionInfoList { get; set; }

        public SlaveDigitalIoInfoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(labelName.Text);
            labelBoardIndex.Text = StringManager.GetString(labelBoardIndex.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

        }

        private void SlaveMotionInfoForm_Load(object sender, EventArgs e)
        {
            foreach (MotionInfo motionInfo in MotionInfoList)
            {
                if (DigitalIoFactory.IsMasterDevice(motionInfo.Type))
                {
                    masterDeviceList.Items.Add(motionInfo.Name);
                }
            }

            txtName.Text = SlaveDigitalIoInfo.Name;
            masterDeviceList.Text = SlaveDigitalIoInfo.MasterDeviceName;

            numInPortGroup.Value = SlaveDigitalIoInfo.NumInPortGroup;
            inPortStartGroupIndex.Value = SlaveDigitalIoInfo.InPortStartGroupIndex;
            numInPort.Value = SlaveDigitalIoInfo.NumInPort;
            numOutPortGroup.Value = SlaveDigitalIoInfo.NumOutPortGroup;
            outPortStartGroupIndex.Value = SlaveDigitalIoInfo.OutPortStartGroupIndex;
            numOutPort.Value = SlaveDigitalIoInfo.NumOutPort;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SlaveDigitalIoInfo.Name = txtName.Text;
            SlaveDigitalIoInfo.MasterDeviceName = masterDeviceList.Text;
            SlaveDigitalIoInfo.NumInPortGroup = (int)numInPortGroup.Value;
            SlaveDigitalIoInfo.InPortStartGroupIndex = (int)inPortStartGroupIndex.Value;
            SlaveDigitalIoInfo.NumInPort = (int)numInPort.Value;
            SlaveDigitalIoInfo.NumOutPortGroup = (int)numOutPortGroup.Value;
            SlaveDigitalIoInfo.OutPortStartGroupIndex = (int)outPortStartGroupIndex.Value;
            SlaveDigitalIoInfo.NumOutPort = (int)numOutPort.Value;
        }
    }
}
