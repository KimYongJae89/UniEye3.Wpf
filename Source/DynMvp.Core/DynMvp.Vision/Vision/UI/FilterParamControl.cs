using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Vision.UI
{
    public delegate void FilterParamValueChangedDelegate();

    public interface IFilterParamControl
    {
        FilterType GetFilterType();
        void ClearSelectedFilter();
        void AddSelectedFilter(IFilter filter);
        IFilter CreateFilter();

        void SetValueChanged(FilterParamValueChangedDelegate valueChanged);
    }
}
