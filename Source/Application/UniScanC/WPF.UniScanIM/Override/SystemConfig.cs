using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Unieye.WPF.Base.Extensions;
using Unieye.WPF.Base.Helpers;
using UniScanC.Data;
using UniScanC.Enums;
using WPF.UniScanIM.Helpers;

namespace WPF.UniScanIM.Override
{
    public enum DefectSignalNonOverlayMode
    {
        Time, Distance
    }

    public delegate void SelectedModuleInfoChanged(ModuleInfo SelectedModuleInfo);
    public delegate void SaveSelectedSourceImageDelegate(ModuleInfo SelectedModuleInfo, string file);

    public class SystemConfig : Observable
    {
        private const string key = "SysConfig";

        private static SystemConfig instance;
        public static SystemConfig Instance => instance ?? (instance = new SystemConfig());

        //public bool SectionTestMode { get; set; } = false;
        //public double SectionSizeMM { get; set; } = 20;
        //public int SectionCountEA { get; set; } = 3;
        //public int SectionPositionMM { get; set; } = 120;
        public float LineSpeed { get; set; } = 10; // m/min
        public int InspectionFrameLengthmm { get; set; } = 200; //mm

        //UI Save버튼 색 업데이트에 사용됨. 파라미터 변경 여부를 판단하는 플래그
        private bool flayoutSettingViewModelChanged = false;
        [JsonIgnore]
        public bool FlyoutSettingViewModelChanged
        {
            get => flayoutSettingViewModelChanged;
            set => Set(ref flayoutSettingViewModelChanged, value);
        }

        #region ParamSettingViewModel
        private int signalDuration = 100;
        public int SignalDuration
        {
            get => signalDuration;
            set => Set(ref signalDuration, value);
        }

        private double signalTimeDelayMs = 0.0D;
        public double SignalTimeDelayMs
        {
            get => signalTimeDelayMs;
            set => Set(ref signalTimeDelayMs, value);
        }

        private double signalDistanceM = 0.0D;
        public double SignalDistanceM
        {
            get => signalDistanceM;
            set => Set(ref signalDistanceM, value);
        }

        private DefectSignalNonOverlayMode defectSignalNonOverlayMode = DefectSignalNonOverlayMode.Time;
        public DefectSignalNonOverlayMode DefectSignalNonOverlayMode
        {
            get => defectSignalNonOverlayMode;
            set => Set(ref defectSignalNonOverlayMode, value);
        }

        private double signalNonOverlayTimeMs = 0.0D;
        public double SignalNonOverlayTimeMs
        {
            get => signalNonOverlayTimeMs;
            set => Set(ref signalNonOverlayTimeMs, value);
        }

        private double signalNonOverlayDistanceM = 0.0D;
        public double SignalNonOverlayDistanceM
        {
            get => signalNonOverlayDistanceM;
            set => Set(ref signalNonOverlayDistanceM, value);
        }

        private bool mergeBorderDefects = false;
        public bool MergeBorderDefects
        {
            get => mergeBorderDefects;
            set => Set(ref mergeBorderDefects, value);
        }
        #endregion

        #region ModuleSettingView
        //public ObservableCollection<ModuleInfo> ModuleList { get; set; } = new ObservableCollection<ModuleInfo>();
        private ObservableCollection<ModuleInfo> moduleList = new ObservableCollection<ModuleInfo>();
        public ObservableCollection<ModuleInfo> ModuleList
        {
            get => moduleList;
            set => Set(ref moduleList, value);
        }
        #endregion

        #region CommSettingViewModel
        private string iMIpAddress = "127.0.0.1";
        public string IMIpAddress
        {
            get => iMIpAddress;
            set => Set(ref iMIpAddress, value);
        }

        private string cMDBIpAddress = "127.0.0.1";
        public string CMDBIpAddress
        {
            get => cMDBIpAddress;
            set => Set(ref cMDBIpAddress, value);
        }

        private string cMDBUserName = "postgres";
        public string CMDBUserName
        {
            get => cMDBUserName;
            set => Set(ref cMDBUserName, value);
        }

