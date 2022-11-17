using Authentication.Core;
using Authentication.Core.Enums;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings.UI;
using UniEye.Base.UI;

namespace UniEye.Base
{
    public enum OpMode { Idle, Wait, Teach, Inspect, Model, Etc }

    public class SystemManager
    {
        protected static SystemManager instance = null;
        public static SystemManager Instance()
        {
            return instance;
        }

        public static void SetInstance(SystemManager systemManager)
        {
            instance = systemManager;
        }

        protected List<InspectRunner> inspectRunnerList = new List<InspectRunner>();
        public List<InspectRunner> InspectRunnerList
        {
            get => inspectRunnerList;
            set => inspectRunnerList = value;
        }

        public InspectRunner InspectRunner
        {
            get
            {
                if (inspectRunnerList.Count() == 0)
                {
                    return null;
                }

                return inspectRunnerList[0];
            }
        }

        protected List<MachineIf> machineIfList = new List<MachineIf>();
        public List<MachineIf> MachineIfList => machineIfList;
        public ProductResult InspectionResult { get; } = null;

        private string lastResultPath;

        protected List<IDataExporter> dataExporterList = new List<IDataExporter>();
        public List<IDataExporter> DataExporterList => dataExporterList;
        public List<Calibration> CameraCalibrationList { get; } = new List<Calibration>();

        private DataRemover dataRemover;
        public DataRemover DataRemover { get; set; }

        private ProgressForm progressForm = new ProgressForm();
        public bool LiveMode { get; set; } = false;
        public bool NonVision { get; set; } = false;
        public string ModuleId { get; set; }
        public IImageSequence ImageSequence { get; set; }

        public SystemManager()
        {
        }

        public void AddMachineIf(MachineIf machineIf)
        {
            machineIfList.Add(machineIf);
        }

        public void AddInspectRunner(InspectRunner inspectRunner)
        {
            inspectRunnerList.Add(inspectRunner);
        }

        public virtual bool Setup()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init SystemManager");

            if (OperationConfig.Instance().UseUserManager)
            {
                var loginForm = new LogInForm();
                loginForm.ShowDialog();
                if (loginForm.DialogResult == DialogResult.Cancel)
                {
                    return false;
                }

                UserHandler.Instance.CurrentUser = loginForm.LogInUser;
            }
            else
            {
#if DEBUG
                UserHandler.Instance.CurrentUser = UserHandler.Instance.GetUser("developer", "masterkey");
#endif
            }

            if (AlgorithmFactory.Instance() != null)
            {
                AlgorithmFactory.Instance().Setup(OperationConfig.Instance().ImagingLibrary);
            }

            LogHelper.Debug(LoggerType.StartUp, "Show SplashForm");
            //SplashForm form2 = new SplashForm();
            //form2.ShowDialog();

            var form = new SplashForm();
            form.ConfigAction = SplashConfigAction;
            form.SetupAction = SplashSetupAction;
            form.title.Text = UiConfig.Instance().ProgramTitle;
            if (File.Exists(PathConfig.Instance().CompanyLogo) == true)
            {
                form.companyLogo.Image = new Bitmap(PathConfig.Instance().CompanyLogo);
            }

            form.copyrightText.Text = UiConfig.Instance().Copyright;
            form.title.Text = UiConfig.Instance().Title;
            form.versionText.Text = string.Format("Version {0}", OperationConfig.Instance().SystemVersion);
            form.buildText.Text = string.Format("Build {0}", OperationConfig.Instance().GetBuildNo());

            //form.DialogResult = DialogResult.Abort;

            form.ShowDialog();

            LogHelper.Debug(LoggerType.StartUp, "app processor Setup() finish.");

            if (form.DialogResult == DialogResult.Abort)
            {
                if (AlgorithmFactory.Instance()?.IsUseMatroxMil() == true)
                {
                    MatroxHelper.FreeApplication();
                }

                return false;
            }

            return true;
        }

        public void SetDataRemover(DataRemover dataRemover)
        {
            this.dataRemover = dataRemover;
        }

        public void AddDataExporter(IDataExporter dataExporter)
        {
            dataExporterList.Add(dataExporter);
        }

        protected void DoReportProgress(IReportProgress reportProgress, int percentage, string message)
        {
            LogHelper.Debug(LoggerType.StartUp, message);

            if (reportProgress != null)
            {
                reportProgress.ReportProgress(percentage, StringManager.GetString(message));
            }
        }

        /// <summary>
        /// 상속 클래스에서 추가 알고리즘에 대한 AlgorithmStrategyManager 설정 및 
        /// AlgorithmBuilder.CreateAdditionalAlgorithm 의 delegation을 설정한다.
        /// </summary>

