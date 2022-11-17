using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Unieye.WPF.Base.Converters
{
    internal class LampValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var lamp = (Lamp)value;

            if (Enum.TryParse<TowerLampValue>((string)parameter, out TowerLampValue lampState))
            {
                return lamp.Value == lampState;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
