using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Inspect;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Config;
using UniEye.Base.Data;

namespace UniEye.Base.Inspect
{
    public class StepModelInspectRunnerExtener : InspectRunnerExtender
    {
        public int[] UsedCamIndexArr { get; set; } = null;

        /// <summary>
        /// TriggerIndex에 따라 Step 목록을 재 필터링 한다.
        /// </summary>
        /// <param name="inspectStepList"></param>
        /// <param name="triggerIndex"></param>
        /// <returns></returns>
        protected virtual List<InspectStep> FilterInspectStep(List<InspectStep> inspectStepList, int triggerIndex)
        {
            return inspectStepList;
        }

        protected virtual bool IsInspectionStepValid(InspectStep inspectStep)
        {
            if (inspectStep.NumTargets == 0)
            {
                return false;
            }

            return true;
        }

        public override void DoInspect(int triggerIndex)
        {
            if (!(inspectRunner.CurModel is StepModel stepModel))
            {
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "SingleTriggerInspectRunner : DoInspect");

            ObjectPool<ImageBuffer> imageBufferPool = DeviceManager.Instance().ImageBufferPool;

            var sw = new Stopwatch();
            sw.Start();
            stepModel.GetInspectOrderRange(out int minOrder, out int maxOrder);

            for (int stepOrder = minOrder; stepOrder <= maxOrder; stepOrder++)
            {
                List<InspectStep> inspectStepList = stepModel.GetInspectStepList(stepOrder);

                inspectStepList = FilterInspectStep(inspectStepList, triggerIndex);

                var taskList = new List<Task>();

                Task acquireTask = null;

                foreach (InspectStep inspectStep in inspectStepList)
                {
                    if (IsInspectionStepValid(inspectStep) == false)
                    {
                        continue;
                    }

                    if (acquireTask != null)
                    {
                        if (acquireTask.Wait(TimeConfig.Instance().InspectTimeOut) == false)
                        {
                            throw new ActionTimeoutException("Save Timeout");
                        }
                    }

                    Task actuateTask = ActuateMachine(inspectStep);

                    ImageBuffer imageBuffer = imageBufferPool.GetObject();
                    acquireTask = AcquireImage(actuateTask, inspectStep, imageBuffer);

                    Task saveTask = null;
                    if (InspectConfig.Instance().SaveCameraImage == true)
                    {
                        saveTask = acquireTask.ContinueWith((antecedent) =>
                        {
                            string prefixStr = string.Format("Image_{0:00}", inspectStep.StepNo);
                            imageBuffer.Save(inspectRunner.ProductResult.ResultPath, prefixStr, ImageFormat.Bmp);
                        });
                    }

                    //acquire3DTask = Acquire3DImage(acquireTask, imageBuffer);

                    Task inspectionTask = acquireTask.ContinueWith((antecedent) =>
                    {
                        InspectInspectStep(inspectStep, imageBuffer);

                        if (saveTask != null)
                        {
                            if (saveTask.Wait(TimeConfig.Instance().InspectTimeOut) == false)
                            {
                                throw new ActionTimeoutException("Save Timeout");
                            }
                        }

                        imageBufferPool.PutObject(imageBuffer);
                    });

                    taskList.Add(inspectionTask);

                    inspectRunner.CheckThrowExcpetion();

                    if (inspectRunner.ProductResult.InspectionCancelled)
                    {
                        break;
                    }
                }

                if (Task.WaitAll(taskList.ToArray(), TimeConfig.Instance().InspectTimeOut) == false)
                {
                    throw new TimeoutException();
                }

                inspectEventHandler?.StepOrderEndInspect(stepModel, stepOrder, inspectRunner.ProductResult);
            }

            LogHelper.Debug(LoggerType.Inspection, string.Format("All InspectInspectionStep was done. [{0} ms]", sw.ElapsedMilliseconds));

            SystemState.Instance().SetInspectState(InspectState.Done);

            LogHelper.Debug(LoggerType.Inspection, "Thread Stopped : InspectionThreadFunc");
        }

