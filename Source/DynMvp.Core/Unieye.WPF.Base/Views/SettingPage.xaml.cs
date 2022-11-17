using MahApps.Metro.Controls.Dialogs;
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
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Override;
using Unieye.WPF.Base.ViewModels;

namespace Unieye.WPF.Base.Views
{
    /// <summary>
    /// SettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingPage : UserControl
    {
        public static readonly DependencyProperty CustomSettingControlProperty =
            DependencyProperty.Register("CustomSettingControl", typeof(ICustomSettingControl), typeof(SettingPage));

        public ICustomSettingControl CustomSettingControl
        {
            get => (ICustomSettingControl)GetValue(CustomSettingControlProperty);
            set => SetValue(CustomSettingControlProperty, value);
        }

        public SettingPage()
        {
            InitializeComponent();
        }
    }
}
