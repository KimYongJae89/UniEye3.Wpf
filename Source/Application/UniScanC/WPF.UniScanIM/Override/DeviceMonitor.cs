using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;

namespace WPF.UniScanIM.Override
{
    public delegate void BooleanDelegate(bool value);

    public class DeviceMonitor : DynMvp.Devices.DeviceMonitor
    {
        public BooleanDelegate FrameTriggerChanged { get; set; }

        public bool IsFrameTriggerOn { get; set; }

        private InputStateHandler FrameTriggerState { get; set; }

        public override void Initialize()
        {
            if (DeviceConfig.Instance().VirtualMode)
            {
                return;
            }

            IgnoreAirPresure = true;

            PortMap portMap = DeviceManager.Instance().PortMap;
            DigitalIoHandler digitalIoHandler = DeviceManager.Instance().DigitalIoHandler;

            IoPort frameTriggerPort = portMap.GetPort("InFrameTriggerSignal");
            if (frameTriggerPort != null)
            {
                FrameTriggerState = new InputStateHandler("InFrameTriggerSignal", digitalIoHandler, frameTriggerPort);
                FrameTriggerState.OnInputOn += FrameTriggerState_OnInputOn;
                FrameTriggerState.OnInputOff += FrameTriggerState_OnInputOff;
            }

            base.Initialize();
        }

        private void FrameTriggerState_OnInputOn(InputStateHandler eventSource)
        {
            IsFrameTriggerOn = true;
            LogHelper.Info(LoggerType.Comm, "Frame Trigger On");
            FrameTriggerChanged?.Invoke(IsFrameTriggerOn);
        }

        private void FrameTriggerState_OnInputOff(InputStateHandler eventSource)
        {
            IsFrameTriggerOn = false;
            LogHelper.Info(LoggerType.Comm, "Frame Trigger Off");
            FrameTriggerChanged?.Invoke(IsFrameTriggerOn);
        }

        public override bool ProcessInputChanged(DioValue inputValue)
        {
            bool stateChaned = false;

            stateChaned = base.ProcessInputChanged(inputValue);

            if (FrameTriggerState != null)
            {
                stateChaned |= FrameTriggerState.CheckState(inputValue);
            }

            return stateChaned;
        }
    }
}
