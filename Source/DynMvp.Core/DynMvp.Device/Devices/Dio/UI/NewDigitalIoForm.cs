using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
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
    public partial class NewDigitalIoForm : Form
    {
        private DigitalIoType digitalIoType;
        public DigitalIoType DigitalIoType
        {
            set => digitalIoType = value;
        }

        private string digitalIoName;
        public string DigitalIoName
        {
            set => digitalIoName = value;
        }
        public DigitalIoInfo DigitalIoInfo { get; set; }

        public NewDigitalIoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(labelName.Text);
            labelDigitalIoType.Text = StringManager.GetString(labelDigitalIoType.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

            cmbDigitalIoType.DataSource = Enum.GetNames(typeof(DigitalIoType));
        }

        private void GrabberInfoForm_Load(object sender, EventArgs e)
        {
            cmbDigitalIoType.SelectedIndex = (int)digitalIoType;
            txtName.Text = digitalIoName;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DigitalIoInfo = DigitalIoInfoFactory.Create((DigitalIoType)cmbDigitalIoType.SelectedIndex);
            DigitalIoInfo.Name = txtName.Text;
            DigitalIoInfo.NumInPortGroup = (int)numInPortGroup.Value;
            DigitalIoInfo.InPortStartGroupIndex = (int)inPortStartGroupIndex.Value;
            DigitalIoInfo.NumInPort = (int)numInPort.Value;
            DigitalIoInfo.NumOutPortGroup = (int)numOutPortGroup.Value;
            DigitalIoInfo.OutPortStartGroupIndex = (int)outPortStartGroupIndex.Value;
            DigitalIoInfo.NumOutPort = (int)numOutPort.Value;
        }

        private void cmbDigitalIoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txtName.Text))
            {
                digitalIoName = ((DigitalIoType)cmbDigitalIoType.SelectedIndex).ToString();
                txtName.Text = digitalIoName;
            }
        }
    }
}
