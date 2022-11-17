using Authentication.Core.Database;
using Authentication.Manager.Logging.Extentions;
using Authentication.Manager.Logging.Interfaces;
using Authentication.Manager.Properties;
using Authentication.Manager.Services;
using System;
using System.Globalization;
using UniEye.Translation.Helpers;

namespace Authentication.Manager.ViewModels
{
    public class SettingViewModel
    {
        public CultureInfo CurrentCultureInfo
        {
            get => TranslationHelper.Instance.CurrentCultureInfo;
            set => TranslationHelper.Instance.CurrentCultureInfo = value;
        }

        public CultureInfo[] CultureInfos => TranslationHelper.CultureInfos;
        ILoggerFacade<SettingViewModel> _logger;

        public string[] DatabaseTypes => Enum.GetNames(typeof(AuthDatabaseType));

        public string DbType
        {
            get => Settings.Default.DbType;
            set
            {
                Settings.Default.DbType = value;

                _authenticationService.ChangeDbType((AuthDatabaseType)Enum.Parse(typeof(AuthDatabaseType), value));
            }
        }

        AuthenticationService _authenticationService;

        public SettingViewModel(AuthenticationService authenticationService, ILoggerFacade<SettingViewModel> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _logger.Info("Initialized");
        }
    }
}
