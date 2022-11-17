using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Daq
{
    internal class DaqChannelVirtual : DaqChannel
    {
        public DaqChannelVirtual() : base(DaqChannelType.Virtual)
        {

        }

        public override double[] ReadVoltage(int numSamples)
        {
            return new double[10];
        }

        public override double[] ReadData(int numSamples)
        {
            return new double[10];
        }

        public override void Initialize(DaqChannelProperty daqChannelProperty)
        {
            ChannelProperty = daqChannelProperty;
        }
    }
}