        private string cMDBPassword = "masterkey";
        public string CMDBPassword
        {
            get => cMDBPassword;
            set => Set(ref cMDBPassword, value);
        }

        private string cMMQTTIpAddress = "127.0.0.1";
        public string CMMQTTIpAddress
        {
            get => cMMQTTIpAddress;
            set => Set(ref cMMQTTIpAddress, value);
        }

        private string cMTopicName = "UniscanC.CM";
        public string CMTopicName
        {
            get => cMTopicName;
            set => Set(ref cMTopicName, value);
        }

        private string cMNetworkIpAddress = "127.0.0.1";
        public string CMNetworkIpAddress
        {
            get => cMNetworkIpAddress;
            set => Set(ref cMNetworkIpAddress, value);
        }

        private string cMNetworkUserName = "user";
        public string CMNetworkUserName
        {
            get => cMNetworkUserName;
            set => Set(ref cMNetworkUserName, value);
        }

        private string cMNetworkPassword = "admin1111";
        public string CMNetworkPassword
        {
            get => cMNetworkPassword;
            set => Set(ref cMNetworkPassword, value);
        }
        #endregion

        #region DeviceSettingViewModel
        private int dIFrameTriggerSignal = -1;
        public int DIFrameTriggerSignal
        {
            get => dIFrameTriggerSignal;
            set => Set(ref dIFrameTriggerSignal, value);
        }

        private int dODefectSignal = -1;
        public int DODefectSignal
        {
            get => dODefectSignal;
            set => Set(ref dODefectSignal, value);
        }
        #endregion

        #region SystemSettingViewModel
        private double windowWidth = 1000;
        public double WindowWidth
        {
            get => windowWidth;
            set => Set(ref windowWidth, value);
        }

        private double windowHeight = 800;
        public double WindowHeight
        {
            get => windowHeight;
            set => Set(ref windowHeight, value);
        }

        private bool isSaveFrameImage = false;
        public bool IsSaveFrameImage
        {
            get => isSaveFrameImage;
            set => Set(ref isSaveFrameImage, value);
        }

        private bool isSaveDefectImage = true;
        public bool IsSaveDefectImage
        {
            get => isSaveDefectImage;
            set => Set(ref isSaveDefectImage, value);
        }

        private bool isSaveDebugData = false;
        public bool IsSaveDebugData
        {
            get => isSaveDebugData;
            set => Set(ref isSaveDebugData, value);
        }

        private int isSaveDebugDataCount = 0;
        public int IsSaveDebugDataCount
        {
            get => isSaveDebugDataCount;
            set => Set(ref isSaveDebugDataCount, value);
        }

        private bool overrideATMSetting = false;
        [JsonIgnore]
        public bool OverrideATMSetting
        {
            get => overrideATMSetting;
            set => Set(ref overrideATMSetting, value);
        }
        #endregion


        private ModuleInfo selectedModuleInfo;
        [JsonIgnore]
        public ModuleInfo SelectedModuleInfo
        {
            get => selectedModuleInfo;
            set
            {
                selectedModuleInfo = value;
                SelectedModuleInfoChangedDelegate?.Invoke(value);
            }
        }

        [JsonIgnore]
        public SelectedModuleInfoChanged SelectedModuleInfoChangedDelegate { get; set; }

        [JsonIgnore]
        public SaveSelectedSourceImageDelegate SaveSelectedSourceImage { get; set; }

        public async Task SaveAsync()
        {
            var directoryInfo = new DirectoryInfo(BaseConfig.Instance().ConfigPath);
            await directoryInfo.SaveAsync<SystemConfig>(key, instance, new CameraJsonConverter());
        }

        public async Task LoadAsync()
        {
            var directoryInfo = new DirectoryInfo(BaseConfig.Instance().ConfigPath);
            instance = await directoryInfo.ReadAsync<SystemConfig>(key, new CameraJsonConverter()) ?? new SystemConfig();
        }
    }
}
