using DynMvp.Base;
using DynMvp.Devices;
using MahApps.Metro;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Helpers;
using Unieye.WPF.Base.Layout.ViewModels;
using Unieye.WPF.Base.Override;
using Unieye.WPF.Base.Services;
using Unieye.WPF.Base.Views;
using UniEye.Base.Config;
using UniEye.Translation.Helpers;
using ICommand = System.Windows.Input.ICommand;

namespace Unieye.WPF.Base.ViewModels
{
    public enum MiniDumpType
    {
        TiniDump = MinidumpWriter.MiniDumpType.TiniDump,
        MiniDump = MinidumpWriter.MiniDumpType.MiniDump,
        MidiDump = MinidumpWriter.MiniDumpType.MidiDump,
        MaxiDump = MinidumpWriter.MiniDumpType.MaxiDump,
    }

    public class SettingViewModel : Observable
    {
        public TranslationHelper TranslationHelper => TranslationHelper.Instance;

        public ICommand LanguageChangedCommand { get; }

        public ICommand TowerLampSettingCommand { get; }

        public ICommand InspectLayoutSettingCommand { get; }

        public ICommand ReportLayoutSettingCommand { get; }

        public ICommand StatisticsLayoutSettingCommand { get; }

        public ICommand LogInCommand { get; }

        public ICommand SaveCommand { get; }

        public AppTheme AppTheme { get => ThemeSelectorService.Theme; set => ThemeSelectorService.Theme = value; }

        public Accent Accent { get => ThemeSelectorService.Accent; set => ThemeSelectorService.Accent = value; }

        public static IEnumerable<Accent> Accents => ThemeSelectorService.Accents;

        public static IEnumerable<AppTheme> AppThemes => ThemeSelectorService.Themes;

        private MiniDumpType dumpType = (MiniDumpType)MinidumpWriter.DumpType;
        public MiniDumpType DumpType
        {
            get => dumpType;
            set => Set(ref dumpType, value);
        }

        public SettingViewModel()
        {
            DumpType = (MiniDumpType)OperationConfig.Instance().DumpType;

            LanguageChangedCommand = new Helpers.RelayCommand<CultureInfo>(LanguageChangedCommandAction);
            TowerLampSettingCommand = new Helpers.RelayCommand(TowerLampSettingCommandAction);
            InspectLayoutSettingCommand = new Helpers.RelayCommand(InspectLayoutSettingCommandAction);
            ReportLayoutSettingCommand = new Helpers.RelayCommand(ReportLayoutSettingCommandAction);
            StatisticsLayoutSettingCommand = new Helpers.RelayCommand(StatisticsLayoutSettingCommandAction);
            LogInCommand = new Helpers.RelayCommand(LogInCommandAction);
            SaveCommand = new Helpers.RelayCommand<ICustomSettingControl>(SaveCommandAction);
        }

        public void LanguageChangedCommandAction(CultureInfo cultureInfo)
        {
            TranslationHelper.Instance.CurrentCultureInfo = cultureInfo;
            LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Language [{cultureInfo.DisplayName}]");
        }

        private async void TowerLampSettingCommandAction()
        {
            var towerLampSettingWindow = new TowerLampSettingWindow();
            await Application.Current.MainWindow.ShowChildWindowAsync(towerLampSettingWindow);
            LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Tower Lamp Setting");
        }

        private async void InspectLayoutSettingCommandAction()
        {
            var view = new Layout.Views.LayoutSettingView();
            var viewModel = new LayoutSettingViewModel(UiManager.Instance.InspectLayoutHandler, UiManager.Instance.InspectLayoutControlTypeList);
            view.DataContext = viewModel;
            if (await MessageWindowHelper.ShowChildWindow<bool>(view) == true)
            {
                UiManager.Instance.InspectLayoutHandler.CopyFrom(viewModel.LayoutHandler);
                UiManager.Instance.OnInspectLayoutChanged?.Invoke(UiManager.Instance.InspectLayoutHandler);
                LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Inspect Layout Setting Complete");
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Setting Page] Cancel Change Inspect Layout Setting");
            }
        }

