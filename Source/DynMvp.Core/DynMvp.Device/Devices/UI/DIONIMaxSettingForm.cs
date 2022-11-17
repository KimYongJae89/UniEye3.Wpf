using DynMvp.Base;
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
    public partial class DIONIMaxSettingForm : Form
    {
        public string InputLineStr { get; set; }
        public string OutputLineStr { get; set; }

        public DIONIMaxSettingForm()
        {
            InitializeComponent();

            groupBox1.Text = StringManager.GetString(groupBox1.Text);
            labelInputLines.Text = StringManager.GetString(labelInputLines.Text);
            label1.Text = StringManager.GetString(label1.Text);
            btnOK.Text = StringManager.GetString(btnOK.Text);
            btnCancel.Text = StringManager.GetString(btnCancel.Text);

            inputLines.Text = InputLineStr;
            outputLines.Text = OutputLineStr;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            InputLineStr = inputLines.Text;
            OutputLineStr = outputLines.Text;
        }
    }
}
