using Authentication.Manager.Logging.Extentions;
using Authentication.Manager.Logging.Interfaces;
using Authentication.Manager.Logging.Notified;
using System.Collections.Generic;

namespace Authentication.Manager.ViewModels
{
    public class LogViewModel
    {
        ILoggerFacade<LogViewModel> _logger;

        ILoggerNotifyer _notifyer;
        public IEnumerable<NotifiedInfo> Logs => _notifyer.Logs;

        public LogViewModel(ILoggerNotifyer notifyer, ILoggerFacade<LogViewModel> logger)
        {
            _notifyer = notifyer;
            _logger = logger;

            _logger.Info("Initialized");
        }
    }
}
