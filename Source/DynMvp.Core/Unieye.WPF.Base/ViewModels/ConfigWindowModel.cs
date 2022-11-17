using Authentication.Core.Database;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.Daq;
using DynMvp.Devices.Daq.UI;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Dio.UI;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.FrameGrabber.UI;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.MotionController.UI;
using DynMvp.Devices.UI;
using DynMvp.Inspect;
using DynMvp.Vision;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Override;
using UniEye.Base.Config;
using UniEye.Base.UI;
using UniEye.Base.UI.CameraCalibration;
using UniEye.Translation.Helpers;
using DataGrid = System.Windows.Controls.DataGrid;
using DialogResult = System.Windows.Forms.DialogResult;
using ICommand = System.Windows.Input.ICommand;

namespace Unieye.WPF.Base.ViewModels
{
    public enum SystemType
    {
        None,
        ScriberAlign,
        CurvedGlassAlign,
    }

    public enum ImagingLibraryType
    {
        Open_CV,
        Open_eVision,
        VisionPro,
        MIL,
        Halcon,
        Cuda,
        Custom,
    }

    public enum DeviceListType
    {
        Grabber, Motion, DigitalIo, LightController, Daq
    }

    internal class ConfigWindowModel : Observable
    {
        #region Variable

        private bool isVirtualMode = false;
        public bool IsVirtualMode
        {
            get => isVirtualMode;
            set => Set(ref isVirtualMode, value);
        }

        private bool isShowScore = false;
        public bool IsShowScore
        {
            get => isShowScore;
            set => Set(ref isShowScore, value);
        }

        private bool isShowNGImage = false;
        public bool IsShowNGImage
        {
            get => isShowNGImage;
            set => Set(ref isShowNGImage, value);
        }

        private bool isSaveTargetImage = false;
        public bool IsSaveTargetImage
        {
            get => isSaveTargetImage;
            set => Set(ref isSaveTargetImage, value);
        }

        private bool isSaveProbeImage = false;
        public bool IsSaveProbeImage
        {
            get => isSaveProbeImage;
            set => Set(ref isSaveProbeImage, value);
        }

        private bool isSaveCameraImage = false;
        public bool IsSaveCameraImage
        {
            get => isSaveCameraImage;
            set => Set(ref isSaveCameraImage, value);
        }

        private bool isSaveResultFigure = false;
        public bool IsSaveResultFigure
        {
            get => isSaveResultFigure;
            set => Set(ref isSaveResultFigure, value);
        }

        private bool useUserManager = false;
        public bool UseUserManager
        {
            get => useUserManager;
            set => Set(ref useUserManager, value);
        }

        private bool highLevelUser = false;
        public bool HighLevelUser
        {
            get => highLevelUser;
            set => Set(ref highLevelUser, value);
        }

        private bool useDoorSensor = false;
        public bool UseDoorSensor
        {
            get => useDoorSensor;
            set => Set(ref useDoorSensor, value);
        }

        private bool useModelBarcode = false;
        public bool UseModelBarcode
        {
            get => useModelBarcode;
            set => Set(ref useModelBarcode, value);
        }

        private bool useRobotStage = false;
        public bool UseRobotStage
        {
            get => useRobotStage;
            set => Set(ref useRobotStage, value);
        }

        private bool useConveyorMotor = false;
        public bool UseConveyorMotor
        {
            get => useConveyorMotor;
            set => Set(ref useConveyorMotor, value);
        }

        private bool useConveyorSystem = false;
        public bool UseConveyorSystem
        {
            get => useConveyorSystem;
            set => Set(ref useConveyorSystem, value);
        }

        private bool useTowerLamp = false;
        public bool UseTowerLamp
        {
            get => useTowerLamp;
            set => Set(ref useTowerLamp, value);
        }

        private bool useSoundBuzzer = false;
        public bool UseSoundBuzzer
        {
            get => useSoundBuzzer;
            set => Set(ref useSoundBuzzer, value);
        }

        private bool isEnableDeviceEditButton = false;
        public bool IsEnableDeviceEditButton
        {
            get => isEnableDeviceEditButton;
            set => Set(ref isEnableDeviceEditButton, value);
        }

        private string[] languageList =
        {
            "English",
            "Korean[ko-kr]",
            "Chinese(Simplified)[zh-cn]",
        };

