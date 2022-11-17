using Authentication.Core;
using Authentication.Core.Datas;
using Authentication.Core.Enums;
using DynMvp.Base;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Views;
using UniEye.Base.MachineInterface;
using UniEye.Base.MachineInterface.AllenBreadley;
using UniEye.Base.MachineInterface.Melsec;
using UniScanC.AlgoTask;
using UniScanC.Enums;
using UniScanC.Module;
using WPF.UniScanCM.Enums;
using WPF.UniScanCM.MachineIf;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.PLC;
using WPF.UniScanCM.PLC.AllenBreadley;
using WPF.UniScanCM.PLC.Melsec;
using WPF.UniScanCM.Windows.ViewModels;
using WPF.UniScanCM.Windows.Views;

namespace WPF.UniScanCM.Pages.ViewModels
{
    public class CustomSettingViewModel : Observable
    {
        public System.Windows.Input.ICommand ModelCategorySettingCommand { get; }
        public System.Windows.Input.ICommand SearchResultPathCommand { get; }
        public System.Windows.Input.ICommand EncoderSettingCommand { get; }
        public System.Windows.Input.ICommand IOPortSettingCommand { get; }
        public System.Windows.Input.ICommand PLCAddressSettingCommand { get; }
        public System.Windows.Input.ICommand PLCSettingCommand { get; }
        public System.Windows.Input.ICommand SequenceSettingCommand { get; }

        private bool useAutoLotNo;
        public bool UseAutoLotNo
        {
            get => useAutoLotNo;
            set
            {
                Set(ref useAutoLotNo, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use Auto LotNo [{value}]");
                }
            }
        }

        private string deviceCode;
        public string DeviceCode
        {
            get => deviceCode;
            set
            {
                Set(ref deviceCode, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change DeviceCode [{value}]");
                }
            }
        }

        private string workplaceCode;
        public string WorkplaceCode
        {
            get => workplaceCode;
            set
            {
                Set(ref workplaceCode, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change WorkplaceCode [{value}]");
                }
            }
        }

        private string brokerIpAddress;
        public string BrokerIpAddress
        {
            get => brokerIpAddress;
            set
            {
                Set(ref brokerIpAddress, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change MQTT Broker IP Address [{value}]");
                }
            }
        }

        private string cmMQTTTopic;
        public string CMMQTTTopic
        {
            get => cmMQTTTopic;
            set
            {
                Set(ref cmMQTTTopic, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change MQTT Topic [{value}]");
                }
            }
        }

        private string resultPath;
        public string ResultPath
        {
            get => resultPath;
            set
            {
                Set(ref resultPath, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Result Path [{value}]");
                }
            }
        }


        private string databaseIpAddress;
        public string DatabaseIpAddress
        {
            get => databaseIpAddress;
            set
            {
                Set(ref databaseIpAddress, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change DB IpAddress [{value}]");
                }
            }
        }

        private string databaseName;
        public string DatabaseName
        {
            get => databaseName;
            set
            {
                Set(ref databaseName, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change DB Name [{value}]");
                }
            }
        }

        private string databaseUserName;
        public string DatabaseUserName
        {
            get => databaseUserName;
            set
            {
                Set(ref databaseUserName, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change DB User Name [{value}]");
                }
            }
        }

        private string databasePassword;
        public string DatabasePassword
        {
            get => databasePassword;
            set
            {
                Set(ref databasePassword, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change DB User Password [{value}]");
                }
            }
        }


