using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Inspect;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Config;
using UniEye.Base.Data;

namespace UniEye.Base.Inspect
{
    public class StepResult
    {
        public bool ResultGood { get; set; } = false;
        public bool OpSuccess { get; set; } = false;
    }

    /// <summary>
    /// 시작 신호를 받을 때 마다 InspectionStep을 증기시키면서 검사를 수행
    /// </summary>
    public class StepTriggerInspectRunner : InspectRunner
    {
        private bool onScanProcess = false;

        public StepTriggerInspectRunner() : base()
        {
            inspectRunnerType = InspectRunnerType.Step;
        }

        public override void BeginInspect(int triggerIndex = -1)
        {
            ProductBeginInspect(triggerIndex);
        }

        public override void EndInspect()
        {
            ProductInspected();

            ProductEndInspect();
        }

        public override void Scan(int triggerIndex, string scanImagePath)
        {
            if (onScanProcess == false)
            {
                if (Directory.Exists(scanImagePath) == false)
                {
                    Directory.CreateDirectory(scanImagePath);
                }

                onScanProcess = true;
            }
            else
            {
                onScanProcess = false;
            }
        }

        public override async void Inspect(int triggerIndex = -1)
        {
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await DoInspect(triggerIndex);
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

        public virtual Task DoInspect(int stepIndex)
        {
            return Task.Run(() => inspectRunnerExtender.DoStepInspect(stepIndex));
        }
    }
}
