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
    public partial class BarcodeReaderParamControl : UserControl, IAlgorithmParamControl
    {
        public AlgorithmValueChangedDelegate ValueChanged { get; set; }
        public CommandManager CommandManager { get; set; }

        private List<BarcodeReader> barcodeReaderList = new List<BarcodeReader>();
        private ProbeList probeList = new ProbeList();
        private bool onValueUpdate = false;

        public BarcodeReaderParamControl()
        {
            InitializeComponent();

            desiredString.Text = StringManager.GetString(desiredString.Text);
            desiredNum.Text = StringManager.GetString(desiredNum.Text);
            searchRangeWidth.Text = StringManager.GetString(searchRangeWidth.Text);
            searchRangeHeight.Text = StringManager.GetString(searchRangeHeight.Text);
            fiducialProbe.Text = StringManager.GetString(fiducialProbe.Text);
            addBarcodeTypeButton.Text = StringManager.GetString(addBarcodeTypeButton.Text);
            deleteBarcodeTypeButton.Text = StringManager.GetString(deleteBarcodeTypeButton.Text);
            labelDesiredString.Text = StringManager.GetString(labelDesiredString.Text);
            labelDesiredNum.Text = StringManager.GetString(labelDesiredNum.Text);
            labelBarcodeType.Text = StringManager.GetString(labelBarcodeType.Text);
            buttonStringInsert.Text = StringManager.GetString(buttonStringInsert.Text);
            labelBarcodeType.Text = StringManager.GetString(labelBarcodeType.Text);
            labelDesiredNum.Text = StringManager.GetString(labelDesiredNum.Text);
            labelDesiredString.Text = StringManager.GetString(labelDesiredString.Text);
            buttonStringInsert.Text = StringManager.GetString(buttonStringInsert.Text);
            labelSearchRange.Text = StringManager.GetString(labelSearchRange.Text);
            offsetRange.Text = StringManager.GetString(offsetRange.Text);

            labelW.Text = StringManager.GetString(labelW.Text);
            labelH.Text = StringManager.GetString(labelH.Text);
            labelRangeLeft.Text = StringManager.GetString(labelRangeLeft.Text);
            labelRangeRight.Text = StringManager.GetString(labelRangeRight.Text);
            labelRangeBottom.Text = StringManager.GetString(labelRangeBottom.Text);
            labelRangeTop.Text = StringManager.GetString(labelRangeTop.Text);

            AlgorithmStrategy barcodeAlgorithmStrategy = AlgorithmFactory.Instance().GetStrategy(BarcodeReader.TypeName);
            if (barcodeAlgorithmStrategy != null)
            {
                comboBoxBarcodeType.Items.Add(BarcodeType.DataMatrix.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.Codabar.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.Code128.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.Code39.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.Code93.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.Interleaved2of5.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.Pharmacode.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.PLANET.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.POSTNET.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.FourStatePostal.ToString());
                comboBoxBarcodeType.Items.Add(BarcodeType.QRCode.ToString());

                if (barcodeAlgorithmStrategy.LibraryType == ImagingLibrary.CognexVisionPro)
                {
                    comboBoxBarcodeType.Items.Add(BarcodeType.UPCEAN.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.EANUCCComposite.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.PDF417.ToString());
                }
                else
                {
                    comboBoxBarcodeType.Items.Add(BarcodeType.BC412.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.EAN8.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.EAN13.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.EAN14.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.UPC_A.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.UPC_E.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.GS1_128.ToString());
                    comboBoxBarcodeType.Items.Add(BarcodeType.GS1Databar.ToString());
                }
            }
        }

        public void SetupBarcodeType(List<BarcodeType> barcodeTypeList)
        {
            comboBoxBarcodeType.Items.Clear();
            foreach (BarcodeType barcodeType in barcodeTypeList)
            {
                comboBoxBarcodeType.Items.Add(barcodeType.ToString());
            }
        }

        public void ClearSelectedProbe()
        {
            barcodeReaderList.Clear();
            probeList.Clear();
        }

        public void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - SetSelectedProbe");

            probeList.Clear();
            barcodeReaderList.Clear();

            var visionProbe = (VisionProbe)probe;
            if (visionProbe.InspAlgorithm.GetAlgorithmType() == BarcodeReader.TypeName)
            {
                probeList.AddProbe(visionProbe);
                barcodeReaderList.Add((BarcodeReader)visionProbe.InspAlgorithm);
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
            barcodeReaderList.Clear();

            foreach (Probe probe in selectedProbeList)
            {
                probeList.AddProbe(probe);
                barcodeReaderList.Add((BarcodeReader)((VisionProbe)probe).InspAlgorithm);
            }
            UpdateData();
        }

        private void UpdateData()
        {
            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - UpdateData");

            onValueUpdate = true;

            if (barcodeReaderList.Count == 0)
            {
                return;
            }

            BarcodeReader barcodeReader = barcodeReaderList[0];

            var barcodeReaderParam = (BarcodeReaderParam)barcodeReader.Param;

            desiredString.Text = probeList.GetParamValueStr("DesiredString");
            desiredNum.Text = probeList.GetParamValueStr("DesiredNum");
            searchRangeWidth.Text = probeList.GetParamValueStr("SearchRangeWidth");
            searchRangeHeight.Text = probeList.GetParamValueStr("SearchRangeHeight");
            fiducialProbe.CheckState = probeList.GetCheckState("UseForFiducial");
            offsetRange.CheckState = probeList.GetCheckState("OffsetRange");
            rangeThresholdLeft.Text = probeList.GetParamValueStr("RangeThresholdLeft");
            rangeThresholdBottom.Text = probeList.GetParamValueStr("RangeThresholdBottom");
            rangeThresholdTop.Text = probeList.GetParamValueStr("RangeThresholdTop");
            rangeThresholdRight.Text = probeList.GetParamValueStr("RangeThresholdRight");
            timeoutTime.Text = probeList.GetParamValueStr("TimeoutTime");
            RefreshBarcodeTypeList();

            if (fiducialProbe.Checked)
            {
                offsetRange.Enabled = false;
                EnableFiducialProbeItems(true);

                barcodeReaderParam.DesiredNum = (int)desiredNum.Value;
            }
            else
            {
                if (offsetRange.Checked == true)
                {
                    fiducialProbe.Enabled = false;
                }
                else
                {
                    offsetRange.Enabled = true;
                    fiducialProbe.Enabled = true;
                }
                EnableFiducialProbeItems(offsetRange.Checked);
                EnableOffSetRangeItems(offsetRange.Checked);
            }

            useAreaFilter.Checked = barcodeReaderParam.UseAreaFilter;
            minArea.Value = barcodeReaderParam.MinArea;
            maxArea.Value = barcodeReaderParam.MaxArea;

            labelMinArea.Enabled = useAreaFilter.Checked;
            labelMaxArea.Enabled = useAreaFilter.Checked;
            minArea.Enabled = useAreaFilter.Checked;
            maxArea.Enabled = useAreaFilter.Checked;

            closeNum.Value = barcodeReaderParam.CloseNum;

            useBlobing.Checked = barcodeReaderParam.UseBlobing;
            groupBoxBlobing.Enabled = barcodeReaderParam.UseBlobing;

            RefreshThresholdList();

            onValueUpdate = false;
        }

        private void RefreshBarcodeTypeList()
        {
            barcodeTypeList.Items.Clear();

            if (barcodeReaderList.Count == 0)
            {
                return;
            }

            BarcodeReader barcodeReader = barcodeReaderList[0];

            var barcodeReaderParam = (BarcodeReaderParam)barcodeReader.Param;
            foreach (BarcodeType barcodeType in barcodeReaderParam.BarcodeTypeList)
            {
                barcodeTypeList.Items.Add(barcodeType.ToString());
            }
        }

        public void ParamControl_ValueChanged(ValueChangedType valueChangedType, Algorithm algorithm, AlgorithmParam newParam)
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - ParamControl_ValueChanged");

                ValueChanged?.Invoke(valueChangedType, algorithm, newParam, true);
            }
        }

        private void desiredString_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - desiredString_TextChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).DesiredString = desiredString.Text;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        private void desiredNum_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - desiredNum_ValueChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).DesiredNum = (int)desiredNum.Value;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        private void fiducialProbe_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - fiducialProbe_CheckedChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                var barcodeReaderParam = (BarcodeReaderParam)newParam;

                if (fiducialProbe.Checked)
                {
                    offsetRange.Enabled = false;
                    barcodeReaderParam.DesiredNum = (int)desiredNum.Value;
                }
                else
                {
                    offsetRange.Enabled = true;
                    desiredNum.Enabled = true;
                }

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }

            UpdateData();

            EnableFiducialProbeItems(fiducialProbe.Checked);
        }

        private void searchRangeWidth_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - searchRangeWidth_ValueChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).SearchRangeWidth = (int)searchRangeWidth.Value;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        private void searchRangeHeight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - searchRangeWidth_ValueChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).SearchRangeHeight = (int)searchRangeHeight.Value;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        private void addBarcodeTypeButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - addBarcodeTypeButton_Click");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                var barcodeReaderParam = (BarcodeReaderParam)newParam;

                barcodeReaderParam.BarcodeTypeList.Remove((BarcodeType)Enum.Parse(typeof(BarcodeType), (string)comboBoxBarcodeType.SelectedItem));
                barcodeReaderParam.BarcodeTypeList.Add((BarcodeType)Enum.Parse(typeof(BarcodeType), (string)comboBoxBarcodeType.SelectedItem));

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }

            RefreshBarcodeTypeList();
        }

        private void DeleteBarcodeTypeButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - deleteBarcodeTypeButton_Click");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                var barcodeReaderParam = (BarcodeReaderParam)newParam;

                barcodeReaderParam.BarcodeTypeList.Remove((BarcodeType)Enum.Parse(typeof(BarcodeType), (string)barcodeTypeList.SelectedItem));

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }

            RefreshBarcodeTypeList();
        }

        private void useRange_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - useRange_CheckedChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                var barcodeReaderParam = (BarcodeReaderParam)newParam;

                if (offsetRange.Checked)
                {
                    barcodeReaderParam.OffsetRange = true;
                    fiducialProbe.Enabled = false;

                    barcodeReaderParam.DesiredNum = (int)desiredNum.Value;
                }
                else
                {
                    barcodeReaderParam.OffsetRange = false;
                    fiducialProbe.Enabled = true;

                    desiredNum.Enabled = true;
                }

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }

            EnableFiducialProbeItems(offsetRange.Checked);
            EnableOffSetRangeItems(offsetRange.Checked);

            UpdateData();
        }

        private void EnableOffSetRangeItems(bool enable)
        {
            onValueUpdate = true;

            if (enable)
            {
                desiredNum.Value = 1;
            }
            else
            {
                rangeThresholdLeft.Value = 0;
                rangeThresholdRight.Value = 0;
                rangeThresholdBottom.Value = 0;
                rangeThresholdTop.Value = 0;

                foreach (BarcodeReader barcodeReader in barcodeReaderList)
                {
                    var barcodeReaderParam = (BarcodeReaderParam)barcodeReader.Param;

                    barcodeReaderParam.RangeThresholdLeft = 0;
                    barcodeReaderParam.RangeThresholdRight = 0;
                    barcodeReaderParam.RangeThresholdBottom = 0;
                    barcodeReaderParam.RangeThresholdTop = 0;
                }
            }

            desiredNum.Enabled = !enable;

            labelRangeLeft.Enabled = enable;
            labelRangeRight.Enabled = enable;
            labelRangeTop.Enabled = enable;
            labelRangeBottom.Enabled = enable;

            rangeThresholdLeft.Enabled = enable;
            rangeThresholdRight.Enabled = enable;
            rangeThresholdBottom.Enabled = enable;
            rangeThresholdTop.Enabled = enable;

            onValueUpdate = false;
        }

        private void EnableFiducialProbeItems(bool enable)
        {
            onValueUpdate = true;

            if (enable)
            {
                desiredNum.Value = 1;
            }
            else
            {
                searchRangeWidth.Value = 0;
                searchRangeHeight.Value = 0;
            }

            onValueUpdate = false;

            desiredNum.Enabled = !enable;
            labelSearchRange.Enabled = enable;
            labelH.Enabled = enable;
            labelW.Enabled = enable;
            searchRangeWidth.Enabled = enable;
            searchRangeHeight.Enabled = enable;
        }

        private void rangeThresholdLeft_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - rangeThresholdLeft_ValueChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).RangeThresholdLeft = (int)rangeThresholdLeft.Value;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        private void rangeThresholdRight_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - rangeThresholdRight_ValueChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).RangeThresholdRight = (int)rangeThresholdRight.Value;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        private void rangeThresholdBottom_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - rangeThresholdBottom_ValueChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).RangeThresholdBottom = (int)rangeThresholdBottom.Value;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        private void rangeThresholdTop_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - rangeThresholdTop_ValueChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).RangeThresholdTop = (int)rangeThresholdTop.Value;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        private void closeNum_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - comboBoxAutoThresholdType_SelectedIndexChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).CloseNum = (int)closeNum.Value;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        private void minArea_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - minArea_ValueChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).MinArea = (int)minArea.Value;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        private void maxArea_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - maxArea_ValueChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).MaxArea = (int)maxArea.Value;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        private void useAreaFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - comboBoxAutoThresholdType_SelectedIndexChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).UseAreaFilter = useAreaFilter.Checked;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }

            labelMinArea.Enabled = useAreaFilter.Checked;
            labelMaxArea.Enabled = useAreaFilter.Checked;
            minArea.Enabled = useAreaFilter.Checked;
            maxArea.Enabled = useAreaFilter.Checked;
        }

        private void buttonStringInsert_Click(object sender, EventArgs e)
        {
            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                desiredString.Text = barcodeReader.LastReadString;
            }
        }

        private void thresholdPercent_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - thresholdPercent_ValueChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).ThresholdPercent = (int)thresholdPercent.Value;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        private void addThresholdButton_Click(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - addThresholdButton_Click");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();

                var barcodeReaderParam = (BarcodeReaderParam)newParam;
                int index = barcodeReaderParam.ThresholdPercentList.IndexOf((int)thresholdPercent.Value);
                if (index == -1)
                {
                    barcodeReaderParam.ThresholdPercentList.Add((int)thresholdPercent.Value);
                }

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }

            RefreshThresholdList();
        }

        private void DeleteThresholdButton_Click(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - addThresholdButton_Click");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).ThresholdPercentList.Remove((int)thresholdList.SelectedItem);

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }

            RefreshThresholdList();
        }

        private void RefreshThresholdList()
        {
            thresholdList.Items.Clear();

            if (barcodeReaderList.Count == 0)
            {
                return;
            }

            BarcodeReader barcodeReader = barcodeReaderList[0];

            var barcodeReaderParam = (BarcodeReaderParam)barcodeReader.Param;

            foreach (int ThresholdPercent in barcodeReaderParam.ThresholdPercentList)
            {
                thresholdList.Items.Add(ThresholdPercent);
            }
        }

        private void useBlobing_CheckedChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - useBlobing_CheckedChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).UseBlobing = useBlobing.Checked;

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }

            groupBoxBlobing.Enabled = useBlobing.Checked;
        }

        private void timeoutTime_ValueChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
            {
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "BarcodeReaderParamControl - useBlobing_CheckedChanged");

            foreach (BarcodeReader barcodeReader in barcodeReaderList)
            {
                AlgorithmParam newParam = barcodeReader.Param.Clone();
                ((BarcodeReaderParam)newParam).TimeoutTime = Convert.ToInt32(timeoutTime.Value);

                ParamControl_ValueChanged(ValueChangedType.None, barcodeReader, newParam);
            }
        }

        public void PointSelected(Point clickPos, ref bool processingCancelled)
        {
        }

        public string GetTypeName()
        {
            return BarcodeReader.TypeName;
        }

        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }
    }
}
