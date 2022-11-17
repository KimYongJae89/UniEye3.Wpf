using MahApps.Metro.Controls;
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

namespace Unieye.WPF.Base.Controls
{
    /// <summary>
    /// PopupWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PopupWindow : MetroWindow
    {
        public static readonly DependencyProperty CustomControlProperty =
            DependencyProperty.Register("CustomControl", typeof(FrameworkElement), typeof(PopupWindow));

        public FrameworkElement CustomControl
        {
            get => (FrameworkElement)GetValue(CustomControlProperty);
            set => SetValue(CustomControlProperty, value);
        }

        public static readonly DependencyProperty IsDialogModeProperty =
            DependencyProperty.Register("IsDialogMode", typeof(bool), typeof(PopupWindow),
                new FrameworkPropertyMetadata(true));

        public bool IsDialogMode
        {
            get => (bool)GetValue(IsDialogModeProperty);
            set => SetValue(IsDialogModeProperty, value);
        }

        public PopupWindow(FrameworkElement frameworkElement, bool isDialogMode = true)
        {
            InitializeComponent();
            CustomControl = frameworkElement;
            IsDialogMode = isDialogMode;
        }

        private void ParentWindow_StateChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    break;
                case WindowState.Minimized:
                    if (!IsDialogMode)
                    {
                        WindowState = System.Windows.WindowState.Normal;
                        Hide();
                    }
                    break;
                case WindowState.Maximized:
                    break;
            }
        }

        private void ParentWindow_Deactivated(object sender, EventArgs e)
        {
            if (!IsDialogMode)
            {
                WindowState = System.Windows.WindowState.Normal;
                Hide();
            }

            //switch (WindowState)
            //{
            //    case WindowState.Normal:
            //        break;
            //    case WindowState.Minimized:
            //        if (!IsDialogMode)
            //        {
            //            WindowState = System.Windows.WindowState.Normal;
            //            this.Hide();
            //        }
            //        break;
            //    case WindowState.Maximized:
            //        break;
            //}
        }
    }
}
