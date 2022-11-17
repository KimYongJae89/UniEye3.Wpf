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
    public partial class GridSubtractorParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private List<GridSubtractor> gridSubtractorList = new List<GridSubtractor>();
        private ProbeList probeList = new ProbeList();
        private bool onValueUpdate = false;

        public GridSubtractorParamControl()
        {
            InitializeComponent();

            maskImageSelector.RowTemplate.Height = maskImageSelector.Height / 4;
        }

        public void ClearSelectedProbe()
        {
            gridSubtractorList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - AddSelectedProbe");

            var selectedProbe = (VisionProbe)probe;
            if (selectedProbe.InspAlgorithm.GetAlgorithmType() == GridSubtractor.TypeName)
            {
                gridSubtractorList.Add((GridSubtractor)selectedProbe.InspAlgorithm);
                UpdateData();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void AddSelectedProbe(ProbeList selectedProbeList)
        {
            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - AddSelectedProbe");

            gridSubtractorList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                if (visionProbe.InspAlgorithm.GetAlgorithmType() == GridSubtractor.TypeName)
                {
                    gridSubtractorList.Add((GridSubtractor)visionProbe.InspAlgorithm);
                }
            }

            UpdateData();
        }

        public void UpdateProbeImage()
        {

        }

        public void SelectProbe(ProbeList selectedProbeList)
        {
            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - SelectProbe");

            probeList.Clear();
            gridSubtractorList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                probeList.AddProbe(visionProbe);
                gridSubtractorList.Add((GridSubtractor)visionProbe.InspAlgorithm);
            }

            UpdateData();
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return GridSubtractor.TypeName;
        }

        private void UpdateData()
        {
            if (gridSubtractorList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - UpdateData");

            onValueUpdate = true;

            var gridSubtractorParam = (GridSubtractorParam)gridSubtractorList[0].Param;

            minPixelCount.Value = gridSubtractorParam.MinCount;
            projectionRatio.Value = (decimal)gridSubtractorParam.ProjectionRatio;
            invert.Checked = gridSubtractorParam.Invert;

            patternScore.Value = gridSubtractorParam.MatchScore;
            patternMaxOffsetX.Value = gridSubtractorParam.MaxOffsetX;
            patternMaxOffsetY.Value = gridSubtractorParam.MaxOffsetY;
            minThreshold.Value = gridSubtractorParam.MinThreshold;
            maxThreshold.Value = gridSubtractorParam.MaxThreshold;

            searchRangeX.Value = gridSubtractorParam.SearchRangeX;
            searchRangeY.Value = gridSubtractorParam.SearchRangeY;

            UpdateMaskImageSelector();

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - ParamControl_ValueChanged");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        private void addMaskButton_Click(object sender, EventArgs e)
        {
            if (gridSubtractorList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - addMaskButton_Click");

            AddMask();
            UpdateMaskImageSelector();
        }

        private void AddMask()
        {
            if (probeList.Count == 0)
            {
                return;
            }

            if (gridSubtractorList.Count == 0)
            {
                return;
            }

            if (probeList.Count != gridSubtractorList.Count)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - AddMask");

            foreach (Probe probe in probeList)
            {
                var gridSubtractor = (GridSubtractor)((VisionProbe)probe).InspAlgorithm;

                Target selectedTarget = probe.Target;
                ImageD targetGroupImage = TeachState.Instance().TargetGroupImage;

                var imageRegion = new RectangleF(0, 0, targetGroupImage.Width, targetGroupImage.Height);

                RectangleF probeRegion = probe.BaseRegion.GetBoundRect();
                if (probeRegion == RectangleF.Intersect(probeRegion, imageRegion))
                {
                    RotatedRect probeRotatedRect = probe.BaseRegion;

                    ImageD clipImage = targetGroupImage.ClipImage(probeRotatedRect);

                    var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();
                    newParam.AddMaskImage(clipImage);

                    ParamControl_ValueChanged(ValueChangedType.None, gridSubtractor, newParam);
                }
            }
        }

        private void refreshMaskButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - refreshMaskButton_Click");

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();
                newParam.RemoveAllMaskImage();

                ParamControl_ValueChanged(ValueChangedType.None, gridSubtractor, newParam);
            }

            AddMask();

            UpdateMaskImageSelector();
        }

        private void DeleteMaskButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - deleteMaskButton_Click");

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                if (maskImageSelector.SelectedRows.Count > 0)
                {
                    int index = maskImageSelector.SelectedRows[0].Index;
                    if (index > -1)
                    {
                        var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();

                        newParam.RemoveMaskImage(index);

                        maskImageSelector.Rows.RemoveAt(index);

                        ParamControl_ValueChanged(ValueChangedType.None, gridSubtractor, newParam);
                    }
                }
            }
        }

        private void UpdateMaskImageSelector()
        {
            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - UpdateMaskImageSelector");

            maskImageSelector.Rows.Clear();

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                var gridSubtractorParam = (GridSubtractorParam)gridSubtractor.Param;//.Clone();

                foreach (ImageD maskImage in gridSubtractorParam.MaskImageList)
                {
                    int index = maskImageSelector.Rows.Add(maskImage.ToBitmap());

                    if (maskImage.Width * 3 < maskImage.Height)
                    {
                        maskImageSelector.Rows[index].Height = maskImageSelector.RowTemplate.Height * 2;
                    }

                    /*maskImageSelector.Rows[index].Height = maskImageSelector.Rows[index].Cells[0].ContentBounds.Height;
                    if (maskImageSelector.Rows[index].Height > maskImageSelector.Height - maskImageSelector.ColumnHeadersHeight)
                    {
                        maskImageSelector.Rows[index].Height = (maskImageSelector.Height - maskImageSelector.ColumnHeadersHeight);
                    }*/
                }

                if (maskImageSelector.Rows.Count > 0)
                {
                    maskImageSelector.Rows[0].Selected = true;
                }
            }
        }

        private void minPixelCount_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - minPixelCount_ValueChanged");

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();
                newParam.MinCount = (int)minPixelCount.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, gridSubtractor, newParam);
            }
        }

        private void projectionRatio_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - projectionRatio_ValueChanged");

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();
                newParam.ProjectionRatio = (float)projectionRatio.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, gridSubtractor, newParam);
            }
        }

        private void invert_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - projectionRatio_ValueChanged");

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();
                newParam.Invert = invert.Checked;

                ParamControl_ValueChanged(ValueChangedType.Position, gridSubtractor, newParam);
            }
        }

        private void patternScore_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - edgeZoneThreshold_ValueChanged");

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();
                newParam.MatchScore = (int)patternScore.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, gridSubtractor, newParam);
            }
        }

        private void patternMaxOffsetX_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - patternMaxOffsetX_ValueChanged");

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();
                newParam.MaxOffsetX = (int)patternMaxOffsetX.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, gridSubtractor, newParam);
            }
        }

        private void patternMaxOffsetY_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - patternMaxOffsetY_ValueChanged");

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();
                newParam.MaxOffsetY = (int)patternMaxOffsetY.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, gridSubtractor, newParam);
            }
        }

        private void searchRangeX_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - searchRangeX_ValueChanged");

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();
                newParam.SearchRangeX = (int)searchRangeX.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, gridSubtractor, newParam);
            }
        }

        private void searchRangeY_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - searchRangeY_ValueChanged");

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();
                newParam.SearchRangeY = (int)searchRangeY.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, gridSubtractor, newParam);
            }
        }

        private void minThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - minThreshold_ValueChanged");

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();
                newParam.MinThreshold = (int)minThreshold.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, gridSubtractor, newParam);
            }
        }

        private void maxThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "GridSubtractorParamControl - maxThreshold_ValueChanged");

            foreach (GridSubtractor gridSubtractor in gridSubtractorList)
            {
                var newParam = (GridSubtractorParam)gridSubtractor.Param.Clone();
                newParam.MaxThreshold = (int)maxThreshold.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, gridSubtractor, newParam);
            }
        }
    }
}
