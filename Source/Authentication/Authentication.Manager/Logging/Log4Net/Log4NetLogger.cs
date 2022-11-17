using Authentication.Manager.Logging.Interfaces;
using log4net;
using Prism.Logging;

namespace Authentication.Manager.Logging.Log4Net
{
    public class Log4NetLogger<T> : ILoggerFacade<T>
    {
        ILog _logger;

        public Log4NetLogger()
        {
            _logger = log4net.LogManager.GetLogger(typeof(T));
        }


        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    _logger.Debug(message);
                    break;
                case Category.Exception:
                    _logger.Error(message);
                    break;
                case Category.Info:
                    _logger.Info(message);
                    break;
                case Category.Warn:
                    _logger.Warn(message);
                    break;
            }
        }
    }
}
