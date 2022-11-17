using DynMvp.Devices;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Devices
{
    public enum ConveyorState
    {
        NoChange,
        Idle,
        Wait,
        FastReceiving,
        SlowReceiving,
        Received,
        Clamping,
        Clamped,
        SidePusherForward,
        SidePusherBackward,
        Unclamping,
        Unclamped,
        Sending,
        Sending2,
        Sended,
        Backward,
        Backward2,
        BackwardWait,
        Flushing
    }

    internal class Clamper
    {
        public void ClampUp()
        {

        }

        public void ClampDown()
        {

        }

        public void ClampReady()
        {

        }
    }

    public class ConveyorSystem
    {
        private bool vipMode = false;
        public bool EmulateNextMachineAvailable { get; set; } = false;
        public int ConveyorStopTimeMs { get; set; } = 100;
        public int FastMoveTimeMs { get; set; } = 1000;
        public int TimeOutTimeMs { get; set; } = 10000;
        public int InsufficientTimeMs { get; set; } = 30000;
        public int ClampStableTimeMs { get; set; } = 500;
        public int SidePusherTimeMs { get; set; } = 500;
        public int OutputFastMovingTimeMs { get; set; } = 1000;
        public int LoadingStopDelayTimeMs { get; set; } = 500;
        public int EjectStopDelayTimeMs { get; set; } = 500;
        public int UpdateIntervalMs { get; set; } = 10;

        private Stopwatch conveyorStopTimer = new Stopwatch();
        private Stopwatch fastMoveTimer = new Stopwatch();
        private Stopwatch timeOutTimer = new Stopwatch();
        private Stopwatch insufficientTimer = new Stopwatch();
        private Stopwatch clampStableTimer = new Stopwatch();
        private Stopwatch sidePusherTimer = new Stopwatch();
        private Stopwatch outputFastMovingTimer = new Stopwatch();
        private Cylinder stopper;
        private IoPort outStopper;
        private IoPort inStopperUp;
        private Cylinder sidePusher;
        private IoPort outSidePusherForward;
        private IoPort inSidePusherForward;
        private Clamper clamper = new Clamper();
        private DigitalIoHandler digitalIoHandler = DeviceManager.Instance().DigitalIoHandler;
        private IoPort outMotorMoving;
        private IoPort outMotorDir;
        private IoPort outMotorSpeedDown;
        private IoPort outBoardAvailable;
        private IoPort outNGPcb;
        private IoPort outMachineAvailable;
        public Sensor EntrySensor { get; } = new Sensor();
        public Sensor ReadySensor { get; } = new Sensor();
        public Sensor EjectSensor { get; } = new Sensor();
        public Sensor SpeedDownSensor { get; } = new Sensor();
        public Sensor NextMachineAvailableSensor { get; } = new Sensor();

        private IoPort inEntry;
        private IoPort inReady;
        private IoPort inEject;
        private IoPort inSpeedDown;
        private IoPort inNextMachineAvaliable;
        private IoPort inPreviousMachineAvailable;
        private IoPort inPreviousMachineNGPcb;
        private IoPort inClampUp;

        public bool ClampUpState { get; set; }
        public bool SidePusherForwardState { get; set; }
        public bool StopperUpState { get; set; }

        private Task workingTask;
        private bool timeOut = false;
        public bool TimeOut
        {
            get => timeOut;
            set
            {
                if (vipMode == false)
                {
                    timeOut = value;
                }
                else
                {
                    timeOut = false;
                }
            }
        }
        public ConveyorState CurrentState { get; set; } = ConveyorState.Idle;
        public ConveyorState LastState { get; set; } = ConveyorState.Idle;

        private ConveyorState transitState = ConveyorState.Idle;
        public bool Available { get; set; } = false;
        public bool AutoReceiving { get; set; } = false;
        public bool WaitReceiving { get; set; } = false;
        public bool IsWaitReceiving()
        {
            return AutoReceiving || WaitReceiving;
        }
        public bool Paused { get; set; } = false;

        public ConveyorSystem()
        {
            Init();
        }

        public void Init()
        {
            LastState = ConveyorState.Idle;
            CurrentState = ConveyorState.Idle;
        }

        public void UpdatePortNo()
        {
            PortMap portMap = DeviceManager.Instance().PortMap;

            outStopper = portMap.GetPort("OutStopperUp");
            inStopperUp = portMap.GetPort("InStopperUpSensor");
            outSidePusherForward = portMap.GetPort("OutSidePusherForward");
            inSidePusherForward = portMap.GetPort("InSidePusherSensor");

            inEntry = portMap.GetPort("InEntrySensor");
            inReady = portMap.GetPort("InInspectReadySensor");
            inEject = portMap.GetPort("InOutputReadySensor");
            inSpeedDown = portMap.GetPort("InSpeedDownSensor");
            inNextMachineAvaliable = portMap.GetPort("InPcbEjectRear");
            inPreviousMachineAvailable = portMap.GetPort("InPcbAvailableFront");
            inPreviousMachineNGPcb = portMap.GetPort("InNgPcbFront");
            inClampUp = portMap.GetPort("InClampUpSensor");


            outMotorMoving = portMap.GetPort("OutConveyorRun");
            outMotorDir = portMap.GetPort("OutConveyorDir");
            outMotorSpeedDown = portMap.GetPort("OutConveyorSpeedDown");
            outBoardAvailable = portMap.GetPort("OutPcbEjectRear");
            outNGPcb = portMap.GetPort("OutNgPcbRear");
            //outPCBEject = portMap.GetPort("OutPcbEjectRear");
            outMachineAvailable = portMap.GetPort("OutPcbAvailableFront");
            //outByPass = portMap.GetPort("InEntrySensor");

            stopper = new Cylinder("Stopper", DeviceManager.Instance().DigitalIoHandler, null, outStopper, null, inStopperUp);
            sidePusher = new Cylinder("SidePusher", DeviceManager.Instance().DigitalIoHandler, null, outSidePusherForward, null, inSidePusherForward);
        }

        public void Reset()
        {
            CurrentState = ConveyorState.Idle;

            WaitReceiving = false;
            AutoReceiving = false;
            Paused = false;

            StopConveyorNow();

            conveyorStopTimer.Reset();
            fastMoveTimer.Reset();
            timeOutTimer.Reset();
            insufficientTimer.Reset();
        }

        public enum MotorSpeed
        {
            Fast,
            Slow
        }

        public enum MotorDirection
        {
            Forward,
            Backward
        }

        public void MoveConveyor(MotorSpeed speed, MotorDirection direction)
        {
            timeOutTimer.Stop();
            conveyorStopTimer.Stop();

            digitalIoHandler.WriteOutput(outMotorMoving, false);

            if (direction == MotorDirection.Forward)
            {
                digitalIoHandler.WriteOutput(outMotorDir, true);
            }
            else
            {
                digitalIoHandler.WriteOutput(outMotorDir, false);
            }

            if (speed == MotorSpeed.Fast)
            {
                digitalIoHandler.WriteOutput(outMotorSpeedDown, false);
            }
            else
            {
                digitalIoHandler.WriteOutput(outMotorSpeedDown, true);
            }

            digitalIoHandler.WriteOutput(outMotorMoving, true);

            timeOutTimer.Start();
        }

        public void MoveForward()
        {
            MoveConveyor(MotorSpeed.Fast, MotorDirection.Forward);
        }

        public void MoveSlowForward()
        {
            MoveConveyor(MotorSpeed.Slow, MotorDirection.Forward);
        }

        public void MoveBackward()
        {
            StopperDown();
            MoveConveyor(MotorSpeed.Fast, MotorDirection.Backward);
        }

        public void StopConveyor(int stopTime, ConveyorState transitState = ConveyorState.NoChange)
        {
            timeOutTimer.Stop();

            this.transitState = transitState;

            if (stopTime > 0)
            {
                if (!conveyorStopTimer.IsRunning)
                {
                    conveyorStopTimer.Start();
                }
                else if (conveyorStopTimer.ElapsedMilliseconds > stopTime)
                {
                    StopConveyorNow();
                }
            }
            else
            {
                StopConveyorNow();
            }
        }

        public void StopConveyorNow()
        {
            timeOutTimer.Stop();
            conveyorStopTimer.Stop();

            StopMotor();

            if (transitState != ConveyorState.NoChange)
            {
                if (transitState == ConveyorState.Wait)
                {
                    EnterWaitState();
                }
                else
                {
                    CurrentState = transitState;
                }

                transitState = ConveyorState.NoChange;
            }
        }

        public void StopMotor()
        {
            digitalIoHandler.WriteOutput(outMotorDir, false);
            digitalIoHandler.WriteOutput(outMotorMoving, false);
        }

        public void DelayStopConveyor()
        {
            if (conveyorStopTimer.ElapsedMilliseconds > ConveyorStopTimeMs)
            {
                StopConveyorNow();
            }
        }

        public void SpeedDown()
        {
            MoveSlowForward();
            CurrentState = ConveyorState.SlowReceiving;
        }

        public bool IsStopperUp()
        {
            return stopper.IsEjected();
        }

        public void StopperUp()
        {
            if (IsStopperUp() == false)
            {
                stopper.Eject();
            }
        }

        public void StopperDown()
        {
            if (IsStopperUp() == true)
            {
                stopper.Inject();
            }
        }

        public bool IsConveyorRunning()
        {
            if (conveyorStopTimer.IsRunning)
            {
                return false;
            }

            return digitalIoHandler.ReadOutput(outMotorMoving);
        }

        public bool CheckTimeOut()
        {
            if (vipMode == false && timeOut == false && timeOutTimer.ElapsedMilliseconds > TimeOutTimeMs)
            {
                timeOut = true;
            }

            return timeOut;
        }

        public void ResetTimeOut()
        {
            timeOut = false;
        }

        public bool IsTimeOut()
        {
            return false;
        }

        public void SensorUpdate()
        {
            EntrySensor.Update(digitalIoHandler.ReadInput(inEntry));
            SpeedDownSensor.Update(digitalIoHandler.ReadInput(inSpeedDown));
            ReadySensor.Update(digitalIoHandler.ReadInput(inReady));
            EjectSensor.Update(digitalIoHandler.ReadInput(inEject));
            NextMachineAvailableSensor.Update(digitalIoHandler.ReadInput(inNextMachineAvaliable));

            ClampUpState = digitalIoHandler.ReadInput(inClampUp);
            SidePusherForwardState = digitalIoHandler.ReadInput(inSidePusherForward);
            StopperUpState = digitalIoHandler.ReadInput(inStopperUp);
        }

        private bool useEntryOutput = false;
        private bool isPause = false;
        public void ProecssEvent()
        {
            DelayStopConveyor();
            SensorUpdate();

            if (CurrentState == ConveyorState.Flushing)
            {
                Flushing();
                return;
            }

            if (CurrentState == ConveyorState.Idle)
            {
                if (IsLaneEmpty())
                {
                    EnterWaitState();
                }
            }

            if (CurrentState == ConveyorState.Wait)
            {
                if (IsWaitReceiving() == false
                    || (SpeedDownSensor.IsSignalOn() == true || ReadySensor.IsSignalOn() == true || EjectSensor.IsSignalOn() == true))
                {
                    CurrentState = ConveyorState.Idle;
                    Available = false;
                }
            }
            else
            {
                if (IsLaneEmpty() == true)
                {
                    Available = true;
                }
                else
                {
                    Available = false;
                }
            }

            if (CheckTimeOut() == true)
            {
                return;
            }

            if (repeatMode == false && Available == true && IsWaitReceiving() == true)
            {
                digitalIoHandler.WriteOutput(outMachineAvailable, true);
            }
            else
            {
                digitalIoHandler.WriteOutput(outMachineAvailable, false);
            }

            if (transitState != ConveyorState.NoChange)
            {
                return;
            }

            switch (CurrentState)
            {
                case ConveyorState.Idle:
                    break;
                case ConveyorState.Wait:
                    if (EntrySensor.IsSignalOn() == true)
                    {
                        if (InsufficientTimeMs > 0)
                        {
                            insufficientTimer.Start();
                        }

                        StopConveyorNow();
                        Available = false;

                        CurrentState = ConveyorState.FastReceiving;
                        fastMoveTimer.Start();
                    }
                    break;
                case ConveyorState.FastReceiving:
                    if (EntrySensor.IsSignalOn() == true)
                    {
                        timeOutTimer.Start();
                        fastMoveTimer.Start();
                        MoveForward();
                        StopperUp();
                    }
                    else
                    {
                        if (SpeedDownSensor.IsSignalOn() == true
                            || (fastMoveTimer.ElapsedMilliseconds > FastMoveTimeMs))
                        {
                            SpeedDown();
                        }
                    }
                    break;
                case ConveyorState.SlowReceiving:
                    if (ReadySensor.IsSignalOn() == true)
                    {
                        if (insufficientTimer.IsRunning == true && insufficientTimer.ElapsedMilliseconds < InsufficientTimeMs)
                        {
                            timeOut = true;
                        }

                        if (IsConveyorRunning() == true)
                        {
                            StopConveyor(LoadingStopDelayTimeMs);
                        }
                        else if (conveyorStopTimer.IsRunning == false)
                        {
                            if (passMode == true)
                            {
                                PassPCB();
                            }
                            else
                            {
                                ExitReceiving();
                            }
                        }
                    }
                    else
                    {
                        if (isPause == true)
                        {
                            StopMotor();
                        }
                        else if (IsConveyorRunning() == false)
                        {
                            MoveForward();
                        }
                    }
                    break;
                case ConveyorState.SidePusherForward:
                    if (sidePusherTimer.ElapsedMilliseconds > SidePusherTimeMs || useSidePusher == false)
                    {
                        if (useSidePushAfterClamp == true)
                        {
                            //PostMessage();
                            CurrentState = ConveyorState.Clamped;
                        }
                        else
                        {
                            ClampPCB();
                        }
                    }
                    break;
                case ConveyorState.Clamping:
                    if (clampStableTimer.ElapsedMilliseconds > ClampStableTimeMs)
                    {
                        ExitClamping();
                    }
                    break;
                case ConveyorState.Clamped:
                    StopperDown();
                    break;
                case ConveyorState.Unclamping:
                    if (clampStableTimer.ElapsedMilliseconds > ClampStableTimeMs)
                    {
                        CurrentState = ConveyorState.Unclamped;
                    }
                    break;
                case ConveyorState.Unclamped:
                    if (ReadySensor.IsSignalOn() == true)
                    {
                        outputFastMovingTimer.Start();

                        if (useEntryOutput == true)
                        {
                            MoveBackward();
                            CurrentState = ConveyorState.BackwardWait;
                        }
                        else
                        {
                            MoveForward();
                            CurrentState = ConveyorState.Received;
                        }
                    }
                    else
                    {
                        StopConveyorNow();
                        CurrentState = ConveyorState.Sending;
                    }
                    break;
                case ConveyorState.Received:
                    if (EjectSensor.IsSignalOn() == true)
                    {
                        outputFastMovingTimer.Stop();

                        if (IsConveyorRunning() == true)
                        {
                            StopConveyorNow();
                        }

                        CurrentState = ConveyorState.Sending;
                    }
                    else
                    {
                        if (outputFastMovingTimer.ElapsedMilliseconds > OutputFastMovingTimeMs)
                        {
                            outputFastMovingTimer.Stop();
                            MoveSlowForward();
                        }
                    }
                    break;
                case ConveyorState.Sending:
                    if (EjectSensor.IsSignalOn() == true)
                    {
                        if (repeatMode == true)
                        {
                            ReturnPCB();
                        }
                        else
                        {
                            //if (re)
                            SendSignal();
                        }
                    }
                    else
                    {
                        digitalIoHandler.WriteOutput(outBoardAvailable, false);
                        EnterWaitState();
                    }
                    break;
                case ConveyorState.Sending2:
                    if (EjectSensor.IsSignalOn() == false)
                    {
                        StopConveyor(EjectStopDelayTimeMs);

                        digitalIoHandler.WriteOutput(outBoardAvailable, false);
                        digitalIoHandler.WriteOutput(outNGPcb, false);

                        EnterWaitState();
                    }
                    break;
                case ConveyorState.Backward:
                    if (EjectSensor.IsSignalOn() == false)
                    {
                        Available = false;
                        CurrentState = ConveyorState.Backward2;
                    }
                    break;
                case ConveyorState.Backward2:
                    if (EntrySensor.IsSignalOn() == true)
                    {
                        StopConveyorNow();
                        CurrentState = ConveyorState.Idle;

                        WaitReceiving = true;
                        EnterWaitState();
                    }
                    break;
                case ConveyorState.BackwardWait:
                    if (EntrySensor.IsSignalOn() == true)
                    {
                        StopConveyorNow();
                        AutoReceiving = false;
                        WaitReceiving = false;

                        CurrentState = ConveyorState.Idle;
                    }
                    break;
                default:
                    break;
            }
        }

        public void EnterWaitState()
        {
            if (IsWaitReceiving() == true)
            {
                CurrentState = ConveyorState.Wait;
            }
            else
            {
                CurrentState = ConveyorState.Idle;
            }

            /*
             if (m_pConveyorLane->IsWaitReceiving() || (GetPreviousConveyor() && GetPreviousConveyor()->GetState() != IdleState))
			        m_state = WaitState;
		        else
			        m_state = IdleState;

		        if (GetClosedLoopConfig().GetFujiConfig().IsUseAutoModelChange() 
				        && GetClosedLoopConfig().GetFujiConfig().GetChangeOverMode() == eFujiFSFDefinition::ChangeOverMode_PanelID)
		        {
			        if (m_state == IdleState)
			        {
				        GetClosedLoopConfig().GetFujiConfig().DeleteLaneNoTxtForNext(m_pConveyorLane->GetConveyorLaneId());
			        }
		        }

		        ResetBoardInfo();
             */
        }

        private bool useSidePushAfterClamp = true;
        public void ExitClamping()
        {
            if (useSidePushAfterClamp == true)
            {
                clamper?.ClampUp();
                sidePusherTimer.Start();
                CurrentState = ConveyorState.Clamping;
            }
            else
            {
                sidePusher.Eject();
                sidePusherTimer.Start();
                CurrentState = ConveyorState.SidePusherForward;
            }
        }

        private bool useClamp = true;
        public void ClampPCB()
        {
            if (useClamp == true)
            {
                clamper?.ClampUp();

                if (ClampStableTimeMs > 0)
                {
                    clampStableTimer.Start();
                    CurrentState = ConveyorState.Clamping;
                }
                else
                {
                    CurrentState = ConveyorState.Clamped;
                }
            }
            else
            {
                ExitClamping();
            }
        }

        public void EjectPCB()
        {
            CurrentState = ConveyorState.Unclamping;
            UnclampPCB();
        }

        public void UnclampPCB()
        {
            if (useClamp)
            {
                clamper?.ClampDown();
            }

            stopper.Inject();
            sidePusher.Inject();

            if (useClamp)
            {
                clampStableTimer.Start();
            }
        }

        private bool passMode = false;
        public void SendBoard()
        {
            CurrentState = ConveyorState.Sending;

            StopperDown();
            MoveForward();
        }

        public void PassPCB()
        {
            CurrentState = ConveyorState.Received;

            StopperDown();
            MoveSlowForward();
        }

        public bool IsPCBClamped()
        {
            return CurrentState == ConveyorState.Clamped;
        }

        public void ReturnPCB()
        {
            StopConveyorNow();

            if (EntrySensor.IsSignalOn() == true)
            {
                EnterWaitState();
            }
            else
            {
                MoveBackward();
                CurrentState = ConveyorState.Backward;
            }
        }

        private bool ngResult = false;
        public void SendSignal()
        {
            if (passMode == true)
            {
                digitalIoHandler.WriteOutput(outNGPcb, false);
                digitalIoHandler.WriteOutput(outMachineAvailable, true);
            }
            else
            {
                digitalIoHandler.WriteOutput(outNGPcb, ngResult);
                digitalIoHandler.WriteOutput(outMachineAvailable, true);
            }

            if (NextMachineAvailableSensor.IsSignalOn() == false)
            {
                return;
            }

            MoveForward();
            CurrentState = ConveyorState.Sending2;
        }

        private bool repeatMode = false;
        //bool autoEject = true;
        //bool useNgBuffer = false;
        public bool IsNextMachineAvailable()
        {
            if (EmulateNextMachineAvailable == true)
            {
                EmulateNextMachineAvailable = false;
                return true;
            }

            if (repeatMode == true)
            {
                return false;
            }

            if (passMode == true)
            {
                return NextMachineAvailableSensor.IsSignalOn();
            }

            /*
            if (ngResult == true)
            {
                if (autoEject == false && useNgBuffer == false)
                {
                    return nextMachineAvailable.SignalOn();
                }
            }
            */

            return NextMachineAvailableSensor.IsSignalOn();
        }

        public bool IsLaneEmpty()
        {
            bool result = true;

            if (EntrySensor.IsSignalOn() == true || SpeedDownSensor.IsSignalOn() == true
                || ReadySensor.IsSignalOn() == true || EjectSensor.IsSignalOn() == true)
            {
                result = false;
            }

            return result;
        }

        private bool useSidePusher = true;
        public void ExitReceiving()
        {
            fastMoveTimer.Stop();

            if (useSidePusher)
            {
                if (useSidePushAfterClamp)
                {
                    clamper?.ClampUp();
                }

                if (ClampStableTimeMs > 0)
                {
                    clampStableTimer.Start();
                    CurrentState = ConveyorState.Clamping;
                    return;
                }
            }
            else
            {
                sidePusher.Eject();
                sidePusherTimer.Start();
            }

            CurrentState = ConveyorState.SidePusherForward;
        }

        public void EnterFlush()
        {
            WaitReceiving = false;

            EnterWaitState();

            StopConveyorNow();
            StopperDown();
            UnclampPCB();

            CurrentState = ConveyorState.Flushing;
        }

        private Stopwatch flushingTimer = new Stopwatch();
        public void Flushing()
        {
            bool pcbCheck = EjectSensor.IsSignalOn();

            if (pcbCheck == true)
            {
                Reset();
                StopMotor();
            }
            else
            {
                if (IsConveyorRunning() == false)
                {
                    MoveForward();
                    flushingTimer.Start();
                }
                else
                {
                    if (flushingTimer.ElapsedMilliseconds > 5000)
                    {
                        Flused();
                    }
                    else
                    {
                        if (IsLaneEmpty() == false)
                        {
                            flushingTimer.Start();
                        }
                    }
                }
            }
        }

        public void Flused(bool exit = false)
        {
            CurrentState = ConveyorState.Idle;

            StopConveyorNow();
            Reset();
            flushingTimer.Stop();

            if (exit == true)
            {
                return;
            }

            if (passMode == true)
            {
                WaitReceiving = true;
                AutoReceiving = true;
            }
            else
            {
                WaitReceiving = false;
                AutoReceiving = false;
            }
        }

        public void StartWorkingProc()
        {
            CurrentState = ConveyorState.Idle;
            stopWorking = false;
            workingTask = new Task(new Action(WorkingProc));
            workingTask.Start();
        }

        private bool stopWorking = false;
        public void StopWorkingProc()
        {
            stopWorking = true;
        }
        public bool PauseWorkingProc { get; set; } = false;

        public void WorkingProc()
        {
            while (stopWorking == false)
            {
                if (PauseWorkingProc == true)
                {
                    Thread.Sleep(UpdateIntervalMs);
                    continue;
                }

                ProecssEvent();

                //Thread.Sleep(updateIntervalMs);
            }

            stopWorking = false;
        }
    }
}
