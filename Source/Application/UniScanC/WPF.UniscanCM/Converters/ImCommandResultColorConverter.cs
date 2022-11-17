using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using UniScanC.Enums;
using UniScanC.Module;

namespace WPF.UniScanCM.Converters
{
    public class ImCommandResultColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var moduleState = values[0] as ModuleState;
            var command = (EUniScanCCommand)values[1];

            SolidColorBrush brush;

            var workingBrush = Application.Current.Resources["LightYellow"] as SolidColorBrush;
            var succeccBrush = Application.Current.Resources["CheckedBrush"] as SolidColorBrush;
            var failBrush = Application.Current.Resources["UncheckedBrush"] as SolidColorBrush;

            if (!moduleState.IsCommandDone(command))
            {
                brush = workingBrush;
            }
            else if (moduleState.IsCommandSuccess(command))
            {
                brush = succeccBrush;
            }
            else
            {
                brush = failBrush;
            }

            return brush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
