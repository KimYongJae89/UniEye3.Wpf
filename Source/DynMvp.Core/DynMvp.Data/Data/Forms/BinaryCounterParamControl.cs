using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Planbss;
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
    public partial class BinaryCounterParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private ProbeList probeList = new ProbeList();
        private List<BinaryCounter> binaryCounterList = new List<BinaryCounter>();
        private bool onValueUpdate = false;

        public BinaryCounterParamControl()
        {
            InitializeComponent();
        }

        public void ClearSelectedProbe()
        {
            probeList.Clear();
            binaryCounterList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - SetSelectedProbe");

            var selectedProbe = (VisionProbe)probe;
            probeList.AddProbe(selectedProbe);
            if (selectedProbe.InspAlgorithm.GetAlgorithmType() == BinaryCounter.TypeName)
            {
                binaryCounterList.Add((BinaryCounter)selectedProbe.InspAlgorithm);
                UpdateData();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void UpdateProbeImage()
        {

        }

        public void SelectProbe(ProbeList selectedProbeList)
        {
            probeList.Clear();
            binaryCounterList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                var visionProbe = (VisionProbe)probe;
                if (visionProbe.InspAlgorithm.GetAlgorithmType() == BinaryCounter.TypeName)
                {
                    probeList.AddProbe(visionProbe);
                    binaryCounterList.Add((BinaryCounter)visionProbe.InspAlgorithm);
                }
            }

            UpdateData();
        }

        private void UpdateData()
        {
            if (binaryCounterList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - UpdateData");

            onValueUpdate = true;

            minScore.Text = probeList.GetParamValueStr("MinScore");
            maxScore.Text = probeList.GetParamValueStr("MaxScore");
            scoreType.Text = probeList.GetParamValueStr("ScoreType");

            useGrid.CheckState = probeList.GetCheckState("UseGrid");
            gridRowCount.Text = probeList.GetParamValueStr("RowCount");
            gridColumnCount.Text = probeList.GetParamValueStr("ColumnCount");
            cellAcceptRatio.Text = probeList.GetParamValueStr("CellAcceptRatio");

            onValueUpdate = false;

            EnableItems();
        }

        private void EnableItems()
        {
            gridRowCount.Enabled = useGrid.Checked;
            gridColumnCount.Enabled = useGrid.Checked;
            cellAcceptRatio.Enabled = useGrid.Checked;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - ParamControl_ValueChanged");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        private void minScore_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - minScore_ValueChanged");

            if (binaryCounterList.Count == 0)
            {
                return;
            }

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).MinScore = (int)minScore.Value;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            if (sender != minScore)
            {
                return;
            }

            UpDownControl.ShowControl("MinScore", (Control)sender);
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        private void maxScore_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - maxPixelRatio_ValueChanged");

            if (binaryCounterList.Count == 0)
            {
                return;
            }

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).MaxScore = (int)maxScore.Value;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void useGrid_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - useGrid_CheckedChanged");

            if (binaryCounterList.Count == 0)
            {
                return;
            }

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).GridParam.UseGrid = useGrid.Checked;

                ParamControl_ValueChanged(ValueChangedType.ImageProcessing, binaryCounter, newParam);
            }

            EnableItems();
        }

        private void gridRowCount_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - girdRowCount_ValueChanged");

            if (binaryCounterList.Count == 0)
            {
                return;
            }

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).GridParam.RowCount = (int)gridRowCount.Value;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void gridColumnCount_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - gridColumnCount_ValueChanged");

            if (binaryCounterList.Count == 0)
            {
                return;
            }

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).GridParam.ColumnCount = (int)gridColumnCount.Value;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void scoreType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - scoreType_SelectedIndexChanged");

            if (binaryCounterList.Count == 0)
            {
                return;
            }

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).ScoreType = (ScoreType)scoreType.SelectedIndex;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        private void cellAcceptRatio_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BinaryCounterParamControl - cellAcceptRatio_ValueChanged");

            if (binaryCounterList.Count == 0)
            {
                return;
            }

            foreach (BinaryCounter binaryCounter in binaryCounterList)
            {
                AlgorithmParam newParam = binaryCounter.Param.Clone();
                ((BinaryCounterParam)newParam).GridParam.CellAcceptRatio = (int)cellAcceptRatio.Value;

                ParamControl_ValueChanged(ValueChangedType.None, binaryCounter, newParam);
            }
        }

        public string GetTypeName()
        {
            return BinaryCounter.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }
    }
}
