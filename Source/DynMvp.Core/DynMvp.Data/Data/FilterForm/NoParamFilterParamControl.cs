using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DynMvp.Data.FilterForm
{
    public partial class NoParamFilterParamControl : UserControl, IFilterParamControl
    {
        public FilterParamValueChangedDelegate ValueChanged = null;
        private List<IFilter> filterList = new List<IFilter>();

        public NoParamFilterParamControl()
        {
            InitializeComponent();

            //change language
            labelNoParameter.Text = StringManager.GetString(labelNoParameter.Text);
        }

        public FilterType GetFilterType()
        {
            return FilterType.Morphology;
        }

        public void ClearSelectedFilter()
        {
            filterList.Clear();
        }

        public void AddSelectedFilter(IFilter filter)
        {
            LogHelper.Debug(LoggerType.Operation, "NoParamFilterParamControl - SetSelectedFilter");

            filterList.Add(filter);
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {

        }

        public IFilter CreateFilter()
        {
            return null;
        }

        public void SetValueChanged(FilterParamValueChangedDelegate valueChanged)
        {
            ValueChanged = valueChanged;
        }
    }
}
