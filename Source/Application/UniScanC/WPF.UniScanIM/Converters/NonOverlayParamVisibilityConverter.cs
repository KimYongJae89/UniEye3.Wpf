using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.Converters
{
    public class NonOverlayParamVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Enum.TryParse<DefectSignalNonOverlayMode>(value.ToString(), out DefectSignalNonOverlayMode mode))
            {
                if (Enum.TryParse<DefectSignalNonOverlayMode>(parameter.ToString(), out DefectSignalNonOverlayMode targetMode))
                {
                    if (mode == targetMode)
                    {
                        return Visibility.Visible;
                    }
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
