using System.Collections.Generic;
using System.Windows;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Override;
using Unieye.WPF.Base.Views;
using UniScanC.Controls.ViewModels;
using WPF.UniScanIM.Views;

namespace WPF.UniScanIM.Override
{
    public class UiManager : Unieye.WPF.Base.Override.UiManager
    {
        public UiManager() : base()
        {

        }

        public override IShellWindow CreateMainWindow()
        {
            MainWindow = new ShellWindowView();
            return MainWindow;
        }
    }
}
