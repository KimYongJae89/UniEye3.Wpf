using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Component.DepthSystem.DepthViewer
{
    public partial class SetupHeightForm : Form
    {

        public SetupHeightForm()
        {
            InitializeComponent();
            labelMinValue.Text = StringManager.GetString(labelMinValue.Text);
            labelMaxValue.Text = StringManager.GetString(labelMaxValue.Text);
            okButton.Text = StringManager.GetString(okButton.Text);
            cancelButton.Text = StringManager.GetString(cancelButton.Text);
        }

        public void SetHeightValue(float minValue, float maxValue)
        {
            this.minValue.Text = minValue.ToString();
            this.maxValue.Text = maxValue.ToString();
        }

        public void GetHeightValue(ref float minValue, ref float maxValue)
        {
            minValue = Convert.ToSingle(this.minValue.Text);
            maxValue = Convert.ToSingle(this.maxValue.Text);
        }

        private void okButton_Click(object sender, EventArgs e)
        {

        }

        private void SetupHeightForm_Load(object sender, EventArgs e)
        {

        }
    }
}
