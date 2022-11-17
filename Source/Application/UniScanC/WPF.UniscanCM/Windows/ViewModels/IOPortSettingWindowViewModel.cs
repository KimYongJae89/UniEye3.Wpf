using MahApps.Metro.SimpleChildWindow;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.Windows.Views;

namespace WPF.UniScanCM.Windows.ViewModels
{
    public class IOPortSettingWindowViewModel : Observable
    {
        public SystemConfig SystemConfig { get; set; }

        public ICommand TestCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public IOPortSettingWindowViewModel()
        {
            SystemConfig = SystemConfig.Instance as SystemConfig;

            TestCommand = new RelayCommand<ChildWindow>(async (wnd) =>
            {
                var view = new IOPortStatusWindowView();
                if (await MessageWindowHelper.ShowChildWindow<bool>(view) == true)
                {

                }
            });

            OkCommand = new RelayCommand<ChildWindow>((wnd) =>
            {
                // IN
                SystemConfig.Instance.DILabelRun = SystemConfig.DILabelRun;
                SystemConfig.Instance.DILabelError = SystemConfig.DILabelError;
                SystemConfig.Instance.DILabelEmpty = SystemConfig.DILabelEmpty;

                SystemConfig.Instance.DIDefectOccured = SystemConfig.DIDefectOccured;
                SystemConfig.Instance.DIDefectReset = SystemConfig.DIDefectReset;

                // OUT
                SystemConfig.Instance.DODefectSignal = SystemConfig.DODefectSignal;

                SystemConfig.Instance.DOLabelReady = SystemConfig.DOLabelReady;
                SystemConfig.Instance.DOLabelPublish = SystemConfig.DOLabelPublish;
                SystemConfig.Instance.DOLabelReset = SystemConfig.DOLabelReset;

                SystemConfig.Instance.DOTowerLampRed = SystemConfig.DOTowerLampRed;
                SystemConfig.Instance.DOTowerLampYellow = SystemConfig.DOTowerLampYellow;
                SystemConfig.Instance.DOTowerLampGreen = SystemConfig.DOTowerLampGreen;
                SystemConfig.Instance.DOTowerLampBuzzer = SystemConfig.DOTowerLampBuzzer;

                SystemConfig.Instance.Save();

                wnd.Close();
            });

            CancelCommand = new RelayCommand<ChildWindow>((wnd) => wnd.Close());
        }
    }
}
