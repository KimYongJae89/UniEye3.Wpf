using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.Dio
{
    public class DigitalIoSerial : DigitalIo
    {
        private SerialDigitalIoInfo serialDigitalIoInfo = null;
        private SerialPortEx serialPortEx = null;
        private bool initialized = false;
        private int dtrBitMask;
        private int rtsBitMask;

        public DigitalIoSerial(string name)
            : base(DigitalIoType.Serial2DIO, name)
        {
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            if (initialized)
            {
                return false;
            }

            if (!(digitalIoInfo is SerialDigitalIoInfo serialDigitalIoInfo))
            {
                return false;
            }

            bool ok = base.Initialize(serialDigitalIoInfo);
            if (!ok)
            {
                return false;
            }

            this.serialDigitalIoInfo = serialDigitalIoInfo;

            dtrBitMask = this.serialDigitalIoInfo.DtrPort;
            rtsBitMask = this.serialDigitalIoInfo.RtsPort;

            serialPortEx = new SerialPortEx();
            initialized = serialPortEx.Open(Name, serialDigitalIoInfo.SerialPortInfo);
            return initialized;

        }

        public override bool IsReady()
        {
            return initialized;
        }

        public override void Release()
        {
            base.Release();

            serialPortEx.Close();
            UpdateState(DeviceState.Idle, "Device unloaded");

        }

        public override uint ReadInputGroup(int groupNo)
        {
            return 0;
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            int dtrPort = serialPortEx.SerialPort.DtrEnable ? 1 : 0;
            int rtsPort = serialPortEx.SerialPort.RtsEnable ? 1 : 0;
            int value = (dtrPort << (dtrBitMask - 1)) | (rtsPort << (rtsBitMask - 1));
            return (uint)value;
            //if(serialDigitalIoInfo.rts)
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            bool dtrValue = (outputPortStatus & dtrBitMask) > 0;
            bool rtsValue = (outputPortStatus & rtsBitMask) > 0;

            serialPortEx.SerialPort.DtrEnable = dtrValue;
            serialPortEx.SerialPort.RtsEnable = rtsValue;
        }
    }
}