        private async void ReportLayoutSettingCommandAction()
        {
            var view = new Layout.Views.LayoutSettingView();
            var viewModel = new LayoutSettingViewModel(UiManager.Instance.ReportLayoutHandler, UiManager.Instance.ReportLayoutControlTypeList);
            view.DataContext = viewModel;
            if (await MessageWindowHelper.ShowChildWindow<bool>(view) == true)
            {
                UiManager.Instance.ReportLayoutHandler.CopyFrom(viewModel.LayoutHandler);
                UiManager.Instance.OnReportLayoutChanged?.Invoke(UiManager.Instance.ReportLayoutHandler);
                LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Report Layout Setting Complete");
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Setting Page] Cancel Change Report Layout Setting");
            }
        }

        private async void StatisticsLayoutSettingCommandAction()
        {
            var view = new Layout.Views.LayoutSettingView();
            var viewModel = new LayoutSettingViewModel(UiManager.Instance.StatisticsLayoutHandler, UiManager.Instance.StatisticsLayoutControlTypeList);
            view.DataContext = viewModel;
            if (await MessageWindowHelper.ShowChildWindow<bool>(view) == true)
            {
                UiManager.Instance.StatisticsLayoutHandler.CopyFrom(viewModel.LayoutHandler);
                UiManager.Instance.OnStatisticsLayoutChanged?.Invoke(UiManager.Instance.StatisticsLayoutHandler);
                LogHelper.Info(LoggerType.Operation, $"[Setting Page] Change Statistics Layout Setting Complete");
            }
            else
            {
                LogHelper.Info(LoggerType.Operation, $"[Setting Page] Cancel Change Statistics Layout Setting");
            }
        }

        private void LogInCommandAction()
        {
            LogHelper.Info(LoggerType.Operation, $"[Setting Page] Click Log In");
            var loginWindow = new LoginWindow();
            var loginViewModel = new LoginViewModel(false);
            loginWindow.DataContext = loginViewModel;
            loginWindow.ShowDialog();
        }

        private async void SaveCommandAction(ICustomSettingControl CustomSettingControl)
        {
            LogHelper.Info(LoggerType.Operation, $"[Setting Page] Click Save");
            string header = TranslationHelper.Instance.Translate("Save");
            string message = TranslationHelper.Instance.Translate("SAVE_WARNING_MESSAGE");
            if (await MessageWindowHelper.ShowMessageBox(header, message, MessageBoxButton.OKCancel) == true)
            {
                TowerLamp towerLamp = DeviceManager.Instance().TowerLamp;

                UiConfig.Instance().SaveLanguage(TranslationHelper.CurrentCultureInfo);
                OperationConfig.Instance().DumpType = (int)DumpType;

                MinidumpWriter.DumpType = (MinidumpWriter.MiniDumpType)DumpType;

                var actionList = new List<Action>();
                actionList.Add(() => towerLamp?.Save(BaseConfig.Instance().ConfigPath));
                actionList.Add(() => UiConfig.Instance().Save());
                actionList.Add(() => OperationConfig.Instance().Save());
                actionList.Add(() => UiManager.Instance.InspectLayoutHandler.Save("InspectLayout"));
                actionList.Add(() => UiManager.Instance.ReportLayoutHandler.Save("ReportLayout"));
                actionList.Add(() => UiManager.Instance.StatisticsLayoutHandler.Save("StatisticsLayout"));
                actionList.Add(() => CustomSettingControl?.Save());
                actionList.Add(() => ThemeSelectorService.SaveThemeInSettingsAsync());

                var source = new ProgressSource();
                source.CancellationTokenSource = new System.Threading.CancellationTokenSource();
                await MessageWindowHelper.ShowProgress(TranslationHelper.Translate("Setting"),
                    TranslationHelper.Translate("Save_the_configuration_file") + ("..."),
                    actionList, true, source);

                CustomSettingControl?.AdditionalWork();
                LogHelper.Info(LoggerType.Operation, $"[Setting Page] Save Complete");
            }
            {
                LogHelper.Info(LoggerType.Operation, $"[Setting Page] Cancel Save");
            }
        }
    }
}
