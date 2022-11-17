using Authentication.Manager.Logging.Interfaces;
using System.Collections.Generic;

namespace Authentication.Manager.Logging.Notified
{
    public class NotifiedLoggerProvider : ILoggerProvider
    {
        public ICollection<ILoggerNotifyer> Notifyers { get; }

        public NotifiedLoggerProvider()
        {
            Notifyers = new List<ILoggerNotifyer>();
        }

        public ILoggerFacade<T> CreateLogger<T>()
        {
            return new NotifiedLogger<T>(Notifyers);
        }
    }
}
