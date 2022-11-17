using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Inspect;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniEye.Base.Settings;

namespace UniEye.Base.Inspect
{
    /// <summary>
    /// 카메라를 HW Trigger로 설정하고, 이미지 Grab Callback후 검사를 수행
    /// Multi camera일 경우, 
    /// </summary>
    public class DirectTriggerInspectRunner : InspectRunner
    {
        public int[] UsedCamIndexArr { get; set; } = null;

        private object scanLock = new object();
        private int scanRemains = 0;
        private bool onScanProcess;
        private string scanImagePath;

        protected List<Task> inspectTaskList = new List<Task>();

        protected bool multiStepInspect = false;

        public DirectTriggerInspectRunner() : base()
        {
            inspectRunnerType = InspectRunnerType.Direct;
        }

        public override bool EnterWaitInspection(ModelBase curModel = null)
        {
            if (!base.EnterWaitInspection(curModel))
            {
                return false;
            }

            TurnOnTriggerMode();

            cancellationTokenSource = new CancellationTokenSource();

            return true;
        }

        protected int[] GetCamIndexArr()
        {
            if (UsedCamIndexArr != null)
            {
                return UsedCamIndexArr;
            }

            return DeviceManager.Instance().CameraHandler.GetCameraIndexArr();
        }

        protected virtual void TurnOnTriggerMode()
        {
            int[] camIndexArr = GetCamIndexArr();

            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;
            foreach (int camIndex in camIndexArr)
            {
                Camera camera = cameraHandler.GetCamera(camIndex);

                camera.ImageGrabbed += ImageGrabbed;
                camera.GrabMulti();
            }
        }

        public override void ExitWaitInspection()
        {
            base.ExitWaitInspection();

            TurnOffTriggerMode();

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
        }

        protected void TurnOffTriggerMode()
        {
            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;
            foreach (Camera camera in cameraHandler)
            {
                if (camera is CameraGenTL cameraGenTL)
                {
                    camera.Stop();
                    camera.ImageGrabbed -= ImageGrabbed;
                    camera.SetTriggerMode(TriggerMode.Software);
                }
            }
        }

        public override void BeginInspect(int triggerIndex = -1)
        {
            if (multiStepInspect == true)
            {
                productResult = inspectRunnerExtender.BuildProductResult();
            }

            TurnOnTriggerMode();
        }

        public override void EndInspect()
        {
            if (multiStepInspect == true)
            {
                ProductInspected();
            }

            TurnOffTriggerMode();
        }

        public override async void Scan(int triggerIndex, string scanImagePath)
        {
            CameraHandler cameraHandler = DeviceManager.Instance().CameraHandler;
            scanRemains = cameraHandler.NumCamera;
            this.scanImagePath = scanImagePath;

            foreach (Camera camera in cameraHandler)
            {
                if (camera is CameraGenTL cameraGenTL)
                {
                    camera.ImageGrabbed += ImageGrabbed;
                    camera.SetTriggerMode(TriggerMode.Hardware);
                }
            }

            cancellationTokenSource = new CancellationTokenSource();

            if (Directory.Exists(scanImagePath) == false)
            {
                Directory.CreateDirectory(scanImagePath);
            }

            SystemState.Instance().SetInspectState(InspectState.Run);

            onScanProcess = true;

            await Task.Run(() =>
            {
                try
                {
                    var scanTask = Task.Run(() =>
                    {
                        while (scanRemains > 0)
                        {
                            Thread.Sleep(100);
                        }
                    }, cancellationTokenSource.Token);

                    if (scanTask.Wait(TimeConfig.Instance().InspectTimeOut) == false)
                    {
                        LogHelper.Error("Scan Task Timeout");
                    }
                }
                catch (OperationCanceledException)
                {
                    MessageBox.Show("Scan Task Cancelled");
                }
            });

            onScanProcess = false;
        }

        public virtual void ImageGrabbed(Camera camera)
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectRunner.ImageGrabbed");

            if (onScanProcess)
            {
                Scan(camera);
            }
            else
            {
                Inspect(camera);
            }
        }

