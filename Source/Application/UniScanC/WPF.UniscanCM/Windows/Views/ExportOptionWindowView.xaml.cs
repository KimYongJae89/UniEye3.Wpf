using MahApps.Metro.SimpleChildWindow;
using System.Windows;
using WPF.UniScanCM.Models;

namespace WPF.UniScanCM.Windows.Views
{
    /// <summary>
    /// ExportOptionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExportOptionWindowView : ChildWindow
    {
        #region DependencyProperty

        public static readonly DependencyProperty ReportModelProperty =
            DependencyProperty.Register("ReportModel", typeof(ReportModel), typeof(ExportOptionWindowView));

        public ReportModel ReportModel
        {
            get => (ReportModel)GetValue(ReportModelProperty);
            set => SetValue(ReportModelProperty, value);
        }

        public static readonly DependencyProperty ExportOptionModelProperty =
            DependencyProperty.Register("ExportOptionModel", typeof(ExportOptionModel), typeof(ExportOptionWindowView));

        public ExportOptionModel ExportOptionModel
        {
            get => (ExportOptionModel)GetValue(ExportOptionModelProperty);
            set => SetValue(ExportOptionModelProperty, value);
        }

        #endregion

        public ExportOptionWindowView()
        {
            InitializeComponent();
        }
    }
}
