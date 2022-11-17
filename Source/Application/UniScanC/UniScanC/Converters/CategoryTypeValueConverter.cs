using System;
using System.Globalization;
using System.Windows.Data;
using UniScanC.Data;
using UniScanC.Enums;

namespace UniScanC.Converters
{
    public class CategoryTypeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is CategoryType categoryType))
            {
                return null;
            }

            string sign = "";
            double data = System.Convert.ToDouble(categoryType.Data);

            if (categoryType.Type.ToString().Contains("LOWER"))
            {
                sign = "<";
            }
            else if (categoryType.Type.ToString().Contains("UPPER"))
            {
                sign = ">";
            }

            switch (categoryType.Type)
            {
                case ECategoryTypeName.EdgeLower:
                case ECategoryTypeName.EdgeUpper:
                case ECategoryTypeName.WidthLower:
                case ECategoryTypeName.WidthUpper:
                case ECategoryTypeName.HeightLower:
                case ECategoryTypeName.HeightUpper:
                    data /= 1000;
                    return string.Format("{0} {1:0.000}", sign, data);
                case ECategoryTypeName.AreaLower:
                case ECategoryTypeName.AreaUpper:
                    data /= 1000000;
                    return string.Format("{0} {1:0.000000}", sign, data);
                case ECategoryTypeName.MinGvLower:
                case ECategoryTypeName.MinGvUpper:
                case ECategoryTypeName.MaxGvLower:
                case ECategoryTypeName.MaxGvUpper:
                case ECategoryTypeName.AvgGvLower:
                case ECategoryTypeName.AvgGvUpper:
                    return categoryType.Data.ToString();
            }

            return string.Format("{0} {1:0.000}", sign, data);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble((string)value);
        }
    }
}
