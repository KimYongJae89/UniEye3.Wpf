using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices
{
    public enum SensorLogic
    {
        And, Or
    }

    public class Cylinder
    {
        private string name;
        private DigitalIoHandler digitalIoHandler;
        private List<IoPort> injectionPortList = new List<IoPort>();
        private List<IoPort> ejectPortList = new List<IoPort>();
        private List<IoPort> injectionSensorPortList = new List<IoPort>();
        private List<IoPort> ejectSensorPortList = new List<IoPort>();
        private bool injectSensorDetected = false;
        private bool ejectSensorDetected = false;
        public IsActionError IsActionError { get; set; }
        public SensorLogic EjectSensorLogic { get; set; } = SensorLogic.And;
        public SensorLogic InjectSensorLogic { get; set; } = SensorLogic.And;

        private static int actionDoneWaitS = 10;
        public static int ActionDoneWaitS
        {
            get => Cylinder.actionDoneWaitS;
            set => Cylinder.actionDoneWaitS = value;
        }

        private static int airActionStableTimeMs;
        public static int AirActionStableTimeMs
        {
            get => Cylinder.airActionStableTimeMs;
            set => Cylinder.airActionStableTimeMs = value;
        }
        public bool UseInjectionDoneCheck { get; set; } = true;
        public bool UseEjectionDoneCheck { get; set; } = true;

        public Cylinder(string name)
        {
            this.name = name;
        }

        public Cylinder(string name, DigitalIoHandler digitalIoHandler, IoPort injectionPort, IoPort ejectPort, IoPort injectionSensorPort = null, IoPort ejectSensorPort = null)
        {
            this.name = name;
            this.digitalIoHandler = digitalIoHandler;
            if (injectionPort != null)
            {
                injectionPortList.Add(injectionPort);
            }

            if (ejectPort != null)
            {
                ejectPortList.Add(ejectPort);
            }

            if (injectionSensorPort != null)
            {
                injectionSensorPortList.Add(injectionSensorPort);
            }

            if (ejectSensorPort != null)
            {
                ejectSensorPortList.Add(ejectSensorPort);
            }

            ResetSensorDetectedFlags();
        }

        public void AddActuatorPort(IoPort injectionPort, IoPort ejectPort)
        {
            if (injectionPort != null)
            {
                injectionPortList.Add(injectionPort);
            }

            if (ejectPort != null)
            {
                ejectPortList.Add(ejectPort);
            }
        }

        public void AddSensorPort(IoPort injectionSensorPort, IoPort ejectSensorPort)
        {
            if (injectionSensorPort != null)
            {
                injectionSensorPortList.Add(injectionSensorPort);
            }

            if (ejectSensorPort != null)
            {
                ejectSensorPortList.Add(ejectSensorPort);
            }
        }

        public void ResetSensorDetectedFlags()
        {
            injectSensorDetected = false;
            ejectSensorDetected = false;
        }

        public bool IsOutInjected(bool enableLog = true)
        {
            bool isOutInjected;
            if (EjectSensorLogic == SensorLogic.Or)
            {
                isOutInjected = false;
            }
            else
            {
                isOutInjected = true;
            }

            if (injectionPortList.Count > 0)
            {
                foreach (IoPort ioPort in injectionPortList)
                {
                    if (ioPort.PortNo == IoPort.UNUSED_PORT_NO)
                    {
                        continue;
                    }

                    if (EjectSensorLogic == SensorLogic.Or)
                    {
                        isOutInjected |= digitalIoHandler.ReadOutput(ioPort);
                    }
                    else
                    {
                        isOutInjected &= digitalIoHandler.ReadOutput(ioPort);
                    }
                }
            }
            else
            {
                foreach (IoPort ioPort in ejectPortList)
                {
                    if (ioPort.PortNo == IoPort.UNUSED_PORT_NO)
                    {
                        continue;
                    }

                    if (EjectSensorLogic == SensorLogic.Or)
                    {
                        isOutInjected |= (!digitalIoHandler.ReadOutput(ioPort));
                    }
                    else
                    {
                        isOutInjected &= (!digitalIoHandler.ReadOutput(ioPort));
                    }
                }
            }

            if (isOutInjected && enableLog)
            {
                LogHelper.Debug(LoggerType.Device, string.Format("{0} Eject was Injected", name));
            }

            return isOutInjected;
        }

        public bool IsInjected(bool enableLog = true)
        {
            if (injectionSensorPortList.Count == 0)
            {
                return IsOutInjected();
                //if (enableLog)
                //    LogHelper.Debug(LoggerType.IO, String.Format("{0} inject sensor is inactivated", name));
                //return true;
            }

            ResetSensorDetectedFlags();

            if (injectSensorDetected == false)
            {
                if (InjectSensorLogic == SensorLogic.Or)
                {
                    injectSensorDetected = false;
                }
                else
                {
                    injectSensorDetected = true;
                }

                foreach (IoPort ioPort in injectionSensorPortList)
                {
                    if (ioPort.PortNo == IoPort.UNUSED_PORT_NO)
                    {
                        continue;
                    }

                    if (InjectSensorLogic == SensorLogic.Or)
                    {
                        injectSensorDetected |= digitalIoHandler.ReadInput(ioPort);
                    }
                    else
                    {
                        injectSensorDetected &= digitalIoHandler.ReadInput(ioPort);
                    }
                }

                if (injectSensorDetected && enableLog)
                {
                    LogHelper.Debug(LoggerType.Device, string.Format("{0} inject sensor is detected", name));
                }
            }

            return injectSensorDetected;
        }

        public bool IsOutEjected(bool enableLog = true)
        {
            bool isOutEjected;
            if (EjectSensorLogic == SensorLogic.Or)
            {
                isOutEjected = false;
            }
            else
            {
                isOutEjected = true;
            }

            if (ejectPortList.Count > 0)
            {
                foreach (IoPort ioPort in ejectPortList)
                {
                    if (ioPort.PortNo == IoPort.UNUSED_PORT_NO)
                    {
                        continue;
                    }

                    if (EjectSensorLogic == SensorLogic.Or)
                    {
                        isOutEjected |= digitalIoHandler.ReadOutput(ioPort);
                    }
                    else
                    {
                        isOutEjected &= digitalIoHandler.ReadOutput(ioPort);
                    }
                }
            }
            else
            {
                foreach (IoPort ioPort in injectionPortList)
                {
                    if (ioPort.PortNo == IoPort.UNUSED_PORT_NO)
                    {
                        continue;
                    }

                    if (EjectSensorLogic == SensorLogic.Or)
                    {
                        isOutEjected |= (!digitalIoHandler.ReadOutput(ioPort));
                    }
                    else
                    {
                        isOutEjected &= (!digitalIoHandler.ReadOutput(ioPort));
                    }
                }
            }

            if (isOutEjected && enableLog)
            {
                LogHelper.Debug(LoggerType.Device, string.Format("{0} Eject was Ejected", name));
            }

            return isOutEjected;
        }

        public bool IsEjected(bool enableLog = true)
        {
            if (ejectSensorPortList.Count == 0)
            {
                return IsOutEjected();
            }

            ResetSensorDetectedFlags();

            if (ejectSensorDetected == false)
            {
                if (EjectSensorLogic == SensorLogic.Or)
                {
                    ejectSensorDetected = false;
                }
                else
                {
                    ejectSensorDetected = true;
                }

                foreach (IoPort ioPort in ejectSensorPortList)
                {
                    if (ioPort.PortNo == IoPort.UNUSED_PORT_NO)
                    {
                        continue;
                    }

                    if (EjectSensorLogic == SensorLogic.Or)
                    {
                        ejectSensorDetected |= digitalIoHandler.ReadInput(ioPort);
                    }
                    else
                    {
                        ejectSensorDetected &= digitalIoHandler.ReadInput(ioPort);
                    }
                }

                if (ejectSensorDetected && enableLog)
                {
                    LogHelper.Debug(LoggerType.Device, string.Format("{0} Eject Sensor is detected", name));
                }
            }

            return ejectSensorDetected;
        }

        public bool Inject()
        {
            if (IsInjected() == true)
            {
                return true;
            }

            try
            {
                LogHelper.Debug(LoggerType.Device, string.Format("Inject {0}", name));

                if (Act(true, false) == true)
                {
                    return true;
                }

                LogHelper.Debug(LoggerType.Device, string.Format("Inject - Retry {0}", name));

                return Act(true, true);
            }
            catch (AlarmException)
            {
                return false;
            }
        }

        public bool Eject()
        {
            if (IsEjected() == true)
            {
                return true;
            }

            try
            {
                LogHelper.Debug(LoggerType.Device, string.Format("Eject {0}", name));

                if (Act(false, false) == true)
                {
                    return true;
                }

                LogHelper.Debug(LoggerType.Device, string.Format("Eject - Retry {0}", name));

                return Act(false, true);
            }
            catch (AlarmException)
            {
                return false;
            }
        }

        public void ActAsync(bool inject)
        {
            ErrorManager.Instance().ThrowIfAlarm();

            ResetSensorDetectedFlags();

            //DioValue outputValue = digitalIoHandler.ReadOutput(true);
            //if (inject)
            //    Thread.Sleep(10000);
            //else
            //    Thread.Sleep(5000);

            if (inject)
            {
                foreach (IoPort injectionPort in injectionPortList)
                {
                    if (injectionPort.PortNo == IoPort.UNUSED_PORT_NO)
                    {
                        continue;
                    }

                    digitalIoHandler.WriteOutput(injectionPort, true);

                    //outputValue.UpdateBitFlag(injectionPort, true);
                }

                foreach (IoPort ejectPort in ejectPortList)
                {
                    if (ejectPort.PortNo == IoPort.UNUSED_PORT_NO)
                    {
                        continue;
                    }

                    digitalIoHandler.WriteOutput(ejectPort, false);

                    //outputValue.UpdateBitFlag(ejectPort, false);
                }
            }
            else
            {
                foreach (IoPort injectionPort in injectionPortList)
                {
                    if (injectionPort.PortNo == IoPort.UNUSED_PORT_NO)
                    {
                        continue;
                    }

                    digitalIoHandler.WriteOutput(injectionPort, false);

                    //outputValue.UpdateBitFlag(injectionPort, false);
                }

                foreach (IoPort ejectPort in ejectPortList)
                {
                    if (ejectPort.PortNo == IoPort.UNUSED_PORT_NO)
                    {
                        continue;
                    }

                    digitalIoHandler.WriteOutput(ejectPort, true);
                    //outputValue.UpdateBitFlag(ejectPort, true);
                }
            }

            //digitalIoHandler.WriteOutput(outputValue, true);
        }

        public bool Act(bool inject, bool reportError)
        {
            ActAsync(inject);

            Thread.Sleep(100);

            var actionDoneChecker = new ActionDoneChecker();
            if (inject)
            {
                if (UseInjectionDoneCheck)
                {
                    actionDoneChecker.IsActionDone = IsInjected;
                }
            }
            else
            {
                if (UseEjectionDoneCheck)
                {
                    actionDoneChecker.IsActionDone = IsEjected;
                }
            }

            actionDoneChecker.IsActionError = IsActionError;

            IsActionError = null;

            if (actionDoneChecker.IsActionDone != null)
            {
                if (actionDoneChecker.WaitActionDone(actionDoneWaitS * 1000) == false)
                {
                    if (reportError == true && ActionDoneChecker.StopDoneChecker == false)
                    {
                        MachineError errorType = (inject ? MachineError.CylinderInjection : MachineError.CylinderEjection);

                        ErrorManager.Instance().Report((int)ErrorSection.Machine, (int)errorType,
                            ErrorLevel.Error, ErrorSection.ExternalIF.ToString(), errorType.ToString(), "Cylinder Error : " + name);
                    }

                    return false;
                }
            }

            Thread.Sleep(airActionStableTimeMs);

            return true;
        }
    }
}
