using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Devices.MotionController
{
    public class MotionMonitor
    {
        private Task monitoringTask;
        private MotionHandler motionHandler;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public MotionMonitor(MotionHandler motionHandler)
        {
            this.motionHandler = motionHandler;
        }

        public void Start()
        {
            monitoringTask = new Task(new Action(MonitoringProc), cancellationTokenSource.Token);
            monitoringTask.Start();
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            monitoringTask.Wait();
        }

        public void MonitoringProc()
        {
            try
            {
                while (true)
                {
                    if (ErrorManager.Instance().IsAlarmed() == false)
                    {
                        foreach (Motion motion in motionHandler)
                        {
                            for (int i = 0; i < motion.NumAxis; i++)
                            {
                                if (motion.IsAmpFault(i) == true)
                                {
                                    ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)MotionError.AmpFault, ErrorLevel.Error,
                                        ErrorSection.Motion.ToString(), MotionError.AmpFault.ToString(), string.Format("Amp Fault : Axis No = {0}", i.ToString()));
                                }
                            }
                        }
                    }

                    cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    Thread.Sleep(100);
                }
            }
            catch (OperationCanceledException)
            {

            }
        }
    }
}
