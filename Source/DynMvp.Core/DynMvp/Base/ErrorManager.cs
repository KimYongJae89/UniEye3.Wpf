using DynMvp.UI;
using Infragistics.Win;
using Infragistics.Win.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Base
{
    public class ErrorItem
    {
        public int ErrorCode { get; private set; }
        public string SectionStr { get; private set; }
        public string ErrorStr { get; private set; }
        public string Message { get; private set; }
        public string ResasonMsg { get; private set; }
        public string SolutionMsg { get; private set; }
        public ErrorLevel ErrorLevel { get; private set; }
        public bool Displayed { get; set; }
        public bool Alarmed { get; set; }
        public DateTime ErrorTime { get; private set; }

        public Action ResetHandler;

        public ErrorItem(int errorCode, ErrorLevel errorLevel, string sectionStr, string errorStr, string message, string resasonMsg = "", string solutionMsg = "")
        {
            ErrorTime = DateTime.Now;
            ErrorCode = errorCode;
            ErrorLevel = errorLevel;
            SectionStr = sectionStr;
            ErrorStr = errorStr;
            Message = message;
            ResasonMsg = resasonMsg;
            SolutionMsg = solutionMsg;
            Displayed = false;
            Alarmed = true;
        }

        public ErrorItem(string valueStr)
        {
            SetData(valueStr);
        }

        private void SetData(string valueStr)
        {
            string[] tokens = valueStr.Split(';');

            ErrorTime = DateTime.Parse(tokens[0]);
            ErrorCode = Convert.ToInt32(tokens[1]);
            ErrorLevel = (ErrorLevel)Enum.Parse(typeof(ErrorLevel), tokens[2]);
            SectionStr = tokens[3];
            ErrorStr = tokens[4];
            Message = tokens[5];
            ResasonMsg = tokens[6];
            SolutionMsg = tokens[7];

            Displayed = true;
        }

        public override string ToString()
        {
            return $"{ErrorTime.ToString("yyyy/MM/dd HH:mm:ss")};{ErrorCode};{ErrorLevel};{SectionStr};{ErrorStr};{Message};{ResasonMsg};{SolutionMsg}";
        }
    }

    public class AlarmException : ApplicationException
    {
        private string defaultMessage = StringManager.GetString("Application Alarm");

        public AlarmException()
        {
            LogHelper.Error(defaultMessage);
        }
        public AlarmException(string message)
            : base(message)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
        public AlarmException(string message, Exception innerEx)
            : base(message, innerEx)
        {
            LogHelper.Error(string.Format("{0} - {1}", defaultMessage, message));
        }
    }

    public delegate void AlarmEventDelegate();

    public class ErrorManager
    {
        private static ErrorManager instance = null;

        public AlarmEventDelegate OnStartAlarm;
        public AlarmEventDelegate OnResetAlarmStatus;
        public AlarmEventDelegate OnStopRunProcess;

        internal void StopProcess()
        {
            foreach (ErrorItem errorItem in ErrorItemList)
            {
                errorItem.Alarmed = false;
            }

            OnStopRunProcess?.Invoke();
        }
        public string FileName { get; set; }
        public bool BuzzerOn { get; set; }
        public List<ErrorItem> ErrorItemList { get; } = new List<ErrorItem>();
        public bool Updated { get; set; } = true;

        public List<ErrorItem> ActiveErrorItemList
        {
            get
            {
                var activeErrorItemList = new List<ErrorItem>();

                ErrorItemList.ForEach(x => { if (x.Alarmed) { activeErrorItemList.Add(x); } });

                return activeErrorItemList;
            }
        }

        private object lockObject = new object();

        private ErrorManager()
        {

        }

        public static ErrorManager Instance()
        {
            if (instance == null)
            {
                instance = new ErrorManager();
            }

            return instance;
        }

        public void ResetAlarm()
        {
            lock (ErrorItemList)
            {
                foreach (ErrorItem errorItem in ErrorItemList)
                {
                    errorItem.Alarmed = false;
                    errorItem.ResetHandler?.Invoke();
                }
            }

            OnResetAlarmStatus?.Invoke();
        }

        public bool IsAlarmed()
        {
            lock (ErrorItemList)
            {
                foreach (ErrorItem errorItem in ErrorItemList)
                {
                    if (errorItem.Alarmed == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void ThrowIfAlarm()
        {
            if (IsAlarmed())
            {
                throw new AlarmException();
            }
        }

        public bool IsError()
        {
            lock (ErrorItemList)
            {
                foreach (ErrorItem errorItem in ErrorItemList)
                {
                    if (errorItem.Alarmed == true && errorItem.ErrorLevel != ErrorLevel.Warning)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsWarning()
        {
            lock (ErrorItemList)
            {
                foreach (ErrorItem errorItem in ErrorItemList)
                {
                    if (errorItem.Alarmed == true && errorItem.ErrorLevel == ErrorLevel.Warning)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public ErrorItem Report(int errorSection, int errorType, ErrorLevel errorLevel, string sectionStr, string errorStr, string message, string reasonMsg = "", string solutionMsg = "")
        {
            return Report(errorSection + errorType, errorLevel, sectionStr, errorStr, message, reasonMsg, solutionMsg);
        }

        public ErrorItem Report(int errorCode, ErrorLevel errorLevel, string sectionStr, string errorStr, string message, string reasonMsg = "", string solutionMsg = "")
        {
            //if (errorItemList.Count() > 0)
            //{
            //    ErrorItem lastErrorItem = errorItemList.Last();
            //    if(lastErrorItem.ErrorCode == errorCode)
            //    {
            //        TimeSpan elapsedTime = DateTime.Now - lastErrorItem.ErrorTime;
            //        if(elapsedTime.Minutes < 1)
            //            return;
            //    }
            //}

            lock (lockObject)
            {
                OnStartAlarm?.Invoke();

                var errorItem = new ErrorItem(errorCode, errorLevel, sectionStr, errorStr, message, reasonMsg, solutionMsg);
                BuzzerOn = true;
                ErrorItemList.Add(errorItem);
                SaveErrorList();

                return errorItem;
            }
        }

        public void LoadErrorList(string configPath)
        {
            FileName = configPath + "\\ErrorList.txt";
            if (File.Exists(FileName) == false)
            {
                return;
            }

            //var stringLines = File.ReadAllLines(FileName);

            //foreach (var line in stringLines)
            //{
            //    var errorItem = new ErrorItem(line);
            //    ErrorItemList.Add(errorItem);
            //}
        }

        public void SaveErrorList()
        {
            var stringBuilder = new StringBuilder();
            foreach (ErrorItem errorItem in ErrorItemList)
            {
                stringBuilder.Append(errorItem.ToString());
                stringBuilder.AppendLine();
            }

            File.WriteAllText(FileName, stringBuilder.ToString());
        }
    }
}
