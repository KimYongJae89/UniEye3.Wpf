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
    public partial class PciDigitalIoInfoForm : Form
    {
        public PciDigitalIoInfo PciDigitalIoInfo { get; set; }

        public PciDigitalIoInfoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(labelName.Text);
            labelBoardIndex.Text = StringManager.GetString(labelBoardIndex.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);
        }

        private void PciMotionInfoForm_Load(object sender, EventArgs e)
        {
            name.Text = PciDigitalIoInfo.Name;
            boardIndex.Value = PciDigitalIoInfo.Index;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            PciDigitalIoInfo.Name = name.Text;
            PciDigitalIoInfo.Index = (int)boardIndex.Value;
        }
    }
}
