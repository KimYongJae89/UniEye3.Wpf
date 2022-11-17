using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.ViewModels
{
    public class DeviceSettingViewModel : SystemConfigSettingViewModel
    {
        private int dIFrameTriggerSignal = -1;
        public int DIFrameTriggerSignal
        {
            get => dIFrameTriggerSignal;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref dIFrameTriggerSignal, value);
        }

        private int dODefectSignal = -1;
        public int DODefectSignal
        {
            get => dODefectSignal;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref dODefectSignal, value);
        }
    }
}