        private LanguageSettings languageSettings = LanguageSettings.English;
        public LanguageSettings LanguageSettings
        {
            get => languageSettings;
            set => Set(ref languageSettings, value);
        }

        private SystemType systemType = SystemType.None;
        public SystemType SystemType
        {
            get => systemType;
            set => Set(ref systemType, value);
        }

        private ImagingLibrary imagingLibrary = ImagingLibrary.OpenCv;
        public ImagingLibrary ImagingLibrary
        {
            get => imagingLibrary;
            set => Set(ref imagingLibrary, value);
        }

        private DataPathType dataPathType = DataPathType.Model_Day;
        public DataPathType DataPathType
        {
            get => dataPathType;
            set => Set(ref dataPathType, value);
        }

        private string title;
        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        private bool showProgramTitle;
        public bool ShowProgramTitle
        {
            get => showProgramTitle;
            set => Set(ref showProgramTitle, value);
        }

        private string programTitle;
        public string ProgramTitle
        {
            get => programTitle;
            set => Set(ref programTitle, value);
        }

        public string[] imageNameFormatList =
        {
            "Image_{0:0000}_C{1:00}.bmp",
            "Image_C{0:00}_S{1:000}_L{2:00}.bmp"
        };

        private string imageNameFormat;
        public string ImageNameFormat
        {
            get => imageNameFormat;
            set => Set(ref imageNameFormat, value);
        }

        private string titleBar;
        public string TitleBar
        {
            get => titleBar;
            set => Set(ref titleBar, value);
        }

        private string companyLogo;
        public string CompanyLogo
        {
            get => companyLogo;
            set => Set(ref companyLogo, value);
        }

        private string productLogo;
        public string ProductLogo
        {
            get => productLogo;
            set => Set(ref productLogo, value);
        }

        private int resultStoringDays;
        public int ResultStoringDays
        {
            get => resultStoringDays;
            set => Set(ref resultStoringDays, value);
        }

        private int numLightType;
        public int NumLightType
        {
            get => numLightType;
            set => Set(ref numLightType, value);
        }

        private GrabberInfoList grabberInfoList;
        public GrabberInfoList GrabberInfoList
        {
            get => grabberInfoList;
            set => Set(ref grabberInfoList, value);
        }

        private MotionInfoList motionInfoList;
        public MotionInfoList MotionInfoList
        {
            get => motionInfoList;
            set => Set(ref motionInfoList, value);
        }

        private DigitalIoInfoList digitalIoInfoList;
        public DigitalIoInfoList DigitalIoInfoList
        {
            get => digitalIoInfoList;
            set => Set(ref digitalIoInfoList, value);
        }

        private LightCtrlInfoList lightCtrlInfoList;
        public LightCtrlInfoList LightCtrlInfoList
        {
            get => lightCtrlInfoList;
            set => Set(ref lightCtrlInfoList, value);
        }

        private DaqChannelPropertyList daqChannelPropertyList;
        public DaqChannelPropertyList DaqChannelPropertyList
        {
            get => daqChannelPropertyList;
            set => Set(ref daqChannelPropertyList, value);
        }

        private IEnumerable dataGridItem;
        public IEnumerable DataGridItem
        {
            get => dataGridItem;
            set => Set(ref dataGridItem, value);
        }

        private DeviceListType deviceListType;
        public DeviceListType DeviceListType
        {
            get => deviceListType;
            set => Set(ref deviceListType, value);
        }

        private object selectedDevice;
        public object SelectedDevice
        {
            get => selectedDevice;
            set
            {
                Set(ref selectedDevice, value);
                IsEnableDeviceEditButton = value != null;
            }
        }

        public OperationConfig OperationConfig => OperationConfig.Instance();
        public Array AuthDatabaseTypes => Enum.GetValues(typeof(AuthDatabaseType));
        #endregion

        #region Command

        private ICommand selectDeviceCommand;
        public ICommand SelectDeviceCommand => selectDeviceCommand ?? (selectDeviceCommand = new RelayCommand<string>(SelectDeviceAction));

        private void SelectDeviceAction(string type)
        {
            if (Enum.TryParse<DeviceListType>(type, out DeviceListType resultType))
            {
                DeviceListType = resultType;
            }

            UpdateDeviceItemSource();
        }

