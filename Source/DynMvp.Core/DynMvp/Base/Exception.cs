using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Base
{
    public class ExceptionBase : Exception
    {
        private LoggerType loggerType;
        private LogLevel logLevel;

        public ExceptionBase(LoggerType loggerType, string message) : base(message)
        {
            this.loggerType = loggerType;
            logLevel = LogLevel.Error;
        }

        public ExceptionBase(LoggerType loggerType, string message, Exception innerEx) : base(message, innerEx)
        {
            this.loggerType = LoggerType.Device;
            logLevel = LogLevel.Error;
        }

        public ExceptionBase(LoggerType loggerType, LogLevel logLevel, string message, Exception innerEx) : base(message, innerEx)
        {
            this.loggerType = loggerType;
            this.logLevel = logLevel;
        }

        public void Report()
        {
            switch (logLevel)
            {
                case LogLevel.Fatal:
                    LogHelper.Fatal(Message);
                    break;
                case LogLevel.Error:
                    LogHelper.Error(Message);
                    break;
                case LogLevel.Warn:
                    LogHelper.Warn(loggerType, Message);
                    break;
            }

            LogHelper.Debug(LoggerType.Device, StackTrace);

            if (InnerException != null)
            {
                LogHelper.Debug(LoggerType.Device, InnerException.StackTrace);
            }
        }
    }

    public class GrabFailException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Grab Fail");

        public GrabFailException()
        {
            LogHelper.Error(defaultMessage);
        }
        public GrabFailException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public GrabFailException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    // Vision Object의 유효성에 문제가 있을 때 발생하는 Exception
    public class InvalidObjectException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Invalid Object");

        public InvalidObjectException()
            : base()
        {
            LogHelper.Error(defaultMessage);
        }
        public InvalidObjectException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidObjectException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class AllocFailedException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Allocation Failed");

        public AllocFailedException()
            : base()
        {
            LogHelper.Error(defaultMessage);
        }
        public AllocFailedException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public AllocFailedException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidModelNameException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Invalid Model Name");

        public InvalidModelNameException()
            : base()
        {
            LogHelper.Error(defaultMessage);
        }
        public InvalidModelNameException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidModelNameException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidSourceException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Invalid Source");

        public InvalidSourceException()
            : base()
        {
            LogHelper.Error(defaultMessage);
        }
        public InvalidSourceException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidSourceException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidTargetException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Invalid Source");

        public InvalidTargetException()
            : base()
        {
            LogHelper.Error(defaultMessage);
        }
        public InvalidTargetException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidTargetException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidDataException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Data format is invalid");

        public InvalidDataException()
        {
            LogHelper.Error(defaultMessage);
        }
        public InvalidDataException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidDataException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class TooManyItemsException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Excess number of items");

        public TooManyItemsException()
        {
            LogHelper.Error(defaultMessage);
        }
        public TooManyItemsException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public TooManyItemsException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidImageFormatException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Invalid Image");

        public InvalidImageFormatException()
        {
            LogHelper.Error(defaultMessage);
        }
        public InvalidImageFormatException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidImageFormatException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidTypeException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Invalid Type");

        public InvalidTypeException()
        {
            LogHelper.Error(defaultMessage);
        }
        public InvalidTypeException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidTypeException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class DepthScannerInitializeFailException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Depth Scanner Initialization is Failed");

        public DepthScannerInitializeFailException()
        {
            LogHelper.Error(defaultMessage);
        }
        public DepthScannerInitializeFailException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public DepthScannerInitializeFailException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class CameraInitializeFailException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Camera Initialization is Failed");

        public CameraInitializeFailException()
        {
            LogHelper.Error(defaultMessage);
        }
        public CameraInitializeFailException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public CameraInitializeFailException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public class InvalidResourceException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Camera Initialization is Failed");

        public InvalidResourceException()
        {
            LogHelper.Error(defaultMessage);
        }
        public InvalidResourceException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public InvalidResourceException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }
}
