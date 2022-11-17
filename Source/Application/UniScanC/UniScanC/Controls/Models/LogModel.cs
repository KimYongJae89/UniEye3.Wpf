using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using UniScanC.Data;
using UniScanC.Module;

namespace UniScanC.Controls.Models
{
    public class LogModel
    {
        public string DateTimeString { get; set; } = "";
        public LogLevel LogLevel { get; set; } = LogLevel.Info;
        public LoggerType LoggerType { get; set; } = LoggerType.Operation;
        public string Message { get; set; } = "";

        public LogModel(LogLevel logLevel, LoggerType loggerType, string message)
        {
            DateTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ");
            LogLevel = logLevel;
            LoggerType = loggerType;
            Message = message;
        }
    }
}
