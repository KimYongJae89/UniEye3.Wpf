using System;
using System.Globalization;
using System.Windows.Data;

namespace UniScanC.Converters
{
    public enum CategoryDisplyValues
    {
        Count,
        Length
    }

    public class DisplayValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var displayValue = (CategoryDisplyValues)values[1];
            if (displayValue == CategoryDisplyValues.Length)
            {
                return string.Format("{0:0.00} m", values[0]);
            }

            return string.Format("{0:0} EA", values[0]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
