using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Data.FilterForm
{
    public partial class EdgeExtractionFilterParamControl : UserControl, IFilterParamControl
    {
        public FilterParamValueChangedDelegate ValueChanged = null;
        private List<EdgeExtractionFilter> edgeExtractionFilterList = new List<EdgeExtractionFilter>();
        private bool onValueUpdate = false;

        public EdgeExtractionFilterParamControl()
        {
            InitializeComponent();

            labelKernelSize.Text = StringManager.GetString(labelKernelSize.Text);

            UpdateData(new EdgeExtractionFilter());

            //change language
            labelKernelSize.Text = StringManager.GetString(labelKernelSize.Text);
        }

        public FilterType GetFilterType()
        {
            return FilterType.EdgeExtraction;
        }

        public void ClearSelectedFilter()
        {
            edgeExtractionFilterList.Clear();
        }

        public void AddSelectedFilter(IFilter filter)
        {
            LogHelper.Debug(LoggerType.Operation, "EdgeExtractionFilterParamControl - SetSelectedFilter");

            if (filter is EdgeExtractionFilter)
            {
                edgeExtractionFilterList.Add((EdgeExtractionFilter)filter);
                UpdateData(edgeExtractionFilterList[0]);
            }
        }

        public IFilter CreateFilter()
        {
            return new EdgeExtractionFilter((int)kernelSize.Value);
        }

        private void UpdateData(EdgeExtractionFilter edgeExtractionFilter)
        {
            LogHelper.Debug(LoggerType.Operation, "EdgeExtractionFilterParamControl - UpdateData");

            onValueUpdate = true;

            kernelSize.Value = edgeExtractionFilter.KernelSize;

            onValueUpdate = false;
        }

        public void ParamControl_ValueChanged()
        {
            if (onValueUpdate == false)
            {
                LogHelper.Debug(LoggerType.Operation, "VisionParamControl - VisionParamControl_PositionUpdated");

                ValueChanged?.Invoke();
            }
        }

        private void lowerValue_ValueChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "BrightnessCheckerParamControl - lowerValue_ValueChanged");

            if (edgeExtractionFilterList.Count == 0)
            {
                return;
            }

            foreach (EdgeExtractionFilter edgeExtractionFilter in edgeExtractionFilterList)
            {
                edgeExtractionFilter.KernelSize = (int)kernelSize.Value;
            }

            ParamControl_ValueChanged();
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string valueName = "";
            if (sender == kernelSize)
            {
                valueName = "Kernel Size";
            }

            UpDownControl.ShowControl(StringManager.GetString(valueName), (Control)sender);
        }

        public void SetValueChanged(FilterParamValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }
    }
}
