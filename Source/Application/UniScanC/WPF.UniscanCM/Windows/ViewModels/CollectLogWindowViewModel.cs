using DynMvp.Base;
using DynMvp.Devices.Dio;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Input;
using Unieye.WPF.Base.Controls;
using Unieye.WPF.Base.Helpers;
using UniEye.Base.MachineInterface;
using UniEye.Translation.Helpers;
using UniScanC.Controls.Models;
using UniScanC.Module;
using WPF.UniScanCM.Override;

namespace WPF.UniScanCM.Windows.ViewModels
{
    public class CollectLogWindowViewModel : Observable
    {
        #region 생성자
        public CollectLogWindowViewModel()
        {
            OkCommand = new RelayCommand<ChildWindow>(OkCommandAction);
            CancelCommand = new RelayCommand<ChildWindow>(CancelCommandAction);

            SystemConfig config = SystemConfig.Instance;
            InspectModuleInfoList = new List<InspectModuleInfo>();
            InspectModuleInfoList.Add(new InspectModuleInfo() { ModuleTopic = "CM", ModuleIP = "127.0.0.1" });
            inspectModuleInfoList.AddRange(config.ImModuleList);

            LogFileInfoDic = new Dictionary<string, FileInfo[]>();

            DurationDate = 7;
            IsCompress = true;
            IsDeleteFolder = true;
        }
        #endregion


        #region 속성
        public System.Windows.Input.ICommand OkCommand { get; }

        public System.Windows.Input.ICommand CancelCommand { get; }

        private int durationDate;
        public int DurationDate
        {
            get => durationDate;
            set => Set(ref durationDate, value);
        }

        private bool isCompress;
        public bool IsCompress
        {
            get => isCompress;
            set => Set(ref isCompress, value);
        }

        private bool isDeleteFolder;
        public bool IsDeleteFolder
        {
            get => isDeleteFolder;
            set => Set(ref isDeleteFolder, value);
        }

        private List<InspectModuleInfo> inspectModuleInfoList;
        public List<InspectModuleInfo> InspectModuleInfoList
        {
            get => inspectModuleInfoList;
            set => Set(ref inspectModuleInfoList, value);
        }

        private Dictionary<string, FileInfo[]> LogFileInfoDic { get; set; }
        #endregion


        #region 메서드
        private async void OkCommandAction(ChildWindow wnd)
        {
            var dlg = new FolderBrowserDialog()
            {
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                ShowNewFolderButton = true,
                Description = "Select Folder",
            };

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            // Target
            var targetInfo = new DirectoryInfo(Path.Combine(dlg.SelectedPath, $"CollectLog_{DateTime.Now.ToString("yyyy_MM_dd")}"));
            if (!targetInfo.Exists)
            {
                targetInfo.Create();
            }

            int fileCount = 0;
            foreach (InspectModuleInfo moduleInfo in InspectModuleInfoList)
            {
                string logPath = $@"\\{moduleInfo.ModuleIP}\{moduleInfo.ModuleTopic}Log\";
                var src = new DirectoryInfo(logPath);
                FileInfo[] fileInfos = GetFileInfos(src, DurationDate);
                LogFileInfoDic.Add(moduleInfo.ModuleTopic, fileInfos);
                fileCount += fileInfos.Length;

                var directoryInfo = new DirectoryInfo(Path.Combine(targetInfo.FullName, moduleInfo.ModuleTopic));
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
            }

            var progressSource = new ProgressSource();
            progressSource.Range = fileCount;
            progressSource.Step = 1;
            progressSource.CancellationTokenSource = new System.Threading.CancellationTokenSource();
            await MessageWindowHelper.ShowProgress(
                TranslationHelper.Instance.Translate("Save") + " " + TranslationHelper.Instance.Translate("Log"),
                TranslationHelper.Instance.Translate("Loading") + ("..."),
                new Action(() =>
                {
                    foreach (KeyValuePair<string, FileInfo[]> pair in LogFileInfoDic)
                    {
                        foreach (FileInfo fileInfo in pair.Value)
                        {
                            try
                            {
                                File.Copy(fileInfo.FullName, Path.Combine(targetInfo.FullName, pair.Key, fileInfo.Name));

                                progressSource.StepIt();
                                if (progressSource.CancellationTokenSource.IsCancellationRequested)
                                {
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error(LoggerType.Error, $"CollectLogWindowViewModel::OkCommandAction - Save, {ex.GetType().Name}: {ex.Message}");
                            }
                        }
                    }
                }), true, progressSource);

            if (IsCompress)
            {
                progressSource = new ProgressSource();
                progressSource.Range = 1;
                progressSource.Step = 1;
                progressSource.CancellationTokenSource = new System.Threading.CancellationTokenSource();
                await MessageWindowHelper.ShowProgress(
                    TranslationHelper.Instance.Translate("Compress") + " " + TranslationHelper.Instance.Translate("Log"),
                    TranslationHelper.Instance.Translate("Loading") + ("..."),
                    new Action(() =>
                    {
                        try
                        {
                            var zipInfo = new FileInfo($"{targetInfo.FullName}.zip");
                            FileHelper.CompressZip(targetInfo, zipInfo, progressSource.CancellationTokenSource);

                            progressSource.StepIt();
                            if (progressSource.CancellationTokenSource.IsCancellationRequested)
                            {
                                return;
                            }

                            if (IsDeleteFolder)
                            {
                                targetInfo.Delete(true);
                            }
                            //FileInfo zipInfo2 = new FileInfo(Path.Combine(targetInfo.FullName, $"{targetInfo.Name}.zip"));
                            //FileHelper.Move(zipInfo.FullName, zipInfo2.FullName);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(LoggerType.Error, $"CollectLogWindowViewModel::OkCommandAction - Compress, {ex.GetType().Name}: {ex.Message}");
                        }
                    }), true, progressSource);
            }

            wnd.Close(true);
        }

        private void CancelCommandAction(ChildWindow wnd)
        {
            wnd.Close(false);
        }

        private FileInfo[] GetFileInfos(DirectoryInfo directoryInfo, int days)
        {
            if (!directoryInfo.Exists)
            {
                return new FileInfo[0];
            }

            var fInfoList = directoryInfo.GetFiles().ToList();
            fInfoList.RemoveAll(f =>
            {
                string extension = f.Extension;
                if (DateTime.TryParseExact(extension, ".yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime datetime))
                {
                    TimeSpan timeSpan = DateTime.Today - datetime;
                    if (timeSpan.Days >= days)
                    {
                        return true;
                    }
                }
                return false;
            });

            return fInfoList.ToArray();
        }
        #endregion
    }
}
