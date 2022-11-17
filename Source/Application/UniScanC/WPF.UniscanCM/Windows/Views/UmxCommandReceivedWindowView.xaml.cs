using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Unieye.WPF.Base.Helpers;
using UniEye.Translation.Helpers;
using UniScanC.Enums;
using UniScanC.Module;
using CommManager = WPF.UniScanCM.Override.CommManager;

namespace WPF.UniScanCM.Windows.Views
{
    /// <summary>
    /// UmxCommandRececveControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UmxCommandReceivedWindowView : ChildWindow, INotifyPropertyChanged
    {
        #region Observable

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private string[] args;
        private DispatcherTimer timer = new DispatcherTimer();

        private ObservableCollection<ModuleState> moduleStatusList = new ObservableCollection<ModuleState>();
        public ObservableCollection<ModuleState> ModuleStatusList
        {
            get => moduleStatusList;
            set => Set(ref moduleStatusList, value);
        }

        private int moduleCount;
        public int ModuleCount
        {
            get => moduleCount;
            set => Set(ref moduleCount, value);
        }

        private int progressValue;
        public int ProgressValue
        {
            get => progressValue;
            set => Set(ref progressValue, value);
        }

        private bool isProgress;
        public bool IsProgress
        {
            get => isProgress;
            set
            {
                Set(ref isProgress, value);
                OnPropertyChanged("Glyph");
                OnPropertyChanged("ButtonText");
            }
        }

        private EUniScanCCommand executeCommand;
        public EUniScanCCommand ExecuteCommand
        {
            get => executeCommand;
            set => Set(ref executeCommand, value);
        }

        public string Glyph => IsProgress ? "\uE711" : "\uE711";
        public string ButtonText => IsProgress ? "Cancel" : "Close";

        public ICommand CancelCommand { get; }

        public UmxCommandReceivedWindowView(EUniScanCCommand command, string[] args = null)
        {
            InitializeComponent();

            DataContext = this;
            this.args = args;

            ExecuteCommand = command;

            CancelCommand = new RelayCommand(async () =>
            {
                timer.Stop();

                var list = new List<ModuleState>(ModuleStatusList);
                int failCount = list.Count(f => f.IsCommandFail(ExecuteCommand));
                Debug.WriteLine($"ChildWindow_Unloaded : {failCount}");
                if (failCount > 0)
                {
                    var messages = list.Select(f => f.IsCommandFail(executeCommand)
                    ? $"[{f.ToString()}]: {TranslationHelper.Instance.Translate(f.GetCommandResult(executeCommand))}" : "").ToList();
                    messages.RemoveAll(f => string.IsNullOrEmpty(f));
                    string message = string.Join(Environment.NewLine, messages);
                    await MessageWindowHelper.ShowMessageBox(
                          TranslationHelper.Instance.Translate("NOTIFICATION"),
                          message,
                          System.Windows.MessageBoxButton.OK);
                }
                Close(false);
            });
        }

        private async void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<ModuleState> moduleStateList = ModuleManager.Instance.ModuleStateList;
            ModuleCount = moduleStateList.Count;

            IsProgress = true;
            foreach (ModuleState moduleState in moduleStateList)
            {
                moduleState.ResetCommandDone(ExecuteCommand);
                ModuleStatusList.Add(moduleState);
            }

            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Command_Tick;
            timer.Start();

            if (!await SendCommand(ExecuteCommand, args))
            {
                moduleStateList.ForEach(f => f.SetCommandDone(ExecuteCommand, "Command Send Fail"));
            }

        }

        private async Task<bool> SendCommand(EUniScanCCommand command, string[] args)
        {
            return await Task<bool>.Run(() =>
            {
                int retryCount = 5;
                bool isCmdSuccess = false;

                for (int i = 0; i < retryCount; i++)
                {
                    if ((bool)CommManager.Instance().SendMessage(command, args))
                    {
                        isCmdSuccess = true;
                        break;
                    }

                    System.Threading.Thread.Sleep(1000);
                }

                return isCmdSuccess;
            });
        }

        private void Command_Tick(object sender, EventArgs e)
        {
            // Datagrid를 갱신하기 위해 값을 변화시킴
            var list = new List<ModuleState>(ModuleStatusList);
            ModuleStatusList = null;

            int doneCount = list.Count(f => f.IsCommandDone(ExecuteCommand));
            int failCount = list.Count(f => f.IsCommandFail(ExecuteCommand));

            ModuleStatusList = new ObservableCollection<ModuleState>(list);

            ProgressValue = doneCount;
            if (ModuleStatusList.Count == doneCount)
            {
                IsProgress = false;
                timer.Stop();
                if (failCount == 0)
                {
                    Close(true);
                }
            }
        }
    }
}
