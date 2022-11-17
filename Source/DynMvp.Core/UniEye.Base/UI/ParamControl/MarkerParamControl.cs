using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UniEye.Base.UI.ParamControl
{
    public partial class MarkerParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }
        public StepModel Model { get; set; }

        private List<MarkerProbe> markerProbeList = new List<MarkerProbe>();
        private bool onValueUpdate = false;

        public MarkerParamControl()
        {
            LogHelper.Debug(LoggerType.Operation, "Begin MarkerParamControl-Ctor");

            InitializeComponent();

            labelMarkerType.Text = StringManager.GetString(labelMarkerType.Text);
            labelMergeSource.Text = StringManager.GetString(labelMergeSource.Text);
            label1.Text = StringManager.GetString(label1.Text);

            markerType.DataSource = Enum.GetNames(typeof(MarkerType));

            LogHelper.Debug(LoggerType.Operation, "End MarkerParamControl-Ctor");
        }

        public void UpdateProbeImage()
        {

        }

        public void ClearSelectedProbe()
        {
            markerProbeList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - SetSelectedProbe");

            markerProbeList.Clear();

            if (probe != null)
            {
                markerProbeList.Add((MarkerProbe)probe);
                UpdateData();
            }
        }

        public void SelectProbe(ProbeList probeList)
        {
            probeList.Clear();

            foreach (Probe probe in probeList)
            {
                markerProbeList.Add((MarkerProbe)probe);
            }

            if (probeList.Count > 0)
            {
                UpdateData();
            }
        }

        private void UpdateData()
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - UpdateData");

            if (markerProbeList.Count == 0)
            {
                return;
            }

            onValueUpdate = true;

            MarkerProbe markerProbe = markerProbeList[0];

            markerType.Text = markerProbe.MarkerType.ToString();
            mergeSource.Text = markerProbe.MergeSourceId;
            mergeOffsetX.Value = (decimal)markerProbe.MergeOffset.X;
            mergeOffsetY.Value = (decimal)markerProbe.MergeOffset.Y;
            mergeOffsetZ.Value = (decimal)markerProbe.MergeOffset.Z;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - ParamControl_ValueChanged");

                ValueChanged?.Invoke(valueChangedType, null, null, true);
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            //if (sender == numSample)
            //    valueName = StringManager.GetString(labelNumSample.Text);


            UpDownControl.ShowControl(valueName, (Control)sender);
        }

        private void mergeSource_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - mergeSource_TextChanged");

            try
            {
                foreach (MarkerProbe markerProbe in markerProbeList)
                {
                    markerProbe.MergeSourceId = mergeSource.Text;
                    ParamControl_ValueChanged(ValueChangedType.None);
                }
            }
            catch
            {
            }
        }

        private void mergeOffsetX_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - mergeOffsetX_ValueChanged");

            try
            {
                foreach (MarkerProbe markerProbe in markerProbeList)
                {
                    markerProbe.MergeOffset.X = Convert.ToSingle(mergeOffsetX.Value);
                    ParamControl_ValueChanged(ValueChangedType.None);
                }
            }
            catch
            {
            }
        }

        private void mergeOffsetY_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - mergeSource_TextChanged");

            try
            {
                foreach (MarkerProbe markerProbe in markerProbeList)
                {
                    markerProbe.MergeOffset.Y = Convert.ToSingle(mergeOffsetY.Value);
                    ParamControl_ValueChanged(ValueChangedType.None);
                }
            }
            catch
            {
            }
        }

        private void mergeOffsetZ_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - mergeSource_TextChanged");

            try
            {
                foreach (MarkerProbe markerProbe in markerProbeList)
                {
                    markerProbe.MergeOffset.Z = Convert.ToSingle(mergeOffsetZ.Value);
                    ParamControl_ValueChanged(ValueChangedType.None);
                }
            }
            catch
            {
            }
        }

        private void markerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "MarkerParamControl - markerType_SelectedIndexChanged");

            try
            {
                foreach (MarkerProbe markerProbe in markerProbeList)
                {
                    markerProbe.MarkerType = (MarkerType)Enum.Parse(typeof(MarkerType), markerType.Text);
                    ParamControl_ValueChanged(ValueChangedType.None);
                }
            }
            catch
            {
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return "Marker Probe";
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {

        }
    }
}
