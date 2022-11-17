using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace UniScanC.Converters
{
    public class ChildHeightBindingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
            {
                return null;
            }

            int colNum = System.Convert.ToInt32(values[0]);
            var listBox = values[1] as ListBox;
            double listBoxWidth = listBox.ActualWidth;
            double listBoxHeight = listBox.ActualHeight;

            if (listBoxHeight != 0)
            {
                return (listBoxWidth / colNum) + 34;
            }
            else
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
