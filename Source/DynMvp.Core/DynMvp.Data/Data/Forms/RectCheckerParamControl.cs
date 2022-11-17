using DynMvp.Base;
using DynMvp.Vision;
using DynMvp.Vision.Planbss;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.Forms
{
    public partial class RectCheckerParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private List<RectChecker> rectCheckerList = new List<RectChecker>();
        private bool onValueUpdate = false;

        public RectCheckerParamControl()
        {
            InitializeComponent();

            labelSearchRange.Text = StringManager.GetString(labelSearchRange.Text);
            labelSearchLength.Text = StringManager.GetString(labelSearchLength.Text);
            labelEdgeType.Text = StringManager.GetString(labelEdgeType.Text);
            labelEdgeThickWidth.Text = StringManager.GetString(labelEdgeThickWidth.Text);
            labelEdgeThickHeight.Text = StringManager.GetString(labelEdgeThickHeight.Text);
            labelGrayValue.Text = StringManager.GetString(labelGrayValue.Text);
            labelScan.Text = StringManager.GetString(labelScan.Text);
            labelPassRate.Text = StringManager.GetString(labelPassRate.Text);
            labelCardinalPoint.Text = StringManager.GetString(labelCardinalPoint.Text);
            labelConvexShape.Text = StringManager.GetString(labelConvexShape.Text);
            outToIn.Text = StringManager.GetString(outToIn.Text);
        }

        public void ClearSelectedProbe()
        {
            rectCheckerList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - SetSelectedProbe");

            var visionProbe = (VisionProbe)probe;
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == RectChecker.TypeName)
            {
                rectCheckerList.Add((RectChecker)visionProbe.InspAlgorithm);
                UpdateData();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void SelectProbe(ProbeList selectedProbeList)
        {
            rectCheckerList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                if (visionProbe.InspAlgorithm.GetAlgorithmType() == CornerChecker.TypeName)
                {
                    rectCheckerList.Add((RectChecker)visionProbe.InspAlgorithm);
                }
            }

            UpdateData();
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            if (rectCheckerList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - UpdateData");

            onValueUpdate = true;

            var param = (RectCheckerParam)rectCheckerList[0].Param;

            searchRange.Value = param.SearchRange;
            edgeType.SelectedIndex = (int)param.EdgeType;
            edgeThickWidth.Value = param.EdgeThickWidth;
            edgeThickHeight.Value = param.EdgeThickHeight;
            grayValue.Value = param.GrayValue;
            projectionHeight.Value = param.ProjectionHeight;
            passRate.Value = param.PassRate;
            cardinalPoint.SelectedIndex = (int)param.CardinalPoint;
            outToIn.Checked = param.OutToIn;
            searchLength.Value = param.SearchLength;
            convexShape.SelectedIndex = (int)param.ConvexShape;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "CircleCheckeParamControl - ParamControl_ValueChanged");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        private void searchRange_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - searchRange_ValueChanged");

            if (rectCheckerList.Count == 0)
            {
                return;
            }

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).SearchRange = Convert.ToInt32(searchRange.Text);

                ParamControl_ValueChanged(ValueChangedType.Position, rectChecker, newParam);
            }
        }

        private void RectCheckerParamControl_Load(object sender, EventArgs e)
        {

        }

        private void edgeThickWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - edgeThickWidth_ValueChanged");

            if (rectCheckerList.Count == 0)
            {
                return;
            }

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).EdgeThickWidth = Convert.ToInt32(edgeThickWidth.Text);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void edgeThickHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - edgeThickHeight_ValueChanged");

            if (rectCheckerList.Count == 0)
            {
                return;
            }

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).EdgeThickHeight = Convert.ToInt32(edgeThickHeight.Text);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void grayValue_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - grayValue_ValueChanged");

            if (rectCheckerList.Count == 0)
            {
                return;
            }

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).GrayValue = Convert.ToInt32(grayValue.Text);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void projectionHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - projectionHeight_ValueChanged");

            if (rectCheckerList.Count == 0)
            {
                return;
            }

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).ProjectionHeight = Convert.ToInt32(projectionHeight.Text);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void passRate_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - passRate_ValueChanged");

            if (rectCheckerList.Count == 0)
            {
                return;
            }

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).PassRate = Convert.ToInt32(passRate.Text);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void cardinalPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - cardinalPoint_SelectedIndexChanged");

            if (rectCheckerList.Count == 0)
            {
                return;
            }

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).CardinalPoint = (CardinalPoint)cardinalPoint.SelectedIndex;

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void convexShape_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - convexShape_SelectedIndexChanged");

            if (rectCheckerList.Count == 0)
            {
                return;
            }

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).ConvexShape = (ConvexShape)convexShape.SelectedIndex;

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void edgeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - edgeType_SelectedIndexChanged");

            if (rectCheckerList.Count == 0)
            {
                return;
            }

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).EdgeType = (EdgeType)(edgeType.SelectedIndex);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        private void searchLength_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "RectCheckerParamControl - searchLength_ValueChanged");

            if (rectCheckerList.Count == 0)
            {
                return;
            }

            foreach (RectChecker rectChecker in rectCheckerList)
            {
                AlgorithmParam newParam = rectChecker.Param.Clone();
                ((RectCheckerParam)newParam).SearchLength = Convert.ToInt32(searchLength.Text);

                ParamControl_ValueChanged(ValueChangedType.None, rectChecker, newParam);
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return RectChecker.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }
    }
}
