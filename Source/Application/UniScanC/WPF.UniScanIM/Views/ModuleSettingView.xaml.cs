using System.Windows.Controls;
using WPF.UniScanIM.ViewModels;

namespace WPF.UniScanIM.Views
{
    /// <summary>
    /// InspectPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModuleSettingView : UserControl
    {
        public ModuleSettingViewModel ViewModel { get; set; } = new ModuleSettingViewModel();

        public ModuleSettingView()
        {
            DataContext = ViewModel;

            InitializeComponent();
        }
    }
}
