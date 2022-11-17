using Authentication.Core;
using Authentication.Core.Enums;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;
using Unieye.WPF.Base.Layout.ViewModels;
using UniEye.Base.Data;
using UniEye.Translation.Helpers;
using UniScanC.Controls.ViewModels;
using UniScanC.Controls.Views;
using UniScanC.Data;
using UniScanC.Enums;
using UniScanC.Models;
using UniScanC.Windows.ViewModels;
using UniScanC.Windows.Views;
using WPF.UniScanCM.Events;
using WPF.UniScanCM.Override;
using WPF.UniScanCM.PLC;
using WPF.UniScanCM.Service;
using WPF.UniScanCM.Windows.Views;
using ICommand = System.Windows.Input.ICommand;
using ModelManager = UniScanC.Models.ModelManager;
using UiManager = WPF.UniScanCM.Override.UiManager;

namespace WPF.UniScanCM.Pages.ViewModels
{
    public class MonitoringPageViewModel : Observable
    {
        #region 생성자
        public MonitoringPageViewModel()
        {
            StartCommand = new RelayCommand(StartCommandAction);
            HoldCommand = new RelayCommand(HoldCommandAction);
            StopCommand = new RelayCommand(StopCommandAction);
            NextLotNoCommand = new RelayCommand(NextLotNoCommandAction);
            ModelParameterCommand = new RelayCommand(ModelParameterCommandAction);
            ClearInspectResultCommand = new RelayCommand(ClearInspectResultCommandAction);
            IOStatusCommand = new RelayCommand(IOStatusCommandAction);
            SetSpeedCommand = new RelayCommand(SetSpeedCommandAction);

            InspectEventListener.Instance.UpdateResultDelegate += OnUpdateResults;

            ModelEventListener.Instance.OnModelOpened += ModelOpened;
            ModelEventListener.Instance.OnModelClosed += ModelClosed;

            LotNo = SystemConfig.Instance.LastLotNo;
            TargetSpeed = SystemConfig.Instance.TargetSpeed;

            OnLayoutChanged(UiManager.Instance.InspectLayoutHandler);
            UiManager.Instance.OnInspectLayoutChanged += OnLayoutChanged;

            //DeviceManager.LotNoChanged += OnLotNoChanged;
            if (PLCMachineIf != null && PLCMachineIf.IsConnected())
            {
                //PLCMachineIf.CurrentPositionChanged += OnCurrentPositionChanged;
                PLCMachineIf.CurrentSpeedChanged += OnCurrentSpeedChanged;
                PLCMachineIf.LotNoChanged += OnLotNoChanged;
                if (PLCMachineIf.MachineReadyChanged == null)
                {
                    PLCMachineIf.MachineReadyChanged += OnPlcReadyChanged;
                }
            }
            else
            {
                UseManualSpeedSetting = true;
            }

            SystemState.Instance().OpStateChanged += OpStateChanged;
            SystemState.Instance().SetIdle();

            AlarmCheckTimer = new DispatcherTimer();
            AlarmCheckTimer.Tick += AlarmCheckTimer_Tick;
            AlarmCheckTimer.Interval = TimeSpan.FromMilliseconds(500);
            AlarmCheckTimer.Start();
        }
        #endregion


        #region 속성
        public ICommand StartCommand { get; }

        public ICommand HoldCommand { get; }

        public ICommand StopCommand { get; }

        public ICommand NextLotNoCommand { get; }

        public ICommand ModelParameterCommand { get; }

        public ICommand ClearInspectResultCommand { get; }

        public ICommand IOStatusCommand { get; }

        public ICommand SetSpeedCommand { get; }

        private string lotNo;
        public string LotNo
        {
            get => lotNo;
            set => Set(ref lotNo, value);
        }

        private float inspectLength = 0;
        public float InspectLength
        {
            get => inspectLength;
            set => Set(ref inspectLength, value);
        }

        private float currentSpeed = 0;
        public float TargetSpeed
        {
            get => currentSpeed;
            set => Set(ref currentSpeed, value);
        }

        private UniScanC.Models.Model model;
        public UniScanC.Models.Model Model
        {
            get => model;
            set => Set(ref model, value);
        }

