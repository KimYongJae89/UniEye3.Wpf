using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices
{
    public delegate void IoEventHandler(InputStateHandler eventSource);

    public class InputStateHandler
    {
        private string name;
        private DigitalIoHandler digitalIoHandler;
        private IoPort inputPort;
        private bool inputOn = false;
        public IoEventHandler OnInputOn;
        public IoEventHandler OnInputOff;
        public ErrorItem ErrorItem { get; set; }

        public string PortName => inputPort.Name;


        public InputStateHandler(string name, DigitalIoHandler digitalIoHandler, IoPort inputPort)
        {
            this.name = name;
            this.digitalIoHandler = digitalIoHandler;
            this.inputPort = inputPort;
        }

        public bool CheckState(DioValue inputValue)
        {
            bool buttonState = IoMonitor.CheckInput(inputValue, inputPort);
            if (inputPort.Invert)
            {
                buttonState = !buttonState;
            }

            if (buttonState)
            {
                if (inputOn == false)
                {
                    inputOn = true;
                    LogHelper.Debug(LoggerType.Device, string.Format("{0} is Turned On", name));
                    OnInputOn?.Invoke(this);
                }
            }
            else
            {
                if (inputOn == true)
                {
                    if (OnInputOff != null)
                    {
                        LogHelper.Debug(LoggerType.Device, string.Format("{0} is Turned Off", name));
                        OnInputOff(this);
                    }
                }

                inputOn = false;
            }

            return inputOn;
        }

        public bool CheckStateReverse(DioValue inputValue)
        {
            bool buttonState = IoMonitor.CheckInput(inputValue, inputPort);
            if (buttonState == false)
            {
                if (inputOn == false)
                {
                    inputOn = true;
                    LogHelper.Debug(LoggerType.Device, string.Format("{0} is Turned On", name));
                    OnInputOn?.Invoke(this);
                }
            }
            else
            {
                if (inputOn == true)
                {
                    if (OnInputOff != null)
                    {
                        LogHelper.Debug(LoggerType.Device, string.Format("{0} is Turned Off", name));
                        OnInputOff(this);
                    }
                }

                inputOn = false;
            }

            return inputOn;
        }

        internal void Reset()
        {
            inputOn = false;
        }
    }

    public delegate void IoButtonHandler(IoButton eventSource);

    public class IoButton
    {
        private string name;
        private DigitalIoHandler digitalIoHandler;
        private IoPort buttonInPort;
        private IoPort lampOutPort;
        private bool lastButtonOn = false;
        public IoButtonHandler ButtonPushed;
        public IoButtonHandler ButtonPulled;

        public IoButton(string name, DigitalIoHandler digitalIoHandler, IoPort buttonInPort, IoPort lampOutPort)
        {
            this.name = name;
            this.digitalIoHandler = digitalIoHandler;
            this.buttonInPort = buttonInPort;
            this.lampOutPort = lampOutPort;
        }

        public void TurnOn()
        {
            if (lampOutPort != null)
            {
                digitalIoHandler.WriteOutput(lampOutPort, true);
            }
        }

        public void TurnOff()
        {
            if (lampOutPort != null)
            {
                digitalIoHandler.WriteOutput(lampOutPort, false);
            }
        }

        public void ResetState()
        {
            lastButtonOn = false;
        }

        public bool CheckState(DioValue inputValue)
        {
            bool curButtonOn = IoMonitor.CheckInput(inputValue, buttonInPort);
            if (curButtonOn)
            {
                if (lastButtonOn == false)
                {
                    lastButtonOn = true;
                    LogHelper.Debug(LoggerType.Device, string.Format("{0} Button Pushed", name));
                    ButtonPushed?.Invoke(this);
                }
            }
            else
            {
                if (lastButtonOn == true)
                {
                    if (ButtonPulled != null)
                    {
                        LogHelper.Debug(LoggerType.Device, string.Format("{0} Button Pulled", name));
                        ButtonPulled(this);
                    }
                }

                lastButtonOn = false;
            }

            return lastButtonOn;
        }
    }
}
