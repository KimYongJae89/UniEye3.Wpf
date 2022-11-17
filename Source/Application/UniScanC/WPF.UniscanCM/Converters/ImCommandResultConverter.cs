using System;
using System.Globalization;
using System.Windows.Data;
using UniEye.Translation.Helpers;
using UniScanC.Enums;
using UniScanC.Module;

namespace WPF.UniScanCM.Converters
{
    public class ImCommandResultConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var moduleState = values[0] as ModuleState;
            var command = (EUniScanCCommand)values[1];

            if (!moduleState.IsCommandDone(command))
            {
                return TranslationHelper.Instance.Translate("Excuting");
            }

            if (moduleState.IsCommandSuccess(command))
            {
                return TranslationHelper.Instance.Translate("Complete");
            }

            return TranslationHelper.Instance.Translate("Fail");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