        protected virtual void Scan(Camera camera)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var currentModel = (StepModel)ModelManager.Instance().CurrentModel;

            lock (scanLock)
            {
                ImageD grabImage = camera.GetGrabbedImage();
                //camera.ImageGrabbed -= ImageGrabbed;

                string imageNameFormat = InspectConfig.Instance().ImageNameFormat;
                string fileName = Path.Combine(scanImagePath, string.Format(imageNameFormat, 0, camera.Index));

                grabImage.Save(fileName);

                scanRemains--;
            }
        }

        protected virtual async void Inspect(Camera camera)
        {
            if (SystemState.Instance().OnInspection == true)
            {
                //LogHelper.Warn(LoggerType.Inspection, String.Format("Image Grabbed Overlapped - Camera {0}", camera.Index));
                return;
            }

            SystemState.Instance().SetInspectState(InspectState.Run);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            LogHelper.Debug(LoggerType.Inspection, string.Format("InspectRunner.Image Grabbed - Camera {0}", camera.Index));

            inspectEventHandler.ProbeBeginInspect();

            if (multiStepInspect == false)
            {
                productResult = inspectRunnerExtender.BuildProductResult();
            }

            if (productResult == null)
            {
                LogHelper.Error(string.Format("InspectRunner.Image Grabbed - Camera {0}", camera.Index));
                SystemState.Instance().SetWait();
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, string.Format("Inspection Time ( Phase 2 ) : {0} ms", stopWatch.ElapsedMilliseconds));

            ImageD grabImage = camera.GetGrabbedImage();

            LogHelper.Debug(LoggerType.Inspection, string.Format("Inspection Time ( Phase 3 ) : {0} ms", stopWatch.ElapsedMilliseconds));

            ObjectPool<ImageBuffer> imageBufferPool = DeviceManager.Instance().ImageBufferPool;
            ImageBuffer imageBuffer = imageBufferPool.GetObject();
            if (imageBuffer == null)
            {
                LogHelper.Error(string.Format("InspectRunner.Inspect() imageBuffer is null", camera.Index));
                SystemState.Instance().SetWait();
                return;
            }

            Image2D image = imageBuffer.GetImage(camera.Index, 0);
            image.CopyFrom(grabImage);
            image.ConvertFromDataPtr();

            LogHelper.Debug(LoggerType.Inspection, string.Format("Image Grabbed : {0} ms", stopWatch.ElapsedMilliseconds));

            Task inspectTask = DoInspect(imageBuffer, camera.Index);

            imageBufferPool.PutObject(imageBuffer);

            lock (inspectTaskList)
            {
                inspectTaskList.Add(inspectTask);
            }

            if (await Task.WhenAny(inspectTask, Task.Delay(TimeConfig.Instance().InspectTimeOut)) == inspectTask)
            {
                inspectEventHandler.ProbeEndInspect();

                if (multiStepInspect == false)
                {
                    ProductInspected();
                }
            }
            else
            {
                // Timeout
                LogHelper.Warn(LoggerType.Inspection, "Inspect Timeout");
            }

            lock (inspectTaskList)
            {
                inspectTaskList.Remove(inspectTask);
            }
        }

        public virtual Task DoInspect(ImageBuffer imageBuffer, int cameraIndex)
        {
            var currentModel = (StepModel)ModelManager.Instance().CurrentModel;

            InspectStep inspectStep = currentModel.GetInspectStep(0);

            List<Target> targetList = inspectStep.GetTargets(cameraIndex, 0);
            InspectParam inspectParam = GetInspectParam(imageBuffer, cameraIndex, inspectStep.StepNo);
            inspectParam.LocalPositionAligner = inspectStep.GetPositionAligner(cameraIndex, productResult);
            inspectParam.LocalCameraCalibration = inspectStep.GetCalibration(cameraIndex, productResult);

            return Task.Run(() =>
            {
                foreach (Target target in targetList)
                {
                    target.Inspect(inspectParam);
                }
            }, cancellationTokenSource.Token);
        }

        public override void Inspect(int triggerIndex = -1)
        {
            // 구현 필요 없음
        }
    }
}