        private int imCount;
        public int ImCount
        {
            get => imCount;
            set
            {
                if (Set(ref imCount, value))
                {
                    if (!OnUpdatePage)
                    {
                        LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change IM Count [{value}]");
                    }

                    if (InspectModuleInfoList == null || InspectModuleInfoList.Count == value)
                    {
                        return;
                    }

                    if (AlgoTaskManagerSettingDictionary.Count != InspectModuleInfoList.Count)
                    {
                        InspectModuleInfoList.Clear();
                        AlgoTaskManagerSettingDictionary.Clear();
                    }

                    if (value == 0)
                    {
                        InspectModuleInfoList.Clear();
                        AlgoTaskManagerSettingDictionary.Clear();
                    }
                    else if (InspectModuleInfoList.Count > value)
                    // 삭제
                    {
                        int remCount = Math.Abs(InspectModuleInfoList.Count - value);
                        for (int i = 0; i < remCount; i++)
                        {
                            InspectModuleInfo info = InspectModuleInfoList.Last();
                            InspectModuleInfoList.Remove(info);
                            AlgoTaskManagerSettingDictionary.Remove(AlgoTaskManagerSettingDictionary.Last().Key);
                            //if (AlgoTaskManagerSettingDictionary.Count > info.ModuleNo)
                            //{
                            //    string key = AlgoTaskManagerSettingDictionary.ElementAt(info.ModuleNo).Key;
                            //    AlgoTaskManagerSettingDictionary.Remove(key);
                            //}
                        }
                    }
                    else if (InspectModuleInfoList.Count < value)
                    // 추가
                    {
                        int addCount = Math.Abs(InspectModuleInfoList.Count - value);
                        for (int i = 0; i < addCount; i++)
                        {
                            var info = new InspectModuleInfo()
                            {
                                ModuleNo = InspectModuleInfoList.Count,
                                ModuleTopic = string.Format("{0}", InspectModuleInfoList.Count)
                            };
                            InspectModuleInfoList.Add(info);
                            AlgoTaskManagerSettingDictionary.Add(info.ModuleTopic, new AlgoTaskManagerSetting());
                        }
                    }
                }
            }
        }

        private ObservableCollection<InspectModuleInfo> inspectModuleInfoList;
        public ObservableCollection<InspectModuleInfo> InspectModuleInfoList
        {
            get => inspectModuleInfoList;
            set => Set(ref inspectModuleInfoList, value);
        }

        private AlgoTaskManagerSettingDictionary algoTaskManagerSettingDictionary;
        public AlgoTaskManagerSettingDictionary AlgoTaskManagerSettingDictionary
        {
            get => algoTaskManagerSettingDictionary;
            set => Set(ref algoTaskManagerSettingDictionary, value);
        }


        private string thicknessModuleTopic;
        public string ThicknessModuleTopic
        {
            get => thicknessModuleTopic;
            set => Set(ref thicknessModuleTopic, value);
        }

        private string thicknessModuleIP;
        public string ThicknessModuleIP
        {
            get => thicknessModuleIP;
            set => Set(ref thicknessModuleIP, value);
        }

        private string glossModuleTopic;
        public string GlossModuleTopic
        {
            get => glossModuleTopic;
            set => Set(ref glossModuleTopic, value);
        }

        private string glossModuleIP;
        public string GlossModuleIP
        {
            get => glossModuleIP;
            set => Set(ref glossModuleIP, value);
        }

        private ObservableCollection<string> serialPortList;
        public ObservableCollection<string> SerialPortList
        {
            get => serialPortList;
            set => Set(ref serialPortList, value);
        }

        private ObservableCollection<string> encoderModelList;
        public ObservableCollection<string> EncoderModelList
        {
            get => encoderModelList;
            set => Set(ref encoderModelList, value);
        }

        private string encoderPort;
        public string EncoderPort
        {
            get => encoderPort;
            set
            {
                Set(ref encoderPort, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Encoder Port [{value}]");
                }
            }
        }

        private string encoderModel;
        public string EncoderModel
        {
            get => encoderModel;
            set
            {
                Set(ref encoderModel, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Encoder Model [{value}]");
                }
            }
        }


        private int uiUpdateDelay = 500;
        public int UIUpdateDelay
        {
            get => uiUpdateDelay;
            set
            {
                Set(ref uiUpdateDelay, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change UI Delay [{value}]");
                }
            }
        }

        private int heartBeatSignalDuration = 500;
        public int HeartBeatSignalDuration
        {
            get => heartBeatSignalDuration;
            set
            {
                Set(ref heartBeatSignalDuration, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Heart Beat Signal Duration [{value}]");
                }
            }
        }


        private MachineIfSetting machineIfSetting;
        public MachineIfSetting MachineIfSetting
        {
            get => machineIfSetting;
            set => Set(ref machineIfSetting, value);
        }

