using System;
using System.Globalization;
using System.Windows.Data;
using UniScanC.Data;
using UniScanC.Enums;

namespace UniScanC.Converters
{
    public class CategoryTypeValueUnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is CategoryType categoryType))
            {
                return "";
            }

            switch (categoryType.Type)
            {
                case ECategoryTypeName.EdgeLower:
                case ECategoryTypeName.EdgeUpper:
                case ECategoryTypeName.WidthLower:
                case ECategoryTypeName.WidthUpper:
                case ECategoryTypeName.HeightLower:
                case ECategoryTypeName.HeightUpper:
                    {
                        float data = System.Convert.ToSingle(categoryType.Data) / 1000f;
                        return $"{data}mm";
                    }
                case ECategoryTypeName.AreaLower:
                case ECategoryTypeName.AreaUpper:
                    {
                        float data = System.Convert.ToSingle(categoryType.Data) / 1000000f;
                        return $"{data}mm²";
                    }
                case ECategoryTypeName.MinGvLower:
                case ECategoryTypeName.MinGvUpper:
                case ECategoryTypeName.MaxGvLower:
                case ECategoryTypeName.MaxGvUpper:
                case ECategoryTypeName.AvgGvLower:
                case ECategoryTypeName.AvgGvUpper:
                    return categoryType.Data.ToString();
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
