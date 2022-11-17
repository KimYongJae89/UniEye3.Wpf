using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Unieye.WPF.Base.Converters
{
    public class DataGridSelectedItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && targetType.Equals(value.GetType()))
            {
                return value;
            }

            return null;
        }
    }

    public class DataGridAutoRowNumberConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object item = values[0];

            int baseIndex = System.Convert.ToInt32(parameter);

            if (values[1].GetType().IsArray)
            {
                var itemList = values[1] as Array;

                var objectList = itemList.Cast<object>().ToList();
                return objectList.FindIndex(x => ReferenceEquals(x, item)) + baseIndex;
            }
            else if (values[1] is IEnumerable<object>)
            {
                var objectList = values[1] as IEnumerable<object>;
                return Array.FindIndex(objectList.ToArray(), x => ReferenceEquals(x, item)) + baseIndex;
            }

            return 0;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