        private EPlcType plcType = EPlcType.None;
        public EPlcType PlcType
        {
            get => plcType;
            set
            {
                if (Set(ref plcType, value))
                {
                    if (!OnUpdatePage)
                    {
                        LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Plc Type [{value}]");
                    }

                    // PLC 최초 설정 시 필요한 항목 (이미 선택한 항목이 있다면 크게 의미는 없음)
                    MachineIfItemInfoList.Set(new MachineIfItemInfoListCM());

                    switch (value)
                    {
                        case EPlcType.Melsec:
                            MachineIfSetting = UniEye.Base.MachineInterface.MachineIfSetting.Create(EMachineIfType.Melsec);
                            break;
                        case EPlcType.AllenBreadley:
                            MachineIfSetting = UniEye.Base.MachineInterface.MachineIfSetting.Create(EMachineIfType.AllenBreadley);
                            break;
                        case EPlcType.None:
                        default:
                            MachineIfSetting = null;
                            break;
                    }
                }
            }
        }

        private int alarmDetectLengthRangeM;
        public int AlarmDetectLengthRangeM
        {
            get => alarmDetectLengthRangeM;
            set
            {
                Set(ref alarmDetectLengthRangeM, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Detect Length Range M [{value}]");
                }
            }
        }

        private int alarmLineDefectHeightMm;
        public int AlarmLineDefectHeightMm
        {
            get => alarmLineDefectHeightMm;
            set
            {
                Set(ref alarmLineDefectHeightMm, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Line Defect Height Mm [{value}]");
                }
            }
        }

        private int alarmDefectCount;
        public int AlarmDefectCount
        {
            get => alarmDefectCount;
            set
            {
                Set(ref alarmDefectCount, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Alarm Defect Count [{value}]");
                }
            }
        }

        private bool isShowOthersDefect = false;
        public bool IsShowOthersDefect
        {
            get => isShowOthersDefect;
            set
            {
                Set(ref isShowOthersDefect, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Is Show Others Defect [{value}]");
                }
            }
        }

        private bool isInspectPattern = false;
        public bool IsInspectPattern
        {
            get => isInspectPattern;
            set
            {
                Set(ref isInspectPattern, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Is Inspect Pattern [{value}]");
                }
            }
        }

        private bool useIMDefectAlarm = false;
        public bool UseIMDefectAlarm
        {
            get => useIMDefectAlarm;
            set
            {
                Set(ref useIMDefectAlarm, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use IM Defect Alarm [{value}]");
                }
            }
        }

        private bool useCustomDefectAlarm = false;
        public bool UseCustomDefectAlarm
        {
            get => useCustomDefectAlarm;
            set
            {
                Set(ref useCustomDefectAlarm, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use Custom Defect Alarm [{value}]");
                }
            }
        }

        private bool useAutoStart = true;
        public bool UseAutoStart
        {
            get => useAutoStart;
            set
            {
                Set(ref useAutoStart, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use Auto Start [{value}]");
                }
            }
        }

        private bool useAutoStop = true;
        public bool UseAutoStop
        {
            get => useAutoStop;
            set
            {
                Set(ref useAutoStop, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use Auto Stop [{value}]");
                }
            }
        }

        private bool useDefectAlarm = true;
        public bool UseDefectAlarm
        {
            get => useDefectAlarm;
            set
            {
                Set(ref useDefectAlarm, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use Defect Alarm [{value}]");
                }
            }
        }

        private bool useInspectModule = false;
        public bool UseInspectModule
        {
            get => useInspectModule;
            set
            {
                Set(ref useInspectModule, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use Inspect Module [{value}]");
                }
            }
        }

        private bool useThicknessModule = false;
        public bool UseThicknessModule
        {
            get => useThicknessModule;
            set
            {
                Set(ref useThicknessModule, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use Thickness Module [{value}]");
                }
            }
        }

        private bool useGlossModule = false;
        public bool UseGlossModule
        {
            get => useGlossModule;
            set
            {
                Set(ref useGlossModule, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use Gloss Module [{value}]");
                }
            }
        }

        private bool useLabelMarker = false;
        public bool UseLabelMarker
        {
            get => useLabelMarker;
            set
            {
                Set(ref useLabelMarker, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use Label Marker [{value}]");
                }
            }
        }

        private bool useHeartBeat = false;
        public bool UseHeartBeat
        {
            get => useHeartBeat;
            set
            {
                Set(ref useHeartBeat, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use Heart Beat [{value}]");
                }
            }
        }

