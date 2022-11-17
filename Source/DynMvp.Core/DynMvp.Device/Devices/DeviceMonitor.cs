using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Devices
{
    /// <summary>
    /// 시스템에 연결된 장치 목록을 관리
    /// </summary>
    public class DeviceMonitor
    {
        private static DeviceMonitor instance = null;
        public static DeviceMonitor Instance()
        {
            if (instance == null)
            {
                instance = new DeviceMonitor();
            }

            return instance;
        }

        public static void SetInstance(DeviceMonitor deviceMonitor)
        {
            instance = deviceMonitor;
        }

        protected IoMonitor ioMonitor;
        public IoMonitor IoMonitor => ioMonitor;

        protected MotionMonitor motionMonitor;
        public MotionMonitor MotionMonitor => motionMonitor;

        public bool IgnoreOriginButton { get; set; } = false;
        public bool IgnoreDoorOpened { get; set; } = false;
        public bool IgnoreEmergencyButton { get; set; } = false;
        public bool IgnoreAirPresure { get; set; } = false;
        public bool IgnoreServoOn { get; set; } = false;

        protected List<InputStateHandler> originButtonList = new List<InputStateHandler>();
        public List<InputStateHandler> OriginButtonList => originButtonList;

        protected List<InputStateHandler> doorOpenedList = new List<InputStateHandler>();
        public List<InputStateHandler> DoorOpenedList => doorOpenedList;

        protected List<InputStateHandler> emergencyButtonList = new List<InputStateHandler>();
        public List<InputStateHandler> EmergencyButtonList => emergencyButtonList;

        protected List<InputStateHandler> airPressureList = new List<InputStateHandler>();
        public List<InputStateHandler> AirPressureList => airPressureList;

        protected List<InputStateHandler> servoAlarmList = new List<InputStateHandler>();
        public List<InputStateHandler> ServoAlarmList => servoAlarmList;

        public DeviceMonitor() { }

        public virtual void Initialize()
        {
            LogHelper.Debug(LoggerType.StartUp, "Start - Initialize Machine");

            PortMap portMap = DeviceManager.Instance().PortMap;
            DigitalIoHandler digitalIoHandler = DeviceManager.Instance().DigitalIoHandler;

            List<IoPort> originPorts = portMap.GetPorts(IoGroup.Origin);
            foreach (IoPort ioPort in originPorts)
            {
                var originButton = new InputStateHandler("Emergency", digitalIoHandler, ioPort);
                originButton.OnInputOn += OriginButton_OnInputOn;

                originButtonList.Add(originButton);
            }

            List<IoPort> emergencyPorts = portMap.GetPorts(IoGroup.Emergency);
            foreach (IoPort ioPort in emergencyPorts)
            {
                var emergencyButton = new InputStateHandler("Emergency", digitalIoHandler, ioPort);
                emergencyButton.OnInputOn += EmergencyButton_ButtonOn;
                emergencyButton.OnInputOff += Alarmstate_reset;

                emergencyButtonList.Add(emergencyButton);
            }

            List<IoPort> airPressureLowPorts = portMap.GetPorts(IoGroup.AirPressure);
            foreach (IoPort ioPort in airPressureLowPorts)
            {
                var airPressurePort = new InputStateHandler("Air Pressure", digitalIoHandler, ioPort);
                airPressurePort.OnInputOn += AirPressure_OnInputOn;
                airPressurePort.OnInputOff += Alarmstate_reset;

                airPressureList.Add(airPressurePort);
            }

            List<IoPort> doorPorts = portMap.GetPorts(IoGroup.Door);
            foreach (IoPort ioPort in doorPorts)
            {
                var doorOpened = new InputStateHandler("Door Opened", digitalIoHandler, ioPort);
                doorOpened.OnInputOn += DoorOpened_OnInputOn;
                doorOpened.OnInputOff += Alarmstate_reset;

                doorOpenedList.Add(doorOpened);
            }

            List<IoPort> servoAlarmPorts = portMap.GetPorts(IoGroup.ServoAlarm);
            foreach (IoPort ioPort in servoAlarmPorts)
            {
                var servoAlarm = new InputStateHandler("Servo Alarm", digitalIoHandler, ioPort);
                servoAlarm.OnInputOn += ServoAlarm_OnInputOn;
                servoAlarm.OnInputOff += Alarmstate_reset;

                ServoAlarmList.Add(servoAlarm);
            }

            if (DeviceConfig.Instance().UseRobotStage)
            {
                motionMonitor = new MotionMonitor(DeviceManager.Instance().MotionHandler);
                motionMonitor.Start();
            }

            ioMonitor = new IoMonitor(digitalIoHandler);
            ioMonitor.ProcessInputChanged += ProcessInputChanged;
            ioMonitor.Start();
        }

        private void OriginButton_OnInputOn(InputStateHandler eventSource)
        {
            if (IgnoreOriginButton == false)
            {
                Task.Run(() =>
                {
                    Thread.Sleep(2000);
                    DeviceManager.Instance().RobotOrigin();
                });
            }
        }

        private void DoorOpened_OnInputOn(InputStateHandler eventSource)
        {
            if (IgnoreDoorOpened == false)
            {
                ErrorItem errorItem = ErrorManager.Instance().Report((int)ErrorSection.Safety, (int)SafetyError.DoorOpen, ErrorLevel.Error,
                        ErrorSection.Safety.ToString(), SafetyError.DoorOpen.ToString(), "Door is opened : " + eventSource.PortName);

                AddResetHandler(errorItem, eventSource);
            }
        }

        private void AirPressure_OnInputOn(InputStateHandler eventSource)
        {
            if (IgnoreAirPresure == false)
            {
                ErrorItem errorItem = ErrorManager.Instance().Report((int)ErrorSection.Machine, (int)MachineError.AirPressure, ErrorLevel.Error,
                    ErrorSection.Machine.ToString(), MachineError.AirPressure.ToString(), "Air Pressure is not supplied : " + eventSource.PortName);

                AddResetHandler(errorItem, eventSource);
            }
        }

        private void EmergencyButton_ButtonOn(InputStateHandler eventSource)
        {
            if (IgnoreEmergencyButton == false)
            {
                ErrorItem errorItem = ErrorManager.Instance().Report((int)ErrorSection.Safety, (int)SafetyError.EmergencySwitch, ErrorLevel.Error,
                    ErrorSection.Safety.ToString(), SafetyError.EmergencySwitch.ToString(), "Emergency stop button is pressed : " + eventSource.PortName);

                AddResetHandler(errorItem, eventSource);
            }
        }

        private void ServoAlarm_OnInputOn(InputStateHandler eventSource)
        {
            if (IgnoreServoOn == false)
            {
                ErrorItem errorItem = ErrorManager.Instance().Report((int)ErrorSection.Safety, (int)SafetyError.ServoOff, ErrorLevel.Error,
                        ErrorSection.Safety.ToString(), SafetyError.ServoOff.ToString(), "Servo power offed: " + eventSource.PortName);

                AddResetHandler(errorItem, eventSource);
            }
        }

        private void AddResetHandler(ErrorItem errorItem, InputStateHandler eventSource)
        {
            if (errorItem != null)
            {
                eventSource.ErrorItem = errorItem;
                errorItem.ResetHandler = new Action(() => { eventSource.Reset(); eventSource.ErrorItem = null; });
            }
        }

        private void Alarmstate_reset(InputStateHandler eventSource)
        {
            eventSource.Reset();

            if (eventSource.ErrorItem != null)
            {
                eventSource.ErrorItem.Alarmed = false;
                eventSource.ErrorItem = null;
                ErrorManager.Instance().Updated = true;
            }
        }

        public virtual bool OnEnterWaitInspection()
        {
            return true;
        }

        internal virtual bool OnExitWaitInspection()
        {
            return true;
        }

        internal virtual void OnStartInspection() { }

        public virtual void OnStopInspection() { }

        public virtual bool ProcessInputChanged(DioValue inputValue)
        {
            bool stateChaned = false;
            foreach (InputStateHandler doorOpened in doorOpenedList)
            {
                stateChaned |= doorOpened.CheckState(inputValue);
            }

            foreach (InputStateHandler airPressure in airPressureList)
            {
                stateChaned |= airPressure.CheckState(inputValue);
            }

            foreach (InputStateHandler emergencyButton in emergencyButtonList)
            {
                stateChaned |= emergencyButton.CheckState(inputValue);
            }

            foreach (InputStateHandler originButton in originButtonList)
            {
                stateChaned |= originButton.CheckState(inputValue);
            }

            foreach (InputStateHandler servoAlarm in servoAlarmList)
            {
                stateChaned |= servoAlarm.CheckState(inputValue);
            }
            return stateChaned;
        }
    }
}
