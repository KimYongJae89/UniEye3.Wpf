using DynMvp.Base;
using NationalInstruments;
using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices.Daq
{
    public class DaqChannelNiDaqmx : DaqChannel
    {
        private string NameToAssignChannel { get; set; } = "";

        private Task SingleAnalogReaderTask { get; set; }
        private AnalogSingleChannelReader SingleAnalogReader { get; set; }

        private Task MultiAnalogReaderTask { get; set; }
        private AnalogMultiChannelReader MultiAnalogReader { get; set; }

        private Task SingleDigitalWriterTask { get; set; }
        private DigitalSingleChannelWriter SingleDigitalWriter { get; set; }

        private Task MultiDigitalWriterTask { get; set; }
        private DigitalMultiChannelWriter MultiDigitalWriter { get; set; }

        public DaqChannelNiDaqmx() : base(DaqChannelType.Daqmx)
        {

        }

        public override void Initialize(DaqChannelProperty daqChannelProperty)
        {
            ChannelProperty = daqChannelProperty;
        }

        public override double[] ReadData(int numSamples)
        {
            return ReadVoltage(numSamples);
        }

        public override double[] ReadVoltage(int numSamples)
        {
            double[] values = null;

            for (int i = 0; i < 3; i++)
            {
                values = ReadVoltageOnce(numSamples);
                if (values != null && values.Count() > 0)
                {
                    break;
                }

                Thread.Sleep(50);
            }

            if (values == null || values.Count() == 0)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DAQ, (int)CommonError.FailToReadValue,
                                 ErrorLevel.Error, ErrorSection.DAQ.ToString(), CommonError.FailToReadValue.ToString(), "Can't read values");
            }

            return values;
        }

        private double[] ReadVoltageOnce(int numSamples)
        {
            try
            {
                var analogInTask = new Task();

                AIChannel aiChannel = analogInTask.AIChannels.CreateVoltageChannel(
                                ChannelProperty.ChannelName, NameToAssignChannel,
                                AITerminalConfiguration.Rse, ChannelProperty.MinValue, ChannelProperty.MaxValue, AIVoltageUnits.Volts);

                analogInTask.Timing.ConfigureSampleClock(NameToAssignChannel, ChannelProperty.SamplingHz,
                        SampleClockActiveEdge.Rising, SampleQuantityMode.FiniteSamples, numSamples);

                analogInTask.Stream.Timeout = 1000;

                var reader = new AnalogSingleChannelReader(analogInTask.Stream);
                return reader.ReadMultiSample(numSamples);
            }
            catch (DaqException exception)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DAQ, (int)CommonError.FailToReadValue, ErrorLevel.Error,
                    ErrorSection.DAQ.ToString(), CommonError.FailToReadValue.ToString(), exception.Message);
            }

            return null;
        }



        public double[,] ReadAnalogMultiData(int numSamples)
        {
            return MultiAnalogReader.ReadMultiSample(numSamples);
        }

        public void CreateMultiAnalogReader()
        {
            try
            {
                MultiAnalogReaderTask = new Task();
                MultiAnalogReaderTask.AIChannels.CreateVoltageChannel(
                    ChannelProperty.ChannelName, NameToAssignChannel, AITerminalConfiguration.Rse,
                    ChannelProperty.MinValue, ChannelProperty.MaxValue, AIVoltageUnits.Volts);
                MultiAnalogReaderTask.Timing.ConfigureSampleClock(NameToAssignChannel, ChannelProperty.SamplingHz,
                        SampleClockActiveEdge.Rising, SampleQuantityMode.ContinuousSamples);
                MultiAnalogReaderTask.Control(TaskAction.Verify);

                MultiAnalogReader = new AnalogMultiChannelReader(MultiAnalogReaderTask.Stream);
            }
            catch (DaqException exception)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DAQ, (int)CommonError.FailToReadValue, ErrorLevel.Error,
                    ErrorSection.DAQ.ToString(), CommonError.FailToReadValue.ToString(), exception.Message);
            }
        }

        public void DisposeMultiAnalogReader()
        {
            try
            {
                MultiAnalogReaderTask.Stop();
                MultiAnalogReaderTask.Dispose();
            }
            catch (DaqException exception)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DAQ, (int)CommonError.FailToReadValue, ErrorLevel.Error,
                    ErrorSection.DAQ.ToString(), CommonError.FailToReadValue.ToString(), exception.Message);
            }
        }

        public void WriteData(string port, int data)
        {
            try
            {
                var digitalOutTask = new Task();

                DOChannel doChannel = digitalOutTask.DOChannels.CreateChannel(port, "port0", ChannelLineGrouping.OneChannelForAllLines);

                var writer = new DigitalSingleChannelWriter(digitalOutTask.Stream);

                writer.WriteSingleSamplePort(true, (uint)data);
            }
            catch (DaqException exception)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DAQ, (int)CommonError.FailToReadValue, ErrorLevel.Error,
                    ErrorSection.DAQ.ToString(), CommonError.FailToReadValue.ToString(), exception.Message);
            }
        }
    }
}
