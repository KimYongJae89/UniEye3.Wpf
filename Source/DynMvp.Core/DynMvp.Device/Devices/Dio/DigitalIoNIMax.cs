using DynMvp.Base;
using NationalInstruments;
using NationalInstruments.DAQmx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices.Dio
{
    internal class DigitalIoNIMax : DigitalIo
    {
        private DigitalSingleChannelReader digitalInputReader;
        private DigitalSingleChannelReader digitalOutputReader;
        private DigitalSingleChannelWriter digitalOutputWriter;

        public DigitalIoNIMax(string name)
            : base(DigitalIoType.NIMax, name)
        {
            NumInPort = NumOutPort = 4;
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            var digitalInputTask = new Task();
            var digitalOutputTask = new Task();

            var niDigitalIoInfo = (NiDigitalIoInfo)digitalIoInfo;

            digitalInputTask.DIChannels.CreateChannel(niDigitalIoInfo.InputChannelLine, "inputChannel", ChannelLineGrouping.OneChannelForAllLines);
            digitalInputReader = new DigitalSingleChannelReader(digitalInputTask.Stream);

            digitalOutputTask.DOChannels.CreateChannel(niDigitalIoInfo.OutputChannelLine, "outputChannel", ChannelLineGrouping.OneChannelForAllLines);
            digitalOutputWriter = new DigitalSingleChannelWriter(digitalOutputTask.Stream);
            digitalOutputReader = new DigitalSingleChannelReader(digitalOutputTask.Stream);

            return true;
        }

        public override bool IsReady()
        {
            return true;
        }

        public override void Release()
        {
            base.Release();
            digitalInputReader = null;
            digitalOutputWriter = null;
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            try
            {
                bool[] dataArray = new bool[NumInPort];

                byte[] bytes = BitConverter.GetBytes(outputPortStatus);
                bool[] convertArray = bytes.SelectMany(GetBitsStartingFromLSB).ToArray();
                for (int index = 0; index < convertArray.Length; index++)
                {
                    convertArray[index] = !convertArray[index];
                }

                Array.Copy(convertArray, dataArray, dataArray.Length);

                digitalOutputWriter.WriteSingleSampleMultiLine(true, dataArray);
            }
            catch (DaqException exception)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToWriteValue, ErrorLevel.Error,
                    ErrorSection.DigitalIo.ToString(), CommonError.FailToWriteValue.ToString(), exception.Message);
            }
        }

        private IEnumerable<bool> GetBits(byte b)
        {
            for (int i = 0; i < 8; i++)
            {
                yield return (b & 0x80) != 0;
                b *= 2;
            }
        }

        private static IEnumerable<bool> GetBitsStartingFromLSB(byte b)
        {
            for (int i = 0; i < 8; i++)
            {
                yield return (b % 2 == 0) ? false : true;
                b = (byte)(b >> 1);
            }
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            uint outputPortStatus = 0;
            try
            {
                bool[] readData;

                //Read the digital channel
                readData = digitalOutputReader.ReadSingleSampleMultiLine();

                int val = 0;
                for (int i = 0; i < readData.Length; i++)
                {
                    if (readData[i] == false)
                    {   //if bit is true
                        //add decimal value of bit
                        val += 1 << i;
                    }
                }

                outputPortStatus = Convert.ToUInt32(val);
            }
            catch (DaqException exception)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToReadValue,
                    ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToReadValue.ToString(), exception.Message);
            }

            return outputPortStatus;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            uint inputPortStatus = 0;
            try
            {
                bool[] readData;

                //Read the digital channel
                readData = digitalInputReader.ReadSingleSampleMultiLine();

                int val = 0;
                for (int i = 0; i < readData.Length; i++)
                {
                    if (readData[i] == false)
                    {   //if bit is true
                        //add decimal value of bit
                        val += 1 << i;
                    }
                }

                inputPortStatus = Convert.ToUInt32(val);
            }
            catch (DaqException exception)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToReadValue, ErrorLevel.Error,
                    ErrorSection.DigitalIo.ToString(), CommonError.FailToReadValue.ToString(), exception.Message);
            }

            return inputPortStatus;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }
    }
}
