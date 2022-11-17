using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Data;

namespace UniEye.Base.MachineInterface
{
    /*     < Single Trigger >
     *      VisionReady On  ->
     *      CommandWait On  ->
     *                     <-   Trigger On
     *      CommandWait Off ->
     *                     <-   Trigger Off
     *      Result On       ->                  
     *      Complete On     ->                  
     *                     <-   CommandDone On // Done
     *      Result Off      ->                  
     *      Complete Off    ->                  
     *                     <-   CommandDone Off // Ready
     *      CommandWait On  ->
     *      VisionReady Off ->
     */

    /*     < Multi Trigger >
     *      VisionReady On  ->
     *      CommandWait On  ->
     *                     <-   InspStart On
     *      CommandWait Off ->
     *                     <-   InspStart Off
     *      Complete On     ->                  
     *                     <-   CommandDone On
     *      Complete Off    ->                  
     *                     <-   CommandDone Off
     *      CommandWait On  ->
     *                     <-   Trigger On
     *      CommandWait Off ->
     *                     <-   Trigger Off
     *      Complete On     ->                  
     *                     <-   CommandDone On
     *      Complete Off    ->                  
     *                     <-   CommandDone Off
     *      CommandWait On  ->
     *                     <-   InspEnd On
     *      CommandWait Off ->
     *                     <-   InspEnd Off
     *      Result On       ->                  
     *      Complete On     ->                  
     *                     <-   CommandDone On
     *      Result Off      ->                  
     *      Complete Off    ->                  
     *                     <-   CommandDone Off
     *      CommandWait On  ->
     *      VisionReady Off ->
     */

    public class IoTriggerChannel
    {
        public int Index { get; set; } = -1;
        public DigitalIoHandler DigitalIoHandler { get; set; }

        public IoPort OutBusyPort { get; set; } = null;
        public IoPort OutCompletePort { get; set; } = null;
        public IoPort OutResultNgPort { get; set; } = null;
        public IoPort InTriggerPort { get; set; } = null;
        public IoPort InDonePort { get; set; } = null;

        public IoTriggerChannel(IoPort inTriggerPort, IoPort outBusyPort, IoPort outCompletePort, IoPort outResultNgPort, IoPort inDonePort)
        {
            InTriggerPort = inTriggerPort;
            OutBusyPort = outBusyPort;
            OutCompletePort = outCompletePort;
            OutResultNgPort = outResultNgPort;
            InDonePort = inDonePort;
        }

        public void SetBusy()
        {
            DigitalIoHandler.WriteOutput(OutBusyPort, true);
            DigitalIoHandler.WriteOutput(OutResultNgPort, false);
            DigitalIoHandler.WriteOutput(OutCompletePort, false);
        }

        public void SetComplete()
        {
            DigitalIoHandler.WriteOutput(OutBusyPort, false);
            DigitalIoHandler.WriteOutput(OutCompletePort, true);
        }

        public void SetSignalOff()
        {
            DigitalIoHandler.WriteOutput(OutBusyPort, false);
            DigitalIoHandler.WriteOutput(OutResultNgPort, false);
            DigitalIoHandler.WriteOutput(OutCompletePort, false);
        }

        public void SetResult(bool result)
        {
            DigitalIoHandler.WriteOutput(OutResultNgPort, result == false);
        }
    }

    public abstract class IoTriggerMachineIfBase : MachineIf
    {
        protected DigitalIoHandler digitalIoHandler;
        protected IoMonitor ioMonitor;
        private IoPort outVisionAlivePort = null;
        private IoPort inMachineAlivePort = null;
        private IoPort outReadyPort = null;

        public IoTriggerMachineIfBase(MachineIfSetting machineIfSetting) : base(machineIfSetting, null)
        {

        }

        public override void Initialize()
        {
            digitalIoHandler = DeviceManager.Instance().DigitalIoHandler;

            ioMonitor = new IoMonitor(digitalIoHandler);
            ioMonitor.ProcessInitial += ProcessIo;
            ioMonitor.ProcessInputChanged += ProcessIo;
            ioMonitor.ProcessIdle += ProcessIdle;

            SystemState.Instance().SetupState(1);
        }