        private LayoutHandler layoutHandler;
        public LayoutHandler LayoutHandler
        {
            get => layoutHandler;
            set => Set(ref layoutHandler, value);
        }

        private LayoutViewModel layoutViewModel;
        public LayoutViewModel LayoutViewModel
        {
            get => layoutViewModel;
            set => Set(ref layoutViewModel, value);
        }

        private Defect selectedDefect;
        public Defect SelectedDefect
        {
            get => selectedDefect;
            set
            {
                if (Set(ref selectedDefect, value) && value != null)
                {
                    var view = new DefectDetailHorizentalControlView();
                    var viewModel = new DefectDetailHorizentalControlViewModel();
                    viewModel.Model = Model;
                    viewModel.UpdateSelectedDefect(selectedDefect);
                    view.DataContext = viewModel;
                    DefectDetailView.Title = "DEFECT_INFO";
                    DefectDetailView.CustomControl = view;
                    if (!DefectDetailView.IsVisible)
                    {
                        DefectDetailView.Show();
                    }
                }
            }
        }

        private bool isStartInspection = false;
        public bool IsStartInspection
        {
            get => isStartInspection;
            set => Set(ref isStartInspection, value);
        }

        private bool isSkipMode = false;
        public bool IsSkipMode
        {
            get => isSkipMode;
            set
            {
                if (Set(ref isSkipMode, value))
                {
                    TurnOnSkipMode(value);
                }
            }
        }

        private bool useManualSpeedSetting = false;
        public bool UseManualSpeedSetting
        {
            get => useManualSpeedSetting;
            set => Set(ref useManualSpeedSetting, value);
        }

        private StatusModel statusModel;
        public StatusModel StatusModel
        {
            get => statusModel;
            set => Set(ref statusModel, value);
        }

        // System
        private PopupWindow DefectDetailView { get; set; } = ((UiManager)UiManager.Instance).DefectDetailView;

        private AlarmMessageWindowView AlarmWindow { get; set; } = null; //시스템 알람 관리 창

        private DispatcherTimer AlarmCheckTimer { get; set; } = null;

        // Delegate
        private ModelEventDelegate UpdateModelDelegate { get; set; }

        private UpdateResultDelegate UpdateResultDelegate { get; set; }

        private SelectedDefectUpdateDelegate UpdateSelectedDefectDelegate { get; set; }

        // Getter
        private DeviceManager DeviceManager => DeviceManager.Instance() as DeviceManager;

        private DeviceMonitor DevMonitor => DeviceMonitor.Instance() as DeviceMonitor;

        private PlcBase PLCMachineIf => DeviceManager.PLCMachineIf;
        #endregion


        #region 메서드
        private async void StartCommandAction()
        {
            if (!IsStartInspection)
            {
                if (model == null)
                {
                    await MessageWindowHelper.ShowMessageBox(
                          TranslationHelper.Instance.Translate("NOTIFICATION"),
                          TranslationHelper.Instance.Translate("FAILED_TO_OPEN_MODEL"),
                          System.Windows.MessageBoxButton.OK);
                    return;
                }

                // PLC 신호 기능
                if (PLCMachineIf != null && PLCMachineIf.IsConnected() && PLCMachineIf.CoatingStartChanged == null)
                {
                    PLCMachineIf.CoatingStartChanged += OnPlcStartChanged;
                }

                // IO 신호 기능
                if (DevMonitor.IOStartChanged == null)
                {
                    DevMonitor.IOStartChanged += OnPlcStartChanged;
                }

                if (DevMonitor.DefectOccrued == null)
                {
                    DevMonitor.DefectOccrued += OnDefectOccured;
                }

                if (DevMonitor.DefectReset == null)
                {
                    DevMonitor.DefectReset += OnDefectReset;
                }

                bool isOK = await StartInspection();
                if (isOK)
                {
                    LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Start Inspect");
                }
                else
                {
                    LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Start Inspect Fail");
                }
            }
        }

        private void HoldCommandAction()
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke(new Action(() => { if (IsStartInspection) { IsSkipMode = !IsSkipMode; } }));
            }
            else
            {
                if (IsStartInspection)
                {
                    IsSkipMode = !IsSkipMode;
                }
            }

