using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Manager.Logging.Notified
{
    public interface ILoggerNotifyer
    {
        IEnumerable<NotifiedInfo> Logs { get; }

        void Handle(NotifiedInfo info);
        Task HandleAsync(NotifiedInfo info);
    }
}
