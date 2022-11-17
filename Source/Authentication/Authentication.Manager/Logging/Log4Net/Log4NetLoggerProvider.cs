using Authentication.Manager.Logging.Interfaces;

namespace Authentication.Manager.Logging.Log4Net
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        public ILoggerFacade<T> CreateLogger<T>()
        {
            return new Log4NetLogger<T>();
        }
    }
}
