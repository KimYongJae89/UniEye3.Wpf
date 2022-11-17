using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Inspect;
using DynMvp.InspectData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Config;

namespace UniEye.Base.Data
{
    public enum OpState
    {
        Idle, Wait, Align, Inspect, Teach
    }

    public enum InspectState
    {
        Align, Run, Pause, Review, Done, Scan, Ready
    }

    public delegate void OpStateChangedDelegate();
    public class SystemState : IInspectEventListener
    {
        private TimeOutTimer aliveCheckTimer = new TimeOutTimer();
        private bool lastVisionAliveOn = false;
        public bool MachineDead { get; private set; } = false;

        private int machineAliveHoldCount = 0;
        private bool lastMachineAliveOn = false;
        public ValueTable<bool> MachineState { get; } = new ValueTable<bool>();
        public ValueTable<bool> VisionState { get; } = new ValueTable<bool>();
        public OpState OpState { get; private set; }
        public OpStateChangedDelegate OpStateChanged { get; set; }
        public InspectState InspectState { get; private set; }
        public bool OnWaitStop { get; set; } = true;
        public bool Alarmed { get; set; }

        private static SystemState _instance;
        public static SystemState Instance()
        {
            if (_instance == null)
            {
                _instance = new SystemState();
            }

            return _instance;
        }

        public bool OnInspection => OpState == OpState.Inspect;

        public bool OnInspectOrWait => OpState == OpState.Inspect || OpState == OpState.Wait;

        public bool OnTeaching => OpState == OpState.Teach;

        private SystemState()
        {

        }

        public void SetIdle()
        {
            DeviceManager.Instance().TowerLamp?.SetState(TowerLampStateType.Idle);
            OpState = OpState.Idle;
            InspectState = InspectState.Done;
            OpStateChanged?.Invoke();
        }

        public void SetWait()
        {
            DeviceManager.Instance().TowerLamp?.SetState(TowerLampStateType.Wait);
            OpState = OpState.Wait;
            InspectState = InspectState.Done;
            OpStateChanged?.Invoke();
        }

        public void SetAlign()
        {
            DeviceManager.Instance().TowerLamp?.SetState(TowerLampStateType.Working);
            OpState = OpState.Align;
            InspectState = InspectState.Done;
            OpStateChanged?.Invoke();
        }

        public void SetInspectState(InspectState inspectState)
        {
            OpState = OpState.Inspect;
            InspectState = inspectState;
            DeviceManager.Instance().TowerLamp?.SetState(TowerLampStateType.Working);
            OpStateChanged?.Invoke();
        }

        public void SetTeach()
        {
            OpState = OpState.Teach;
            InspectState = InspectState.Done;
            DeviceManager.Instance().TowerLamp?.SetState(TowerLampStateType.Working);
            OpStateChanged?.Invoke();
        }

        public virtual void SetupState(int numTrigger = 1)
        {
            VisionState.Clear();
            MachineState.Clear();

            VisionState.AddValue("ALIVE", false);
            VisionState.AddValue("READY", false);

            MachineState.AddValue("ALIVE", false);

            if (numTrigger > 1)
            {
                for (int i = 0; i < numTrigger; i++)
                {
                    VisionState.AddValue("BUSY" + i, false);
                    VisionState.AddValue("COMPLETE" + i, false);
                    VisionState.AddValue("RESULT_NG" + i, false);

                    MachineState.AddValue("TRIG" + i, false);
                    MachineState.AddValue("DONE" + i, false);
                    MachineState.AddValue("CANCEL + i", false);
                }
            }
            else
            {
                VisionState.AddValue("BUSY", false);
                VisionState.AddValue("COMPLETE", false);
                VisionState.AddValue("RESULT_NG", false);

                MachineState.AddValue("TRIG", false);
                MachineState.AddValue("DONE", false);
                MachineState.AddValue("CANCEL", false);
            }
        }

        public virtual void SetVisionAlive(bool flag)
        {
            VisionState["ALIVE"] = flag;
        }

        public virtual bool IsVisionAlive()
        {
            return VisionState["ALIVE"];
        }

        public virtual void ToggleMachineAlive()
        {
            MachineState["ALIVE"] = !MachineState["ALIVE"];
        }

        public virtual void SetMachineAlive(bool flag)
        {
            MachineState["ALIVE"] = flag;
        }

        public virtual bool IsMachineAlive()
        {
            return MachineState["ALIVE"];
        }

        public virtual void SetReady(bool flag)
        {
            VisionState["READY"] = flag;

            if (flag == true)
            {
                VisionState.SetValue("BUSY", false);
                VisionState.SetValue("COMPLETE", false);
                VisionState.SetValue("RESULT_NG", false);
            }
        }

        public virtual bool IsReady()
        {
            return VisionState["READY"];
        }

        public virtual void SetBusy(int triggerIndex = -1)
        {
            VisionState["READY"] = false;

            if (triggerIndex > -1)
            {
                VisionState["BUSY" + triggerIndex] = true;
                VisionState["COMPLETE" + triggerIndex] = false;
                VisionState["RESULT_NG" + triggerIndex] = false;
            }
            else
            {
                VisionState["BUSY"] = true;
                VisionState["COMPLETE"] = false;
                VisionState["RESULT_NG"] = false;
            }
        }

        public virtual bool IsBusy(int triggerIndex = -1)
        {
            if (triggerIndex > -1)
            {
                return VisionState["BUSY" + triggerIndex];
            }
            else
            {
                return VisionState["BUSY"];
            }
        }

        public virtual void SetResult(ProductResult productResult)
        {
            if (productResult.TriggerIndex > -1)
            {
                VisionState["RESULT_NG" + productResult.TriggerIndex] = productResult.IsDefected();
            }
            else
            {
                VisionState["RESULT_NG"] = productResult.IsDefected();
            }
        }

