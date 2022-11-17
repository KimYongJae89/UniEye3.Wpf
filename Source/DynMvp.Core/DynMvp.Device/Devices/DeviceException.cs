using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices
{
    public class DeviceException : ExceptionBase
    {
        public DeviceException(string message) : base(LoggerType.Device, message)
        {
        }

        public DeviceException(string message, Exception innerEx)
            : base(LoggerType.Device, message, innerEx)
        {
        }

        public DeviceException(LogLevel logLevel, string message, Exception innerEx)
            : base(LoggerType.Device, logLevel, message, innerEx)
        {
        }

        public DeviceException(LoggerType loggerType, LogLevel logLevel, string message, Exception innerEx)
            : base(loggerType, logLevel, message, innerEx)
        {
        }
    }
}
