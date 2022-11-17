using DynMvp.Base;
using DynMvp.Devices.Daq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniEye.Base.UI
{
    public partial class DaqPropertyForm : Form
    {
        public DaqChannelProperty DaqChannelProperty { get; set; }

        private double tempScaleFactor;
        private double tempOffset;

        public DaqPropertyForm()
        {
            InitializeComponent();

            labelDaqName.Text = StringManager.GetString(labelDaqName.Text);
            labelDeviceName.Text = StringManager.GetString(labelDeviceName.Text);
            labelLeftDaqChannel.Text = StringManager.GetString(labelLeftDaqChannel.Text);
            labelMinValue.Text = StringManager.GetString(labelMinValue.Text);
            labelMaxValue.Text = StringManager.GetString(labelMaxValue.Text);
            labelSamplingHz.Text = StringManager.GetString(labelSamplingHz.Text);
            label1.Text = StringManager.GetString(label1.Text);
            buttonCalculateScale.Text = StringManager.GetString(buttonCalculateScale.Text);
            labelScaleFactor.Text = StringManager.GetString(labelScaleFactor.Text);
            checkUseCutomScaleFactor.Text = StringManager.GetString(checkUseCutomScaleFactor.Text);
            buttonUseThisScaleFactor.Text = StringManager.GetString(buttonUseThisScaleFactor.Text);
            labelValueOffset.Text = StringManager.GetString(labelValueOffset.Text);
            buttonOk.Text = StringManager.GetString(buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);
        }

        private void DaqPropertyForm_Load(object sender, EventArgs e)
        {
            txtName.Text = DaqChannelProperty.Name;
            deviceName.Text = DaqChannelProperty.DeviceName;
            daqChannelName.Text = DaqChannelProperty.ChannelName;

            minValue.Value = (decimal)DaqChannelProperty.MinValue;
            maxValue.Value = (decimal)DaqChannelProperty.MaxValue;
            samplingHz.Value = (decimal)DaqChannelProperty.SamplingHz;
            scaleFactor.Text = DaqChannelProperty.ScaleFactor.ToString();
            valueOffset.Text = DaqChannelProperty.ValueOffset.ToString();
            resisterValue.Text = DaqChannelProperty.ResisterValue.ToString();
            checkUseCutomScaleFactor.Checked = DaqChannelProperty.UseCustomScale;

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DaqChannelProperty.Name = txtName.Text;
            DaqChannelProperty.DeviceName = deviceName.Text;
            DaqChannelProperty.ChannelName = daqChannelName.Text;

            DaqChannelProperty.MinValue = (double)minValue.Value;
            DaqChannelProperty.MaxValue = (double)maxValue.Value;
            DaqChannelProperty.SamplingHz = (double)samplingHz.Value;
            DaqChannelProperty.ScaleFactor = Convert.ToDouble(scaleFactor.Text);
            DaqChannelProperty.ValueOffset = Convert.ToDouble(valueOffset.Text);
            DaqChannelProperty.ResisterValue = Convert.ToInt32(resisterValue.Text);
            DaqChannelProperty.UseCustomScale = checkUseCutomScaleFactor.Checked;
        }

        private void buttonCalculateScale_Click(object sender, EventArgs e)
        {
            if (resisterValue.Text == "")
            {
                MessageBox.Show(StringManager.GetString("Resister Value is empty. Please, input the value."));
                return;
            }

            double value = Convert.ToDouble(resisterValue.Text);

            double minValue = 0.004F * value;
            double maxValue = 0.02F * value;

            scaleFactor.Text = (70 / (maxValue - minValue)).ToString("0.00");
        }

        private void scaleFactor_TextChanged(object sender, EventArgs e)
        {
            DaqChannelProperty.ScaleFactor = Convert.ToDouble(scaleFactor.Text);
        }

        private void buttonCalcScaleFactor_Click(object sender, EventArgs e)
        {
            CalcCustomScaleFactor();
        }

        private void CalcCustomScaleFactor()
        {
            if (string.IsNullOrEmpty(txtMinDistance.Text) || string.IsNullOrEmpty(txtMaxDistance.Text) || string.IsNullOrEmpty(txtMinVoltage.Text) || string.IsNullOrEmpty(txtMaxVoltage.Text))
            {
                return;
            }

            double distance = Convert.ToDouble(txtMinDistance.Text) - Convert.ToDouble(txtMaxDistance.Text);
            double voltage = Convert.ToDouble(txtMaxVoltage.Text) - Convert.ToDouble(txtMinVoltage.Text);

            double scaleFactor = voltage / distance; // use this
            tempScaleFactor = scaleFactor;

            double maxVoltage = Convert.ToDouble(txtMaxVoltage.Text) + Convert.ToDouble(txtMinVoltage.Text);
            double midVoltage = maxVoltage / 2;

            double midDistance = Convert.ToDouble(txtMaxDistance.Text) - Convert.ToDouble(txtMinDistance.Text);

            double offset = midVoltage + (scaleFactor * midDistance); // use this
            tempOffset = offset;
            buttonUseThisScaleFactor.Text = tempScaleFactor.ToString();
        }

        private void buttonUseThisScaleFactor_Click(object sender, EventArgs e)
        {
            scaleFactor.Text = tempScaleFactor.ToString();
            valueOffset.Text = tempOffset.ToString();
        }

        private void checkUseCutomScaleFactor_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
