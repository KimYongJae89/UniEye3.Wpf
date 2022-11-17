using System.Windows;
using Unieye.WPF.Base.Layout.Models;
using UniScanC.Data;
using WPF.ThicknessMeasure.Controls.ViewModels;
using WPF.ThicknessMeasure.Data;

namespace WPF.ThicknessMeasure.Controls.Views
{
    /// <summary>
    /// TrendChartControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TrendChartControlView : CustomizeControl
    {
        public static readonly DependencyProperty ChartSettingProperty =
            DependencyProperty.RegisterAttached("ChartSetting", typeof(ThicknessChartSetting), typeof(TrendChartControlView),
                new FrameworkPropertyMetadata(new ThicknessChartSetting(), OnChartSettingChanged));

        public static void OnChartSettingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var local = d as TrendChartControlView;
            ((TrendChartControlViewModel)local.DataContext).ChartSetting = e.NewValue as ThicknessChartSetting;
        }

        public ThicknessChartSetting ChartSetting
        {
            get => (ThicknessChartSetting)GetValue(ChartSettingProperty);
            set => SetValue(ChartSettingProperty, value);
        }

        public TrendChartControlView()
        {
            InitializeComponent();
            Set(ChartSettingProperty);

            var viewModel = new TrendChartControlViewModel(ChartSetting);
            DataContext = viewModel;
        }
    }
}