        private bool useIO = false;
        public bool UseIO
        {
            get => useIO;
            set
            {
                Set(ref useIO, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use IO [{value}]");
                }
            }
        }

        private bool useEncoder = false;
        public bool UseEncoder
        {
            get => useEncoder;
            set
            {
                Set(ref useEncoder, value);
                var deviceManager = DeviceManager.Instance() as DeviceManager;
                if (!OnUpdatePage)
                {
                    if (value)
                    {
                        deviceManager.InitEncoder();
                    }
                    else
                    {
                        deviceManager.DisconnectSerialEncoder();
                    }

                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use Encoder [{value}]");
                }
            }
        }

        private bool usePLC = false;
        public bool UsePLC
        {
            get => usePLC;
            set
            {
                Set(ref usePLC, value);
                if (!OnUpdatePage)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Use PLC [{value}]");
                }
            }
        }


        private bool isGeneralAuthorized = false;
        public bool IsGeneralAuthorized
        {
            get => isGeneralAuthorized;
            set => Set(ref isGeneralAuthorized, value);
        }

        private bool isDetailAuthorized = false;
        public bool IsDetailAuthorized
        {
            get => isDetailAuthorized;
            set => Set(ref isDetailAuthorized, value);
        }

        private bool isDeviceAuthorized = false;
        public bool IsDeviceAuthorized
        {
            get => isDeviceAuthorized;
            set => Set(ref isDeviceAuthorized, value);
        }

        private bool isTaskAuthorized = false;
        public bool IsTaskAuthorized
        {
            get => isTaskAuthorized;
            set => Set(ref isTaskAuthorized, value);
        }

        private bool isCustomerSamsung = false;
        public bool IsCustomerSamsung
        {
            get => isCustomerSamsung;
            set => Set(ref isCustomerSamsung, value);
        }

        private bool OnUpdatePage { get; set; } = false;

