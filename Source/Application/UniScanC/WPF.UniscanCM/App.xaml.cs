using Authentication.Core;
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Services;
using Unieye.WPF.Base.Views;
using UniEye.Base.Config;
using UniEye.Base.Inspect;
using UniEye.Base.Util;
using UniEye.Translation.Helpers;
using UniScanC.AlgoTask;
using UniScanC.Models;
using WPF.UniScanCM.Events;
using WPF.UniScanCM.Override;
using SystemManager = WPF.UniScanCM.Override.SystemManager;

namespace WPF.UniScanCM
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                var mutex = new Mutex(true, "WPF.UniScanCM", out bool bNew);
                if (!bNew)
                {
                    throw new Exception("Program is already running.");
                }

                ApplicationHelper.LoadSettings();
                ApplicationHelper.InitAuthentication();

                await ThemeSelectorService.InitializeAsync();
                await SystemConfig.Instance.LoadAsync();

                ErrorManager.Instance().LoadErrorList(BaseConfig.Instance().ConfigPath);

                System.Reflection.AssemblyName asem = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                Version version = asem.Version;

                OperationConfig.Instance().UseUserManager = false;
                OperationConfig.Instance().SystemVersion = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
                OperationConfig.Instance().SystemRevision = version.Revision;

                if (ApplicationHelper.InitLogSystem() == false)
                {
                    throw new Exception("LogHelper Initialize Fail");
                }

                if (ApplicationHelper.CheckLicense() == false)
                {
                    throw new Exception("License Check Fail");
                }

                SystemLockHandler.CreateLockFile(Path.Combine(PathConfig.Instance().Temp, "~UniEye.lock"), "");
                if (SystemLockHandler.IsLocked == false)
                {
                    throw new Exception(".lock Create Fail");
                }

                LogHelper.Debug(LoggerType.StartUp, "App::OnStartup - Start Setup.");

                BuildSystemManager();

                var mainWindow = new Lazy<IShellWindow>(UiManager.Instance.CreateMainWindow);
                MainWindow = mainWindow.Value as Window;
                MainWindow.Closing += OnMainWindowClosing;

                if (SystemManager.Instance().Setup())
                {
                    var uiMgr = UiManager.Instance as UiManager;
                    uiMgr.MainWindow.Initialize();
#if DEBUG
                    UserHandler.Instance.CurrentUser = new Authentication.Core.Datas.User("developer", "masterkey", true);
#else
                    UserHandler.Instance.CurrentUser = new Authentication.Core.Datas.User("op", "op", false);
#endif
                    LogHelper.Debug(LoggerType.StartUp, "App::OnStartup - Finish Setup.");
                    MainWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                Shutdown(0);
            }
        }

        private void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            var uimgr = UiManager.Instance as UiManager;
            uimgr.DefectDetailView.Close();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            SystemManager.Instance()?.Release();
            SystemLockHandler.Dispose();

            GC.Collect();
            GC.WaitForFullGCComplete();
            GC.WaitForPendingFinalizers();

            LogHelper.Debug(LoggerType.Shutdown, "App::OnExit - Program is closed normally");
        }

        private void BuildSystemManager()
        {
            TranslationHelper.Initialize(BaseConfig.Instance().ConfigPath);

            DeviceManager.SetInstance(new DeviceManager());
            DeviceMonitor.SetInstance(new DeviceMonitor());

            CommManager.SetInstance(new CommManager());
            CommManager.Instance().Connect();

            var systemManager = new SystemManager();
            SystemManager.SetInstance(systemManager);
            systemManager.NonVision = true;

            InspectRunner inspectRunner = new LineScanInspectRunner();
            inspectRunner.InspectRunnerExtender = new UniScanC.Inspect.InspectRunnerExtender();
            inspectRunner.InspectEventHandler = new InspectEventHandler();
            systemManager.AddInspectRunner(inspectRunner);

            var modelManager = new UniScanC.Models.ModelManager(new UniScanCM.Override.ModelBuilder(), PathConfig.Instance().Model);
            modelManager.Init(PathConfig.Instance().Model);
            ModelManager.SetInstance(modelManager);
            ModelManager.Instance().AddListener(ModelEventListener.Instance);

            var uiManager = new UiManager();
            UiManager.SetInstance(uiManager);

            // ATM 초기값 저장. 
            AlgoTaskSettingDefault.SaveAll(Path.Combine(BaseConfig.Instance().ConfigPath, "DefaultAlgoTaskSet"));
        }
    }
}
