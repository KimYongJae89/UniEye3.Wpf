using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Devices.Dio
{
    public delegate bool IoMonitorEventHandler(DioValue inputValue);

    public class IoMonitor
    {
        private CancellationTokenSource cancellationTokenSource;
        private DioValue preInputValue = new DioValue();
        private bool preAlarmed = false;

        private DigitalIoHandler digitalIoHandler = null;

        public IoMonitorEventHandler ProcessInitial;
        public IoMonitorEventHandler ProcessIdle;
        public IoMonitorEventHandler ProcessInputChanged;

        public IoMonitor(DigitalIoHandler digitalIoHandler)
        {
            this.digitalIoHandler = digitalIoHandler;
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }

        public async void Start()
        {
            if (digitalIoHandler == null)
            {
                return;
            }

            cancellationTokenSource = new CancellationTokenSource();
            try
            {
                await IOMonitorThreadFunc();
            }
            catch (OperationCanceledException)
            {

            }
        }

        private Task IOMonitorThreadFunc()
        {
            bool initial = true;

            return Task.Run(() =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        //cancellationTokenSource.Token.ThrowIfCancellationRequested();

                        DioValue inputValue = digitalIoHandler.ReadInput();

                        if (initial == true)
                        {
                            ProcessInitial?.Invoke(inputValue);
                            initial = false;
                        }
                        else
                        {
                            ProcessIdle?.Invoke(inputValue);
                        }

                        if ((inputValue.Equals(preInputValue)) && (ErrorManager.Instance().IsAlarmed() == preAlarmed))
                        {
                            Thread.Sleep(DeviceConfig.Instance().IoMonitorCheckInterval);
                            continue;
                        }

                        preInputValue.Copy(inputValue);
                        preAlarmed = ErrorManager.Instance().IsAlarmed();

                        ProcessInputChanged?.Invoke(inputValue);

                        Thread.Sleep(DeviceConfig.Instance().IoMonitorCheckInterval);
                    }
                    catch (Exception)
                    {

                    }
                }
            });
        }

        public static bool CheckInput(DioValue inputValue, IoPort ioPort)
        {
            if (ioPort == null)
            {
                return false;
            }

            if (ioPort.PortNo == IoPort.UNUSED_PORT_NO)
            {
                return false;
            }

            uint channelValue = inputValue.GetValue(ioPort.DeviceNo, ioPort.GroupNo);

            return ((channelValue >> ioPort.PortNo) & 1) == 1;
        }
    }
}