        public virtual bool SplashConfigAction(IReportProgress reportProgress)
        {
            var loginForm = new LogInForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                UserHandler.Instance.CurrentUser = loginForm.LogInUser;

                if (loginForm.LogInUser.IsAuth(ERoleType.DeviceSetting) == true)
                {
                    var form = new ConfigForm();

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        AlgorithmFactory.Instance()?.Setup(OperationConfig.Instance().ImagingLibrary);
                    }
                }
                else
                {
                    MessageForm.Show(null, StringManager.GetString("You don't have proper permission."));
                }
            }

            return true;
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            SplashSetupAction(progressForm);
        }

        public virtual bool SplashSetupAction(IReportProgress reportProgress)
        {
            try
            {
                DoReportProgress(reportProgress, 10, "Initialize Model List");

                ModelManager.Instance().Refresh(PathConfig.Instance().Model);

                AlgorithmPool.Instance().Initialize();

                DoReportProgress(reportProgress, 40, "Initialize Machine");

                DeviceManager.Instance().Initialize(NonVision, reportProgress);
                DeviceMonitor.Instance().Initialize();

                DoReportProgress(reportProgress, 70, "Start Image Copy");

                if (OperationConfig.Instance().UseRemoteBackup)
                {
                    StartImageCopy();
                }
                else if (TimeConfig.Instance().ResultStoringDays > 0)
                {
                    //FTP 추가 시 삭제 경로를 FTP쪽 경로로 수정할 것
                    dataRemover = new DataRemover(DataPathType.Model_Day, Path.Combine(PathConfig.Instance().Result, "Image"), TimeConfig.Instance().ResultStoringDays, "yyyyMMdd");
                    dataRemover.Start();
                }

                DoReportProgress(reportProgress, 90, "Init Additional Units");

                InitAdditionalUnits();

                if (NonVision == false)
                {
                    InitializeCameraCalibration();
                }
            }
            catch (Exception ex)
            {
                DoReportProgress(reportProgress, 100, ex.Message);
                reportProgress.SetLastError(ex.Message);

                return false;
            }

            DoReportProgress(reportProgress, 90, "End of Initialize");

            return true;
        }


        public void InitializeCameraCalibration()
        {
            foreach (Camera camera in DeviceManager.Instance().CameraHandler)
            {
                string datFileName = string.Format(@"{0}\Calibration{1}.xml", BaseConfig.Instance().ConfigPath, camera.Index);
                string gridFileName = string.Format(@"{0}\Calibration{1}.dat", BaseConfig.Instance().ConfigPath, camera.Index);

                Calibration calibration = AlgorithmFactory.Instance().CreateCalibration();

                if (calibration != null)
                {
                    calibration.Initialize(camera.Index, datFileName, gridFileName);
                    CameraCalibrationList.Add(calibration);

                    camera.UpdateFovSize(calibration.PelSize);
                    calibration.UpdatePelSize(camera.ImageSize.Width, camera.ImageSize.Height);
                }
            }
        }

        public Calibration GetCameraCalibration(int cameraIndex)
        {
            return CameraCalibrationList.Find(x => x.CameraIndex == cameraIndex);
        }

        public virtual void InitAdditionalUnits() { }

        public virtual void Release()
        {
            DeviceManager.Instance().Release();

            foreach (Calibration calibration in CameraCalibrationList)
            {
                calibration.Dispose();
            }

            LogHelper.Debug(LoggerType.Shutdown, "All Thread are dead.");
        }

        public void StartImageCopy()
        {
            Process[] slideShowProcess = Process.GetProcessesByName("ImageCopyPM");
            if (slideShowProcess.Count() == 0)
            {
                string fileName = Path.Combine(Environment.CurrentDirectory + "\\ImageCopyPM.exe");

                if (File.Exists(fileName) == true)
                {
                    Process.Start(fileName);
                }
            }
        }

        public void ExportData(ProductResult inspectionResult)
        {
            lastResultPath = inspectionResult.ResultPath;

            foreach (IDataExporter dataExporter in dataExporterList)
            {
                dataExporter.Export(inspectionResult);
            }
        }

        public virtual void ExportData(string data) { }

        public virtual bool OnEnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Inspection, "Enter Wait Inspection");

            return true;
        }

        internal virtual void OnStartInspection() { }

        public virtual void OnStopInspection() { }

        internal virtual bool OnExitWaitInspection()
        {
            return true;
        }

        public virtual void OnProductInspected(ProductResult productResult)
        {
            ExportData(productResult);

            foreach (MachineIf machineIf in machineIfList)
            {
                machineIf.ProductInspected(productResult);
            }
        }

        public void ChangeOpMode(OpMode opMode)
        {
            UiManager.Instance().ChangeOpMode(opMode);
        }

        public OpMode GetOpMode()
        {
            return UiManager.Instance().GetOpMode();
        }

        public void TestInspect()
        {
            //IMainForm mainForm = UiManager.Instance().MainForm;
            //if (mainForm.GetOpMode() == OpMode.Teach)
            //{
            //    mainForm.TestInspect();
            //}

        }
    }
}
