using System;
using System.Windows;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Views;
using UniEye.Base.UI;
using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.ViewModels
{
    public class ShellWindowViewModel : Observable
    {
        #region 생성자
        public ShellWindowViewModel()
        {

        }
        #endregion


        #region 속성
        public SystemConfig SystemConfig => SystemConfig.Instance;

        private UIElement flyoutView;
        public UIElement FlayoutView
        {
            get => flyoutView;
            set => Set(ref flyoutView, value);
        }

        private UIElement topView;
        public UIElement TopView
        {
            get => topView;
            set => Set(ref topView, value);
        }

        private UIElement mainView;
        public UIElement MainView
        {
            get => mainView;
            set => Set(ref mainView, value);
        }

        private bool flyoutOpen;
        public bool FlyoutOpen
        {
            get => flyoutOpen;
            set => Set(ref flyoutOpen, value);
        }

        public double WindowWidth => SystemConfig.WindowWidth;
        public double WindowHeight => SystemConfig.WindowHeight;
        #endregion


        #region 메서드
        public void FlyoutOpenChanged(bool value)
        {
            FlyoutOpen = value;
        }
        #endregion
    }
}
