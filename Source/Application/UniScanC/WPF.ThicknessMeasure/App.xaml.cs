using Authentication.Core;
using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
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
using WPF.ThicknessMeasure.Data;
using WPF.ThicknessMeasure.Model;
using WPF.ThicknessMeasure.Override;

namespace WPF.ThicknessMeasure
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
                var mutex = new Mutex(true, "WPF.ThicknessMeasure", out bool bNew);
                if (!bNew)
                {
                    throw new Exception("Already Running.");
                }

                ApplicationHelper.LoadSettings();

                await ThemeSelectorService.InitializeAsync();

                SystemConfig.Instance.Load();

                ErrorManager.Instance().LoadErrorList(BaseConfig.Instance().ConfigPath);

                if (ApplicationHelper.InitLogSystem() == false)
                {
                    throw new Exception("LogHelper Initialize Fail.");
                }

                ApplicationHelper.InitAuthentication();

                SystemLockHandler.CreateLockFile(Path.Combine(PathConfig.Instance().Temp, "~UniEye.lock"), "");
                if (SystemLockHandler.IsLocked == false)
                {
                    throw new Exception(".lock Create Fail.");
                }

                System.Reflection.AssemblyName asem = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                System.Version version = asem.Version;

                OperationConfig.Instance().UseUserManager = false;
                OperationConfig.Instance().SystemVersion = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
                OperationConfig.Instance().SystemRevision = version.Revision;

                LogHelper.Debug(LoggerType.StartUp, "Start Setup.");

                BuildSystemManager();

                var mainWindow = new Lazy<IShellWindow>(UiManager.Instance.CreateMainWindow);
                MainWindow = mainWindow.Value as Window;

                if (SystemManager.Instance().Setup() == true)
                {
                    var uiMgr = UiManager.Instance as UiManager;
                    uiMgr.MainWindow.Initialize();
#if DEBUG
                    UserHandler.Instance.CurrentUser = new Authentication.Core.Datas.User("developer", "masterkey", true);
#else
                    UserHandler.Instance.CurrentUser = new Authentication.Core.Datas.User("op", "op", false);
#endif
                    LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");
                    MainWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                Shutdown(0);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            SystemManager.Instance()?.Release();
            SystemLockHandler.Dispose();

            GC.Collect();
            GC.WaitForFullGCComplete();
            GC.WaitForPendingFinalizers();

            LogHelper.Debug(LoggerType.Shutdown, "Program is closed normally");
        }

        private void BuildSystemManager()
        {
            TranslationHelper.Initialize(BaseConfig.Instance().ConfigPath);

            var systemManager = new WPF.ThicknessMeasure.Override.SystemManager();
            SystemManager.SetInstance(systemManager);
            systemManager.NonVision = true;

            var modelManager = new Model.ModelManager(new WPF.ThicknessMeasure.Model.ModelBuilder());
            modelManager.Init(PathConfig.Instance().Model);
            Model.ModelManager.SetInstance(modelManager);

            InspectRunner inspectRunner = new Inspect.InspectRunner();
            inspectRunner.InspectRunnerExtender = new Inspect.InspectRunnerExtender();
            inspectRunner.InspectEventHandler = new InspectEventHandler();
            systemManager.AddInspectRunner(inspectRunner);

            systemManager.AddDataExporter(new ThicknessDataExporter());

            DeviceManager.SetInstance(new WPF.ThicknessMeasure.Override.DeviceManager());

            Model.ModelManager.Instance().AddListener(ModelEventListener.Instance);

            var uiManager = new UiManager();
            UiManager.SetInstance(uiManager);
        }
    }
}
