using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Devices.Daq;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniEye.Base.UI.ParamControl
{
    public partial class DaqParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }
        public StepModel Model { get; set; }

        private List<DaqProbe> probeList = new List<DaqProbe>();
        private bool onValueUpdate = false;

        public DaqParamControl()
        {
            LogHelper.Debug(LoggerType.Operation, "Begin DaqParamControl-Ctor");

            InitializeComponent();

            //change language
            labelDaqChannel.Text = StringManager.GetString(labelDaqChannel.Text);
            labelRange.Text = StringManager.GetString(labelRange.Text);
            labelNumSample.Text = StringManager.GetString(labelNumSample.Text);
            inverseResult.Text = StringManager.GetString(inverseResult.Text);
            modelVerification.Text = StringManager.GetString(modelVerification.Text);
            useLocalScaleFactor.Text = StringManager.GetString(useLocalScaleFactor.Text);
            labelFilterType.Text = StringManager.GetString(labelFilterType.Text);

            DaqChannelManager.Instance().FillComboDaqChannel(daqSelector);

            LogHelper.Debug(LoggerType.Operation, "End DaqParamControl-Ctor");
        }

        public void ClearSelectedProbe()
        {
            probeList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - SetSelectedProbe");

            probeList.Clear();

            if (probe != null)
            {
                probeList.Add((DaqProbe)probe);
                UpdateData();
            }
            else
            {
                EnableControls(false);
            }
        }

        public void SelectProbe(ProbeList probeList)
        {
            probeList.Clear();

            foreach (Probe probe in probeList)
            {
                this.probeList.Add((DaqProbe)probe);
            }

            if (probeList.Count > 0)
            {
                UpdateData();
            }
            else
            {
                EnableControls(false);
            }
        }

        private void EnableControls(bool enable)
        {
            daqSelector.Enabled = enable;
            numSample.Enabled = enable;
            upperValue.Enabled = enable;
            lowerValue.Enabled = enable;
            inverseResult.Enabled = enable;
            modelVerification.Enabled = enable;
            useLocalScaleFactor.Enabled = enable;
            localScaleFactor.Enabled = enable && useLocalScaleFactor.Enabled;
            valueOffset.Enabled = enable && useLocalScaleFactor.Enabled;
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            if (probeList.Count == 0)
            {
                return;
            }

            DaqProbe daqProbe = probeList[0];
            if (daqProbe.DaqChannel == null)
            {
                daqSelector.SelectedIndex = 0;
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - UpdateData");

            onValueUpdate = true;

            daqSelector.Text = daqProbe.DaqChannel.Name;
            measureType.Text = daqProbe.MeasureType.ToString();
            // filterType.Text = selectedProbe.FilterType.ToString();
            numSample.Text = daqProbe.NumSample.ToString();
            upperValue.Text = daqProbe.UpperValue.ToString();
            lowerValue.Text = daqProbe.LowerValue.ToString();
            inverseResult.Checked = daqProbe.InverseResult;
            modelVerification.Checked = daqProbe.ModelVerification;
            useLocalScaleFactor.Checked = daqProbe.UseLocalScaleFactor;
            localScaleFactor.Text = daqProbe.LocalScaleFactor.ToString();
            localScaleFactor.Enabled = useLocalScaleFactor.Checked;
            valueOffset.Text = daqProbe.ValueOffset.ToString();
            valueOffset.Enabled = useLocalScaleFactor.Checked;

            var targetList = new List<Target>();
            Model.GetTargets(targetList);

            target1.Items.Clear();
            target2.Items.Clear();
            foreach (Target target in targetList)
            {
                string targetName = target.Name;
                if (targetName != null || targetName != "")
                {
                    target1.Items.Add(targetName);
                    target2.Items.Add(targetName);
                }
            }

            target1.Text = daqProbe.Target1Name;
            target2.Text = daqProbe.Target2Name;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "VisionParamControl - VisionParamControl_PositionUpdated");

                ValueChanged?.Invoke(valueChangedType, null, null, true);
            }
        }

        private void portSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (DaqProbe daqProbe in probeList)
            {
                daqProbe.DaqChannel = DaqChannelManager.Instance().GetDaqChannel(daqSelector.Text);
            }
        }

        private void lowerValue_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - lowerValue_TextChanged");

            try
            {
                foreach (DaqProbe daqProbe in probeList)
                {
                    daqProbe.LowerValue = (float)Convert.ToDouble(lowerValue.Text);
                    ParamControl_ValueChanged(ValueChangedType.None);
                }
            }
            catch
            {
            }
        }

        private void upperValue_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - upperValue_TextChanged");

            try
            {
                foreach (DaqProbe daqProbe in probeList)
                {
                    daqProbe.UpperValue = (float)Convert.ToDouble(upperValue.Text);
                    ParamControl_ValueChanged(ValueChangedType.None);
                }
            }
            catch
            {
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == numSample)
            {
                valueName = StringManager.GetString(labelNumSample.Text);
            }
            else if (sender == lowerValue)
            {
                valueName = StringManager.GetString("Normal Range Lower");
            }
            else if (sender == upperValue)
            {
                valueName = StringManager.GetString("Normal Range Upper");
            }
            else if (sender == localScaleFactor)
            {
                valueName = StringManager.GetString("Local Scale Factor");
            }
            else if (sender == valueOffset)
            {
                valueName = StringManager.GetString("Value Offset");
            }

            UpDownControl.ShowControl(valueName, (Control)sender);
        }

        private void inverseResult_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - inverseResult_CheckedChanged");

            foreach (DaqProbe daqProbe in probeList)
            {
                daqProbe.InverseResult = inverseResult.Checked;
                ParamControl_ValueChanged(ValueChangedType.None);
            }
        }

        private void modelVerification_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - modelVerification_CheckedChanged");

            foreach (DaqProbe daqProbe in probeList)
            {
                daqProbe.ModelVerification = modelVerification.Checked;
                ParamControl_ValueChanged(ValueChangedType.None);
            }
        }

        private void numSample_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - numSample_ValueChanged");

            foreach (DaqProbe daqProbe in probeList)
            {
                daqProbe.NumSample = Convert.ToInt32(numSample.Text);
                ParamControl_ValueChanged(ValueChangedType.None);
            }
        }

        private void useScaleValue_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - useScaleValue_CheckedChanged");

            foreach (DaqProbe daqProbe in probeList)
            {
                localScaleFactor.Enabled = useLocalScaleFactor.Checked;
                valueOffset.Enabled = useLocalScaleFactor.Checked;

                daqProbe.UseLocalScaleFactor = useLocalScaleFactor.Checked;
                ParamControl_ValueChanged(ValueChangedType.None);
            }
        }

        private void localScaleFactor_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - scaleValue_TextChanged");

            try
            {
                foreach (DaqProbe daqProbe in probeList)
                {
                    daqProbe.LocalScaleFactor = Convert.ToSingle(localScaleFactor.Text);
                    ParamControl_ValueChanged(ValueChangedType.None);
                }
            }
            catch (InvalidCastException)
            {

            }
            catch (FormatException)
            {

            }
        }

        private void labelValueOffset_Click(object sender, EventArgs e)
        {

        }

        private void valueOffset_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - scaleValue_TextChanged");

            try
            {
                foreach (DaqProbe daqProbe in probeList)
                {
                    daqProbe.ValueOffset = Convert.ToSingle(valueOffset.Text);
                    ParamControl_ValueChanged(ValueChangedType.None);
                }
            }
            catch (InvalidCastException)
            {

            }
            catch (FormatException)
            {

            }
        }

        private void measureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (measureType.SelectedIndex == 0)
            {
                panelProbeSelector.Hide();
                panelMeasureParam.Show();
            }
            else
            {
                panelMeasureParam.Hide();
                panelProbeSelector.Location = panelMeasureParam.Location;
                panelProbeSelector.Show();
            }

            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - measureType_SelectedIndexChanged");

            foreach (DaqProbe daqProbe in probeList)
            {
                daqProbe.MeasureType = (DaqMeasureType)Enum.Parse(typeof(DaqMeasureType), measureType.Text);
                ParamControl_ValueChanged(ValueChangedType.None);
            }
        }

        private void target1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - target1_SelectedIndexChanged");

            foreach (DaqProbe daqProbe in probeList)
            {
                daqProbe.Target1Name = target1.Text;
                ParamControl_ValueChanged(ValueChangedType.None);
            }
        }

        private void target2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - target2_SelectedIndexChanged");

            foreach (DaqProbe daqProbe in probeList)
            {
                daqProbe.Target2Name = target2.Text;
                ParamControl_ValueChanged(ValueChangedType.None);
            }
        }

        private void filterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "DaqParamControl - filterType_SelectedIndexChanged");

            // selectedProbe.FilterType = (DaqFilterType)Enum.Parse(typeof(DaqFilterType), filterType.Text);
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return "DAQ Probe";
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {

        }
    }
}
