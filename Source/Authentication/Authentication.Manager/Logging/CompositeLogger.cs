using Authentication.Manager.Logging.Interfaces;
using Prism.Logging;
using System.Collections.Generic;

namespace Authentication.Manager.Logging
{
    public class CompositeLogger<T> : ILoggerFacade<T>
    {
        IEnumerable<ILoggerFacade<T>> _loggers;

        public CompositeLogger(IEnumerable<ILoggerFacade<T>> loggers)
        {
            _loggers = loggers;
        }

        public void Log(string message, Category category, Priority priority)
        {
            foreach (var logger in _loggers)
                logger.Log(message, category, priority);
        }
    }
}
