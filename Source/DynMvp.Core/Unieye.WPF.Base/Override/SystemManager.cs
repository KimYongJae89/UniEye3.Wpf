using Authentication.Core;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.Data;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Unieye.WPF.Base.ViewModels;
using Unieye.WPF.Base.Views;
using UniEye.Base.Config;

namespace Unieye.WPF.Base.Override
{
    public class SystemManager : UniEye.Base.SystemManager
    {
        public IServiceProvider ServiceProvider;

        public SystemManager()
        {
        }

        public override bool Setup()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init SystemManager");

            if (OperationConfig.Instance().UseUserManager)
            {
                var loginWindow = new LoginWindow();
                var loginViewModel = new LoginViewModel();
                loginWindow.DataContext = loginViewModel;
                if (loginWindow.ShowDialog() == false)
                {
                    return false;
                }
            }

            if (AlgorithmFactory.Instance() != null)
            {
                AlgorithmFactory.Instance().Setup(OperationConfig.Instance().ImagingLibrary);
            }

            LogHelper.Debug(LoggerType.StartUp, "Show SplashForm");

            var form = new SplashScreenWindow(SplashSetupAction, SplashConfigAction);
            form.ShowDialog();

            LogHelper.Debug(LoggerType.StartUp, "app processor Setup() finish.");

            if (form.DialogResult == false)
            {
                if (AlgorithmFactory.Instance()?.IsUseMatroxMil() == true)
                {
                    MatroxHelper.FreeApplication();
                }

                return false;
            }

            return true;
        }

        public override bool SplashSetupAction(IReportProgress reportProgress)
        {
            try
            {
                DoReportProgress(reportProgress, 10, "Initialize Model List");

                ModelManager.Instance().Refresh();

                AlgorithmPool.Instance().Initialize();

                DoReportProgress(reportProgress, 40, "Initialize Machine");

                DeviceManager.Instance().Initialize(NonVision, reportProgress);
                DeviceMonitor.Instance().Initialize();

                DoReportProgress(reportProgress, 70, "Start Image Copy");

                if (OperationConfig.Instance().UseRemoteBackup)
                {
                    StartImageCopy();
                }
                else if (OperationConfig.Instance().ResultStoringDays > 0)
                {
                    //FTP 추가 시 삭제 경로를 FTP쪽 경로로 수정할 것
                    //DataRemover = new DataRemover(DataPathType.Model_Day, Path.Combine(PathConfig.Instance().Result, "Image"), TimeConfig.Instance().ResultStoringDays, "yyyyMMdd");
                    DataRemover = new DirectoryDataRemover(PathConfig.Instance().Result, OperationConfig.Instance().ResultStoringDays, RemoverMode.Directory);
                    DataRemover.Start();
                }

                DoReportProgress(reportProgress, 90, "Init Additional Units");

                InitAdditionalUnits();

                if (NonVision == false)
                {
                    InitializeCameraCalibration();
                }
            }
#if !DEBUG
            catch (Exception ex)
            {
                DoReportProgress(reportProgress, 100, ex.Message);
                reportProgress.SetLastError(ex.Message);

                return false;
            }
#endif
            finally { }
            DoReportProgress(reportProgress, 90, "End of Initialize");

            return true;
        }

        public override bool SplashConfigAction(IReportProgress reportProgress)
        {
            var loginWindow = new LoginWindow();
            var loginViewModel = new LoginViewModel();
            loginWindow.DataContext = loginViewModel;
            bool result = (bool)loginWindow.ShowDialog();
            //if (result == true)
            //{
            //    if (UserHandler.Instance.CurrentUser.SuperAccount ?? UserHandler.Instance.CurrentUser.SuperAccount.Value)
            //        AlgorithmFactory.Instance()?.Setup(OperationConfig.Instance().ImagingLibrary);
            //}

            return result;
        }

        public override void Release()
        {
            base.Release();
        }
    }
}
