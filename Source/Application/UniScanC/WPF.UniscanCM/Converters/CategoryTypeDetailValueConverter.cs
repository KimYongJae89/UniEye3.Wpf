using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using UniScanC.Data;
using UniScanC.Enums;

namespace WPF.UniScanCM.Converters
{
    public class CategoryTypeDetailValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var CategoryTypeList = value as List<CategoryType>;

            if (!Enum.TryParse<ECategoryTypeName>((string)parameter, out ECategoryTypeName categoryType))
            {
                return "-";
            }

            return GetCategoryTypeCaption(CategoryTypeList, categoryType);
        }

        private string GetCategoryTypeCaption(List<CategoryType> categoryTypeList, ECategoryTypeName categoryTypeName)
        {
            CategoryType categoryType = categoryTypeList.Find(x => x.Type == categoryTypeName);
            if (!categoryType.Use)
            {
                return "-";
            }

            return System.Convert.ToString(categoryType.Data);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
