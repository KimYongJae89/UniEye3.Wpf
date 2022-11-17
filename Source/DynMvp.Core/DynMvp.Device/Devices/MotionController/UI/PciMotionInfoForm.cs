using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.MotionController.UI
{
    public partial class PciMotionInfoForm : Form
    {
        public PciMotionInfo PciMotionInfo { get; set; }

        public PciMotionInfoForm()
        {
            InitializeComponent();


            labelName.Text = StringManager.GetString(labelName.Text);
            labelNumAxis.Text = StringManager.GetString(labelNumAxis.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            labelBoardIndex.Text = StringManager.GetString(labelBoardIndex.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

        }

        private void PciMotionInfoForm_Load(object sender, EventArgs e)
        {
            name.Text = PciMotionInfo.Name;
            numAxis.Value = PciMotionInfo.NumAxis;
            boardIndex.Value = PciMotionInfo.Index;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            PciMotionInfo.Name = name.Text;
            PciMotionInfo.NumAxis = (int)numAxis.Value;
            PciMotionInfo.Index = (int)boardIndex.Value;
        }
    }
}
