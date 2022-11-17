using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Data.Data
{
    public enum RemoverMode
    {
        File, Directory
    }

    public class DirectoryDataRemover : DataRemover
    {
        #region 생성자
        public DirectoryDataRemover(string resultPath, int resultStoringDays, RemoverMode removerMode) : base(DataPathType.Day_Hour_Model, resultPath, resultStoringDays, "yyyyMMdd")
        {
            this.ResultPath = resultPath;
            this.ResultStoringDays = resultStoringDays;
            this.RemoverMode = removerMode;
        }
        #endregion


        #region 속성
        private CancellationTokenSource CancellationTokenSource { get; set; }
        
        private string ResultPath { get; set; }
        
        private int ResultStoringDays { get; set; }
        
        private RemoverMode RemoverMode { get; set; }

        private float AvailableFreeSpacePersent { get; set; } = 0.01f;
        #endregion


        #region 메서드
        public override async void Start()
        {
            if (Directory.Exists(ResultPath) == false)
            {
                return;
            }

            if (ResultStoringDays > 0)
            {
                CancellationTokenSource = new CancellationTokenSource();
                try
                {
                    await RemoveData();
                }
                catch (OperationCanceledException)
                {

                }
            }
        }

        public override void Stop()
        {
            CancellationTokenSource?.Cancel();
        }

        public override Task RemoveData()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        CancellationTokenSource.Token.ThrowIfCancellationRequested();

                        switch (RemoverMode)
                        {
                            case RemoverMode.File:
                                {
                                    IEnumerable<string> filePaths = Directory.GetFiles(ResultPath, "*", SearchOption.AllDirectories).OrderBy(f => new FileInfo(f).CreationTime);
                                    foreach (string filePath in filePaths)
                                    {
                                        RemoveFile(filePath);
                                    }
                                }
                                break;
                            case RemoverMode.Directory:
                                {
                                    IEnumerable<string> directoryPaths = Directory.GetDirectories(ResultPath, "*", SearchOption.TopDirectoryOnly).OrderBy(f => new DirectoryInfo(f).CreationTime);
                                    foreach (string directoryPath in directoryPaths)
                                    {
                                        RemoveDirectory(directoryPath);
                                    }
                                }
                                break;
                        }

                        Thread.Sleep(1000 * 60 * 60); // 1 시간마다
                    }
                    catch (Exception) { }
                }
            });
        }

        public override void RemoveFile(string filePath)
        {
            if (File.Exists(filePath) == false || new FileInfo(filePath).Name.Contains("Template"))
            {
                return;
            }

            if (IsOldFile(filePath) == true)
            {
                try
                {
                    File.Delete(filePath);
                    DirectoryInfo directory = new FileInfo(filePath).Directory;
                    ClearDirectory(directory);
                    LogHelper.Debug(LoggerType.Operation, "Folder Deleted : " + filePath);
                }
                catch (Exception ex)
                {
                    LogHelper.Debug(LoggerType.Operation, "Fail to delete folder :  " + ex.Message + " / Path : " + filePath);
                }
            }
        }

        private void ClearDirectory(DirectoryInfo directoryInfo)
        {
            if (directoryInfo.FullName == ResultPath)
            {
                return;
            }

            if (directoryInfo.GetFiles("*", SearchOption.AllDirectories).Length == 0)
            {
                directoryInfo.Delete();
            }

            ClearDirectory(directoryInfo.Parent);
        }

        public override bool IsOldFile(string filePath)
        {
            DateTime fileDate = File.GetLastWriteTime(filePath);
            DateTime curTime = DateTime.Now;
            TimeSpan timeSpan = curTime.Date - fileDate.Date;
            return (timeSpan.Days > ResultStoringDays);
        }

        public bool IsFileDriveFull(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            DriveInfo driveInfo = new DriveInfo(fileInfo.Directory.Root.Name);
            return driveInfo.AvailableFreeSpace / driveInfo.TotalSize < 0.005;
        }

        private void RemoveDirectory(string directoryPath)
        {
            if (IsDirectoryDriveFull(directoryPath) || IsOldDirectory(directoryPath))
            {
                try
                {
                    Directory.Delete(directoryPath, true);
                    LogHelper.Debug(LoggerType.Operation, "Folder Deleted : " + directoryPath);
                }
                catch (Exception ex)
                {
                    LogHelper.Debug(LoggerType.Operation, "Fail to delete folder :  " + ex.Message + " / Path : " + directoryPath);
                }
            }
        }

        public bool IsOldDirectory(string directoryPath)
        {
            DateTime fileDate = Directory.GetLastWriteTime(directoryPath);
            DateTime curTime = DateTime.Now;
            TimeSpan timeSpan = curTime.Date - fileDate.Date;
            return (timeSpan.Days > ResultStoringDays);
        }

        public bool IsDirectoryDriveFull(string directoryPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            DriveInfo driveInfo = new DriveInfo(directoryInfo.Root.Name);
            return (float)driveInfo.AvailableFreeSpace / (float)driveInfo.TotalSize < AvailableFreeSpacePersent;
        }
        #endregion
    }
}