        public bool InspectInspectStep(InspectStep inspectStep, ImageBuffer imageBuffer)
        {
            ProductResult productResult = inspectRunner.ProductResult;

            LogHelper.Debug(LoggerType.Inspection, string.Format("InspectInspectionStep({0}) - Start]", inspectStep.Name));

            var sw = new Stopwatch();
            sw.Start();

            inspectEventHandler?.StepBeginInspect(inspectStep, productResult, imageBuffer);

            inspectRunner.CheckThrowExcpetion();

            int numCamera = DeviceManager.Instance().CameraHandler.NumCamera;

            PointF stepPos = inspectRunner.PositionAligner.Align(inspectStep.Position.ToPointF());

            InspectParam inspectParam;

            var taskList = new List<Task>();
            inspectStep.GetInspectOrderRange(out int minOrder, out int maxOrder);

            bool good = true;

            int[] camIndexArr = UsedCamIndexArr;
            if (camIndexArr == null)
            {
                camIndexArr = DeviceManager.Instance().CameraHandler.GetCameraIndexArr();
            }

            for (int inspectOrder = minOrder; inspectOrder <= maxOrder; inspectOrder++)
            {
                foreach (int cameraIndex in camIndexArr)
                {
                    List<Target> targetList = inspectStep.GetTargets(cameraIndex, inspectOrder);
                    inspectParam = inspectRunner.GetInspectParam(imageBuffer, cameraIndex, inspectStep.StepNo);
                    inspectParam.LocalPositionAligner = inspectStep.GetPositionAligner(cameraIndex, productResult);
                    inspectParam.LocalCameraCalibration = inspectStep.GetCalibration(cameraIndex, productResult);
                    inspectParam.InspectStepAlignedPos = stepPos;

                    var inspectionTask = Task.Run(() =>
                    {
                        foreach (Target target in targetList)
                        {
                            good &= target.Inspect(inspectParam);
                        }
                    });

                    taskList.Add(inspectionTask);
                }

                if (Task.WaitAll(taskList.ToArray(), TimeConfig.Instance().InspectTimeOut) == false)
                {
                    throw new TimeoutException();
                }

                inspectEventHandler?.TargetOrderEndInspect(inspectStep, inspectOrder, productResult);

                inspectRunner.CheckThrowExcpetion();
            }

            inspectParam = inspectRunner.GetInspectParam();
            foreach (Target target in inspectStep.TargetList)
            {
                good &= target.Compute(inspectParam, productResult);
            }

            inspectEventHandler?.StepEndInspect(inspectStep, productResult, imageBuffer);

            LogHelper.Debug(LoggerType.Inspection, string.Format("InspectInspectionStep({0}) - Finish [{1} ms]", inspectStep.Name, sw.ElapsedMilliseconds));

            return good;
        }

        protected Task ActuateMachine(InspectStep inspectStep)
        {
            PositionAligner positionAligner = inspectRunner.PositionAligner;

            return Task.Run(() =>
            {
                AxisPosition position = inspectStep.Position;

                AxisHandler axisHandler = DeviceManager.Instance().GetAxisHandler(position.AxisHandlerName);
                if (axisHandler != null)
                {
                    if (positionAligner != null)
                    {
                        PointF newPos = positionAligner.Align(position.ToPointF());
                        position[0] = newPos.X;
                        position[1] = newPos.Y;
                    }

                    axisHandler.Move(position, inspectRunner.CancellationTokenSource.Token);

                    //Thread.Sleep(TimeConfig.Instance().AirActionStableTime);
                }
            });
        }

        protected Task AcquireImage(Task actuateTask, InspectStep inspectStep, ImageBuffer imageBuffer)
        {
            return actuateTask.ContinueWith((antecedent) =>
            {
                ImageAcquisition imageAcquisition = DeviceManager.Instance().ImageAcquisition;
                imageAcquisition.Acquire(imageBuffer, inspectStep.LightParamSet, inspectStep.GetLightTypeIndexArr(), UsedCamIndexArr);
            });
        }

        private Task Acquire3DImage(Task acquiresTask, ImageBuffer imageBuffer)
        {
            return acquiresTask.ContinueWith((antecedent) =>
            {
                ImageAcquisition imageAcquisition = DeviceManager.Instance().ImageAcquisition;
                var calcTaskList = new List<Task>();

                // 3D Image 획득
                var lightParamNorth = new LightParam(0, "North Trigger", 4);
                var imageBufferNorth = new List<Image2D>();
                imageAcquisition.AcquireMulti(0, imageBufferNorth, lightParamNorth);

                var calcNorthTask = Task.Run(() => { /* Calculate 3D North */ });
                calcTaskList.Add(calcNorthTask);

                var lightParamWest = new LightParam(0, "West Trigger", 4);
                var imageBufferWest = new List<Image2D>();
                imageAcquisition.AcquireMulti(0, imageBufferWest, lightParamWest);

                var calcWestTask = Task.Run(() => { /* Calculate 3D North */ });
                calcTaskList.Add(calcWestTask);

                /* Calc other direction */

                Task.WaitAll(calcTaskList.ToArray());
            });
        }

