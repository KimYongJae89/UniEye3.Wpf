using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Unieye.WPF.Base.Converters
{
    public class DecimalPointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (System.Convert.ToInt16(parameter))
            {
                case 0: return string.Format("{0:0}", value);
                case 1: return string.Format("{0:0.0}", value);
                case 2: return string.Format("{0:0.00}", value);
                case 3: return string.Format("{0:0.000}", value);
                default: return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
