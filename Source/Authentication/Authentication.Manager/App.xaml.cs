using Authentication.Manager.Logging;
using Authentication.Manager.Logging.Extentions;
using Authentication.Manager.Logging.Notified;
using Authentication.Manager.Properties;
using Authentication.Manager.Services;
using Authentication.Manager.ViewModels;
using Authentication.Manager.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using System.Globalization;
using System.Windows;
using UniEye.Translation.Helpers;
using Unity;

namespace Authentication.Manager
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void OnInitialized()
        {
            TranslationHelper.Initialize("Authentication.Manager.Strings.Resources", typeof(App).Assembly);
            TranslationHelper.Instance.CurrentCultureInfo = new CultureInfo(Settings.Default.Culture);

            base.OnInitialized();
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var notifyService = new LoggerNotifyService();

            var loggerFactory = new LoggerFactory(containerRegistry);
            loggerFactory
                .AddNotyfied()
                .AddLog4Net()
                .AddLoggerNotifyer(notifyService);

            containerRegistry.RegisterInstance<LoggerFactory>(loggerFactory);
            containerRegistry.RegisterInstance<ILoggerNotifyer>(notifyService);

            containerRegistry.RegisterSingleton<AuthenticationService>();

            containerRegistry.RegisterForNavigation<LogView, LogViewModel>("Log");
            containerRegistry.RegisterForNavigation<RoleView, RoleViewModel>("Role");
            containerRegistry.RegisterForNavigation<SettingView, SettingViewModel>("Setting");
            containerRegistry.RegisterForNavigation<UserView, UserViewModel>("User");
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<Shell, ShellViewModel>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
