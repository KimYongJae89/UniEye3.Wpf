using DynMvp.Base;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.Models;
using Unieye.WPF.Base.Layout.ViewModels;
using UniEye.Base.Data;
using UniEye.Translation.Helpers;
using UniScanC.Data;
using UniScanC.Windows.ViewModels;
using UniScanC.Windows.Views;
using WPF.ThicknessMeasure.Controls.Views;
using WPF.ThicknessMeasure.Data;
using WPF.ThicknessMeasure.Inspect;
using WPF.ThicknessMeasure.Model;
using WPF.ThicknessMeasure.Override;
using static WPF.ThicknessMeasure.Inspect.InspectEventListener;
using static WPF.ThicknessMeasure.Model.ModelEventListener;

namespace WPF.ThicknessMeasure.Pages.ViewModels
{
    public class MonitoringPageViewModel : Observable
    {
        #region 속성
        private string lotNo;
        public string LotNo
        {
            get => lotNo;
            set => Set(ref lotNo, value);
        }

        private double inspectLength = 0;
        public double InspectLength
        {
            get => inspectLength;
            set => Set(ref inspectLength, value);
        }

        private Model.Model model;
        public Model.Model Model
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

        private bool onInspect = false;
        public bool OnInspect
        {
            get => onInspect;
            set => Set(ref onInspect, value);
        }

        private UniScanC.Models.StatusModel statusModel;
        public UniScanC.Models.StatusModel StatusModel
        {
            get => statusModel;
            set => Set(ref statusModel, value);
        }

        private DeviceManager DeviceManager => DeviceManager.Instance() as DeviceManager;

        private CancellationTokenSource InspectionCancelToken { get; set; }

        private ModelOpenEventDelegate UpdateModelDelegate { get; set; }

        private UpdateResultEventDelegate UpdateResultDelegate { get; set; }

        private AlarmMessageWindowView AlarmWindow { get; set; } = null; //시스템 알람 관리 창

        private DispatcherTimer AlarmCheckTimer { get; set; } = null;

        public System.Windows.Input.ICommand StartCommand { get; }

        public System.Windows.Input.ICommand StopCommand { get; }

        public System.Windows.Input.ICommand NextLotNoCommand { get; }

        public System.Windows.Input.ICommand SetSpeedCommand { get; }
        #endregion


        #region 생성자
        public MonitoringPageViewModel()
        {
            StartCommand = new RelayCommand(StartCommandAction);
            StopCommand = new RelayCommand(StopCommandAction);
            NextLotNoCommand = new RelayCommand(NextLotNoCommandAction);

            LotNo = SystemConfig.Instance.LastLotNo;

            OnLayoutChanged(UiManager.Instance.InspectLayoutHandler);
            UiManager.Instance.OnInspectLayoutChanged += OnLayoutChanged;

            ModelEventListener.Instance.OnModelOpened += ModelOpened;
            ModelEventListener.Instance.OnModelClosed += ModelClosed;

            InspectEventListener.Instance.UpdateResult += OnUpdateResult;

            SystemState.Instance().OpStateChanged += OpStateChanged;
            SystemState.Instance().SetIdle();

            AlarmCheckTimer = new DispatcherTimer();
            AlarmCheckTimer.Tick += AlarmCheckTimer_Tick;
            AlarmCheckTimer.Interval = TimeSpan.FromMilliseconds(500);
            AlarmCheckTimer.Start();
        }
        #endregion


        #region 메서드
        private void OnSystemConfigChanged(object sender, PropertyChangedEventArgs e)
        {
            //if(e.PropertyName == "AutoLotPrefix" || e.PropertyName == "AutoLotNumber")
            //    NextLotNo = SystemConfig.Instance.GetLotNo();
        }

        private void OnUpdateResult(IEnumerable<ProductResult> productResults, CancellationTokenSource taskCancelToken = null)
        {
            DynMvp.Base.LogHelper.Debug(DynMvp.Base.LoggerType.Inspection, "MonitoringPageModel - OnUpdateResults / Start");
            if (productResults == null)
            {
                Task.Run(() => UpdateResultDelegate?.Invoke(null));
            }
            else
            {
                // InspectResult Casting 을 위한 리스트 작성
                // Cast는 위험하므로 이런 방식으로 진행
                var filteredList = new List<ThicknessResult>();
                foreach (ProductResult productResult in productResults)
                {
                    if (productResult is ThicknessResult measureResult)
                    {
                        filteredList.Add(measureResult);
                    }
                }

                InspectLength = filteredList.Max(x => x.ScanData.Values.Max(y => y.ReelPosition));

                Task.Run(() => UpdateResultDelegate?.Invoke(filteredList));
            }
            DynMvp.Base.LogHelper.Debug(DynMvp.Base.LoggerType.Inspection, "MonitoringPageModel - OnUpdateResults / End");
        }

        private void OpStateChanged()
        {
            StatusModel = UniScanC.Models.OpStateModel.StatusModel(SystemState.Instance().OpState);
        }

        #region Layout
        private void OnLayoutChanged(LayoutHandler layoutHandler)
        {
            LayoutHandler = layoutHandler;
            LayoutViewModel = new LayoutViewModel(LayoutHandler);
            LinkModelDelegate();
            LinkProductResultsDelegate();
            //LinkLogDelegate();
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
            }
        }
        #endregion

        private void ModelOpened(DynMvp.Data.ModelBase model)
        {
            Model = model as Model.Model;

            UpdateModelDelegate?.Invoke(model);
        }

        private void ModelClosed()
        {
            Model = null;
        }

        private void StartCommandAction()
        {
            // 측정 길이 0으로 초기화
            InspectLength = 0;

            SystemManager.Instance().InspectRunner.EnterWaitInspection();
            SystemManager.Instance().InspectRunner.Inspect();
            OnInspect = true;
        }

        private void StopCommandAction()
        {
            SystemManager.Instance().InspectRunner.ExitWaitInspection();
            OnInspect = false;

            if (AlarmWindow != null)
            {
                ErrorManager.Instance().ResetAlarm();
                AlarmWindow.Close();
            }
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
        #endregion
    }
}