        private void UpdateDeviceItemSource()
        {
            DataGridItem = null;

            switch (DeviceListType)
            {
                case DeviceListType.Grabber:
                    DataGridItem = GrabberInfoList;
                    break;
                case DeviceListType.Motion:
                    DataGridItem = MotionInfoList;
                    break;
                case DeviceListType.DigitalIo:
                    DataGridItem = DigitalIoInfoList;
                    break;
                case DeviceListType.LightController:
                    DataGridItem = LightCtrlInfoList;
                    break;
                case DeviceListType.Daq:
                    DataGridItem = DaqChannelPropertyList;
                    break;
            }
        }

        private ICommand motionConfigButtonClick;
        public ICommand MotionConfigButtonClick => motionConfigButtonClick ?? (motionConfigButtonClick = new RelayCommand(MotionConfigButtonAction));

        private void MotionConfigButtonAction()
        {
            if (motionInfoList.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("There is no Motion device. Please, add Motion device first.");
                return;
            }

            var motionHandler = new MotionHandler();
            motionHandler.Initialize(motionInfoList, false);
            motionHandler.TurnOnServo(true);

            var axisHandlerNames = new List<string>();
            axisHandlerNames.Add("RobotStage");

            if (DeviceConfig.Instance().UseConveyorMotor)
            {
                axisHandlerNames.Add("Conveyor");
            }

            var axisHandlerList = new List<AxisHandler>();

            foreach (string axisHandlerName in axisHandlerNames)
            {
                axisHandlerList.Add(new AxisHandler(axisHandlerName));
            }

            foreach (AxisHandler axisHandler in axisHandlerList)
            {
                axisHandler.Load(motionHandler);
            }

            var form = new AxisConfigurationForm();

            form.Initialize(axisHandlerList, motionHandler);
            if (form.ShowDialog() == DialogResult.OK)
            {
                foreach (AxisHandler axisHandler in axisHandlerList)
                {
                    axisHandler.Save(PathConfig.Instance().Result + "\\..\\Config");
                }
            }

            motionHandler.TurnOnServo(false);
            motionHandler.Release();
        }

        private ICommand cameraCalibrationButtonClick;
        public ICommand CameraCalibrationButtonClick => cameraCalibrationButtonClick ?? (cameraCalibrationButtonClick = new RelayCommand(CameraCalibrationButtonAction));

        private void CameraCalibrationButtonAction()
        {
            if (GrabberInfoList.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("There is no Grabber device. Please, add Grabber device first.");
                return;
            }

            // Dot Mat 계산을 위한 알고리즘 초기화
            var calibrationAlgorithmStrategy = new AlgorithmStrategy(Calibration.TypeName, ImagingLibrary.OpenCv, "");
            AlgorithmFactory.Instance().AddStrategy(calibrationAlgorithmStrategy);
            AlgorithmFactory.Instance().SetAlgorithmEnabled(Calibration.TypeName, true);

            // 카메라와 조명만 일단 초기화
            DeviceManager.Instance().InitializeGrabberNLight(false, null);
            SystemManager.Instance().InitializeCameraCalibration();

            var form = new CameraCalibrationForm();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Initialize();
            form.ShowDialog();
        }

        private ICommand addDevice;
        public ICommand AddDevice => addDevice ?? (addDevice = new RelayCommand(AddDeviceAction));

