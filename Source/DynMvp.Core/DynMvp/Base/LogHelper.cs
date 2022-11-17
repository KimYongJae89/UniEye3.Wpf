using Infragistics.Win.Misc;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Base
{
    public enum LoggerType
    {
        Error,          // 프로그램 오류 로그
        Grab,           // 영상 획득 관련 로그
        Motion,         // 모션 운영 로그
        Device,         // 장비 운영 로그 ( Motion / Grab 제외 )
        Comm,           // 통신 관련 로그
        Network,
        StartUp,        // 프로그램 기동 로그
        Shutdown,       // 프로그램 종료 로그
        Inspection,     // 검사 관련 로그
        Operation,      // 검사를 제외한 운영 로그. 티칭 등
        Block,          // 함수 호출. Start / End
        Utilization,    // 장비 가동 현황 로그
        Function,       // 함수 호출. Start / End
        Serial,
    }

    public enum LogLevel
    {
        Off,
        Fatal,  // 예측 되지 않은 예외가 발생하고 정상 처리되지 않음. 
                // 예) DB 접속이 되지 않음. 프로그램이 멈춰야할 중대한 장애 상태.
        Error,  // 예측 되지 않은 예외가 발생하고 정상 처리됨.
                //  예) DB 접속이 끊겨 재 접속함. 통신 장애, Grab Timeout, 패킷 이상, 파라미터 설정 오류 등 
        Warn,   // 예측된 예외가 발생하고 처리되어 상황을 기록            예) 파일 시스템이 10% 남음.
        Info,   // 운영자에 정보 제공. 기본적인 운영 기록                 예) 시스템 구동 시간. 읽은 설정 파일 위치 등
        Debug,  // 디버깅 정보. 소소한 기록 등 다른 곳에 해당되지 않은 기록    예) 입력 받은 값. DB에서 읽어온 값
        Trace   // 운영 Sequence 추적. 함수 호출 시작 / 종료
    }

    public static class ILogExtentions
    {
        public static void Trace(this ILog log, string message, Exception exception)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                log4net.Core.Level.Trace, message, exception);
        }

        public static void Trace(this ILog log, string message)
        {
            log.Trace(message, null);
        }
    }

    public delegate void NotifyLogDelegate(LogLevel logLevel, LoggerType loggerType, string message);

    public class LogHelper
    {
        public static NotifyLogDelegate NotifyLog;

        public static void InitializeLogSystem(string logConfigFile)
        {
            var fileInfo = new System.IO.FileInfo(logConfigFile);
            log4net.Config.XmlConfigurator.Configure(fileInfo);
        }

        public static void Debug(LoggerType loggerType, string message)
        {
            LogManager.GetLogger(loggerType.ToString()).Debug(message);
#if DEBUG
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss.fff")}, {loggerType}] {message}");
#endif
            NotifyLog?.BeginInvoke(LogLevel.Debug, loggerType, message, null, null);
        }

        public static void Trace(LoggerType loggerType, string message)
        {
            LogManager.GetLogger(loggerType.ToString()).Trace(message);
            NotifyLog?.BeginInvoke(LogLevel.Trace, loggerType, message, null, null);
        }

        public static void Info(LoggerType loggerType, string message)
        {
            LogManager.GetLogger(loggerType.ToString()).Info(message);
            NotifyLog?.BeginInvoke(LogLevel.Info, loggerType, message, null, null);
        }

        public static void Warn(LoggerType loggerType, string message)
        {
            LogManager.GetLogger(loggerType.ToString()).Warn(message);
            NotifyLog?.BeginInvoke(LogLevel.Warn, loggerType, message, null, null);
        }

        public static void Error(LoggerType loggerType, string message)
        {
            LogManager.GetLogger(loggerType.ToString()).Error(message);
            NotifyLog?.BeginInvoke(LogLevel.Error, LoggerType.Error, message, null, null);
        }

        public static void Error(string message)
        {
            LogManager.GetLogger("Error").Error(message);
            NotifyLog?.BeginInvoke(LogLevel.Error, LoggerType.Error, message, null, null);
        }

        public static void Fatal(string message)
        {
            LogManager.GetLogger("Fatal").Fatal(message);
            NotifyLog?.BeginInvoke(LogLevel.Fatal, LoggerType.Error, message, null, null);
        }
    }

    public class BlockTracer : IDisposable
    {
        private string blockName;
        public BlockTracer(string blockName)
        {
            this.blockName = blockName;
            LogHelper.Trace(LoggerType.Block, "Begin " + blockName);
        }

        public void Dispose()
        {
            LogHelper.Trace(LoggerType.Block, "End " + blockName);
        }
    }
}
