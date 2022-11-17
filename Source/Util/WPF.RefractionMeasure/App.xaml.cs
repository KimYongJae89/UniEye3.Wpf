using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using UniEye.Base.Config;
using UniEye.Translation.Helpers;
using WPF.RefractionMeasure.Views;

namespace WPF.RefractionMeasure
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mutex = new Mutex(true, "WPF.RefractionMeasure", out bool bNew);
            if (bNew)
            {
                TranslationHelper.Instance.CurrentCultureInfo = UiConfig.Instance().LoadLanguage();

                var mainWindow = new Lazy<RefractionMeasureView>();
                MainWindow = mainWindow.Value;
                MainWindow.Show();
            }
        }
    }
}
