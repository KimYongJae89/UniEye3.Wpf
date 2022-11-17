using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Unieye.WPF.Base.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        // Parameter 를 Inverse 인자로 사용
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visible;

            if ((bool)value == true)
            {
                visible = Visibility.Visible;
            }
            else
            {
                visible = Visibility.Collapsed;
            }

            if (System.Convert.ToBoolean(parameter))
            {
                if (visible == Visibility.Visible)
                {
                    visible = Visibility.Collapsed;
                }
                else if (visible == Visibility.Collapsed)
                {
                    visible = Visibility.Visible;
                }
            }

            return visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visible;

            if ((bool)value == true)
            {
                visible = Visibility.Visible;
            }
            else
            {
                visible = Visibility.Collapsed;
            }

            if (System.Convert.ToBoolean(parameter))
            {
                if (visible == Visibility.Visible)
                {
                    visible = Visibility.Collapsed;
                }
                else if (visible == Visibility.Collapsed)
                {
                    visible = Visibility.Visible;
                }
            }

            return visible;
        }
    }
}