            TurnOnSkipMode(IsSkipMode);
            LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Hold Inspect");
        }

        private void StopCommandAction()
        {
            // PLC 신호 기능
            if (PLCMachineIf != null && PLCMachineIf.IsConnected())
            {
                PLCMachineIf.CoatingStartChanged = null;
            }

            // IO 신호 기능
            DevMonitor.IOStartChanged = null;
            DevMonitor.DefectOccrued = null;
            DevMonitor.DefectReset = null;

            StopInspection();
            LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Stop Inspect");
        }

        private async void NextLotNoCommandAction()
        {
            LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Click LotNo");
            SystemConfig config = SystemConfig.Instance;
            var window = new InputValueWindowView();
            window.Text = config.LastLotNo;
            if (await MessageWindowHelper.ShowChildWindow<bool>(window) == true)
            {
                var regex = new Regex(string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()))));
                if (regex.IsMatch(window.Text) == true)
                {
                    string header = TranslationHelper.Instance.Translate("Warning");
                    string message = TranslationHelper.Instance.Translate("MODELNAME_WARNING_MESSAGE");
                    await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);
                }

                window.Text = regex.Replace(window.Text, "_");
                window.Text = window.Text.Replace("@", "_");

                LotNo = window.Text.ToUpper();
                config.LastLotNo = LotNo;
                config.Save();
                LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Change LotNo [{LotNo}]");
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Cancel Change LotNo");
            }
        }

        private async void ModelParameterCommandAction()
        {
            ModelBase model = ModelManager.Instance().CurrentModel;
            if (model == null)
            {
                return;
            }

            LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Click Model Parameter");
            var window = new CategoryParamWindowView();
            if (await MessageWindowHelper.ShowChildWindow<bool>(window) == true)
            {

            }
        }

        private void ClearInspectResultCommandAction()
        {
            OnUpdateResults(null);
            LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Click Clear Inspect Result");
        }

        private async void IOStatusCommandAction()
        {
            LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Click IO Status");
            var view = new IOPortStatusWindowView();
            if (await MessageWindowHelper.ShowChildWindow<bool>(view) == true)
            {

            }
        }

        private async void SetSpeedCommandAction()
        {
            var window = new InputValueWindowView();
            LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Click Speed Setting");
            if (await MessageWindowHelper.ShowChildWindow<bool>(window) == true)
            {
                var regex = new Regex(string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()))));
                if (window.Text != null)
                {
                    if (regex.IsMatch(window.Text) == true)
                    {
                        string header = TranslationHelper.Instance.Translate("Warning");
                        string message = TranslationHelper.Instance.Translate("MODELNAME_WARNING_MESSAGE");
                        await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OK);
                    }

                    window.Text = regex.Replace(window.Text, "_");

                    TargetSpeed = Convert.ToSingle(window.Text);
                    SystemConfig.Instance.TargetSpeed = TargetSpeed;
                }
                LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Speed Setting [{TargetSpeed}]");
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Inspect Page] Cancel Speed Setting");
            }
        }

        private async Task<bool> StartInspection()
        {
            if (Model == null)
            {
                return false;
            }

            if (!IsStartInspection)
            {
                IsStartInspection = true;

                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Model, false);
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Teach, false);
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Setting, false);

                // 측정 길이 0으로 초기화
                InspectLength = 0;

                if (!await SystemManager.Instance().StartInspection())
                {
                    StopInspection();
                }

                string tempStr = SystemConfig.Instance.LastLotNo;
                char[] tempStrArray = tempStr.ToCharArray();
                int index = tempStrArray.ToList().FindIndex(x => x == '@');
                if (index > -1)
                {
                    LotNo = SystemConfig.Instance.LastLotNo = tempStr.Substring(0, index);
                }
                else
                {
                    LotNo = SystemConfig.Instance.LastLotNo;
                }
            }

            return true;
        }

        private async void StopInspection()
        {
            if (AlarmWindow != null)
            {
                ErrorManager.Instance().ResetAlarm();
                AlarmWindow.Close();
            }

            // 자동 Lot 기능을 사용할 시에는 종료 할 때 Lot 비워주기
            if (SystemConfig.Instance.UseAutoLotNo)
            {
                LotNo = SystemConfig.Instance.LastLotNo = "";
            }

            await SystemManager.Instance().StopInspection();

            IsStartInspection = false;

            if (UserHandler.Instance.CurrentUser.IsAuth(ERoleType.ModelPage))
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Model, true);
            }

            if (UserHandler.Instance.CurrentUser.IsAuth(ERoleType.TeachPage))
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Teach, true);
            }

            if (UserHandler.Instance.CurrentUser.IsAuth(ERoleType.SettingPage))
            {
                UiManager.Instance.EnableTab(UniEye.Base.UI.TabKey.Setting, true);
            }

            SystemState.Instance().SetIdle();
        }

        private void ModelOpened(ModelBase model)
        {
            Model = model as UniScanC.Models.Model;

            UpdateModelDelegate?.Invoke(model);

            OnPlcStartChanged(false);
        }

        private void ModelClosed()
        {
            Model = null;
        }

        private void OnUpdateResults(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null)
        {
            DynMvp.Base.LogHelper.Debug(DynMvp.Base.LoggerType.Inspection, "MonitoringPageModel::OnUpdateResults - Start");
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            if (productResults == null)
            {
                UpdateResultDelegate?.Invoke(null, taskCancelToken);
                DynMvp.Base.LogHelper.Debug(DynMvp.Base.LoggerType.Inspection, "MonitoringPageModel::OnUpdateResults - productResults is null");
            }
            else
            {
                // InspectResult Casting 을 위한 리스트 작성
                // Cast는 위험하므로 이런 방식으로 진행
                var filteredInspectResultList = new List<InspectResult>();
                foreach (ProductResult productResult in productResults)
                {
                    if (productResult is InspectResult inspectResult)
                    {
                        filteredInspectResultList.Add(inspectResult);
                    }
                }

                if (filteredInspectResultList.Count > 0)
                {
                    // 스캔 길이 값을 갱신
                    int maxFrameIndex = filteredInspectResultList.Max(x => x.FrameIndex);
                    float maxInspectRegionHeight = filteredInspectResultList.Max(x => x.InspectRegion.Height);
                    //InspectLength = Math.Max(InspectLength, ((maxFrameIndex + 1) * maxInspectRegionHeight) / 1000.0f);
                    InspectLength = ((maxFrameIndex + 1) * maxInspectRegionHeight) / 1000.0f;

                    // 미분류 불량을 표시할지 말지 결정
                    if (!SystemConfig.Instance.IsShowOthersDefect)
                    {
                        foreach (InspectResult inspectResult in filteredInspectResultList)
                        {
                            inspectResult.DefectList.RemoveAll(defect => defect.DefectTypeName == DefectCategory.GetDefaultCategory().Name);
                        }
                    }

                    // PNT 기능
                    if (SystemConfig.Instance.UseDefectAlarm == true && SystemConfig.Instance.UseCustomDefectAlarm == true)
                    {
                        DefectAlarmService.CheckCustomDefect(filteredInspectResultList, InspectLength);
                    }

                    if (SystemConfig.Instance.UseDefectCount == true)
                    {
                        DefectCountService.CountDefect(filteredInspectResultList, InspectLength);
                    }
                }
                else
                {
                    DynMvp.Base.LogHelper.Debug(DynMvp.Base.LoggerType.Inspection, "MonitoringPageModel::OnUpdateResults - filteredInspectResultList count is under 0");
                }

                if (PLCMachineIf != null && PLCMachineIf.IsConnected())
                {
                    PLCMachineIf.UpdateRealTimeDefectAlarm(filteredInspectResultList);

                    if (SystemConfig.Instance.UseDefectCount)
                    {
                        PLCMachineIf.UpdateRealTimeDefectCountResult();
                    }
                }

                // 검사 UI에 데이터를 표시하기위한 Delegate
                UpdateResultDelegate?.Invoke(productResults, taskCancelToken);
            }

            LogHelper.Debug(LoggerType.Inspection, $"MonitoringPageModel::OnUpdateResults - End [{sw.ElapsedMilliseconds}ms]");
            sw.Stop();
        }

        private void OnUpdateSelectedDefect(object selectedDefect)
        {
            var defect = selectedDefect as Defect;
            SelectedDefect = defect;
            UpdateSelectedDefectDelegate?.Invoke(selectedDefect);
        }

        private async void TurnOnSkipMode(bool isSkip)
        {
            var comm = CommManager.Instance() as CommManager;
            await comm.ExecuteCommand(EUniScanCCommand.SkipMode, isSkip.ToString());
        }


        #region Alarm
        private void AlarmCheckTimer_Tick(object sender, EventArgs e)
        {
            if (ErrorManager.Instance().IsAlarmed())
            {
                AlarmCheckTimerAction();
            }
        }

        private void AlarmCheckTimerAction()
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => AlarmCheckTimerAction()));
                return;
            }

            if (AlarmWindow == null)
            {
                ErrorItem curErrorItem = ErrorManager.Instance().ActiveErrorItemList[0];
                var AlarmWindowViewModel = new AlarmMessageWindowViewModel();
                AlarmWindowViewModel.AlarmCode = curErrorItem.ErrorCode.ToString();
                AlarmWindowViewModel.AlarmLevel = (int)curErrorItem.ErrorLevel;
                AlarmWindowViewModel.AlarmMessage = curErrorItem.Message;

                AlarmWindow = new AlarmMessageWindowView();
                AlarmWindow.DataContext = AlarmWindowViewModel;
                AlarmWindow.ShowDialog();
                AlarmWindow = null;
                DeviceManager.SendDefectSignal(false);
            }
        }
        #endregion


        #region Monitoring
        private void OpStateChanged()
        {
            StatusModel = OpStateModel.StatusModel(SystemState.Instance().OpState);
        }

        // PLC 정보를 사용할 경우 활성화
        private void OnCurrentPositionChanged(float value)
        {
            InspectLength = value;
        }

        private void OnCurrentSpeedChanged(float value)
        {
            TargetSpeed = value;
        }

        private void OnLotNoChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                LotNo = SystemConfig.Instance.LastLotNo = value;
            }
            else
            {
                LotNo = SystemConfig.Instance.LastLotNo;
            }
        }
        #endregion


        #region Layout
        private void OnLayoutChanged(LayoutHandler layoutHandler)
        {
            LayoutHandler = layoutHandler;
            LayoutViewModel = new LayoutViewModel(LayoutHandler);
            LinkAllDelegate();
            //LinkModelDelegate();
            //LinkProductResultsDelegate();
            //LinkSelectedDefectDelegate();
            //LinkLogDelegate();

            if (Model != null)
            {
                UpdateModelDelegate?.Invoke(Model);
            }
        }

        // 하기 4개의 메서드를 한번에 진행하는 메서드
        private void LinkAllDelegate()
        {
            UpdateModelDelegate = null;
            UpdateResultDelegate = null;
            UpdateSelectedDefectDelegate = null;
            //LogHelper.NotifyLog = null;
            foreach (LayoutPageHandler layoutPageHandler in LayoutHandler)
            {
                var layoutModelList = layoutPageHandler.ModelList.ToList();
                foreach (LayoutModel model in layoutModelList)
                {
                    if (model.ControlViewModel is INotifyModelChanged notifyModelControl)
                    {
                        UpdateModelDelegate += notifyModelControl.OnUpdateModel;
                    }

                    if (model.ControlViewModel is INotifyProductResultChanged notifyProductResultControl)
                    {
                        UpdateResultDelegate += notifyProductResultControl.OnUpdateResult;
                    }

                    if (model.ControlViewModel is INotifySelectedDefectChanged notifySelectedDefectControl)
                    {
                        UpdateSelectedDefectDelegate += notifySelectedDefectControl.OnUpdateSelectedDefect;
                        notifySelectedDefectControl.SelectedDefectUpdate += OnUpdateSelectedDefect;
                    }
                    //if (model.ControlViewModel is INotifyLogChanged notifyLogControl)
                    //    LogHelper.NotifyLog += notifyLogControl.NotifyLog;
                }
            }
        }

        private void LinkModelDelegate()
        {
            UpdateModelDelegate = null;
            foreach (LayoutPageHandler layoutPageHandler in LayoutHandler)
            {
                var layoutModelList = layoutPageHandler.ModelList.ToList();
                foreach (LayoutModel model in layoutModelList)
                {
                    if (model.ControlViewModel is INotifyModelChanged notifyModelControl)
                    {
                        UpdateModelDelegate += notifyModelControl.OnUpdateModel;
                    }
                }
            }
        }

        private void LinkProductResultsDelegate()
        {
            UpdateResultDelegate = null;
            foreach (LayoutPageHandler layoutPageHandler in LayoutHandler)
            {
                var layoutModelList = layoutPageHandler.ModelList.ToList();
                foreach (LayoutModel model in layoutModelList)
                {
                    if (model.ControlViewModel is INotifyProductResultChanged notifyProductResultControl)
                    {
                        UpdateResultDelegate += notifyProductResultControl.OnUpdateResult;
                    }
                }
            }
        }

        private void LinkSelectedDefectDelegate()
        {
            UpdateSelectedDefectDelegate = null;
            foreach (LayoutPageHandler layoutPageHandler in LayoutHandler)
            {
                var layoutModelList = layoutPageHandler.ModelList.ToList();
                foreach (LayoutModel model in layoutModelList)
                {
                    if (model.ControlViewModel is INotifySelectedDefectChanged notifySelectedDefectControl)
                    {
                        UpdateSelectedDefectDelegate += notifySelectedDefectControl.OnUpdateSelectedDefect;
                        notifySelectedDefectControl.SelectedDefectUpdate += OnUpdateSelectedDefect;
                    }
                }
            }
        }

        //private void LinkLogDelegate()
        //{
        //    LogHelper.NotifyLog = null;
        //    foreach (var layoutPageHandler in LayoutHandler)
        //    {
        //        var layoutModelList = layoutPageHandler.ModelList.ToList();
        //        foreach (var model in layoutModelList)
        //        {
        //            if (model.ControlViewModel is INotifyLogChanged notifyLogControl)
        //                LogHelper.NotifyLog += notifyLogControl.NotifyLog;
        //        }
        //    }
        //}
        #endregion


        #region PLC
        private void OnPlcReadyChanged(bool value)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnPlcReadyChanged(value)));
                return;
            }

            if (value)
            {
                //if (SystemConfig.Instance.UseAutoStart)
                //    StartInspection();
            }
            else
            {
                if (SystemConfig.Instance.UseAutoStop)
                {
                    StopInspection();
                }
            }
        }

        private void OnPlcStartChanged(bool value)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnPlcStartChanged(value)));
                return;
            }

            if (value)
            {
                if (SystemConfig.Instance.UseAutoStart)
                {
                    StartInspection();
                }
            }
            else
            {
                if (SystemConfig.Instance.UseAutoStop)
                {
                    StopInspection();
                }
            }
        }

        private void OnPlcHoldChanged(bool value)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(new Action(() => OnPlcHoldChanged(value)));
                return;
            }

            if (IsStartInspection)
            {
                IsSkipMode = value;
            }
        }

        private void OnDefectOccured(bool value)
        {
            if (value)
            {
                Dispatcher dispatcher = Application.Current.Dispatcher;
                if (!dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke(new Action(() => OnDefectOccured(value)));
                    return;
                }
                DefectOccruedAction();
            }
        }

        private void OnDefectReset(bool value)
        {
            if (value)
            {
                Dispatcher dispatcher = Application.Current.Dispatcher;
                if (!dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke(new Action(() => OnDefectReset(value)));
                    return;
                }
                DefectResetAction();
            }
        }
        #endregion


        #region IO
        private void DefectOccruedAction()
        {
            DeviceManager.SendDefectSignal(true);

            if (SystemConfig.Instance.UseDefectAlarm == true && SystemConfig.Instance.UseIMDefectAlarm == true)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Inspect, (int)InspectError.DefectOccrued, ErrorLevel.Warning,
                    ErrorSection.Inspect.ToString(), InspectError.DefectOccrued.ToString(), TranslationHelper.Instance.Translate("DEFECT_OCCURRED_MESSAGE"));
            }
            else
            {
                // 알람 창을 띄우지 않을 경우에는 일정 시간 이후에 바로 신호를 내려주도록 설정
                Thread.Sleep(200);
                DeviceManager.SendDefectSignal(false);
            }
        }

        private void DefectResetAction()
        {
            if (AlarmWindow != null)
            {
                ErrorManager.Instance().ResetAlarm();
                AlarmWindow.Close();
            }

            DeviceManager.SendDefectSignal(false);
        }
        #endregion
        #endregion
    }
}
