using System.Windows.Controls;
using Unieye.WPF.Base.Override;
using WPF.UniScanCM.Pages.ViewModels;

namespace WPF.UniScanCM.Pages.Views
{
    /// <summary>
    /// CustomSettingControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomSettingView : UserControl, ICustomSettingControl
    {
        private CustomSettingViewModel viewModel;
        public CustomSettingView()
        {
            InitializeComponent();
            viewModel = new CustomSettingViewModel();
            DataContext = viewModel;
        }

        public void Save()
        {
            viewModel.Save();
        }

        public void AdditionalWork()
        {

        }
    }
}
