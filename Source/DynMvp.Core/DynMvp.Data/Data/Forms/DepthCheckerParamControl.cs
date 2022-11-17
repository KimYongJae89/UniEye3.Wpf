using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.Forms
{
    public partial class DepthCheckerParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private ProbeList probeList = new ProbeList();
        private List<DepthChecker> depthCheckerList = new List<DepthChecker>();
        private Calibration calibration = null;
        private bool onValueUpdate = false;

        public DepthCheckerParamControl()
        {
            InitializeComponent();

            labelMeasureType.Text = StringManager.GetString(labelMeasureType.Text);
            labelValueRange.Text = StringManager.GetString(labelValueRange.Text);
            labelMinValue.Text = StringManager.GetString(labelMinValue.Text);
            labelMaxValue.Text = StringManager.GetString(labelMaxValue.Text);
            buttonGetDepthValue.Text = StringManager.GetString(buttonGetDepthValue.Text);
            //change language
        }

        public void SetCameraCalibration(Calibration calibration)
        {
            this.calibration = calibration;
        }

        public void ClearSelectedProbe()
        {
            probeList.Clear();
            depthCheckerList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - SetSelectedProbe");

            probeList.Clear();
            depthCheckerList.Clear();

            var visionProbe = (VisionProbe)probe;
            probeList.AddProbe(visionProbe);
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == DepthChecker.TypeName)
            {
                depthCheckerList.Add((DepthChecker)visionProbe.InspAlgorithm);
                UpdateData();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void SelectProbe(ProbeList selectedProbeList)
        {
            probeList.Clear();
            depthCheckerList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                if (visionProbe.InspAlgorithm.GetAlgorithmType() == DepthChecker.TypeName)
                {
                    depthCheckerList.Add((DepthChecker)visionProbe.InspAlgorithm);
                }
            }

            UpdateData();
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            if (depthCheckerList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - UpdateData");

            onValueUpdate = true;

            DepthChecker depthChecker = depthCheckerList[0];

            lowerValue.Text = ((DepthCheckerParam)depthChecker.Param).LowerValue.ToString();
            upperValue.Text = ((DepthCheckerParam)depthChecker.Param).UpperValue.ToString();
            comboBoxMeasureType.SelectedIndex = (int)((DepthCheckerParam)depthChecker.Param).Type;
            UpdateValueUnit();

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - VisionParamControl_PositionUpdated");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        private void lowerValue_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - lowerValue_ValueChanged");

            foreach (DepthChecker depthChecker in depthCheckerList)
            {
                AlgorithmParam newParam = depthChecker.Param.Clone();
                ((DepthCheckerParam)newParam).LowerValue = (float)lowerValue.Value;

                ParamControl_ValueChanged(ValueChangedType.None, depthChecker, newParam);
            }
        }

        private void upperValue_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - upperValue_ValueChanged");

            foreach (DepthChecker depthChecker in depthCheckerList)
            {
                AlgorithmParam newParam = depthChecker.Param.Clone();
                ((DepthCheckerParam)newParam).UpperValue = (float)upperValue.Value;

                ParamControl_ValueChanged(ValueChangedType.None, depthChecker, newParam);
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == lowerValue)
            {
                valueName = "Depth Lower";
            }
            else if (sender == upperValue)
            {
                valueName = "Depth Upper";
            }

            UpDownControl.ShowControl(StringManager.GetString(valueName), (Control)sender);
        }

        private void comboBoxMeasureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "DepthCheckerParamControl - comboBoxValue_ValueChanged");

            foreach (DepthChecker depthChecker in depthCheckerList)
            {
                AlgorithmParam newParam = depthChecker.Param.Clone();
                ((DepthCheckerParam)newParam).Type = (DepthCheckType)comboBoxMeasureType.SelectedIndex;

                ParamControl_ValueChanged(ValueChangedType.None, depthChecker, newParam);
            }

            UpdateValueUnit();
        }

        private void UpdateValueUnit()
        {
            switch ((DepthCheckType)comboBoxMeasureType.SelectedIndex)
            {
                case DepthCheckType.Volume:
                    labelMaxUnit.Text = "㎟";
                    labelMinUnit.Text = "㎟";
                    break;
                default:
                    labelMaxUnit.Text = "㎜";
                    labelMinUnit.Text = "㎜";
                    break;
            }
        }

        private void buttonGetDepthValue_Click(object sender, EventArgs e)
        {
            if (probeList.Count != 1)
            {
                return;
            }

            ImageD targetGroupImage = TeachState.Instance().TargetGroupImage;
            if (targetGroupImage == null)
            {
                MessageBox.Show("Please, scan image first.");
                return;
            }

            Probe probe = probeList[0];

            ImageD clipImage = targetGroupImage.ClipImage(Rectangle.Truncate(probe.BaseRegion.ToRectangleF()));
            float upperPct = (100 + (float)marginPercent.Value) / 100;
            float lowerPct = (100 - (float)marginPercent.Value) / 100;
            float averageHeight = clipImage.GetAverage();
            float area = (probe.BaseRegion.Width * 0.3f) * (probe.BaseRegion.Width * 0.3f);
            switch ((DepthCheckType)comboBoxMeasureType.SelectedIndex)
            {
                case DepthCheckType.Volume:
                    upperValue.Value = (decimal)(averageHeight * area * upperPct);
                    lowerValue.Value = (decimal)(averageHeight * area * lowerPct);
                    break;
                case DepthCheckType.HeightMax:
                    {
                        float maxValue = clipImage.GetMax();
                        upperValue.Value = (decimal)(maxValue * upperPct);
                        lowerValue.Value = (decimal)(maxValue * lowerPct);
                    }
                    break;
                case DepthCheckType.HeightMin:
                    {
                        float minValue = clipImage.GetMin();
                        upperValue.Value = (decimal)(minValue * upperPct);
                        lowerValue.Value = (decimal)(minValue * lowerPct);
                    }
                    break;
                case DepthCheckType.HeightAverage:
                    {
                        upperValue.Value = (decimal)(averageHeight * upperPct);
                        lowerValue.Value = (decimal)(averageHeight * lowerPct);
                    }
                    break;
                default:
                    break;
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return DepthChecker.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }
    }
}
