using MahApps.Metro.Controls;
using System;
using System.Windows;
using Unieye.WPF.Base.Services;
using Unieye.WPF.Base.Views;
using UniEye.Base.UI;
using WPF.UniScanIM.ViewModels;

namespace WPF.UniScanIM.Views
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ShellWindowView : MetroWindow, IShellWindow
    {
        public ShellWindowViewModel ViewModel = new ShellWindowViewModel();

        public ShellWindowView()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        public void Initialize()
        {
            ViewModel.FlayoutView = new FlyoutView();
            var flyoutView = ViewModel.FlayoutView as FlyoutView;
            var flyoutViewModel = flyoutView.DataContext as FlyoutViewModel;

            ViewModel.TopView = new TopView();
            var topView = ViewModel.TopView as TopView;
            var topViewModel = topView.DataContext as TopViewModel;
            topViewModel.FlyoutOpenChanged += ViewModel.FlyoutOpenChanged;

            ViewModel.MainView = new MainView();
            var mainView = ViewModel.MainView as MainView;
            var mainViewModel = mainView.DataContext as MainViewModel;
            mainViewModel.ZoomService = new ZoomService(mainView);
        }

        public void EnableTab(TabKey tabKey, bool enable)
        {

        }

        public void ShowTab(TabKey tabKey)
        {

        }
    }
}
