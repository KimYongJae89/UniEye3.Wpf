using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Data
{
    public enum DataPathType
    {
        Model_Day, Model_Day_Hour, Model_Year_Month_Day, Model_Month_Day, Day_Model, Day_Hour_Model, Year_Month_Day_Model, Month_Day_Model
    }

    public class DataRemover
    {
        private CancellationTokenSource cancellationTokenSource;
        private DataPathType dataPathType;
        private string resultPath;
        private int resultStoringDays;
        private string dateFormat;

        public DataRemover(DataPathType dataPathType, string resultPath, int resultStoringDays, string dateFormat)
        {
            this.dataPathType = dataPathType;
            this.resultPath = resultPath;
            this.resultStoringDays = resultStoringDays;
            this.dateFormat = dateFormat;
        }

        public virtual async void Start()
        {
            if (Directory.Exists(resultPath) == false)
            {
                return;
            }

            if (resultStoringDays > 0)
            {
                cancellationTokenSource = new CancellationTokenSource();
                try
                {
                    await RemoveData();
                }
                catch (OperationCanceledException)
                {

                }
            }
        }

        public virtual void Stop()
        {
            cancellationTokenSource?.Cancel();
        }

        public virtual Task RemoveData()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    //string[] directoryNames = Directory.GetDirectories(resultPath);
                    string[] directoryNames = Directory.GetFiles(resultPath);
                    foreach (string dirName in directoryNames)
                    {
                        string path = Path.Combine(resultPath, dirName);

                        switch (dataPathType)
                        {
                            default:
                            case DataPathType.Model_Day:
                                RemoveFile(path);
                                break;
                            case DataPathType.Model_Day_Hour:
                                RemoveData_Month(path);
                                break;
                            case DataPathType.Model_Year_Month_Day:
                                RemoveData_Year(path);
                                break;
                        }
                    }

                    Thread.Sleep(new TimeSpan(1, 0, 0));
                }
            });
        }

        public virtual bool IsOldFile(string path)
        {
            DateTime fileDate;
            fileDate = File.GetLastWriteTime(path);
            DateTime curTime = DateTime.Now;

            TimeSpan timeSpan = curTime.Date - fileDate.Date;

            return (timeSpan.Days > resultStoringDays);
        }

        public virtual void RemoveFile(string parentPath)
        {
            if (File.Exists(parentPath) == false)
            {
                return;
            }

            //string[] directoryNames = Directory.GetDirectories(parentPath);
            //foreach (string dirName in directoryNames)
            //{
            //    string path = Path.Combine(parentPath, dirName);
            //    if (IsOldData(path) == true)
            //    {
            //        try
            //        {
            //            File.Delete(path);
            //            LogHelper.Debug(LoggerType.Operation, "Folder Deleted : " + path); 
            //        }
            //        catch(Exception ex)
            //        {
            //            LogHelper.Debug(LoggerType.Operation, "Fail to delete folder :  " + ex.Message + " / Path : " + path);
            //        }
            //    }

            //    Thread.Sleep(10);
            //}

            if (IsOldFile(parentPath) == true)
            {
                try
                {
                    File.Delete(parentPath);
                    LogHelper.Debug(LoggerType.Operation, "Folder Deleted : " + parentPath);
                }
                catch (Exception ex)
                {
                    LogHelper.Debug(LoggerType.Operation, "Fail to delete folder :  " + ex.Message + " / Path : " + parentPath);
                }
            }

            Thread.Sleep(10);
        }

        private void RemoveData_Month(string parentPath)
        {
            if (Directory.Exists(parentPath) == false)
            {
                return;
            }

            string[] monthDirectoryNames = Directory.GetDirectories(parentPath);
            foreach (string monthDirName in monthDirectoryNames)
            {
                string monthPath = Path.Combine(parentPath, monthDirName);

                string[] directoryNames = Directory.GetDirectories(monthPath);
                foreach (string dirName in directoryNames)
                {
                    string path = Path.Combine(parentPath, dirName);

                    if (IsOldFile(path) == true)
                    {
                        Directory.Delete(path, true);
                    }
                }

                Thread.Sleep(10);
            }
        }

        private void RemoveData_Year(string parentPath)
        {
            if (Directory.Exists(parentPath) == false)
            {
                return;
            }

            string[] yearDirectoryNames = Directory.GetDirectories(parentPath);
            foreach (string yearDirName in yearDirectoryNames)
            {
                string yearPath = Path.Combine(parentPath, yearDirName);

                string[] directoryNames = Directory.GetDirectories(yearPath);
                foreach (string dirName in directoryNames)
                {
                    string path = Path.Combine(parentPath, dirName);

                    RemoveData_Month(path);
                }
            }
        }
    }
}
