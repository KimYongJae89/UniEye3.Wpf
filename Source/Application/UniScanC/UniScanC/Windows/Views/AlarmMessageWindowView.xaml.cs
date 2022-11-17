using System.Windows;

namespace UniScanC.Windows.Views
{
    /// <summary>
    /// AlarmMessageWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AlarmMessageWindowView : Window
    {
        public AlarmMessageWindowView()
        {
            InitializeComponent();

            //IsVisibleChanged += AlarmMessageWindow_IsVisibleChanged;
        }

        //private void AlarmMessageWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if ((bool)e.NewValue == true)
        //        LogHelper.Warn(LoggerType.Inspection, TranslationHelper.Instance.Translate(AlarmMessage, LanguageSettings.Korean));
        //}
    }
}
