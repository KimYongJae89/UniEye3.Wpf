using Authentication.Core;
using Authentication.Core.Datas;
using Authentication.Core.Enums;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Spectrometer;
using DynMvp.Devices.Spectrometer.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Unieye.WPF.Base.Helpers;
using UniScanC.Models;
using WPF.ThicknessMeasure.Override;

namespace WPF.ThicknessMeasure.Pages.ViewModels
{
    public class CustomSettingViewModel : Observable
    {
        #region 필드
        private Dictionary<string, ObservableCollection<LayerParam>> layerParamList = new Dictionary<string, ObservableCollection<LayerParam>>();
        private ObservableValue<string> selectedLayerName;
        private ObservableCollection<LayerParam> selectedLayerParamList;
        private LayerParam selectedLayerParam;
        private ObservableCollection<ScanWidth> scanWidthList = new ObservableCollection<ScanWidth>();
        private ScanWidth selectedScanWidth;
        private List<DynMvp.Devices.Spectrometer.SpectrometerInfo> infoList;
        private bool useRefraction;
        private DynMvp.Devices.Spectrometer.SpectrometerInfo angleDevice;
        private float angleValue;
        private DynMvp.Devices.Spectrometer.SpectrometerInfo selectedInfo;
        private int integrationTime;
        private int average;
        private int boxcar;
        private int threshold;
        private SpectrometerProperty spectrometerProperty;
        private MelsecInfo melsecInfo;
        private float backGroundPos;
        private float referencePos;
        private float masterSamplePos;
        private double homeStartSpeed = 100;
        private double homeEndSpeed = 10;
        private double jogSpeed = 200;
        private double movingSpeed = 200;
        private double measureSecond = 6;
        private float positionOffset = 0f;
        private float sensorOffset = 0f;
        private MotionPosition motionPosition;
        private int towerLampRed = 16;
        private int towerLampYellow = 17;
        private int towerLampGreen = 18;
        private int towerLampBuzzer = 19;
        private int halogenLampVIS = 20;
        private int halogenLampNIR = 21;
        private int emergencyFront = 17;
        private int emergencyBack = 18;
        private int areaSensor = 19;
        private double sample = 0;
        private double sampleRange = 0;
        private int movingAverageCount = 0;
        private SystemType systemType = new SystemType();
        private HeadType headType = new HeadType();
        private ObservableCollection<ObservableValue<string>> layerNameList = new ObservableCollection<ObservableValue<string>>();
        private ObservableValue<string> layerName;
        private string resultPath;

        private ICommand addLayerParamCommand;
        private ICommand deleteLayerParamCommand;
        private ICommand addModelWidthCommand;
        private ICommand deleteModelWidthCommand;
        private ICommand addLayerCommand;
        private ICommand deleteLayerCommand;
        #endregion

        #region 생성자
        public CustomSettingViewModel()
        {
            SystemConfig config = SystemConfig.Instance;

            // Model Param
            BindingOperations.EnableCollectionSynchronization(LayerParamList, new object());
            foreach (KeyValuePair<string, List<LayerParam>> layerParam in config.LayerParamList)
            {
                LayerParamList.Add(layerParam.Key, new ObservableCollection<LayerParam>(layerParam.Value));
            }
            //if (LayerParamList.Count > 0)
            //    SelectedLayerName = new ObservableValue<string>(LayerParamList.First().Key);

            // Scan Width
            BindingOperations.EnableCollectionSynchronization(ScanWidthList, new object());
            foreach (ScanWidth width in config.ScanWidthList)
            {
                ScanWidthList.Add(width.Clone());
            }

            // Spectrometer
            SpectrometerProperty = config.SpectrometerProperty;
            InfoList = Spectrometer.DeviceList.Values.ToList();

            UseRefraction = config.UseRefraction;

            if (Spectrometer.DeviceList.ContainsKey(config.ReflectionSpectrometer))
            {
                AngleDevice = Spectrometer.DeviceList[config.ReflectionSpectrometer];
            }

            AngleValue = config.AngleValue;
            //if (InfoList.Count > 0)
            //    SelectedInfo = InfoList.First();

            // Machine
            MelsecInfo = config.MelsecInfo;
            BackGroundPos = config.BackGroundPos[0];
            ReferencePos = config.ReferencePos[0];
            MasterSamplePos = config.MasterSamplePos[0];
            HomeStartSpeed = config.HomeStartSpeed;
            HomeEndSpeed = config.HomeEndSpeed;
            JogSpeed = config.JogSpeed;
            MovingSpeed = config.MovingSpeed;
            MeasureSecond = config.MeasureSecond;
            PositionOffset = config.PositionOffset;
            SensorOffset = config.SensorOffset;

            // PortMap Out
            TowerLampRed = config.TowerLampRed;
            TowerLampYellow = config.TowerLampYellow;
            TowerLampGreen = config.TowerLampGreen;
            TowerLampBuzzer = config.TowerLampBuzzer;
            HalogenLampVIS = config.HalogenLampVIS;
            HalogenLampNIR = config.HalogenLampNIR;

            // PortMap In
            EmergencyFront = config.EmergencyFront;
            EmergencyBack = config.EmergencyBack;
            AreaSensor = config.AreaSensor;

            // Data
            Sample = config.Sample;
            SampleRange = config.SampleRange;
            MovingAverageCount = config.MovingAverageCount;

            // System Setting
            SystemType = config.SystemType;
            HeadType = config.HeadType;
            BindingOperations.EnableCollectionSynchronization(LayerNameList, new object());
            foreach (string str in config.SpectrometerProperty.LayerNameList)
            {
                LayerNameList.Add(new ObservableValue<string>(str));
            }

            ResultPath = config.ResultPath;

            UserHandler.Instance.OnUserChanged += UserChanged;
        }
        #endregion

