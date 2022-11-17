using DynMvp.Base;
using DynMvp.Devices.Dio;
using NationalInstruments;
using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Dio
{
    public class DigitalIoNIUsb : DigitalIo
    {
        public DigitalIoNIUsb(string name)
            : base(DigitalIoType.NIUsb, name)
        {
            NumInPort = NumOutPort = 16;
        }

        private string[] Lines { get; set; }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            Lines = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOPort, PhysicalChannelAccess.External);

            return true;
        }

        public override bool IsReady()
        {
            return Lines.Length > 0;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            throw new NotImplementedException();
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            throw new NotImplementedException();
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            try
            {
                Write(groupNo, outputPortStatus);
            }
            catch (DaqException exception)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToWriteValue, ErrorLevel.Error,
                    ErrorSection.DigitalIo.ToString(), CommonError.FailToWriteValue.ToString(), exception.Message);
            }
        }

        private void Write(int groupNo, uint status)
        {
            //using (Task digitalWriteTask = new Task())
            //{
            //    digitalWriteTask.DOChannels.CreateChannel(Lines[groupNo], "port" + groupNo, ChannelLineGrouping.OneChannelForAllLines);
            //    DigitalSingleChannelWriter writer = new DigitalSingleChannelWriter(digitalWriteTask.Stream);
            //    writer.WriteSingleSamplePort(true, status);
            //}
        }
    }
}