        public override void DoScan(int triggerIndex, string scanImagePath)
        {
            LogHelper.Debug(LoggerType.Inspection, "SingleTriggerInspectRunner : DoScan");

            if (!(inspectRunner.CurModel is StepModel stepModel))
            {
                LogHelper.Error("[StepModelInspectRunnerExtender.DoStepInspect] Model is not valid type");
                return;
            }

            var sw = new Stopwatch();
            sw.Start();

            ObjectPool<ImageBuffer> imageBufferPool = DeviceManager.Instance().ImageBufferPool;

            List<InspectStep> inspectStepList = stepModel.GetInspectStepList();
            inspectStepList = FilterInspectStep(inspectStepList, triggerIndex);

            Task acquireTask = null;
            Task saveTask = null;
            foreach (InspectStep inspectStep in inspectStepList)
            {
                if (acquireTask != null)
                {
                    acquireTask.Wait();
                }

                Task actuateTask = ActuateMachine(inspectStep);

                if (saveTask != null)
                {
                    saveTask.Wait();
                }

                ImageBuffer imageBuffer = imageBufferPool.GetObject();
                acquireTask = AcquireImage(actuateTask, inspectStep, imageBuffer);

                saveTask = acquireTask.ContinueWith((antecedent) =>
                {
                    string prefixStr = string.Format("Image_{0:00}", inspectStep.StepNo);
                    imageBuffer.Save(scanImagePath, prefixStr, ImageFormat.Bmp);

                    imageBufferPool.PutObject(imageBuffer);
                });

                inspectRunner.CheckThrowExcpetion();
            }

            LogHelper.Debug(LoggerType.Inspection, "Thread Stopped : InspectionThreadFunc");
        }

        public override StepResult DoStepInspect(int stepNo)
        {
            return DoProcess(stepNo);
        }

        public override StepResult DoStepScan(int stepNo, string scanImagePath)
        {
            return DoProcess(stepNo, scanImagePath);
        }

        public StepResult DoProcess(int stepNo, string scanImagePath = "")
        {
            var stepResult = new StepResult();

            if (!(inspectRunner.CurModel is StepModel stepModel))
            {
                LogHelper.Error("[StepModelInspectRunnerExtender.DoStepInspect] Model is not valid type");
                return stepResult;
            }

            InspectStep inspectStep = stepModel.GetInspectStep(stepNo);
            if (inspectStep == null)
            {
                LogHelper.Warn(LoggerType.Inspection, "[StepModelInspectRunnerExtender.DoStepInspect] Can't find step no : {0}" + stepNo);
                return stepResult;
            }

            if (IsInspectionStepValid(inspectStep) == false)
            {
                return stepResult;
            }

            ObjectPool<ImageBuffer> imageBufferPool = DeviceManager.Instance().ImageBufferPool;

            Task actuateTask = ActuateMachine(inspectStep);

            ImageBuffer imageBuffer = imageBufferPool.GetObject();
            Task acquireTask = AcquireImage(actuateTask, inspectStep, imageBuffer);

            if (string.IsNullOrEmpty(scanImagePath) == false)
            {
                Task saveTask = null;
                if (InspectConfig.Instance().SaveCameraImage == true)
                {
                    saveTask = acquireTask.ContinueWith((antecedent) =>
                    {
                        string prefixStr = string.Format("Image_{0:00}", inspectStep.StepNo);
                        imageBuffer.Save(inspectRunner.ProductResult.ResultPath, prefixStr, ImageFormat.Bmp);
                    });
                }

                Task inspectionTask = acquireTask.ContinueWith((antecedent) =>
                {
                    stepResult.ResultGood = InspectInspectStep(inspectStep, imageBuffer);

                    if (saveTask != null)
                    {
                        if (saveTask.Wait(TimeConfig.Instance().InspectTimeOut) == false)
                        {
                            throw new ActionTimeoutException("[StepModelInspectRunnerExtender.DoStepInspect] Save Timeout");
                        }
                    }

                    imageBufferPool.PutObject(imageBuffer);
                });

                if (inspectionTask.Wait(TimeConfig.Instance().InspectTimeOut) == false)
                {
                    throw new ActionTimeoutException("[StepModelInspectRunnerExtender.DoStepInspect] Inspection Timeout");
                }
            }
            else
            {
                Task saveTask = acquireTask.ContinueWith((antecedent) =>
                {
                    string prefixStr = string.Format("Image_{0:00}", inspectStep.StepNo);
                    imageBuffer.Save(scanImagePath, prefixStr, ImageFormat.Bmp);
                });

                Task releaseTask = saveTask.ContinueWith((antecedent) => imageBufferPool.PutObject(imageBuffer));

                if (releaseTask.Wait(TimeConfig.Instance().InspectTimeOut) == false)
                {
                    throw new ActionTimeoutException("[StepModelInspectRunnerExtender.DoStepInspect] Release Timeout");
                }
            }

            stepResult.OpSuccess = true;

            return stepResult;
        }

    }
}
