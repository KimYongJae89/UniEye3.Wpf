using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF.UniScanCM.Converters
{
    public class UMtoMMConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) / 1000;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!double.TryParse((string)value, out double dblResult))
            {
                return null;
            }

            return System.Convert.ToDouble(value) * 1000;
        }
    }
}