        private void AddDeviceAction()
        {
            switch (DeviceListType)
            {
                case DeviceListType.Grabber:
                    {
                        var form = new NewGrabberForm();
                        form.StartPosition = FormStartPosition.CenterScreen;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            var grabberInfo = new GrabberInfo(form.GrabberName, form.GrabberType, form.NumCamera);
                            GrabberInfoList.Add(grabberInfo);

                            var cameraConfiguration = new CameraConfiguration();
                            for (int i = 0; i < form.NumCamera; i++)
                            {
                                cameraConfiguration.AddCameraInfo(CameraInfo.Create(grabberInfo.Type));
                            }

                            string filePath = string.Format("{0}\\CameraConfiguration_{1}.xml", BaseConfig.Instance().ConfigPath, grabberInfo.Name);
                            cameraConfiguration.SaveCameraConfiguration(filePath);
                        }
                    }
                    break;
                case DeviceListType.Motion:
                    {
                        var form = new NewMotionForm();
                        form.StartPosition = FormStartPosition.CenterScreen;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            MotionInfo motionInfo = MotionInfoFactory.CreateMotionInfo(form.MotionType);
                            motionInfo.Name = form.MotionName;
                            MotionInfoList.Add(motionInfo);
                        }
                    }
                    break;
                case DeviceListType.DigitalIo:
                    {
                        var form = new NewDigitalIoForm();
                        form.StartPosition = FormStartPosition.CenterScreen;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            DigitalIoInfoList.Add(form.DigitalIoInfo);
                        }
                    }
                    break;
                case DeviceListType.LightController:
                    {
                        var digitalIoHandler = new DigitalIoHandler();
                        digitalIoHandler.Build(DigitalIoInfoList, MotionInfoList);

                        var form = new LightConfigForm();
                        form.LightCtrlName = string.Format("Light {0}", LightCtrlInfoList.Count() + 1);
                        form.DigitalIoHandler = digitalIoHandler;
                        form.StartPosition = FormStartPosition.CenterScreen;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LightCtrlInfoList.Add(form.LightCtrlInfo);
                        }
                    }
                    break;
                case DeviceListType.Daq:
                    {
                        var form = new NewDaqChannelForm();
                        form.StartPosition = FormStartPosition.CenterScreen;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            DaqChannelProperty daqChannelProperty = DaqChannelPropertyFactory.Create(form.DaqChannelType);
                            daqChannelProperty.Name = form.DaqChannelName;

                            DaqChannelPropertyList.Add(daqChannelProperty);
                        }
                    }
                    break;
            }

            UpdateDeviceItemSource();
        }

        private ICommand editDevice;
        public ICommand EditDevice => editDevice ?? (editDevice = new RelayCommand(EditDeviceAction));

        private void EditDeviceAction()
        {
            switch (DeviceListType)
            {
                case DeviceListType.Grabber:
                    EditGrabber((GrabberInfo)SelectedDevice);
                    break;
                case DeviceListType.Motion:
                    EditMotionInfo((MotionInfo)SelectedDevice);
                    break;
                case DeviceListType.DigitalIo:
                    EditDigitalIoInfo((DigitalIoInfo)SelectedDevice);
                    break;
                case DeviceListType.LightController:
                    EditLightCtrlInfo((LightCtrlInfo)SelectedDevice);
                    break;
                case DeviceListType.Daq:
                    var form = new DaqPropertyForm();
                    form.DaqChannelProperty = (DaqChannelProperty)SelectedDevice;
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.ShowDialog();
                    //if (form.ShowDialog() == DialogResult.OK)
                    //{
                    //    dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = form.DaqChannelProperty.Name;
                    //}
                    break;
            }

            UpdateDeviceItemSource();
        }

        private ICommand searchTitleBarPathCommand;
        public ICommand SearchTitleBarPathCommand => searchTitleBarPathCommand ?? (searchTitleBarPathCommand = new RelayCommand(SearchTitleBarPath));

        private void SearchTitleBarPath()
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            if (TitleBar != null)
            {
                dialog.InitialDirectory = new FileInfo(TitleBar).DirectoryName;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                TitleBar = dialog.FileName;
            }
        }

        private ICommand searchCompanyLogoPathCommand;
        public ICommand SearchCompanyLogoPathCommand => searchCompanyLogoPathCommand ?? (searchCompanyLogoPathCommand = new RelayCommand(SearchCompanyLogoPath));

        private void SearchCompanyLogoPath()
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            if (CompanyLogo != null)
            {
                dialog.InitialDirectory = new FileInfo(CompanyLogo).DirectoryName;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                CompanyLogo = dialog.FileName;
            }
        }

        private ICommand searchProductLogoPathCommand;
        public ICommand SearchProductLogoPathCommand => searchProductLogoPathCommand ?? (searchProductLogoPathCommand = new RelayCommand(SearchProductLogoPath));

        private void SearchProductLogoPath()
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            if (ProductLogo != null)
            {
                dialog.InitialDirectory = new FileInfo(ProductLogo).DirectoryName;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ProductLogo = dialog.FileName;
            }
        }


        private void EditGrabber(GrabberInfo grabberInfo)
        {
            if (grabberInfo == null)
            {
                return;
            }

            Grabber grabber = GrabberFactory.Create(grabberInfo);

            var cameraConfiguration = new CameraConfiguration();
            string filePath = string.Format("{0}\\CameraConfiguration_{1}.xml", BaseConfig.Instance().ConfigPath, grabberInfo.Name);
            if (File.Exists(filePath) == true)
            {
                cameraConfiguration.LoadCameraConfiguration(filePath);
            }

            if (grabber.SetupCameraConfiguration(grabberInfo.NumCamera, cameraConfiguration) == true)
            {
                grabberInfo.NumCamera = cameraConfiguration.CameraInfoList.Count;
                //dataGridViewDeviceList.SelectedRows[0].Cells[3].Value = grabberInfo.NumCamera.ToString();
            }

            if (grabberInfo.NumCamera > 0 && cameraConfiguration.CameraInfoList.Count < grabberInfo.NumCamera)
            {
                System.Windows.MessageBox.Show("The number of camera is less then required number of camera");
                return;
            }

            cameraConfiguration.SaveCameraConfiguration(filePath);
        }

        private void EditMotionInfo(MotionInfo motionInfo)
        {
            DialogResult dialogResult = DialogResult.Cancel;

            if (motionInfo is PciMotionInfo)
            {
                var form = new PciMotionInfoForm();
                form.PciMotionInfo = (PciMotionInfo)motionInfo;
                form.StartPosition = FormStartPosition.CenterScreen;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is NetworkMotionInfo)
            {
                var form = new NetworkMotionInfoForm();
                form.NetworkMotionInfo = (NetworkMotionInfo)motionInfo;
                form.StartPosition = FormStartPosition.CenterScreen;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is SerialMotionInfo)
            {
                var form = new SerialMotionInfoForm();
                form.SerialMotionInfo = (SerialMotionInfo)motionInfo;
                form.StartPosition = FormStartPosition.CenterScreen;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is VirtualMotionInfo)
            {
                var form = new VirtualMotionInfoForm();
                form.VirtualMotionInfo = (VirtualMotionInfo)motionInfo;
                form.StartPosition = FormStartPosition.CenterScreen;
                dialogResult = form.ShowDialog();
            }

            if (dialogResult == DialogResult.OK)
            {
                SelectedDevice = null;
                SelectedDevice = motionInfo;
            }
        }

        private void EditDigitalIoInfo(DigitalIoInfo digitalIoInfo)
        {
            DialogResult dialogResult = DialogResult.Cancel;
            if (digitalIoInfo is PciDigitalIoInfo)
            {
                var form = new PciDigitalIoInfoForm();
                form.PciDigitalIoInfo = (PciDigitalIoInfo)digitalIoInfo;
                form.StartPosition = FormStartPosition.CenterScreen;
                dialogResult = form.ShowDialog();
            }
            else if (digitalIoInfo is SlaveDigitalIoInfo)
            {
                var form = new SlaveDigitalIoInfoForm();
                form.SlaveDigitalIoInfo = (SlaveDigitalIoInfo)digitalIoInfo;
                form.MotionInfoList = motionInfoList;
                form.StartPosition = FormStartPosition.CenterScreen;
                dialogResult = form.ShowDialog();
            }
            else if (digitalIoInfo is SerialDigitalIoInfo)
            {
                var form = new SerialDigitalIoInfoForm((SerialDigitalIoInfo)digitalIoInfo);
                form.StartPosition = FormStartPosition.CenterScreen;
                dialogResult = form.ShowDialog();
            }

            //if (dialogResult == DialogResult.OK)
            //{
            //    dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = digitalIoInfo.Name;
            //}
        }

        private void EditDaqChannelProperty(DaqChannelProperty daqChannelProperty)
        {
            var form = new DaqPropertyForm();
            form.DaqChannelProperty = daqChannelProperty;
            form.StartPosition = FormStartPosition.CenterScreen;
            if (form.ShowDialog() == DialogResult.OK)
            {
                //dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = daqChannelProperty.Name;
            }
        }

        private void EditLightCtrlInfo(LightCtrlInfo lightCtrlInfo)
        {
            var digitalIoHandler = new DigitalIoHandler();
            digitalIoHandler.Build(digitalIoInfoList, motionInfoList);

            var form = new LightConfigForm();
            form.LightCtrlInfo = lightCtrlInfo;
            form.DigitalIoHandler = digitalIoHandler;
            form.StartPosition = FormStartPosition.CenterScreen;
            if (form.ShowDialog() == DialogResult.OK)
            {
                lightCtrlInfoList.Remove(lightCtrlInfo);

                lightCtrlInfo = form.LightCtrlInfo;

                lightCtrlInfoList.Add(lightCtrlInfo);

                //dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = lightCtrlInfo.Name;
                //dataGridViewDeviceList.SelectedRows[0].Cells[2].Value = lightCtrlInfo.Type.ToString();
                //dataGridViewDeviceList.SelectedRows[0].Cells[3].Value = lightCtrlInfo.NumChannel.ToString();
                //dataGridViewDeviceList.SelectedRows[0].Tag = lightCtrlInfo;
            }
        }

        private ICommand deleteDevice;
        public ICommand DeleteDevice => deleteDevice ?? (deleteDevice = new RelayCommand(DeleteDeviceAction));

        private void DeleteDeviceAction()
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("선택한 항목을 삭제하시겠습니까?", "Warning", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                switch (DeviceListType)
                {
                    case DeviceListType.Grabber:
                        DeleteGrabber((GrabberInfo)SelectedDevice);
                        break;
                    case DeviceListType.Motion:
                        DeleteMotionInfo((MotionInfo)SelectedDevice);
                        break;
                    case DeviceListType.DigitalIo:
                        DeleteDigitalIoInfo((DigitalIoInfo)SelectedDevice);
                        break;
                    case DeviceListType.LightController:
                        DeleteLightCtrlInfo((LightCtrlInfo)SelectedDevice);
                        break;
                    case DeviceListType.Daq:
                        DeleteDaqChannelProperty((DaqChannelProperty)SelectedDevice);
                        break;
                }

                UpdateDeviceItemSource();
            }
        }

        private void DeleteGrabber(GrabberInfo selectedDevice)
        {
            grabberInfoList.Remove(selectedDevice);
        }

        private void DeleteMotionInfo(MotionInfo selectedDevice)
        {
            MotionInfoList.Remove(selectedDevice);
        }

        private void DeleteDigitalIoInfo(DigitalIoInfo selectedDevice)
        {
            DigitalIoInfoList.Remove(selectedDevice);
        }

        private void DeleteLightCtrlInfo(LightCtrlInfo selectedDevice)
        {
            LightCtrlInfoList.Remove(selectedDevice);
        }

        private void DeleteDaqChannelProperty(DaqChannelProperty selectedDevice)
        {
            DaqChannelPropertyList.Remove(selectedDevice);
        }

        private ICommand okButtonClick;
        public ICommand OKButtonClick => okButtonClick ?? (okButtonClick = new RelayCommand<Window>(OKButtonAction));

        private void OKButtonAction(Window wnd)
        {
            wnd.DialogResult = true;

            SaveParameter();

            wnd.Close();
        }

        private ICommand cancelButtonClick;
        public ICommand CancelButtonClick => cancelButtonClick ?? (cancelButtonClick = new RelayCommand<Window>(CancelButtonAction));

        private void CancelButtonAction(Window wnd)
        {
            wnd.DialogResult = false;
            wnd.Close();
        }

        #endregion

        private DataGrid deviceGridList;

        public ConfigWindowModel(DataGrid gridCtrl)
        {
            deviceGridList = gridCtrl;

            LoadParameter();

            SelectDeviceAction(DeviceListType.Grabber.ToString());
        }

        private void SaveParameter()
        {
            var operationConfig = OperationConfig.Instance();
            var deviceConfig = DeviceConfig.Instance();
            var inspectConfig = InspectConfig.Instance();
            var uiConfig = UiConfig.Instance();
            var pathSettings = PathConfig.Instance();
            var lightConfig = LightConfig.Instance();

            // General Tab
            uiConfig.Title = Title;
            uiConfig.ShowProgramTitle = ShowProgramTitle;
            uiConfig.ProgramTitle = ProgramTitle;
            pathSettings.TitleBar = TitleBar;
            pathSettings.CompanyLogo = CompanyLogo;
            pathSettings.ProductLogo = ProductLogo;
            LanguageSettings[] array = Enum.GetValues(typeof(LanguageSettings)).Cast<LanguageSettings>().ToArray();
            uiConfig.Language = languageList[Array.FindIndex(array, x => x == LanguageSettings)];
            operationConfig.SystemType = SystemType.ToString();
            operationConfig.ImagingLibrary = ImagingLibrary;
            deviceConfig.VirtualMode = IsVirtualMode;

            // Device Tab
            deviceConfig.GrabberInfoList = GrabberInfoList.Clone();
            deviceConfig.MotionInfoList = MotionInfoList.Clone();
            deviceConfig.DigitalIoInfoList = DigitalIoInfoList.Clone();
            deviceConfig.LightCtrlInfoList = LightCtrlInfoList.Clone();
            deviceConfig.DaqChannelPropertyList = DaqChannelPropertyList.Clone();

            deviceConfig.UseDoorSensor = UseDoorSensor;
            deviceConfig.UseBarcodeReader = UseModelBarcode;
            deviceConfig.UseRobotStage = UseRobotStage;
            deviceConfig.UseConveyorMotor = UseConveyorMotor;
            deviceConfig.UseConveyorSystem = UseConveyorSystem;
            deviceConfig.UseTowerLamp = UseTowerLamp;
            deviceConfig.UseSoundBuzzer = UseSoundBuzzer;

            // Model Tab
            operationConfig.DataPathType = DataPathType;
            inspectConfig.SaveProbeImage = IsSaveProbeImage;
            inspectConfig.SaveTargetImage = IsSaveTargetImage;
            inspectConfig.SaveCameraImage = IsSaveCameraImage;
            inspectConfig.ImageNameFormat = ImageNameFormat;
            lightConfig.NumLightType = NumLightType;

            // UI Tab
            uiConfig.ShowScore = IsShowScore;
            uiConfig.ShowNGImage = IsShowNGImage;
            operationConfig.UseUserManager = UseUserManager;
            inspectConfig.SaveResultFigure = IsSaveResultFigure;
            operationConfig.ResultStoringDays = ResultStoringDays;

            operationConfig.Save();
            deviceConfig.Save();
            inspectConfig.Save();
            uiConfig.Save();
            pathSettings.Save();
            lightConfig.Save();
        }

        private void LoadParameter()
        {
            var operationConfig = OperationConfig.Instance();
            var deviceConfig = DeviceConfig.Instance();
            var inspectConfig = InspectConfig.Instance();
            var uiConfig = UiConfig.Instance();
            var pathSettings = PathConfig.Instance();
            var lightConfig = LightConfig.Instance();

            // General Tab
            Title = uiConfig.Title;
            ShowProgramTitle = uiConfig.ShowProgramTitle;
            ProgramTitle = uiConfig.ProgramTitle;

            TitleBar = pathSettings.TitleBar;
            CompanyLogo = pathSettings.CompanyLogo;
            ProductLogo = pathSettings.ProductLogo;

            string language = uiConfig.Language;
            int langIndex = Array.FindIndex(languageList, x => x == language);
            LanguageSettings = (LanguageSettings)langIndex;

            if (Enum.TryParse<SystemType>(operationConfig.SystemType, out SystemType sysType))
            {
                SystemType = sysType;
            }
            else
            {
                SystemType = SystemType.None;
            }

            ImagingLibrary = operationConfig.ImagingLibrary;
            IsVirtualMode = deviceConfig.VirtualMode;

            // Device Tab
            grabberInfoList = deviceConfig.GrabberInfoList.Clone();
            motionInfoList = deviceConfig.MotionInfoList.Clone();
            digitalIoInfoList = deviceConfig.DigitalIoInfoList.Clone();
            lightCtrlInfoList = deviceConfig.LightCtrlInfoList.Clone();
            daqChannelPropertyList = deviceConfig.DaqChannelPropertyList.Clone();

            UseDoorSensor = deviceConfig.UseDoorSensor;
            UseModelBarcode = deviceConfig.UseBarcodeReader;
            UseRobotStage = deviceConfig.UseRobotStage;
            UseConveyorMotor = deviceConfig.UseConveyorMotor;
            UseConveyorSystem = deviceConfig.UseConveyorSystem;
            UseTowerLamp = deviceConfig.UseTowerLamp;
            UseSoundBuzzer = deviceConfig.UseSoundBuzzer;

            // Model Tab
            DataPathType = operationConfig.DataPathType;
            IsSaveProbeImage = inspectConfig.SaveProbeImage;
            IsSaveTargetImage = inspectConfig.SaveTargetImage;
            IsSaveCameraImage = inspectConfig.SaveCameraImage;
            ImageNameFormat = inspectConfig.ImageNameFormat;
            NumLightType = lightConfig.NumLightType;

            // UI Tab
            IsShowScore = uiConfig.ShowScore;
            IsShowNGImage = uiConfig.ShowNGImage;
            UseUserManager = operationConfig.UseUserManager;
            IsSaveResultFigure = inspectConfig.SaveResultFigure;
            ResultStoringDays = operationConfig.ResultStoringDays;
        }
    }
}
