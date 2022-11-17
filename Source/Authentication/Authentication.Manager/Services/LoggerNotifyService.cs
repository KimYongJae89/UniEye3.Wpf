using Authentication.Manager.Logging.Notified;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Authentication.Manager.Services
{
    public class LoggerNotifyService : ILoggerNotifyer
    {
        IList<NotifiedInfo> _logs { get; } = new ObservableCollection<NotifiedInfo>();
        public IEnumerable<NotifiedInfo> Logs => _logs;

        public LoggerNotifyService()
        {
            BindingOperations.EnableCollectionSynchronization(Logs, new object());
        }

        public void Handle(NotifiedInfo info)
        {
            _logs.Insert(0, info);
        }

        public Task HandleAsync(NotifiedInfo info)
        {
            return null;
        }
    }
}
