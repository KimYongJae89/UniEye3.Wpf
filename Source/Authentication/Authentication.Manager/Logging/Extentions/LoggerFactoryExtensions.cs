using Authentication.Manager.Logging.Log4Net;
using Authentication.Manager.Logging.Notified;
using System.Linq;

namespace Authentication.Manager.Logging.Extentions
{
    public static class LoggerFactoryExtensions
    {
        public static LoggerFactory AddLog4Net(this LoggerFactory factory)
        {
            factory.Providers.Add(new Log4NetLoggerProvider());
            return factory;
        }

        public static LoggerFactory AddNotyfied(this LoggerFactory factory)
        {
            factory.Providers.Add(new NotifiedLoggerProvider());
            return factory;
        }

        public static LoggerFactory AddLoggerNotifyer(this LoggerFactory factory, ILoggerNotifyer notifyer)
        {
            var loggerFactory = factory as LoggerFactory;

            var providers =
                loggerFactory?
                .Providers
                .Where(p => p is NotifiedLoggerProvider)?
                .Cast<NotifiedLoggerProvider>();

            if (providers == null)
                return factory;

            foreach (var provider in providers)
                provider.Notifyers.Add(notifyer);

            return factory;
        }
    }
}