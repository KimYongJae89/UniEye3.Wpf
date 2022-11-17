using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Daq
{
    public enum DaqChannelType
    {
        None, Virtual, Daqmx, MeDAQ
    }

    public abstract class DaqChannel : Device
    {
        public DaqChannelType DaqChannelType { get; set; }
        public DaqChannelProperty ChannelProperty { get; set; }

        public DaqChannel(DaqChannelType daqChannelType)
        {
            Name = daqChannelType.ToString();
            DeviceType = DeviceType.DaqChannel;
            DaqChannelType = daqChannelType;
            UpdateState(DeviceState.Idle);
        }

        public abstract void Initialize(DaqChannelProperty daqChannelProperty);

        public abstract double[] ReadVoltage(int numSamples);

        public abstract double[] ReadData(int numSamples);
    }
}
