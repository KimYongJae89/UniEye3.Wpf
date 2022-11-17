using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.ViewModels
{
    public class ParamSettingViewModel : SystemConfigSettingViewModel
    {
        private int signalDuration = 100;
        public int SignalDuration
        {
            get => signalDuration;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref signalDuration, value);
        }

        private double signalTimeDelayMs = 0.0D;
        public double SignalTimeDelayMs
        {
            get => signalTimeDelayMs;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref signalTimeDelayMs, value);
        }

        private double signalDistanceM = 0.0D;
        public double SignalDistanceM
        {
            get => signalDistanceM;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref signalDistanceM, value);
        }

        private DefectSignalNonOverlayMode defectSignalNonOverlayMode = DefectSignalNonOverlayMode.Time;
        public DefectSignalNonOverlayMode DefectSignalNonOverlayMode
        {
            get => defectSignalNonOverlayMode;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref defectSignalNonOverlayMode, value);
        }

        private double signalNonOverlayTimeMs = 0.0D;
        public double SignalNonOverlayTimeMs
        {
            get => signalNonOverlayTimeMs;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref signalNonOverlayTimeMs, value);
        }

        public double signalNonOverlayDistanceM = 0.0D;
        public double SignalNonOverlayDistanceM
        {
            get => signalNonOverlayDistanceM;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref signalNonOverlayDistanceM, value);
        }

        private bool mergeBorderDefects = false;
        public bool MergeBorderDefects
        {
            get => mergeBorderDefects;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref mergeBorderDefects, value);
        }
    }
}
