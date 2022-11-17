using MahApps.Metro.SimpleChildWindow;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Unieye.WPF.Base.Layout.Views
{
    /// <summary>
    /// LayoutSettingView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LayoutSettingView : ChildWindow
    {
        public LayoutSettingView()
        {
            InitializeComponent();
        }
    }

    public class LayoutControlNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string layoutNameControl = value.ToString();
            string layoutName = layoutNameControl.Substring(0, layoutNameControl.Length - 9);
            return layoutName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
