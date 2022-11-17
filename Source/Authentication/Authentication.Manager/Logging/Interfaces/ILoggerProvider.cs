namespace Authentication.Manager.Logging.Interfaces
{
    public interface ILoggerProvider
    {
        ILoggerFacade<T> CreateLogger<T>();
    }
}
