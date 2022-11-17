using Authentication.Manager.Logging.Interfaces;
using Prism.Logging;
using System.Collections.Generic;

namespace Authentication.Manager.Logging.Notified
{
    public class NotifiedLogger<T> : ILoggerFacade<T>
    {
        IEnumerable<ILoggerNotifyer> _notifyers;

        public NotifiedLogger(
            IEnumerable<ILoggerNotifyer> notifyers)
        {
            _notifyers = notifyers;
        }

        public void Log(string message, Category category, Priority priority)
        {
            foreach (var notifyer in _notifyers)
                notifyer.Handle(new NotifiedInfo(typeof(T), message, category, priority));
        }
    }
}
