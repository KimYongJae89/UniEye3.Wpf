using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.UniScanCM.Windows.Models
{
    public class WidthChartSetting
    {
        public int StepLength { get; set; } = 10;

        public void CopyFrom(WidthChartSetting srcWidthChartSetting)
        {
            StepLength = srcWidthChartSetting.StepLength;
        }
    }
}
