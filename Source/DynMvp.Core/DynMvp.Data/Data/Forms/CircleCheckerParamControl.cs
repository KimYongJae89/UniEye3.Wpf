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
    public partial class CircleCheckeParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private List<CircleChecker> circleCheckerList = new List<CircleChecker>();
        private bool onValueUpdate = false;

        public CircleCheckeParamControl()
        {
            InitializeComponent();

        }

        public void ClearSelectedProbe()
        {
            circleCheckerList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "CircleCheckeParamControl - SetSelectedProbe");

            var visionProbe = (VisionProbe)probe;
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == CircleChecker.TypeName)
            {
                circleCheckerList.Add((CircleChecker)visionProbe.InspAlgorithm);
                UpdateData();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void SelectProbe(ProbeList selectedProbeList)
        {
            circleCheckerList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                if (visionProbe.InspAlgorithm.GetAlgorithmType() == CircleChecker.TypeName)
                {
                    circleCheckerList.Add((CircleChecker)visionProbe.InspAlgorithm);
                }
            }

            UpdateData();
        }

        public void UpdateProbeImage()
        {

        }

        private void UpdateData()
        {
            if (circleCheckerList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CircleCheckeParamControl - UpdateData");

            onValueUpdate = true;

            var circleCheckerParam = (CircleCheckerParam)circleCheckerList[0].Param;
            CircleDetectorParam circleDetectorParam = circleCheckerParam.CircleDetectorParam;

            innerRadius.Value = (int)circleDetectorParam.InnerRadius;
            outterRadius.Value = (int)circleDetectorParam.OutterRadius;
            comboBoxEdgeType.SelectedIndex = (int)circleDetectorParam.EdgeType;
            checkBoxUseImageCenter.Checked = circleCheckerParam.UseImageCenter;
            checkBoxShowOffset.Checked = circleCheckerParam.ShowOffset;

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

        private void innerRadius_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CircleCheckeParamControl - innerRadius_ValueChanged");

            if (circleCheckerList.Count == 0)
            {
                return;
            }

            foreach (CircleChecker circleChecker in circleCheckerList)
            {
                AlgorithmParam newParam = circleChecker.Param.Clone();
                ((CircleCheckerParam)newParam).CircleDetectorParam.InnerRadius = (int)innerRadius.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, circleChecker, newParam);
            }
        }

        private void outterRadius_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CircleCheckeParamControl - outterRadius_ValueChanged");

            if (circleCheckerList.Count == 0)
            {
                return;
            }

            foreach (CircleChecker circleChecker in circleCheckerList)
            {
                AlgorithmParam newParam = circleChecker.Param.Clone();
                ((CircleCheckerParam)newParam).CircleDetectorParam.OutterRadius = (int)outterRadius.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, circleChecker, newParam);
            }
        }

        private void comboBoxEdgeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CircleCheckeParamControl - comboBoxEdgeType_SelectedIndexChanged");

            if (circleCheckerList.Count == 0)
            {
                return;
            }

            foreach (CircleChecker circleChecker in circleCheckerList)
            {
                AlgorithmParam newParam = circleChecker.Param.Clone();
                ((CircleCheckerParam)newParam).CircleDetectorParam.EdgeType = (EdgeType)comboBoxEdgeType.SelectedIndex;

                ParamControl_ValueChanged(ValueChangedType.Position, circleChecker, newParam);
            }
        }

        private void checkBoxUseImageCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CircleCheckeParamControl - checkBoxUseImageCenter_CheckedChanged");

            if (circleCheckerList.Count == 0)
            {
                return;
            }

            foreach (CircleChecker circleChecker in circleCheckerList)
            {
                AlgorithmParam newParam = circleChecker.Param.Clone();
                ((CircleCheckerParam)newParam).UseImageCenter = checkBoxUseImageCenter.Checked;

                ParamControl_ValueChanged(ValueChangedType.Position, circleChecker, newParam);
            }
        }

        private void checkBoxShowOffset_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "CircleCheckeParamControl - checkBoxShowOffset_CheckedChanged");

            if (circleCheckerList.Count == 0)
            {
                return;
            }

            foreach (CircleChecker circleChecker in circleCheckerList)
            {
                AlgorithmParam newParam = circleChecker.Param.Clone();
                ((CircleCheckerParam)newParam).ShowOffset = checkBoxShowOffset.Checked;

                ParamControl_ValueChanged(ValueChangedType.Position, circleChecker, newParam);
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return CircleChecker.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }
    }
}