        #region 속성
        public bool HighLevelUser => UserHandler.Instance.CurrentUser?.IsAuth(ERoleType.DetailSetting) == true;

        public Dictionary<string, ObservableCollection<LayerParam>> LayerParamList { get => layerParamList; set => Set(ref layerParamList, value); }

        public ObservableValue<string> SelectedLayerName
        {
            get => selectedLayerName;
            set
            {
                Set(ref selectedLayerName, value);
                if (LayerParamList.ContainsKey(SelectedLayerName.Value) == false)
                {
                    LayerParamList.Add(SelectedLayerName.Value, new ObservableCollection<LayerParam>());
                }

                SelectedLayerParamList = LayerParamList[SelectedLayerName.Value];
            }
        }

        public ObservableCollection<LayerParam> SelectedLayerParamList { get => selectedLayerParamList; set => Set(ref selectedLayerParamList, value); }

        public LayerParam SelectedLayerParam { get => selectedLayerParam; set => Set(ref selectedLayerParam, value); }

        public ObservableCollection<ScanWidth> ScanWidthList { get => scanWidthList; set => Set(ref scanWidthList, value); }

        public ScanWidth SelectedScanWidth { get => selectedScanWidth; set => Set(ref selectedScanWidth, value); }

        public DynMvp.Devices.Spectrometer.Spectrometer Spectrometer => ((DeviceManager)DeviceManager.Instance()).Spectrometer;

        public List<DynMvp.Devices.Spectrometer.SpectrometerInfo> InfoList { get => infoList; set => Set(ref infoList, value); }

        public bool UseRefraction
        {
            get => useRefraction;
            set
            {
                Set(ref useRefraction, value);
                Spectrometer.UseRefraction = value;
                if (value == true)
                {
                    AngleDevice = Spectrometer.DeviceList.Values.First();
                }
                else
                {
                    AngleDevice = null;
                }
            }
        }

        public DynMvp.Devices.Spectrometer.SpectrometerInfo AngleDevice
        {
            get => angleDevice;
            set
            {
                Set(ref angleDevice, value);
                if (value == null)
                {
                    Spectrometer.SelectedRefractionDevice = string.Empty;
                }
                else
                {
                    Spectrometer.SelectedRefractionDevice = value.Name;
                }
            }
        }

        public float AngleValue
        {
            get => angleValue;
            set
            {
                Set(ref angleValue, value);
                Spectrometer.AngleValue = value;
            }
        }

        public DynMvp.Devices.Spectrometer.SpectrometerInfo SelectedInfo
        {
            get => selectedInfo;
            set
            {
                Set(ref selectedInfo, value);
                OnPropertyChanged("IntegrationTime");
                OnPropertyChanged("Average");
                OnPropertyChanged("Boxcar");
                OnPropertyChanged("Threshold");
            }
        }

