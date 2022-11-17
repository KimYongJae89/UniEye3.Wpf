using System.Windows.Controls;
using System.Windows.Input;
using WPF.ThicknessMeasure.Pages.ViewModels;

namespace WPF.ThicknessMeasure.Pages.Views
{
    /// <summary>
    /// InspectPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModelPageView : UserControl
    {
        public ModelPageView()
        {
            InitializeComponent();
        }

        private void ModelList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vm = DataContext as ModelPageViewModel;
            vm.SelectCommand.Execute(null);
        }
    }
}
