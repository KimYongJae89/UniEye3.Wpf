using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unieye.WPF.Base.Helpers;
using UniEye.Base.MachineInterface;
using UniScanC.AlgoTask;
using UniScanC.Enums;
using UniScanC.Module;
using WPF.UniScanCM.Enums;
using WPF.UniScanCM.Models;
using WPF.UniScanCM.PLC;

namespace WPF.UniScanCM.Override
{
    public class SystemConfig
    {
        private static SystemConfig _instance;
        public static SystemConfig Instance => _instance ?? (_instance = new SystemConfig());

        public List<string> ModelCategoryList { get; set; } = new List<string>();

        public string BrokerIpAddress { get; set; } = "127.0.0.1";
        public string CMMQTTTopic { get; set; } = "UniscanC.CM";
        public string ResultPath { get; set; }

        public string DatabaseIpAddress { get; set; } = "127.0.0.1";
        public string DatabaseName { get; set; } = "UniScan";
        public string DatabaseUserName { get; set; } = "postgres";
        public string DatabasePassword { get; set; } = "masterkey";

        public List<InspectModuleInfo> ImModuleList { get; set; } = new List<InspectModuleInfo>();
        public AlgoTaskManagerSettingDictionary AlgoTaskManagerSettingDictionary { get; private set; } = new AlgoTaskManagerSettingDictionary();

        public string ThicknessModuleTopic { get; set; } = "UniscanC.TM";
        public string ThicknessModuleIP { get; set; } = "127.0.0.1";

        public string GlossModuleTopic { get; set; } = "UniscanC.GM";
        public string GlossModuleIP { get; set; } = "127.0.0.1";

        public string EncoderPort { get; set; } = "COM1";
        public string EncoderModel { get; set; } = "SerialV105";
        public bool UseEncoderSpeed { get; set; } = false;

        public int UIUpdateDelay { get; set; } = 1000;
        public int HeartBeatSignalDuration { get; set; } = 500;

        public int DefectCountTartgetLengthM { get; set; } = 1000;

        public bool UseAutoLotNo { get; set; } = false;
        public string DeviceCode { get; set; } = "";
        public string WorkplaceCode { get; set; } = "";

        public EPlcType PlcType { get; set; }
        public MachineIfSetting MachineIfSetting { get; set; }

        public string LastLotNo { get; set; } = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        public float TargetSpeed { get; set; } = 10.0f;
        public ExportOptionModel ExportOptionModel { get; set; } = new ExportOptionModel();

        // DIO Out Port
        public int DOAirPressure { get; set; } = -1;
        public int DODefectSignal { get; set; } = -1;
        public int DOHeartSignal { get; set; } = -1;

        public int DOTowerLampRed { get; set; } = -1;
        public int DOTowerLampYellow { get; set; } = -1;
        public int DOTowerLampGreen { get; set; } = -1;
        public int DOTowerLampBuzzer { get; set; } = -1;

        public int DOLabelReady { get; set; } = -1;
        public int DOLabelPublish { get; set; } = -1;
        public int DOLabelReset { get; set; } = -1;

        // DIO In Port
        public int DIStart { get; set; } = -1;
        public int DIDefectOccured { get; set; } = -1;
        public int DIDefectReset { get; set; } = -1;
        public int DILabelRun { get; set; } = -1;
        public int DILabelError { get; set; } = -1;
        public int DILabelEmpty { get; set; } = -1;

        public int AlarmDetectLengthRangeM { get; set; } = 1;
        public int AlarmLineDefectHeightMm { get; set; } = 50;
        public int AlarmDefectCount { get; set; } = 5;

        public bool IsShowOthersDefect { get; set; } = true;
        public bool IsInspectPattern { get; set; } = false;

        public bool UseIMDefectAlarm { get; set; }
        public bool UseCustomDefectAlarm { get; set; }

        public bool UseAutoStart { get; set; } = true;
        public bool UseAutoStop { get; set; } = true;

        public bool UseDefectAlarm { get; set; } = false;
        public bool UseDefectCount { get; set; } = true;
        public bool UseInspectModule { get; set; } = false;
        public bool UseThicknessModule { get; set; } = false;
        public bool UseGlossModule { get; set; }
        public bool UseHeartBeat { get; set; } = false;
        public bool UseIO { get; set; } = false;
        public bool UseEncoder { get; set; } = false;
        public bool UsePLC { get; set; } = false;

        public ECustomer Customer { get; set; } = ECustomer.General;

        public SystemConfig()
        {
            string directory = Directory.GetCurrentDirectory();
            ResultPath = $@"{directory}\..\Result";
        }

        private void AdditionalLoad()
        {
            // PLC 통신 항목이 달라졌을 경우를 대비한 코드
            if (_instance.MachineIfSetting != null)
            {
                foreach (Type itemInfoType in _instance.MachineIfSetting.MachineIfItemInfoList.ItemInfoTypeList)
                {
                    foreach (Enum e in Enum.GetValues(itemInfoType))
                    {
                        if (!_instance.MachineIfSetting.MachineIfItemInfoList.DicList.Exists(x => x.Name == e.ToString()))
                        {
                            MachineIfItemInfo newItemInfo = _instance.MachineIfSetting.CreateItemInfo(e);
                            _instance.MachineIfSetting.MachineIfItemInfoList.DicList.Add(newItemInfo);
                        }
                    }
                }
                _instance.MachineIfSetting.MachineIfItemInfoList.ConvertListToDic();
            }
        }

        public async void Save()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "System.cfg");
            string writeString = await Json.StringifyAsync(_instance, null);
            File.WriteAllText(cfgPath, writeString);
        }

        public async void Load()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "System.cfg");
            if (File.Exists(cfgPath) == false)
            {
                Save();
                return;
            }

            string readString = File.ReadAllText(cfgPath);
            _instance = await Json.ToObjectAsync<SystemConfig>(readString);

            if (_instance.ResultPath != null)
            {
                if (!Directory.Exists(_instance.ResultPath))
                {
                    Directory.CreateDirectory(_instance.ResultPath);
                }
            }

            AdditionalLoad();
        }

        public async Task LoadAsync()
        {
            string cfgPath = Path.Combine(BaseConfig.Instance().ConfigPath, "System.cfg");
            if (File.Exists(cfgPath) == false)
            {
                Save();
                return;
            }

            string readString = File.ReadAllText(cfgPath);
            _instance = await Json.ToObjectAsync<SystemConfig>(readString);

            if (_instance.ResultPath != null)
            {
                if (!Directory.Exists(_instance.ResultPath))
                {
                    Directory.CreateDirectory(_instance.ResultPath);
                }
            }

            AdditionalLoad();
        }
    }
}
