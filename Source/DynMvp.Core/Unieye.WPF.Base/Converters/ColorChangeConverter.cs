using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Unieye.WPF.Base.Converters
{
    //변수의 값이 다르면 다른 색으로 표시.(일단 2개의 값을 비교해서 다를 경우만)
    public class ColorChangeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var BlackBrush = (SolidColorBrush)Application.Current.Resources["BlackBrush"];
            var AccentBaseColorBrush = (SolidColorBrush)Application.Current.Resources["AccentBaseColorBrush"];

            if (values.Length != 2)
            {
                return BlackBrush;
            }

            return Equals(values[0], values[1]) ? BlackBrush : AccentBaseColorBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorChangeConverterSingle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var BlackBrush = (SolidColorBrush)Application.Current.Resources["BlackBrush"];
            var AccentBaseColorBrush = (SolidColorBrush)Application.Current.Resources["AccentBaseColorBrush"];

            return Equals(value, true) ? AccentBaseColorBrush : BlackBrush;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
