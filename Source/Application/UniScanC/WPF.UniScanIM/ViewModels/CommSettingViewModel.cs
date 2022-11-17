using WPF.UniScanIM.Override;

namespace WPF.UniScanIM.ViewModels
{
    public class CommSettingViewModel : SystemConfigSettingViewModel
    {
        private string iMIpAddress = "127.0.0.1";
        public string IMIpAddress
        {
            get => iMIpAddress;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref iMIpAddress, value);
        }

        private string cMDBIpAddress = "127.0.0.1";
        public string CMDBIpAddress
        {
            get => cMDBIpAddress;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref cMDBIpAddress, value);
        }

        private string cMDBUserName = "postgres";
        public string CMDBUserName
        {
            get => cMDBUserName;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref cMDBUserName, value);
        }

        private string cMDBPassword = "masterkey";
        public string CMDBPassword
        {
            get => cMDBPassword;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref cMDBPassword, value);
        }

        private string cMMQTTIpAddress = "127.0.0.1";
        public string CMMQTTIpAddress
        {
            get => cMMQTTIpAddress;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref cMMQTTIpAddress, value);
        }

        private string cMTopicName = "UniscanC.CM";
        public string CMTopicName
        {
            get => cMTopicName;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref cMTopicName, value);
        }

        private string cMNetworkIpAddress = "127.0.0.1";
        public string CMNetworkIpAddress
        {
            get => cMNetworkIpAddress;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref cMNetworkIpAddress, value);
        }

        private string cMNetworkUserName = "user";
        public string CMNetworkUserName
        {
            get => cMNetworkUserName;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref cMNetworkUserName, value);
        }

        private string cMNetworkPassword = "admin1111";
        public string CMNetworkPassword
        {
            get => cMNetworkPassword;
            set => SystemConfig.FlyoutSettingViewModelChanged |= Set(ref cMNetworkPassword, value);
        }
    }
}
