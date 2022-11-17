using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using UniScanC.Data;
using UniScanC.Enums;

namespace UniScanC.Converters
{
    public class DisplayDataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var defects = values[0] as IEnumerable<Defect>;
            if (values[1] == null)
            {
                return null;
            }

            var defectType = (EDisplayDataType)values[1];

            object retValue = 0;

            switch (defectType)
            {
                case EDisplayDataType.Area_MM2:
                    retValue = defects?.Sum(x => x.Area);
                    break;
                case EDisplayDataType.Length_M:
                    if (defects != null)
                    {
                        retValue = defects.Sum(x => x.BoundingRect.Height) / 1000;
                    }

                    break;
            }

            return string.Format("{0:0.00}", retValue);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