        public CustomSettingViewModel()
        {
            UpdateCustomerSetting();
            UpdateConfig();

            UserHandler.Instance.OnUserChanged += OnUserChanged;

            ModelCategorySettingCommand = new RelayCommand(async () =>
            {
                var view = new ModelCategorySettingWindowView();
                if (await MessageWindowHelper.ShowChildWindow<bool>(view) == true)
                {

                }
            });

            SearchResultPathCommand = new RelayCommand(() =>
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.SelectedPath = ResultPath;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ResultPath = dialog.SelectedPath;
                }
            });

            EncoderSettingCommand = new RelayCommand(async () =>
            {
                var devMgr = DeviceManager.Instance() as DeviceManager;

                if (devMgr.IsConnectSerialEncoder())
                {
                    devMgr.DisconnectSerialEncoder();
                }

                devMgr.InitEncoder();
                if (devMgr.ConnectSerialEncoder(EncoderPort))
                {
                    var view = new EncoderSettingWindow(devMgr.SerialEncoder);
                    if (await MessageWindowHelper.ShowChildWindow<bool>(view) == true)
                    {

                    }
                }
                else
                {
                    await MessageWindowHelper.ShowMessageBox("Encoder connection error", "Unable to connect to the encoder.", System.Windows.MessageBoxButton.OK);
                }
            });

            IOPortSettingCommand = new RelayCommand(async () =>
            {
                var view = new IOPortSettingWindowView();
                if (await MessageWindowHelper.ShowChildWindow<bool>(view) == true)
                {

                }
            });

            PLCAddressSettingCommand = new RelayCommand(async () =>
            {
                ChildWindow view = null;
                switch (PlcType)
                {
                    case EPlcType.Melsec:
                        view = new MelsecAddressSettingWindowView();
                        break;
                    case EPlcType.AllenBreadley:
                        view = new AllenBreadleyAddressSettingWindowView();
                        break;
                    case EPlcType.None:
                    default: return;
                }

                view.DataContext = new AddressSettingWindowViewModel(MachineIfSetting.MachineIfItemInfoList.Clone());
                if (view != null && await MessageWindowHelper.ShowChildWindow<bool>(view) == true)
                {
                    MachineIfSetting.MachineIfItemInfoList.DicList = ((AddressSettingWindowViewModel)view.DataContext).MachineIfItemInfos;
                    MachineIfSetting.MachineIfItemInfoList.ConvertListToDic();
                }
            });

            PLCSettingCommand = new RelayCommand(async () =>
            {
                ChildWindow view = null;
                switch (PlcType)
                {
                    case EPlcType.Melsec:
                        view = new MelsecPLCSettingWindowView();
                        view.DataContext = new PlcSettingWindowViewModel((MelsecMachineIfSetting)MachineIfSetting.Clone());
                        break;
                    case EPlcType.AllenBreadley:
                        view = new AllenBreadleyPLCSettingWindowView();
                        view.DataContext = new PlcSettingWindowViewModel((AllenBreadleyMachineIfSetting)MachineIfSetting.Clone());
                        break;
                    case EPlcType.None:
                    default: return;
                }

                if (await MessageWindowHelper.ShowChildWindow<bool>(view) == true)
                {
                    MachineIfSetting = ((PlcSettingWindowViewModel)view.DataContext).MachineIfSetting.Clone();
                }
            });

            SequenceSettingCommand = new RelayCommand(async () =>
            {
                var view = new AlgoTaskSettingWindowView();
                AlgoTaskManagerSettingDictionary.UpdateModuleName(InspectModuleInfoList.Select(f => f.ModuleTopic));
                view.DataContext = new AlgoTaskSettingWindowViewModel(AlgoTaskManagerSettingDictionary, InspectModuleInfoList.ToList());
                AlgoTaskManagerSettingDictionary result = await MessageWindowHelper.ShowChildWindow<AlgoTaskManagerSettingDictionary>(view);
                if (result != null)
                {
                    AlgoTaskManagerSettingDictionary = result;
                }
            });
        }

        private void UpdateCustomerSetting()
        {
            if (SystemConfig.Instance.Customer == ECustomer.Samsung)
            {
                IsCustomerSamsung = true;
            }
        }

        private void UpdateConfig()
        {
            SystemConfig config = SystemConfig.Instance;

            OnUpdatePage = true;

            UseAutoLotNo = config.UseAutoLotNo;
            DeviceCode = config.DeviceCode;
            WorkplaceCode = config.WorkplaceCode;

            BrokerIpAddress = config.BrokerIpAddress;
            CMMQTTTopic = config.CMMQTTTopic;
            ResultPath = config.ResultPath;

            DatabaseIpAddress = config.DatabaseIpAddress;
            DatabaseName = config.DatabaseName;
            DatabaseUserName = config.DatabaseUserName;
            DatabasePassword = config.DatabasePassword;

            InspectModuleInfoList = new ObservableCollection<InspectModuleInfo>(config.ImModuleList);
            AlgoTaskManagerSettingDictionary = config.AlgoTaskManagerSettingDictionary.Clone();
            ImCount = config.ImModuleList.Count;

            ThicknessModuleTopic = config.ThicknessModuleTopic;
            ThicknessModuleIP = config.ThicknessModuleIP;

            GlossModuleTopic = config.GlossModuleTopic;
            GlossModuleIP = config.GlossModuleIP;

            SerialPortList = new ObservableCollection<string>(System.IO.Ports.SerialPort.GetPortNames());
            EncoderModelList = new ObservableCollection<string> { "SerialV105", "SerialV107" };
            EncoderPort = config.EncoderPort;
            EncoderModel = config.EncoderModel;

            UIUpdateDelay = config.UIUpdateDelay;
            HeartBeatSignalDuration = config.HeartBeatSignalDuration;

            PlcType = config.PlcType;
            config.MachineIfSetting?.MachineIfItemInfoList.ConvertListToDic();
            MachineIfSetting = config.MachineIfSetting;

            AlarmDetectLengthRangeM = config.AlarmDetectLengthRangeM;
            AlarmLineDefectHeightMm = config.AlarmLineDefectHeightMm;
            AlarmDefectCount = config.AlarmDefectCount;

            IsShowOthersDefect = config.IsShowOthersDefect;
            IsInspectPattern = config.IsInspectPattern;

            UseIMDefectAlarm = config.UseIMDefectAlarm;
            UseCustomDefectAlarm = config.UseCustomDefectAlarm;

            UseAutoStart = config.UseAutoStart;
            UseAutoStop = config.UseAutoStop;

            UseDefectAlarm = config.UseDefectAlarm;
            UseInspectModule = config.UseInspectModule;
            UseThicknessModule = config.UseThicknessModule;
            UseGlossModule = config.UseGlossModule;
            UseHeartBeat = config.UseHeartBeat;
            UseIO = config.UseIO;
            UseEncoder = config.UseEncoder;
            UsePLC = config.UsePLC;

            OnUpdatePage = false;
        }

        public void Save()
        {
            // PLC 설정이 바뀔 경우 일단 PLC 연결을 끊음
            (DeviceManager.Instance() as DeviceManager).DisconnectPLC();

            SystemConfig config = SystemConfig.Instance;

            config.UseAutoLotNo = UseAutoLotNo;
            config.DeviceCode = DeviceCode;
            config.WorkplaceCode = WorkplaceCode;

            config.BrokerIpAddress = BrokerIpAddress;
            config.CMMQTTTopic = CMMQTTTopic;
            config.ResultPath = ResultPath;

            config.DatabaseIpAddress = DatabaseIpAddress;
            config.DatabaseName = DatabaseName;
            config.DatabaseUserName = DatabaseUserName;
            config.DatabasePassword = DatabasePassword;

            config.ImModuleList = InspectModuleInfoList.ToList();

            AlgoTaskManagerSettingDictionary.UpdateModuleName(InspectModuleInfoList.Select(f => f.ModuleTopic));
            config.AlgoTaskManagerSettingDictionary.CopyFrom(AlgoTaskManagerSettingDictionary);

            config.ThicknessModuleTopic = ThicknessModuleTopic;
            config.ThicknessModuleIP = ThicknessModuleIP;

            config.GlossModuleTopic = GlossModuleTopic;
            config.GlossModuleIP = GlossModuleIP;

            config.EncoderPort = EncoderPort;
            config.EncoderModel = EncoderModel;

            config.UIUpdateDelay = UIUpdateDelay;
            config.HeartBeatSignalDuration = HeartBeatSignalDuration;

            config.PlcType = PlcType;
            config.MachineIfSetting = MachineIfSetting;
            if (config.MachineIfSetting != null)
            {
                config.MachineIfSetting.MachineIfItemInfoList?.ConvertDicToList();
            }

            config.AlarmDetectLengthRangeM = AlarmDetectLengthRangeM;
            config.AlarmLineDefectHeightMm = AlarmLineDefectHeightMm;
            config.AlarmDefectCount = AlarmDefectCount;

            config.IsShowOthersDefect = IsShowOthersDefect;
            config.IsInspectPattern = IsInspectPattern;

            config.UseIMDefectAlarm = UseIMDefectAlarm;
            config.UseCustomDefectAlarm = UseCustomDefectAlarm;

            config.UseAutoStart = UseAutoStart;
            config.UseAutoStop = UseAutoStop;

            config.UseDefectAlarm = UseDefectAlarm;
            config.UseInspectModule = UseInspectModule;
            config.UseThicknessModule = UseThicknessModule;
            config.UseGlossModule = UseGlossModule;
            config.UseHeartBeat = UseHeartBeat;
            config.UseIO = UseIO;
            config.UseEncoder = UseEncoder;
            config.UsePLC = UsePLC;

            // IM 이 변경될 경우 구독 리스트를 재설정
            (CommManager.Instance() as CommManager).TopicInitialize();
            // PLC 설정이 바뀔 경우 새롭게 다시 연결
            (DeviceManager.Instance() as DeviceManager).InitPLC();

            config.Save();

            UpdateConfig();
        }

        private void OnUserChanged(User user)
        {
            if (user.IsAuth(ERoleType.GeneralSetting))
            {
                IsGeneralAuthorized = true;
            }
            else
            {
                IsGeneralAuthorized = false;
            }

            if (user.IsAuth(ERoleType.DetailSetting))
            {
                IsDetailAuthorized = true;
            }
            else
            {
                IsDetailAuthorized = false;
            }

            if (user.IsAuth(ERoleType.DeviceSetting))
            {
                IsDeviceAuthorized = true;
            }
            else
            {
                IsDeviceAuthorized = false;
            }

            if (user.IsAuth(ERoleType.TaskSetting))
            {
                IsTaskAuthorized = true;
            }
            else
            {
                IsTaskAuthorized = false;
            }
        }
    }
}
