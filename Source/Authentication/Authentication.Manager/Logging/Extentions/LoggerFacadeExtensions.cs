using Authentication.Manager.Logging.Interfaces;
using Prism.Logging;

namespace Authentication.Manager.Logging.Extentions
{
    public static class LoggerFacadeExtensions
    {
        public static ILoggerFacade<T> Log<T>(this ILoggerFacade<T> facade, string message, Category category)
        {
            facade.Log(message, category, Priority.None);

            return facade;
        }

        public static ILoggerFacade<T> Info<T>(this ILoggerFacade<T> facade, string message)
        {
            return facade.Log(message, Category.Info);
        }

        public static ILoggerFacade<T> Debug<T>(this ILoggerFacade<T> facade, string message)
        {
            return facade.Log(message, Category.Debug);
        }

        public static ILoggerFacade<T> Exception<T>(this ILoggerFacade<T> facade, string message)
        {
            return facade.Log(message, Category.Exception);
        }

        public static ILoggerFacade<T> Warn<T>(this ILoggerFacade<T> facade, string message)
        {
            return facade.Log(message, Category.Warn);
        }

        public static void DbLog<T>(this ILoggerFacade<T> facade, string message)
        {
            facade.Log(message, Category.Info);
        }
    }
}
