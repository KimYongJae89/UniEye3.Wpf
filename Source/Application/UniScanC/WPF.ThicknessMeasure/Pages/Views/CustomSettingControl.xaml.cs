using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unieye.WPF.Base.Override;
using WPF.ThicknessMeasure.Pages.ViewModels;

namespace WPF.ThicknessMeasure.Pages.Views
{
    /// <summary>
    /// CustomSettingControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomSettingControl : UserControl, ICustomSettingControl
    {
        private CustomSettingViewModel viewModel;
        public CustomSettingControl()
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
