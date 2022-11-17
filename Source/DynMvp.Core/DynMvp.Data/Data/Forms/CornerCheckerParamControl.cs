using DynMvp.Base;
using DynMvp.Data;
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
    public partial class CornerCheckerParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private List<CornerChecker> cornerCheckerList = new List<CornerChecker>();
        private bool onValueUpdate = false;

        public CornerCheckerParamControl()
        {
            InitializeComponent();

            numEdgeDetectorLabel.Text = StringManager.GetString(numEdgeDetectorLabel.Text);
            detectorPropertyBox.Text = StringManager.GetString(detectorPropertyBox.Text);
            searchLengthLabel.Text = StringManager.GetString(searchLengthLabel.Text);
            projectionHeightLabel.Text = StringManager.GetString(projectionHeightLabel.Text);
            searchAngleLabel.Text = StringManager.GetString(searchAngleLabel.Text);
            edgeTypeLabel.Text = StringManager.GetString(edgeTypeLabel.Text);
            labelEdge1.Text = StringManager.GetString(labelEdge1.Text);
            labelEdge2.Text = StringManager.GetString(labelEdge2.Text);

        }

        public void ClearSelectedProbe()
        {
            cornerCheckerList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "CornerCheckerParamControl - SetSelectedProbe");

            cornerCheckerList.Clear();

            var visionProbe = (VisionProbe)probe;
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == CornerChecker.TypeName)
            {
                cornerCheckerList.Add((CornerChecker)visionProbe.InspAlgorithm);
                UpdateData();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void SelectProbe(ProbeList selectedProbeList)
        {
            cornerCheckerList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                if (visionProbe.InspAlgorithm.GetAlgorithmType() == CornerChecker.TypeName)
                {
                    cornerCheckerList.Add((CornerChecker)visionProbe.InspAlgorithm);
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
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            if (cornerCheckerList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CornerCheckerParamControl - UpdateData");

            onValueUpdate = true;

            var param = (CornerCheckerParam)cornerCheckerList[0].Param;

            numEdgeDetector.Text = param.LineDetectorParam.NumEdgeDetector.ToString();
            searchLength.Text = param.LineDetectorParam.SearchLength.ToString();
            projectionHeight.Text = param.LineDetectorParam.ProjectionHeight.ToString();
            searchAngle.Text = param.LineDetectorParam.SearchAngle.ToString();
            edge1TypeCmb.Text = param.Edge1Type.ToString();
            edge2TypeCmb.Text = param.Edge2Type.ToString();

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "CornerCheckerParamControl - ParamControl_ValueChanged");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        private void edge1TypeCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CornerCheckerParamControl - edge1TypeCmb_SelectedIndexChanged");

            if (cornerCheckerList.Count == 0)
            {
                return;
            }

            foreach (CornerChecker cornerChecker in cornerCheckerList)
            {
                AlgorithmParam newParam = cornerChecker.Param.Clone();
                ((CornerCheckerParam)newParam).Edge1Type = (EdgeType)Enum.Parse(typeof(EdgeType), edge1TypeCmb.Text);

                ParamControl_ValueChanged(ValueChangedType.None, cornerChecker, newParam);
            }
        }

        private void numEdgeDetector_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CornerCheckerParamControl - edge1TypeCmb_SelectedIndexChanged");

            if (cornerCheckerList.Count == 0)
            {
                return;
            }

            foreach (CornerChecker cornerChecker in cornerCheckerList)
            {
                AlgorithmParam newParam = cornerChecker.Param.Clone();
                ((CornerCheckerParam)newParam).LineDetectorParam.NumEdgeDetector = (int)numEdgeDetector.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, cornerChecker, newParam);
            }
        }

        private void searchLength_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CornerCheckerParamControl - searchLength_ValueChanged");

            if (cornerCheckerList.Count == 0)
            {
                return;
            }

            foreach (CornerChecker cornerChecker in cornerCheckerList)
            {
                AlgorithmParam newParam = cornerChecker.Param.Clone();
                ((CornerCheckerParam)newParam).LineDetectorParam.SearchLength = (int)searchLength.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, cornerChecker, newParam);
            }
        }

        private void projectionHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CornerCheckerParamControl - projectionHeight_ValueChanged");

            if (cornerCheckerList.Count == 0)
            {
                return;
            }

            foreach (CornerChecker cornerChecker in cornerCheckerList)
            {
                AlgorithmParam newParam = cornerChecker.Param.Clone();
                ((CornerCheckerParam)newParam).LineDetectorParam.ProjectionHeight = (int)projectionHeight.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, cornerChecker, newParam);
            }
        }

        private void searchAngle_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CornerCheckerParamControl - searchAngle_ValueChanged");

            if (cornerCheckerList.Count == 0)
            {
                return;
            }

            foreach (CornerChecker cornerChecker in cornerCheckerList)
            {
                AlgorithmParam newParam = cornerChecker.Param.Clone();
                ((CornerCheckerParam)newParam).LineDetectorParam.SearchAngle = (int)searchAngle.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, cornerChecker, newParam);
            }
        }

        private void edge2TypeCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CornerCheckerParamControl - edge1TypeCmb_SelectedIndexChanged");

            if (cornerCheckerList.Count == 0)
            {
                return;
            }

            foreach (CornerChecker cornerChecker in cornerCheckerList)
            {
                AlgorithmParam newParam = cornerChecker.Param.Clone();
                ((CornerCheckerParam)newParam).Edge2Type = (EdgeType)Enum.Parse(typeof(EdgeType), edge2TypeCmb.Text);

                ParamControl_ValueChanged(ValueChangedType.None, cornerChecker, newParam);
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return CornerChecker.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }
    }
}
