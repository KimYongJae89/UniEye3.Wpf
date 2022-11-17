using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.UniScanCM.Windows.Models
{
    public class SizeChartSetting
    {
        public int MinDefectSize { get; set; } = 10;
        public int DefectStep { get; set; } = 10;
        public int SizeStepCount { get; set; } = 15;

        public void CopyFrom(SizeChartSetting srcSizeChartSetting)
        {
            MinDefectSize = srcSizeChartSetting.MinDefectSize;
            DefectStep = srcSizeChartSetting.DefectStep;
            SizeStepCount = srcSizeChartSetting.SizeStepCount;
        }
    }
}
