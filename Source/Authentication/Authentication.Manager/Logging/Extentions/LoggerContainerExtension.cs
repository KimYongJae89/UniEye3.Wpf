using Prism.Logging;
using System.Linq;
using Unity;
using Unity.Builder;
using Unity.Extension;
using Unity.Strategies;

namespace Authentication.Manager.Logging.Extentions
{
    public class LoggerContainerExtension : UnityContainerExtension
    {
        public LoggerContainerExtension()
        {

        }

        protected override void Initialize()
        {
            Context.Strategies.Add(new LoggerBuilderStrategy(), UnityBuildStage.PreCreation);
        }
    }

    public class LoggerBuilderStrategy : BuilderStrategy
    {
        public LoggerBuilderStrategy()
        {

        }

        public override void PreBuildUp(ref BuilderContext context)
        {
            if (context.RegistrationType.IsInterface && typeof(ILoggerFacade).IsAssignableFrom(context.RegistrationType))
            {
                var loggerFactory = context.Container.Resolve<LoggerFactory>();
                var type = context.RegistrationType.GenericTypeArguments.First();
                var method = typeof(LoggerFactory).GetMethod(LoggerFactory.MethodName);
                var generic = method.MakeGenericMethod(type);

                context.Existing = generic.Invoke(loggerFactory, null);
            }

            base.PreBuildUp(ref context);
        }
    }
}
