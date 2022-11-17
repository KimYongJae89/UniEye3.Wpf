using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.ViewModels
{
    public class FlyoutViewModel : Observable
    {
        public FlyoutViewModel()
        {
            SaveCommand = new RelayCommand(SaveCommandAction);

            SettingviewModelLoad();
        }

        public ICommand SaveCommand { get; }

        public SystemConfig SystemConfig => SystemConfig.Instance;

        public ModuleSettingViewModel ModuleSettingViewModel { get; set; } = new ModuleSettingViewModel();

        public CommSettingViewModel CommSettingViewModel { get; set; } = new CommSettingViewModel();

        public ParamSettingViewModel ParamSettingViewModel { get; set; } = new ParamSettingViewModel();

        public DeviceSettingViewModel DeviceSettingViewModel { get; set; } = new DeviceSettingViewModel();

        public SystemSettingViewModel SystemSettingViewModel { get; set; } = new SystemSettingViewModel();

        public async void SaveCommandAction()
        {
            SettingViewModelSave();

            await SystemConfig.SaveAsync();

            SettingviewModelLoad();
            SystemConfig.FlyoutSettingViewModelChanged = false;
        }

        private void SettingViewModelSave()
        {
            ParamSettingViewModel.Save();
            DeviceSettingViewModel.Save();
            SystemSettingViewModel.Save();
            CommSettingViewModel.Save();
            ModuleSettingViewModel.Save();
        }

        private void SettingviewModelLoad()
        {
            ParamSettingViewModel.Load();
            DeviceSettingViewModel.Load();
            SystemSettingViewModel.Load();
            CommSettingViewModel.Load();
            ModuleSettingViewModel.Load();
        }
    }
}
