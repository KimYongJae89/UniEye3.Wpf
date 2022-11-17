using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Devices.Comm;
using DynMvp.UI;
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
    public partial class TensionChekcerParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private TensionSerialProbe selectedProbe;
        private bool onValueUpdate = false;

        public TensionChekcerParamControl()
        {
            InitializeComponent();

            //change language
            labelPort.Text = StringManager.GetString(labelPort.Text);
            labelRange.Text = StringManager.GetString(labelRange.Text);
            inverseResult.Text = StringManager.GetString(inverseResult.Text);
            modelVerification.Text = StringManager.GetString(modelVerification.Text);

            SerialPortManager.Instance().FillComboUsedPort(portSelector);
        }

        public void ClearSelectedProbe()
        {
            selectedProbe = null;
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "SerialParamControl - SetSelectedProbe");

            selectedProbe = (TensionSerialProbe)probe;
            if (selectedProbe != null)
            {
                UpdateData();
            }
            else
            {
                EnableControls(false);
            }
        }

        public void SelectProbe(ProbeList probeList)
        {

        }

        private void EnableControls(bool enable)
        {
            portSelector.Enabled = enable;
            inverseResult.Enabled = enable;
            modelVerification.Enabled = enable;
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            LogHelper.Debug(LoggerType.Operation, "SerialParamControl - UpdateData");

            onValueUpdate = true;

            portSelector.Text = selectedProbe.PortName;
            numUpperValue.Value = Convert.ToDecimal(selectedProbe.UpperValue);
            numLowerValue.Value = Convert.ToDecimal(selectedProbe.LowerValue);
            numReading.Value = Convert.ToInt32(selectedProbe.NumSerialReading);
            inverseResult.Checked = selectedProbe.InverseResult;
            modelVerification.Checked = selectedProbe.ModelVerification;
            comboBoxTensionUnit.SelectedIndex = (int)selectedProbe.UnitType;

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
            LogHelper.Debug(LoggerType.Operation, "SerialParamControl - portSelector_SelectedIndexChanged");

            selectedProbe.InspectionSerialPort = SerialPortManager.Instance().GetSerialPort(portSelector.Text);
            if (selectedProbe.InspectionSerialPort != null)
            {
                selectedProbe.PortName = selectedProbe.InspectionSerialPort.Name;
                ParamControl_ValueChanged(ValueChangedType.None);
            }
        }


        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            //string valueName = "";
            //if (sender == lowerValue)
            //    valueName = StringManager.GetString("Normal Range Lower");
            //else if (sender == upperValue)
            //    valueName = StringManager.GetString("Normal Range Upper");

            //UpDownControl.ShowControl(valueName, (Control)sender);
        }

        private void inverseResult_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "SerialParamControl - inverseResult_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("SerialParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.InverseResult = inverseResult.Checked;
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void modelVerification_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "SerialParamControl - modelVerification_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("SerialParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.ModelVerification = modelVerification.Checked;
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void stepBlocker_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "VisionParamControl - stepBlocker_CheckedChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            SerialParamControl_VisibleChanged(ValueChangedType.None);
        }

        private void SerialParamControl_VisibleChanged(ValueChangedType valueChangedType, bool modified = true)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "SerialParamControl - SerialParamControl_PositionUpdated");

                ValueChanged?.Invoke(valueChangedType, null, null, modified);
            }
        }

        private void TensionChekcerParamControl_Load(object sender, EventArgs e)
        {

        }

        private void numLowerValue_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "SerialParamControl - numLowerValue_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("SerialParamControl - selectedProbe instance is null.");
                return;
            }

            selectedProbe.LowerValue = (float)Convert.ToDouble(numLowerValue.Value);
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void numMaxValue_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "SerialParamControl - numMaxValue_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("SerialParamControl - selectedProbe instance is null.");
                return;
            }
            selectedProbe.UpperValue = (float)Convert.ToDouble(numUpperValue.Value);
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        private void numReading_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "SerialParamControl - numReading_ValueChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("SerialParamControl - selectedProbe instance is null.");
                return;
            }
            selectedProbe.NumSerialReading = (int)numReading.Value;
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        private void buttonTenstionPath_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "CSV files(*.csv)";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //if ((myStream = openFileDialog1.OpenFile()) != null)
                    //{
                    //    using (myStream)
                    //    {
                    //        // Insert code to read the stream here.
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void comboBoxTensionUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "SerialParamControl - comboBoxTensionUnit_SelectedIndexChanged");

            if (selectedProbe == null)
            {
                LogHelper.Error("SerialParamControl - selectedProbe instance is null.");
                return;
            }
            selectedProbe.UnitType = (TensionUnitType)comboBoxTensionUnit.SelectedIndex;
            ParamControl_ValueChanged(ValueChangedType.None);
        }

        public string GetTypeName()
        {
            return "TensionChecker";
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {

        }
    }
}