        public void InitAlivePort(IoPort outVisionAlivePort, IoPort inMachineAlivePort, IoPort outReadyPort)
        {
            this.outVisionAlivePort = outVisionAlivePort;
            this.inMachineAlivePort = inMachineAlivePort;
            this.outReadyPort = outReadyPort;
        }

        protected virtual bool ProcessIdle(DioValue inputValue)
        {
            var state = SystemState.Instance();
            if (inMachineAlivePort != null)
            {
                bool machineAliveOn = IoMonitor.CheckInput(inputValue, inMachineAlivePort);
                state.SetMachineAlive(machineAliveOn);
            }

            if (outVisionAlivePort != null)
            {
                digitalIoHandler.WriteOutput(outVisionAlivePort, state.IsVisionAlive());
            }

            return true;
        }

        public override void Start()
        {
            if (ioMonitor != null)
            {
                ioMonitor.Start();
            }
        }

        public override void Stop()
        {
            if (ioMonitor != null)
            {
                ioMonitor.Stop();
            }
        }

        public override bool IsStarted()
        {
            throw new NotImplementedException();
        }

        protected abstract bool ProcessIo(DioValue inputValue);
    }

    public class SingleIoTriggerMachineIf : IoTriggerMachineIfBase
    {
        protected IoTriggerChannel triggerChannel;

        public SingleIoTriggerMachineIf(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {

        }

        public void SetTriggerChannel(IoTriggerChannel triggerChannel)
        {
            triggerChannel.DigitalIoHandler = digitalIoHandler;
            this.triggerChannel = triggerChannel;
        }

        protected override bool ProcessIo(DioValue inputValue)
        {
            var state = SystemState.Instance();

            bool busy = state.IsBusy();

            if (busy)
            {
                bool done = IoMonitor.CheckInput(inputValue, triggerChannel.InDonePort);
                if (done)
                {
                    ExecuteCommand("DONE");
                }

                // IO Machine If는 Cancel 지원 안함.
            }
            else
            {
                bool trigger = IoMonitor.CheckInput(inputValue, triggerChannel.InTriggerPort);
                if (trigger)
                {
                    ExecuteCommand("TRIG");
                }
            }

            return true;
        }

        public override bool Send(MachineIfItemInfo itemInfo, params string[] args)
        {
            throw new NotImplementedException();
        }
    }

    public class TwinIoMachineIf : IoTriggerMachineIfBase
    {
        protected IoTriggerChannel[] triggerChannelArr = new IoTriggerChannel[2];

        public TwinIoMachineIf(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {

        }

        public override void Initialize()
        {
            SystemState.Instance().SetupState(2);
        }

        public void SetTriggerChannel(IoTriggerChannel triggerChannel0, IoTriggerChannel triggerChannel1)
        {
            triggerChannelArr[0] = triggerChannel0;
            triggerChannelArr[1] = triggerChannel1;

            triggerChannel0.DigitalIoHandler = digitalIoHandler;
            triggerChannel1.DigitalIoHandler = digitalIoHandler;
        }

        protected override bool ProcessIo(DioValue inputValue)
        {
            bool result0 = ProcessIo(inputValue, triggerChannelArr[0]);
            bool result1 = ProcessIo(inputValue, triggerChannelArr[1]);

            return result0 && result1;
        }

        private bool ProcessIo(DioValue inputValue, IoTriggerChannel triggerChannel)
        {
            var state = SystemState.Instance();

            bool busy = state.IsBusy();

            if (busy)
            {
                bool done = IoMonitor.CheckInput(inputValue, triggerChannel.InDonePort);
                if (done)
                {
                    ExecuteCommand("DONE");
                }

                // IO Machine If는 Cancel 지원 안함.
            }
            else
            {
                bool trigger = IoMonitor.CheckInput(inputValue, triggerChannel.InTriggerPort);
                if (trigger)
                {
                    ExecuteCommand("TRIG");
                }
            }

            return true;
        }

        public override bool Send(MachineIfItemInfo itemInfo, params string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
