using System.Windows;
using Unieye.WPF.Base.Layout.Models;
using UniScanC.Data;
using WPF.ThicknessMeasure.Controls.ViewModels;
using WPF.ThicknessMeasure.Data;

namespace WPF.ThicknessMeasure.Controls.Views
{
    /// <summary>
    /// ProfileChartPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProfileChartControlView : CustomizeControl
    {
        public static readonly DependencyProperty ChartSettingProperty =
            DependencyProperty.RegisterAttached("ChartSetting", typeof(ThicknessChartSetting), typeof(ProfileChartControlView),
                new FrameworkPropertyMetadata(new ThicknessChartSetting(), OnChartSettingChanged));

        public static void OnChartSettingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var local = d as ProfileChartControlView;
            ((ProfileChartControlViewModel)local.DataContext).ChartSetting = e.NewValue as ThicknessChartSetting;
        }

        public ThicknessChartSetting ChartSetting
        {
            get => (ThicknessChartSetting)GetValue(ChartSettingProperty);
            set => SetValue(ChartSettingProperty, value);
        }

        public ProfileChartControlView()
        {
            InitializeComponent();
            Set(ChartSettingProperty);

            var viewModel = new ProfileChartControlViewModel(ChartSetting);
            DataContext = viewModel;
        }
    }
}
