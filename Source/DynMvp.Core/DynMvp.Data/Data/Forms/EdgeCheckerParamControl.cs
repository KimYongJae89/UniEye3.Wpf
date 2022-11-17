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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.Forms
{
    public partial class EdgeCheckerParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private ProbeList probeList = new ProbeList();
        private List<EdgeChecker> edgeCheckerList = new List<EdgeChecker>();
        private bool onValueUpdate = false;

        public EdgeCheckerParamControl()
        {
            InitializeComponent();

            labelDesiredOffset.Text = StringManager.GetString(labelDesiredOffset.Text);
            labelMaxOffset.Text = StringManager.GetString(labelMaxOffset.Text);
            labelEdgeDirection.Text = StringManager.GetString(labelEdgeDirection.Text);
            labelEdgeType.Text = StringManager.GetString(labelEdgeType.Text);
            labelThreshold.Text = StringManager.GetString(labelThreshold.Text);
            labelMorphology.Text = StringManager.GetString(labelMorphology.Text);
            labelGaussianFilterSize.Text = StringManager.GetString(labelGaussianFilterSize.Text);
            labelMedianFilterSize.Text = StringManager.GetString(labelMedianFilterSize.Text);
        }

        public string GetTypeName()
        {
            return EdgeChecker.TypeName;
        }

        public void ClearSelectedProbe()
        {
            probeList.Clear();
            edgeCheckerList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - AddSelectedProbe");

            var visionProbe = (VisionProbe)probe;
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == EdgeChecker.TypeName)
            {
                probeList.AddProbe(visionProbe);
                edgeCheckerList.Add((EdgeChecker)visionProbe.InspAlgorithm);
                UpdateData();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void SelectProbe(ProbeList selectedProbeList)
        {
            LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - SelectProbe");

            probeList.Clear();
            edgeCheckerList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                if (visionProbe.InspAlgorithm.GetAlgorithmType() == EdgeChecker.TypeName)
                {
                    probeList.AddProbe(visionProbe);
                    edgeCheckerList.Add((EdgeChecker)visionProbe.InspAlgorithm);
                }
            }

            UpdateData();
        }

        private void EnableControls(bool enable)
        {
            desiredOffset.Enabled = enable;
            threshold.Enabled = enable;
            morphologyFilterSize.Enabled = enable;
            gaussianFilterSize.Enabled = enable;
            medianFilterSize.Enabled = enable;
            comboEdgeDirection.Enabled = enable;
            comboEdgeType.Enabled = enable;
            maxOffset.Enabled = enable;
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            if (edgeCheckerList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - UpdateData");

            onValueUpdate = true;

            desiredOffset.Text = probeList.GetParamValueStr("DesiredOffset");
            maxOffset.Text = probeList.GetParamValueStr("MaxOffset");
            comboEdgeDirection.Text = probeList.GetParamValueStr("EdgeDirection");
            comboEdgeType.Text = probeList.GetParamValueStr("EdgeType");
            gaussianFilterSize.Text = probeList.GetParamValueStr("FilterSize");
            medianFilterSize.Text = probeList.GetParamValueStr("MedianFilterSize");
            threshold.Text = probeList.GetParamValueStr("EdgeThreshold");
            morphologyFilterSize.Text = probeList.GetParamValueStr("MorphologyFilterSize");
            averageCount.Text = probeList.GetParamValueStr("AverageCount");

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - ParamControl_ValueChanged");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        private void comboEdgeDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - comboEdgeDirection_SelectedIndexChanged");

            if (edgeCheckerList.Count == 0)
            {
                return;
            }

            foreach (EdgeChecker edgeChecker in edgeCheckerList)
            {
                AlgorithmParam newParam = edgeChecker.Param.Clone();
                ((EdgeCheckerParam)newParam).EdgeDirection = (EdgeDirection)Enum.Parse(typeof(EdgeDirection), comboEdgeDirection.Text);
                ((EdgeCheckerParam)newParam).EdgeDetectorParam.EdgeDirection = comboEdgeDirection.Text;

                ParamControl_ValueChanged(ValueChangedType.None, edgeChecker, newParam);
            }
        }

        private void comboEdgeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - comboEdgeType_SelectedIndexChanged");

            if (edgeCheckerList.Count == 0)
            {
                return;
            }

            foreach (EdgeChecker edgeChecker in edgeCheckerList)
            {
                AlgorithmParam newParam = edgeChecker.Param.Clone();
                ((EdgeCheckerParam)newParam).EdgeDetectorParam.EdgeType = (EdgeType)Enum.Parse(typeof(EdgeType), comboEdgeType.Text);

                ParamControl_ValueChanged(ValueChangedType.None, edgeChecker, newParam);
            }
        }

        private void filterSize_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - filterSize_ValueChanged");

            if (edgeCheckerList.Count == 0)
            {
                return;
            }

            if (((int)gaussianFilterSize.Value % 2) == 0)
            {
                gaussianFilterSize.Value = (int)gaussianFilterSize.Value + 1;
            }

            foreach (EdgeChecker edgeChecker in edgeCheckerList)
            {
                AlgorithmParam newParam = edgeChecker.Param.Clone();
                ((EdgeCheckerParam)newParam).EdgeDetectorParam.GausianFilterSize = (int)gaussianFilterSize.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, edgeChecker, newParam);
            }
        }

        private void edgeThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - edgeThreshold_ValueChanged");

            if (edgeCheckerList.Count == 0)
            {
                return;
            }

            foreach (EdgeChecker edgeChecker in edgeCheckerList)
            {
                AlgorithmParam newParam = edgeChecker.Param.Clone();
                ((EdgeCheckerParam)newParam).EdgeDetectorParam.Threshold = (int)threshold.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, edgeChecker, newParam);
            }
        }

        private void morphologyFilterSize_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - morphologyFilterSize_ValueChanged");

            if (edgeCheckerList.Count == 0)
            {
                return;
            }

            foreach (EdgeChecker edgeChecker in edgeCheckerList)
            {
                AlgorithmParam newParam = edgeChecker.Param.Clone();
                ((EdgeCheckerParam)newParam).EdgeDetectorParam.MorphologyFilterSize = (int)morphologyFilterSize.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, edgeChecker, newParam);
            }
        }

        private void AverageCount_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - AverageCount_ValueChanged");

            if (edgeCheckerList.Count == 0)
            {
                return;
            }

            foreach (EdgeChecker edgeChecker in edgeCheckerList)
            {
                AlgorithmParam newParam = edgeChecker.Param.Clone();
                ((EdgeCheckerParam)newParam).EdgeDetectorParam.AverageCount = (int)averageCount.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, edgeChecker, newParam);
            }
        }

        private void desiredOffset_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - desiredOffset_ValueChanged");

            if (edgeCheckerList.Count == 0)
            {
                return;
            }

            foreach (EdgeChecker edgeChecker in edgeCheckerList)
            {
                AlgorithmParam newParam = edgeChecker.Param.Clone();
                ((EdgeCheckerParam)newParam).DesiredOffset = (int)desiredOffset.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, edgeChecker, newParam);
            }
        }

        private void maxOffset_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - maxOffset_SelectedIndexChanged");

            if (edgeCheckerList.Count == 0)
            {
                return;
            }

            foreach (EdgeChecker edgeChecker in edgeCheckerList)
            {
                AlgorithmParam newParam = edgeChecker.Param.Clone();
                ((EdgeCheckerParam)newParam).MaxOffset = Convert.ToSingle(maxOffset.Value);

                ParamControl_ValueChanged(ValueChangedType.Position, edgeChecker, newParam);
            }
        }

        private void medianFilterSize_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "EdgeCheckerParamControl - filterSize_ValueChanged");

            if (edgeCheckerList.Count == 0)
            {
                return;
            }

            foreach (EdgeChecker edgeChecker in edgeCheckerList)
            {
                AlgorithmParam newParam = edgeChecker.Param.Clone();
                ((EdgeCheckerParam)newParam).EdgeDetectorParam.MedianFilterSize = (int)medianFilterSize.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, edgeChecker, newParam);
            }
        }

        private void buttonCenter_Click(object sender, EventArgs e)
        {
            foreach (VisionProbe visionProbe in probeList)
            {
                var edgeChecker = (EdgeChecker)visionProbe.InspAlgorithm;

                var newParam = (EdgeCheckerParam)edgeChecker.Param.Clone();
                if (newParam.EdgeDirection == EdgeDirection.Horizontal)
                {
                    newParam.DesiredOffset = 1400 / 2;
                }
                else
                {
                    newParam.DesiredOffset = (visionProbe.BaseRegion.Top + visionProbe.BaseRegion.Bottom) / 2;
                }

                ParamControl_ValueChanged(ValueChangedType.Position, edgeChecker, newParam);
            }

            desiredOffset.Text = probeList.GetParamValueStr("DesiredOffset");
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            ImageD targetGroupImage = TeachState.Instance().TargetGroupImage;
            var imageRegion = new RectangleF(0, 0, targetGroupImage.Width, targetGroupImage.Height);

            foreach (VisionProbe visionProbe in probeList)
            {
                RectangleF probeRegion = visionProbe.BaseRegion.GetBoundRect();
                if (probeRegion == RectangleF.Intersect(probeRegion, imageRegion))
                {
                    RotatedRect probeRotatedRect = visionProbe.BaseRegion;

                    ImageD clipImage = targetGroupImage.ClipImage(probeRotatedRect);

                    AlgoImage algoTargetImage = ImageBuilder.Build(visionProbe.InspAlgorithm.AlgorithmName, clipImage, ImageType.Grey);
                    ImageProcessing imageProcessing = ImageProcessingPool.GetImageProcessing(algoTargetImage);
                    float otsuValue = imageProcessing.Otsu(algoTargetImage);
                    algoTargetImage.Dispose();
                    clipImage.Dispose();

                    var edgeChecker = (EdgeChecker)visionProbe.InspAlgorithm;
                    var edgeCheckerParam = (EdgeCheckerParam)edgeChecker.Param;

                    edgeCheckerParam.EdgeThreshold = (int)Math.Round(otsuValue);
                }
            }

            UpdateData();
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
