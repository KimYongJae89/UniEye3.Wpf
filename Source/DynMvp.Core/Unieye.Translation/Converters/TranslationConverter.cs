using System;
using System.Globalization;
using System.Windows.Data;
using UniEye.Translation.Helpers;

namespace UniEye.Translation.Converters
{
    public class TranslationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return TranslationHelper.Instance.Translate(values[1] as string);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