        public virtual bool IsResultNg(int triggerIndex = -1)
        {
            if (triggerIndex > -1)
            {
                return VisionState["RESULT_NG" + triggerIndex];
            }
            else
            {
                return VisionState["RESULT_NG"];
            }
        }

        public virtual void SetComplete(int triggerIndex = -1)
        {
            if (triggerIndex > -1)
            {
                VisionState["COMPLETE" + triggerIndex] = true;
                VisionState["BUSY" + triggerIndex] = false;
            }
            else
            {
                VisionState["COMPLETE"] = true;
                VisionState["BUSY"] = false;
            }
        }

        public virtual bool IsComplete(int triggerIndex = -1)
        {
            if (triggerIndex > -1)
            {
                return VisionState["COMPLETE" + triggerIndex];
            }
            else
            {
                return VisionState["COMPLETE"];
            }
        }

        public virtual void SetTrigger(bool flag, int triggerIndex = -1)
        {
            if (triggerIndex > -1)
            {
                MachineState["TRIG" + triggerIndex] = flag;
            }
            else
            {
                MachineState["TRIG"] = flag;
            }
        }

        public virtual bool IsTriggerred(int triggerIndex = -1)
        {
            if (triggerIndex > -1)
            {
                return MachineState["TRIG" + triggerIndex];
            }
            else
            {
                return MachineState["TRIG"];
            }
        }

        public virtual void SetDone(bool flag, int triggerIndex = -1)
        {
            if (triggerIndex > -1)
            {
                MachineState["DONE" + triggerIndex] = flag;
            }
            else
            {
                MachineState["DONE"] = flag;
            }
        }

        public virtual bool IsDone(int triggerIndex = -1)
        {
            if (triggerIndex > -1)
            {
                return MachineState["DONE" + triggerIndex];
            }
            else
            {
                return MachineState["DONE"];
            }
        }

        public virtual void SetCancel(bool flag, int triggerIndex = -1)
        {
            if (triggerIndex > -1)
            {
                MachineState["CANCEL" + triggerIndex] = flag;
            }
            else
            {
                MachineState["CANCEL"] = flag;
            }
        }

        public virtual bool IsCancel(int triggerIndex = -1)
        {
            if (triggerIndex > -1)
            {
                return MachineState["CANCEL" + triggerIndex];
            }
            else
            {
                return MachineState["CANCEL"];
            }
        }

        private bool aliveTaskStop;
        private Task visionAliveTask;
        private Task machineAliveTask;

        /// <summary>
        /// Machine I/F 시작
        /// </summary>
        public virtual void Start()
        {
            SetReady(false);

            aliveTaskStop = false;

            visionAliveTask = Task.Run(() =>
            {
                while (aliveTaskStop == false)
                {
                    SetVisionAlive(!IsVisionAlive());
                    Thread.Sleep(100);
                }
            });

            machineAliveTask = Task.Run(() =>
            {
                int unchangedCount = 0;
                bool preMachineAlive = false;
                while (aliveTaskStop == false)
                {
                    if (SystemState.Instance().MachineDead == false)
                    {
                        if (IsMachineAlive() != preMachineAlive)
                        {
                            preMachineAlive = IsMachineAlive();
                            unchangedCount = 0;
                        }
                        else
                        {
                            unchangedCount++;
                            if (unchangedCount > 20)
                            {
                                MachineDead = true;
                            }
                        }
                    }

                    Thread.Sleep(100);
                }
            });
        }

        /// <summary>
        /// Machine I/F 중지
        /// </summary>
        public virtual void Stop()
        {
            aliveTaskStop = true;

            Task.WaitAll(new Task[] { visionAliveTask, machineAliveTask });
        }

        protected void CheckAlive()
        {
            if (aliveCheckTimer.TimeOut)
            {
                lastVisionAliveOn = !lastVisionAliveOn;
                SetVisionAlive(lastVisionAliveOn);

                if (OperationConfig.Instance().UseMachineAlive)
                {
                    bool machineAliveOn = IsMachineAlive();

                    if (machineAliveOn == lastMachineAliveOn)
                    {
                        machineAliveHoldCount++;
                        if (machineAliveHoldCount > 5)
                        {
                            MachineDead = true;
                        }
                    }
                    else
                    {
                        lastMachineAliveOn = machineAliveOn;
                    }
                }

                aliveCheckTimer.Restart();
            }

            SetReady(OpState == OpState.Wait) /*OnReady*/;
        }


        public virtual bool EnterWaitInspection()
        {
            SetReady(true);

            return true;
        }

        public virtual void ExitWaitInspection()
        {
            SetReady(false);
        }

        public virtual void ProductBeginInspect(ProductResult productResult)
        {
            SetBusy(productResult.TriggerIndex);
        }

        public virtual void ProductInspected(ProductResult productResult)
        {
            SetResult(productResult);
        }

        public virtual void ProductEndInspect(ProductResult productResult)
        {
            SetComplete(productResult.TriggerIndex);
        }

        public void StepOrderEndInspect(ModelBase model, int inspectOrder, ProductResult productResult) { }
        public void StepBeginInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer) { }
        public void StepEndInspect(InspectStep inspectStep, ProductResult productResult, ImageBuffer imageBuffer) { }
        public void TargetBeginInspect(Target target) { }
        public void TargetEndInspect(Target target, ProbeResultList probeResultList) { }
        public void TargetOrderEndInspect(InspectStep inspectStep, int inspectOrder, ProbeResultList probeResultList) { }
        public void ProbeBeginInspect() { }
        public void ProbeEndInspect() { }
    }
}
