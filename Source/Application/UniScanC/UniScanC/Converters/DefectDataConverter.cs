using System;
using System.Globalization;
using System.Windows.Data;
using UniScanC.Data;
using UniScanC.Enums;

namespace UniScanC.Converters
{
    public class DefectDataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var defect = values[0] as Defect;
            var defectType = (EDefectSortType)values[1];
            switch (defectType)
            {
                case EDefectSortType.Camera_No:
                    return defect.ModuleNo.ToString();
                case EDefectSortType.Defect_No:
                    return defect.DefectNo.ToString("0");
                case EDefectSortType.Width_MM:
                    return defect.BoundingRect.Width.ToString("0.000");
                case EDefectSortType.Height_MM:
                    return defect.BoundingRect.Height.ToString("0.000");
                case EDefectSortType.Area_MM2:
                    return defect.Area.ToString("0.000000");
                case EDefectSortType.PosX_MM:
                    return defect.DefectPos.X.ToString("0.000");
                case EDefectSortType.PosY_MM:
                    return defect.DefectPos.Y.ToString("0.000");
                case EDefectSortType.Min_Gv:
                    return defect.MinGv.ToString("0");
                case EDefectSortType.Max_Gv:
                    return defect.MaxGv.ToString("0");
                case EDefectSortType.Avg_Gv:
                    return defect.AvgGv.ToString("0");
                default: return 0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { value };
        }
    }
}
