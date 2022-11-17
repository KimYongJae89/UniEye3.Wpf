using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UniEye.Base.Config;
using UniEye.Base.Settings;

namespace UniEye.Base.Data
{
    public class PathManager
    {
        public static DataPathType DataPathType { get; set; } = DataPathType.Model_Day;

        public static string GetResultPath(string modelName, DateTime dateTime, string serialNo, int sequenceNo = -1, bool isCreatePath = true)
        {
            return GetResultPath(PathConfig.Instance().Result, modelName, dateTime, serialNo, sequenceNo, isCreatePath);
        }

        public static string GetRemoteResultPath(string modelName, DateTime dateTime, string serialNo, int sequenceNo = -1)
        {
            return GetResultPath(PathConfig.Instance().RemoteResult, modelName, dateTime, serialNo, sequenceNo);
        }

        public static string GetResultPath(string resultPath, string modelName, DateTime dateTime, string serialNo, int sequenceNo = -1, bool isCreatePath = true)
        {
            string path;
            DateTime curTime = DateTime.Now;

            switch (DataPathType)
            {
                default:
                case DataPathType.Model_Day:
                    path = string.Format("{0}\\{1}\\{2}", resultPath, modelName, curTime.ToString("yyyyMMdd"));
                    break;
                case DataPathType.Model_Day_Hour:
                    path = string.Format("{0}\\{1}\\{2}\\{3}", resultPath, modelName, curTime.ToString("yyyyMMdd"), curTime.ToString("HH"));
                    break;
                case DataPathType.Day_Model:
                    path = string.Format("{0}\\{1}\\{2}", resultPath, curTime.ToString("yyyyMMdd"), modelName);
                    break;
                case DataPathType.Day_Hour_Model:
                    path = string.Format("{0}\\{1}\\{2}\\{3}", resultPath, curTime.ToString("yyyyMMdd"), curTime.ToString("HH"), modelName);
                    break;
            }

            path += "\\" + serialNo;

            if (sequenceNo > -1)
            {
                path += "\\" + sequenceNo.ToString();
            }

            if (isCreatePath)
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetCustumPath(params string[] paths)
        {
            string path = paths[0];

            for (int i = 1; i < paths.Count(); i++)
            {
                path += "\\" + paths[i];
            }

            Directory.CreateDirectory(path);

            return path;
        }

        public static string GetSummaryPath(string resultPath, string modelName, DateTime dateTime)
        {
            string path;
            DateTime curTime = DateTime.Now;

            switch (DataPathType)
            {
                default:
                case DataPathType.Model_Day:
                    path = string.Format("{0}\\{1}\\{2}", resultPath, modelName, curTime.ToString("yyyyMMdd"));
                    break;
                case DataPathType.Model_Day_Hour:
                    path = string.Format("{0}\\{1}\\{2}\\{3}", resultPath, modelName, curTime.ToString("yyyyMMdd"), curTime.ToString("HH"));
                    break;
                case DataPathType.Day_Model:
                    path = string.Format("{0}\\{1}\\{2}", resultPath, curTime.ToString("yyyyMMdd"), modelName);
                    break;
                case DataPathType.Day_Hour_Model:
                    path = string.Format("{0}\\{1}\\{2}\\{3}", resultPath, curTime.ToString("yyyyMMdd"), curTime.ToString("HH"), modelName);
                    break;
            }
            return path;
        }
    }
}
