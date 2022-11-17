using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unieye.WPF.Base.Override;
using UniEye.Base.Data;
using UniScanC.Data;
using WPF.ThicknessMeasure.Data;

namespace WPF.ThicknessMeasure.Inspect
{
    public class InspectRunner : UniEye.Base.Inspect.InspectRunner
    {
        #region 필드
        // Module
        private MotionController controller;
        private DataCalculator calculator;
        #endregion

        #region 생성자
        public InspectRunner()
        {
            cancellationTokenSource = new CancellationTokenSource();

            calculator = new DataCalculator();
            calculator.CalDone = CalculateDone;
            controller = new MotionController();
            controller.MoveDone = calculator.MotionMoveDone;
        }
        #endregion

        #region 메서드
        // Process
        public void StartProc()
        {
            try
            {
                if (controller.SettingSequence() == false)
                {
                    ExitWaitInspection();
                    return;
                }

                Task.Run(new Action(() => controller.ScanProc()), cancellationTokenSource.Token);
                Task.Run(new Action(() => calculator.ScanProc(true)), cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("StartProc : " + ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Debug(LoggerType.Error, "StartProc : " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public override bool EnterWaitInspection(ModelBase curModel = null)
        {
            if (curModel == null)
            {
                this.curModel = DynMvp.Data.ModelManager.Instance().CurrentModel;
            }
            else
            {
                this.curModel = curModel;
            }

            if (IsModelValid() == false)
            {
                return false;
            }

            cancellationTokenSource = new CancellationTokenSource();
            controller.CancellationTokenSource = cancellationTokenSource;
            SystemState.Instance().SetInspectState(InspectState.Run);

            return inspectEventHandler.EnterWaitInspection();
        }

        public override void ExitWaitInspection()
        {
            calculator.ScanProc(false);
            cancellationTokenSource.Cancel();

            SystemState.Instance().OnWaitStop = true;
            SystemState.Instance().SetIdle();

            inspectEventHandler?.ExitWaitInspection();
        }

        public override void Inspect(int triggerIndex = -1)
        {
            try
            {
                StartProc();
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
        }

        public override void ProductBeginInspect(int triggerIndex = -1)
        {
            productResult = inspectRunnerExtender.BuildProductResult();
            inspectEventHandler?.ProductBeginInspect(productResult);
        }

        public override void ProductInspected()
        {
            inspectEventHandler?.ProductInspected(productResult);
            SystemManager.Instance().OnProductInspected(productResult);
        }

        public override void ProductEndInspect()
        {
            inspectEventHandler?.ProductEndInspect(productResult);
        }

        // 폭을 한번 스캔한 후에 DataCalculator 에서 CalDone Delegate 가 온다.
        private void CalculateDone(Dictionary<string, ThicknessScanData> logScanData)
        {
            ProductBeginInspect();

            var result = productResult as ThicknessResult;
            result.ScanData = logScanData;
            //result.SetReelPosition() // PLC 연동 가능할 때 쓰는 코드

            ProductInspected();

            ProductEndInspect();
        }
        #endregion
    }
}
