using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.Forms
{
    public partial class WidthCheckerParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private List<WidthChecker> widthCheckerList = new List<WidthChecker>();
        private bool onValueUpdate = false;

        public WidthCheckerParamControl()
        {
            InitializeComponent();

            numEdgeDetectorLabel.Text = StringManager.GetString(numEdgeDetectorLabel.Text);
            detectorPropertyBox.Text = StringManager.GetString(detectorPropertyBox.Text);
            searchLengthLabel.Text = StringManager.GetString(searchLengthLabel.Text);
            projectionHeightLabel.Text = StringManager.GetString(projectionHeightLabel.Text);
            searchAngleLabel.Text = StringManager.GetString(searchAngleLabel.Text);
            edgeTypeLabel.Text = StringManager.GetString(edgeTypeLabel.Text);
            edgeTypeLabel.Text = StringManager.GetString(edgeTypeLabel.Text);
            scaleValueLabel.Text = StringManager.GetString(scaleValueLabel.Text);
        }

        public string GetTypeName()
        {
            return WidthChecker.TypeName;
        }

        public void ClearSelectedProbe()
        {
            widthCheckerList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - SetSelectedProbe");

            var visionProbe = (VisionProbe)probe;
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == WidthChecker.TypeName)
            {
                widthCheckerList.Add((WidthChecker)visionProbe.InspAlgorithm);
                UpdateData();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void SelectProbe(ProbeList selectedProbeList)
        {
            widthCheckerList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                if (visionProbe.InspAlgorithm.GetAlgorithmType() == WidthChecker.TypeName)
                {
                    widthCheckerList.Add((WidthChecker)visionProbe.InspAlgorithm);
                }
            }

            UpdateData();
        }

        private void EnableControls(bool enable)
        {
            numEdgeDetector.Enabled = enable;
            searchLength.Enabled = enable;
            projectionHeight.Enabled = enable;
            searchAngle.Enabled = enable;
            edge1TypeCmb.Enabled = enable;
            edge2TypeCmb.Enabled = enable;
            minWidthRatio.Enabled = enable;
            maxWidthRatio.Enabled = enable;
            scaleValue.Enabled = enable;
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            if (widthCheckerList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - UpdateData");

            onValueUpdate = true;

            var widthCheckerParam = (WidthCheckerParam)widthCheckerList[0].Param;

            numEdgeDetector.Text = widthCheckerParam.LineDetectorParam.NumEdgeDetector.ToString();
            searchLength.Text = widthCheckerParam.LineDetectorParam.SearchLength.ToString();
            projectionHeight.Text = widthCheckerParam.LineDetectorParam.ProjectionHeight.ToString();
            searchAngle.Text = widthCheckerParam.LineDetectorParam.SearchAngle.ToString();
            edge1TypeCmb.Text = widthCheckerParam.Edge1Type.ToString();
            edge2TypeCmb.Text = widthCheckerParam.Edge2Type.ToString();
            minWidthRatio.Text = widthCheckerParam.MinWidthRatio.ToString();
            maxWidthRatio.Text = widthCheckerParam.MaxWidthRatio.ToString();
            scaleValue.Text = widthCheckerParam.ScaleValue.ToString();

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - ParamControl_ValueChanged");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        private void edge1TypeCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - edge1TypeCmb_SelectedIndexChanged");

            if (widthCheckerList.Count == 0)
            {
                return;
            }

            foreach (WidthChecker widthChecker in widthCheckerList)
            {
                AlgorithmParam newParam = widthChecker.Param.Clone();
                ((WidthCheckerParam)newParam).Edge1Type = (EdgeType)Enum.Parse(typeof(EdgeType), edge1TypeCmb.Text);

                ParamControl_ValueChanged(ValueChangedType.None, widthChecker, newParam);
            }
        }

        private void numEdgeDetector_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - edge1TypeCmb_SelectedIndexChanged");

            if (widthCheckerList.Count == 0)
            {
                return;
            }

            foreach (WidthChecker widthChecker in widthCheckerList)
            {
                AlgorithmParam newParam = widthChecker.Param.Clone();
                ((WidthCheckerParam)newParam).LineDetectorParam.NumEdgeDetector = (int)numEdgeDetector.Value;

                ParamControl_ValueChanged(ValueChangedType.None, widthChecker, newParam);
            }
        }

        private void searchLength_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - searchLength_ValueChanged");

            if (widthCheckerList.Count == 0)
            {
                return;
            }

            foreach (WidthChecker widthChecker in widthCheckerList)
            {
                AlgorithmParam newParam = widthChecker.Param.Clone();
                ((WidthCheckerParam)newParam).LineDetectorParam.SearchLength = (int)searchLength.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, widthChecker, newParam);
            }
        }

        private void projectionHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - projectionHeight_ValueChanged");

            if (widthCheckerList.Count == 0)
            {
                return;
            }

            foreach (WidthChecker widthChecker in widthCheckerList)
            {
                AlgorithmParam newParam = widthChecker.Param.Clone();
                ((WidthCheckerParam)newParam).LineDetectorParam.ProjectionHeight = (int)projectionHeight.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, widthChecker, newParam);
            }
        }

        private void searchAngle_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - searchAngle_ValueChanged");

            if (widthCheckerList.Count == 0)
            {
                return;
            }

            foreach (WidthChecker widthChecker in widthCheckerList)
            {
                AlgorithmParam newParam = widthChecker.Param.Clone();
                ((WidthCheckerParam)newParam).LineDetectorParam.SearchAngle = (int)searchAngle.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, widthChecker, newParam);
            }
        }

        private void edge2TypeCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - edge1TypeCmb_SelectedIndexChanged");

            if (widthCheckerList.Count == 0)
            {
                return;
            }

            foreach (WidthChecker widthChecker in widthCheckerList)
            {
                AlgorithmParam newParam = widthChecker.Param.Clone();
                ((WidthCheckerParam)newParam).Edge2Type = (EdgeType)Enum.Parse(typeof(EdgeType), edge2TypeCmb.Text);

                ParamControl_ValueChanged(ValueChangedType.None, widthChecker, newParam);
            }
        }

        private void minWidthRatio_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - minWidthRatio_ValueChanged");

            if (widthCheckerList.Count == 0)
            {
                return;
            }

            foreach (WidthChecker widthChecker in widthCheckerList)
            {
                AlgorithmParam newParam = widthChecker.Param.Clone();
                ((WidthCheckerParam)newParam).MinWidthRatio = Convert.ToInt32(minWidthRatio.Text);

                ParamControl_ValueChanged(ValueChangedType.Position, widthChecker, newParam);
            }
        }

        private void maxWidthRatio_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - maxWidthRatio_ValueChanged");

            if (widthCheckerList.Count == 0)
            {
                return;
            }

            foreach (WidthChecker widthChecker in widthCheckerList)
            {
                AlgorithmParam newParam = widthChecker.Param.Clone();
                ((WidthCheckerParam)newParam).MaxWidthRatio = Convert.ToInt32(maxWidthRatio.Text);

                ParamControl_ValueChanged(ValueChangedType.Position, widthChecker, newParam);
            }
        }

        private void scaleValue_TextChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - scaleValue_TextChanged");

            if (widthCheckerList.Count == 0)
            {
                return;
            }

            foreach (WidthChecker widthChecker in widthCheckerList)
            {
                AlgorithmParam newParam = widthChecker.Param.Clone();
                ((WidthCheckerParam)newParam).ScaleValue = Convert.ToInt32(scaleValue.Text);

                ParamControl_ValueChanged(ValueChangedType.None, widthChecker, newParam);
            }
        }

        private void maxCenterGap_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "WidthCheckerParamControl - maxCenterGap_ValueChanged");

            if (widthCheckerList.Count == 0)
            {
                return;
            }

            foreach (WidthChecker widthChecker in widthCheckerList)
            {
                AlgorithmParam newParam = widthChecker.Param.Clone();
                ((WidthCheckerParam)newParam).MaxCenterGap = Convert.ToInt32(scaleValue.Text);

                ParamControl_ValueChanged(ValueChangedType.None, widthChecker, newParam);
            }
        }

        void IAlgorithmParamControl.UpdateProbeImage()
        {
            throw new NotImplementedException();
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }
    }
}
