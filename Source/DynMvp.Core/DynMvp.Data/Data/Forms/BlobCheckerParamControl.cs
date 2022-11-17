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
    public partial class BlobCheckeParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        public FiducialChangedDelegate FiducialChanged = null;
        private List<BlobChecker> blobCheckerList = new List<BlobChecker>();
        private ProbeList probeList = new ProbeList();
        private bool onValueUpdate = false;
        private bool systemOption = false;

        public BlobCheckeParamControl(bool systemOption = false)
        {
            InitializeComponent();

            darkBlob.Text = StringManager.GetString(darkBlob.Text);
            useWholeImage.Text = StringManager.GetString(useWholeImage.Text);
            this.systemOption = systemOption;

            if (systemOption)
            {
                offsetRangeY.Hide();
                offsetRangeX.Hide();
                label2.Hide();
                label1.Hide();
                labelOffsetRange.Hide();
            }
        }

        public void ClearSelectedProbe()
        {
            probeList.Clear();
            blobCheckerList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - SetSelectedProbe");

            probeList.Clear();
            blobCheckerList.Clear();

            var visionProbe = (VisionProbe)probe;
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == BlobChecker.TypeName)
            {
                probeList.AddProbe(visionProbe);
                blobCheckerList.Add((BlobChecker)visionProbe.InspAlgorithm);
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
            blobCheckerList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                probeList.AddProbe(probe);
                blobCheckerList.Add((BlobChecker)((VisionProbe)probe).InspAlgorithm);
            }

            UpdateData();
        }

        private void UpdateData()
        {
            if (blobCheckerList.Count == 0)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - UpdateData");

            onValueUpdate = true;

            var blobCheckerParam = (BlobCheckerParam)blobCheckerList[0].Param;

            darkBlob.Checked = blobCheckerParam.DarkBlob;
            searchRangeWidth.Value = blobCheckerParam.SearchRangeWidth;
            searchRangeHeight.Value = blobCheckerParam.SearchRangeHeight;
            centerX.Value = (decimal)blobCheckerParam.CenterX;
            centerY.Value = (decimal)blobCheckerParam.CenterY;
            offsetRangeX.Value = (decimal)blobCheckerParam.OffsetRangeX;
            offsetRangeY.Value = (decimal)blobCheckerParam.OffsetRangeY;
            areaLowerPct.Value = blobCheckerParam.MinArea;
            areaUpperPct.Value = blobCheckerParam.MaxArea;

            useWholeImage.Checked = blobCheckerParam.UseWholeImage;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - ParamControl_ValueChanged");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        public void ParamControl_FiducialChanged(bool useFiducialProbe)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - ParamControl_FiducialChanged");

                FiducialChanged?.Invoke(useFiducialProbe);
            }
        }

        private void searchRangeWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - searchRangeWidth_ValueChanged");

            if (blobCheckerList.Count == 0)
            {
                return;
            }

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).SearchRangeWidth = (int)searchRangeWidth.Value;
                if (systemOption)
                {
                    ((BlobCheckerParam)newParam).OffsetRangeX = (int)searchRangeWidth.Value;
                }

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void searchRangeHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - searchRangeHeight_ValueChanged");

            if (blobCheckerList.Count == 0)
            {
                return;
            }

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).SearchRangeHeight = (int)searchRangeHeight.Value;
                if (systemOption)
                {
                    ((BlobCheckerParam)newParam).OffsetRangeY = (int)searchRangeHeight.Value;
                }

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void useWholeImage_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - useWholeImage_CheckedChanged");

            if (blobCheckerList.Count == 0)
            {
                return;
            }

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).UseWholeImage = useWholeImage.Checked;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void offsetRangeX_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - offsetRange_ValueChanged");

            if (blobCheckerList.Count == 0)
            {
                return;
            }

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).OffsetRangeX = (int)offsetRangeX.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void offsetRangeY_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - offsetRange_ValueChanged");

            if (blobCheckerList.Count == 0)
            {
                return;
            }

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).OffsetRangeY = (int)offsetRangeY.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void areaLowerPct_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - areaLowerPct_ValueChanged");

            if (blobCheckerList.Count == 0)
            {
                return;
            }

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).MinArea = (int)areaLowerPct.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void areaUpperPct_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - areaUpperPct_ValueChanged");

            if (blobCheckerList.Count == 0)
            {
                return;
            }

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).MaxArea = (int)areaUpperPct.Value;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        private void darkBlob_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BlobCheckeParamControl - areaUpperPct_ValueChanged");

            if (blobCheckerList.Count == 0)
            {
                return;
            }

            foreach (BlobChecker blobChecker in blobCheckerList)
            {
                AlgorithmParam newParam = blobChecker.Param.Clone();
                ((BlobCheckerParam)newParam).DarkBlob = darkBlob.Checked;

                ParamControl_ValueChanged(ValueChangedType.Position, blobChecker, newParam);
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return BlobChecker.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }
    }
}
