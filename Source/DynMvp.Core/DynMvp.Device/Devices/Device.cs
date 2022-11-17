using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices
{
    public enum DeviceType
    {
        FrameGrabber, MotionController, DigitalIo, LightController, DaqChannel, Camera, DepthScanner, BarcodeReader, BarcodePrinter, ConveyorSystem
    }

    public enum DeviceState
    {
        Idle, Ready, Warning, Error
    }

    public abstract class Device : IDisposable
    {
        public string Name { get; set; }
        public DeviceType DeviceType { get; set; }
        public DeviceState State { get; set; }
        public string StateMessage { get; set; }

        public bool IsReady()
        {
            if (State == DeviceState.Ready)
            {
                return true;
            }

            return false;
        }

        public bool IsError()
        {
            if (State == DeviceState.Error)
            {
                return true;
            }

            return false;
        }

        public void UpdateState(DeviceState state, string stateMessage = "")
        {
            State = state;
            StateMessage = stateMessage;
        }

        public virtual void Release()
        {
            DeviceManager.Instance().RemoveDevice(this);
        }

        public void Dispose()
        {
            Release();
        }
    }
}
