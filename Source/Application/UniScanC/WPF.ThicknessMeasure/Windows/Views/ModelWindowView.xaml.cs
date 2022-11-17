using MahApps.Metro.SimpleChildWindow;
using WPF.ThicknessMeasure.Windows.ViewModels;

namespace WPF.ThicknessMeasure.Windows.Views
{
    /// <summary>
    /// ModelWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModelWindowView : ChildWindow
    {
        public ModelWindowView(ModelWindowResult modelWindowResult = null)
        {
            InitializeComponent();

            var modelWindowViewModel = new ModelWindowViewModel(modelWindowResult);
            DataContext = modelWindowViewModel;
        }
    }
}
