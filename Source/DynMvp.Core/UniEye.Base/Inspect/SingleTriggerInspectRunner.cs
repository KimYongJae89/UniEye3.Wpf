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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Config;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;

namespace UniEye.Base.Inspect
{
    /// <summary>
    /// 시작 신호를 한 번 받아 모델내의 모든 InspectionStep을 순회하면서 검사를 수행
    /// </summary>
    public class SingleTriggerInspectRunner : InspectRunner
    {
        public SingleTriggerInspectRunner() : base()
        {
            inspectRunnerType = InspectRunnerType.Single;
        }

        public override async void Inspect(int triggerIndex = -1)
        {
            cancellationTokenSource = new CancellationTokenSource();

            ProductBeginInspect(triggerIndex);

            try
            {
                await DoInspect(triggerIndex);

                ProductInspected();
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

            ProductEndInspect();
        }

        public Task DoInspect(int triggerIndex)
        {
            return Task.Run(() => inspectRunnerExtender.DoInspect(triggerIndex));
        }

        public override Task DoScan(int triggerIndex, string scanImagePath)
        {
            return Task.Run(() => inspectRunnerExtender.DoScan(triggerIndex, scanImagePath));
        }
    }
}
