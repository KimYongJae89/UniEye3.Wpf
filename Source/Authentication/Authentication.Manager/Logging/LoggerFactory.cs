using Authentication.Manager.Logging.Extentions;
using Authentication.Manager.Logging.Interfaces;
using Prism.Ioc;
using Prism.Unity;
using System.Collections.Generic;
using Unity;

namespace Authentication.Manager.Logging
{
    public class LoggerFactory
    {
        public static string MethodName => "CreateLogger";

        public ICollection<ILoggerProvider> Providers { get; }

        public LoggerFactory(IContainerRegistry containerRegistry)
        {
            Providers = new List<ILoggerProvider>();

            containerRegistry
                .GetContainer()
                .AddExtension(new LoggerContainerExtension());
        }

        public ILoggerFacade<T> CreateLogger<T>()
        {
            ICollection<ILoggerFacade<T>> loggers = new List<ILoggerFacade<T>>();
            foreach (var provider in Providers)
                loggers.Add(provider.CreateLogger<T>());

            return new CompositeLogger<T>(loggers);
        }
    }
}
