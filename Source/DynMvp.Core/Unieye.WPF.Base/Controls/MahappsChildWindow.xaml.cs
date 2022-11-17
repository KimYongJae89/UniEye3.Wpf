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

namespace Unieye.WPF.Base.Controls
{
    /// <summary>
    /// MahappsChildWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MahappsChildWindow
    {
        public static readonly DependencyProperty CustomControlProperty =
            DependencyProperty.Register("CustomControl", typeof(FrameworkElement), typeof(MahappsChildWindow));

        public FrameworkElement CustomControl
        {
            get => (FrameworkElement)GetValue(CustomControlProperty);
            set => SetValue(CustomControlProperty, value);
        }

        private ICommand okCommand;
        public ICommand OKCommand => okCommand ?? (okCommand = new RelayCommand(OK));

        private void OK()
        {
            Close(true);
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand => cancelCommand ?? (cancelCommand = new RelayCommand(Cancel));

        private void Cancel()
        {
            Close(false);
        }

        public MahappsChildWindow()
        {
            InitializeComponent();
        }
    }
}
