using Prism.Logging;
using System;

namespace Authentication.Manager.Logging.Notified
{
    public struct NotifiedInfo
    {
        public Type Caller { get; set; }
        public string Message { get; set; }
        public Category Category { get; set; }
        public Priority Priority { get; set; }

        public NotifiedInfo(Type caller, string message, Category category, Priority priority)
        {
            Caller = caller;
            Message = message;
            Category = category;
            Priority = priority;
        }
    }
}
