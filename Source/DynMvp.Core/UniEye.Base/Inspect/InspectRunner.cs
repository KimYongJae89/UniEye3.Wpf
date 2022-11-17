using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.BarcodeReader;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;

namespace UniEye.Base.Inspect
{
    public enum InspectRunnerType
    {
        Direct, Single, Step
    }

    public abstract class InspectRunner
    {
        protected InspectRunnerType inspectRunnerType;
        public InspectRunnerType Type
        {
            get => inspectRunnerType;
            set => inspectRunnerType = value;
        }

        protected InspectRunnerExtender inspectRunnerExtender;
        public InspectRunnerExtender InspectRunnerExtender
        {
            get => inspectRunnerExtender;
            set
            {
                inspectRunnerExtender = value;
                inspectRunnerExtender.InspectRunner = this;
            }
        }

        protected ProductResult productResult;
        public ProductResult ProductResult => productResult;

        protected InspectEventHandler inspectEventHandler;
        public InspectEventHandler InspectEventHandler
        {
            get => inspectEventHandler;
            set => inspectEventHandler = value;
        }

        protected CancellationTokenSource cancellationTokenSource;
        public CancellationTokenSource CancellationTokenSource => cancellationTokenSource;

        protected PositionAligner positionAligner = null;
        public PositionAligner PositionAligner
        {
            get => positionAligner;
            set => positionAligner = value;
        }

        protected ModelBase curModel;
        public ModelBase CurModel { get => curModel; set => curModel = value; }

        public InspectRunner()
        {
        }

        public virtual InspectParam GetInspectParam(ImageBuffer imageBuffer, int cameraIndex, int stepNo = 0)
        {
            Calibration cameraCalibration = SystemManager.Instance().GetCameraCalibration(cameraIndex);
            CancellationToken token = CancellationToken.None;
            if (cancellationTokenSource != null)
            {
                token = cancellationTokenSource.Token;
            }

            return new InspectParam(stepNo, positionAligner, cameraCalibration, imageBuffer, productResult, false, token, inspectEventHandler);
        }

        public virtual InspectParam GetInspectParam()
        {
            CancellationToken token = CancellationToken.None;
            if (cancellationTokenSource != null)
            {
                token = cancellationTokenSource.Token;
            }

            return new InspectParam(0, positionAligner, null, null, productResult, false, token, inspectEventHandler);
        }

        /// <summary>
        /// 검사 대기 상태로 들어갈 때 호출
        /// </summary>
        public virtual bool EnterWaitInspection(ModelBase curModel = null)
        {
            if (curModel == null)
            {
                this.curModel = ModelManager.Instance().CurrentModel;
            }
            else
            {
                this.curModel = curModel;
            }

            if (IsModelValid() == false)
            {
                MessageForm.Show(null, "The model is invalid. Check the model");
                return false;
            }

            SystemState.Instance().SetWait();

            return inspectEventHandler.EnterWaitInspection();
        }

        protected virtual bool IsModelValid()
        {
            if (curModel == null)
            {
                return false;
            }

            if (curModel.IsEmpty() == true)
            {
                return false;
            }

            return true;
        }

        // Step Trigger에서 사용
        public virtual void BeginInspect(int triggerIndex = -1)
        {

        }

        public virtual void EndInspect()
        {

        }

        public abstract void Inspect(int triggerIndex = -1);

        public virtual void AutoTune()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="triggerChannelIndex">-1 : 전체 Step 검사, -1이 아닐 경우, 해당 Trigger에 검사할 Step 지정. InspectRunnerExtender에 지정함. </param>
        public virtual void ProductBeginInspect(int triggerIndex = -1)
        {
            positionAligner = new PositionAligner();

            LogHelper.Debug(LoggerType.Inspection, "CreateInspectionResult");
            productResult = inspectRunnerExtender.BuildProductResult();
            if (productResult == null)
            {
                LogHelper.Debug(LoggerType.Inspection, "Inspection cancel");
                return;
            }

            productResult.TriggerIndex = triggerIndex;

            if (DeviceConfig.Instance().UseBarcodeReader)
            {
                IBarcodeReader barcodeReader = DeviceManager.Instance().BarcodeReader;

                if (string.IsNullOrEmpty(barcodeReader.BarcodeRead) == true)
                {
                    MessageForm.Show(null, "Reading product barcode number, please.");
                    return;
                }

                productResult.InputBarcode = barcodeReader.BarcodeRead;
                barcodeReader.Reset();
            }

            SystemState.Instance().SetInspectState(InspectState.Run);

            inspectEventHandler?.ProductBeginInspect(productResult);
        }

        /// <summary>
        /// 정상적으로 검사 동작이 완료되어 결과에 대한 정리 작업 등을 수행하는 함수
        /// </summary>
        public virtual void ProductInspected()
        {
            productResult.ArrangeResultList();

            inspectEventHandler?.ProductInspected(productResult);

            SystemManager.Instance().OnProductInspected(productResult);
        }

        /// <summary>
        /// 검사 완료 후, 최종 정리. 검사가 취소된 상태가 있을 수 있기 때문에 검사 결과에 대한 처리는 수행하지 않아야 함
        /// </summary>
        public virtual void ProductEndInspect()
        {
            Thread.Sleep(TimeConfig.Instance().InspectionDelay); // 늘려진 검사 시간 만큼 대기 한다.

            inspectEventHandler?.ProductEndInspect(productResult);

            DeviceManager.Instance().LightCtrlHandler.TurnOff();
            SystemState.Instance().SetWait();
        }

        public virtual async void Scan(int triggerIndex, string scanImagePath)
        {
            cancellationTokenSource = new CancellationTokenSource();

            if (Directory.Exists(scanImagePath) == false)
            {
                Directory.CreateDirectory(scanImagePath);
            }

            SystemState.Instance().SetInspectState(InspectState.Run);

            try
            {
                await DoScan(triggerIndex, scanImagePath);
            }
            catch (OperationCanceledException)
            {
            }
            catch (ActionTimeoutException ex)
            {
                LogHelper.Error(ex.Message);
            }
            catch (AlarmException ex)
            {
                LogHelper.Error(ex.Message);
            }
            catch (TimeoutException ex)
            {
                LogHelper.Error(ex.Message);
            }

            SystemState.Instance().SetWait();
        }

        public virtual Task DoScan(int triggerIndex, string scanImagePath)
        {
            return Task.Run(() => { });
        }

        /// <summary>
        /// 검사 중에 검사를 멈출 때 호출
        /// </summary>
        public virtual void CancelInspect()
        {
            StopProcess();
        }

        public virtual void PauseInspect()
        {

        }

        public virtual void LotChange()
        {

        }

        public void StopProcess()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }

            SystemState.Instance().OnWaitStop = true;
        }

        /// <summary>
        /// 검사 대기 상태를 해제할 때 호출
        /// </summary>
        public virtual void ExitWaitInspection()
        {
            StopProcess();
            SystemState.Instance().SetIdle();
            inspectEventHandler?.ExitWaitInspection();
        }

        public void CheckThrowExcpetion()
        {
            ErrorManager.Instance().ThrowIfAlarm();
            //cancellationTokenSource.Token.ThrowIfCancellationRequested();
        }

        public virtual void ResetState()
        {

        }
    }
}
