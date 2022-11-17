using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF.UniScanCM.Converters
{
    public class HoldButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)parameter == "0")
            {
                if ((bool)value == true)
                {
                    return new SolidColorBrush(Colors.Yellow);
                }
                else
                {
                    return Application.Current.Resources["GrayBrush10"] as SolidColorBrush;
                }
            }
            else
            {
                if ((bool)value == true)
                {
                    return new SolidColorBrush(Colors.Black);
                }
                else
                {
                    return Application.Current.Resources["GrayBrush1"] as SolidColorBrush;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