        public int IntegrationTime
        {
            get
            {
                if (SelectedInfo != null)
                {
                    return SpectrometerProperty.IntegrationTime[SelectedInfo.Name];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Set(ref integrationTime, value);
                if (SelectedInfo != null)
                {
                    SpectrometerProperty.IntegrationTime[SelectedInfo.Name] = integrationTime;
                }

                OnPropertyChanged("SpectrometerProperty");
            }
        }

        public int Average
        {
            get
            {
                if (SelectedInfo != null)
                {
                    return SpectrometerProperty.Average[SelectedInfo.Name];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Set(ref average, value);
                if (SelectedInfo != null)
                {
                    SpectrometerProperty.Average[SelectedInfo.Name] = average;
                }

                OnPropertyChanged("SpectrometerProperty");
            }
        }

        public int Boxcar
        {
            get
            {
                if (SelectedInfo != null)
                {
                    return SpectrometerProperty.Boxcar[SelectedInfo.Name];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Set(ref boxcar, value);
                if (SelectedInfo != null)
                {
                    SpectrometerProperty.Boxcar[SelectedInfo.Name] = boxcar;
                }

                OnPropertyChanged("SpectrometerProperty");
            }
        }

        public int Threshold
        {
            get
            {
                if (SelectedInfo != null)
                {
                    return SpectrometerProperty.LampPitch[SelectedInfo.Name];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Set(ref threshold, value);
                if (SelectedInfo != null)
                {
                    SpectrometerProperty.LampPitch[SelectedInfo.Name] = threshold;
                }

                OnPropertyChanged("SpectrometerProperty");
            }
        }

        public SpectrometerProperty SpectrometerProperty { get => spectrometerProperty; set => Set(ref spectrometerProperty, value); }

        // Machine
        public MelsecInfo MelsecInfo { get => melsecInfo; set => Set(ref melsecInfo, value); }

        public float BackGroundPos { get => backGroundPos; set => Set(ref backGroundPos, value); }

        public float ReferencePos { get => referencePos; set => Set(ref referencePos, value); }

        public float MasterSamplePos { get => masterSamplePos; set => Set(ref masterSamplePos, value); }

        public double HomeStartSpeed { get => homeStartSpeed; set => Set(ref homeStartSpeed, value); }

        public double HomeEndSpeed { get => homeEndSpeed; set => Set(ref homeEndSpeed, value); }

        public double JogSpeed { get => jogSpeed; set => Set(ref jogSpeed, value); }

        public double MovingSpeed { get => movingSpeed; set => Set(ref movingSpeed, value); }

        public double MeasureSecond { get => measureSecond; set => Set(ref measureSecond, value); }

        public float PositionOffset { get => positionOffset; set => Set(ref positionOffset, value); }

        public float SensorOffset { get => sensorOffset; set => Set(ref sensorOffset, value); }

        public MotionPosition MotionPosition { get => motionPosition; set => Set(ref motionPosition, value); }

        // PortMap Out
        public int TowerLampRed { get => towerLampRed; set => Set(ref towerLampRed, value); }

        public int TowerLampYellow { get => towerLampYellow; set => Set(ref towerLampYellow, value); }

        public int TowerLampGreen { get => towerLampGreen; set => Set(ref towerLampGreen, value); }

        public int TowerLampBuzzer { get => towerLampBuzzer; set => Set(ref towerLampBuzzer, value); }

        public int HalogenLampVIS { get => halogenLampVIS; set => Set(ref halogenLampVIS, value); }

        public int HalogenLampNIR { get => halogenLampNIR; set => Set(ref halogenLampNIR, value); }

        // PortMap In
        public int EmergencyFront { get => emergencyFront; set => Set(ref emergencyFront, value); }

        public int EmergencyBack { get => emergencyBack; set => Set(ref emergencyBack, value); }

        public int AreaSensor { get => areaSensor; set => Set(ref areaSensor, value); }

        // Data
        public double Sample { get => sample; set => Set(ref sample, value); }

        public double SampleRange { get => sampleRange; set => Set(ref sampleRange, value); }

        public int MovingAverageCount { get => movingAverageCount; set => Set(ref movingAverageCount, value); }

        // System Setting
        public SystemType SystemType { get => systemType; set => Set(ref systemType, value); }

        public HeadType HeadType { get => headType; set => Set(ref headType, value); }

        public ObservableCollection<ObservableValue<string>> LayerNameList
        {
            get => layerNameList;
            set
            {
                Set(ref layerNameList, value);

                var tempList = new List<string>();
                for (int i = 0; i < layerNameList.Count; i++)
                {
                    tempList.Add(layerNameList[i].Value);
                }

                spectrometerProperty.LayerNameList = tempList;
            }
        }

        public ObservableValue<string> LayerName { get => layerName; set => Set(ref layerName, value); }

        public string ResultPath { get => resultPath; set => Set(ref resultPath, value); }

        // Command
        public ICommand AddLayerParamCommand => addLayerParamCommand ?? (addLayerParamCommand = new RelayCommand(AddLayerParam));

        public ICommand DeleteLayerParamCommand => deleteLayerParamCommand ?? (deleteLayerParamCommand = new RelayCommand(DeleteLayerParam));

        public ICommand AddModelWidthCommand => addModelWidthCommand ?? (addModelWidthCommand = new RelayCommand(AddModelWidth));

        public ICommand DeleteModelWidthCommand => deleteModelWidthCommand ?? (deleteModelWidthCommand = new RelayCommand(DeleteModelWidth));

        public ICommand AddLayerCommand => addLayerCommand ?? (addLayerCommand = new RelayCommand(AddLayer));

        public ICommand DeleteLayerCommand => deleteLayerCommand ?? (deleteLayerCommand = new RelayCommand(DeleteLayer));
        #endregion

        #region 메서드
        public void UserChanged(User user)
        {
            OnPropertyChanged("HighLevelUser");
        }

        public void Save()
        {
            SystemConfig config = SystemConfig.Instance;

            // Model Param
            foreach (KeyValuePair<string, ObservableCollection<LayerParam>> layerParam in LayerParamList)
            {
                config.LayerParamList[layerParam.Key] = layerParam.Value.ToList();
            }

            // Scan Width
            config.ScanWidthList = ScanWidthList.ToList();

            // Spectrometer
            config.SpectrometerProperty = SpectrometerProperty;

            config.UseRefraction = UseRefraction;
            config.ReflectionSpectrometer = AngleDevice == null ? string.Empty : AngleDevice.Name;
            config.AngleValue = AngleValue;

            // Machine
            config.MelsecInfo = MelsecInfo;
            config.ReferencePos[0] = ReferencePos;
            config.BackGroundPos[0] = backGroundPos;
            config.MasterSamplePos[0] = masterSamplePos;
            config.HomeStartSpeed = HomeStartSpeed;
            config.HomeEndSpeed = HomeEndSpeed;
            config.JogSpeed = JogSpeed;
            config.MovingSpeed = MovingSpeed;
            config.MeasureSecond = MeasureSecond;
            config.PositionOffset = PositionOffset;
            config.SensorOffset = SensorOffset;

            // PortMap Out
            config.TowerLampRed = TowerLampRed;
            config.TowerLampYellow = TowerLampYellow;
            config.TowerLampGreen = TowerLampGreen;
            config.TowerLampBuzzer = TowerLampBuzzer;
            config.HalogenLampVIS = HalogenLampVIS;
            config.HalogenLampNIR = HalogenLampNIR;

            // PortMap In
            config.EmergencyFront = EmergencyFront;
            config.EmergencyBack = EmergencyBack;
            config.AreaSensor = AreaSensor;

            // Data
            config.Sample = Sample;
            config.SampleRange = SampleRange;
            config.MovingAverageCount = MovingAverageCount;

            // System Setting
            config.SpectrometerProperty.LayerNameList.Clear();
            foreach (ObservableValue<string> str in LayerNameList)
            {
                config.SpectrometerProperty.LayerNameList.Add(str.Value);
            }

            config.SystemType = SystemType;
            config.HeadType = HeadType;

            config.Save();
        }

        private void AddLayerParam()
        {
            if (SelectedLayerParamList == null)
            {
                return;
            }

            SelectedLayerParamList.Add(new LayerParam());
        }

        private void DeleteLayerParam()
        {
            if (SelectedLayerParamList == null)
            {
                return;
            }

            if (SelectedLayerParamList.Contains(SelectedLayerParam) == true)
            {
                //var result = await MessageWindowHelper.ShowMessage(this, "삭제", "파라미터를 삭제하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);
                //if (result == MessageDialogResult.Affirmative)
                SelectedLayerParamList.Remove(SelectedLayerParam);
            }
        }

        private void AddModelWidth()
        {
            ScanWidthList.Add(new ScanWidth());
        }

        private void DeleteModelWidth()
        {
            if (ScanWidthList.Contains(SelectedScanWidth) == true)
            {
                //var result = await MessageWindowHelper.ShowMessage(this, "삭제", "스켄 너비를 삭제하시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);
                //if (result == MessageDialogResult.Affirmative)
                ScanWidthList.Remove(SelectedScanWidth);
            }
        }

        private void AddLayer()
        {
            LayerNameList.Add(new ObservableValue<string>("New Layer"));
        }

        private void DeleteLayer()
        {
            if (LayerNameList.Contains(LayerName) == true)
            {
                LayerNameList.Remove(LayerName);
            }
        }
        #endregion
    }
}
