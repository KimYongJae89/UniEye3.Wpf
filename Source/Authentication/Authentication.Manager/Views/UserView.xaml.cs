using Authentication.Manager.ViewModels;
using MaterialDesignThemes.Wpf;
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
using System.Windows.Shapes;

namespace Authentication.Manager.Views
{
    /// <summary>
    /// UserView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserView : UserControl
    {
        UserViewModel _viewModel;

        public UserView()
        {
            InitializeComponent();

            _viewModel = this.DataContext as UserViewModel;
            _viewModel.PasswordBox = passwordBox;
            _viewModel.ConfirmPasswordBox = confirmPasswordBox;
        }
    }
}
