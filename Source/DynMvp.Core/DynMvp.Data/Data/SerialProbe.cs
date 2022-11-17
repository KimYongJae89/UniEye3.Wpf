using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DynMvp.Data
{
    public class SerialProbe : Probe
    {
        public string PortName { get; set; }
        public SerialPortEx InspectionSerialPort { get; set; }
        public float UpperValue { get; set; }
        public float LowerValue { get; set; }
        public int NumSerialReading { get; set; } = 1;

        public override int[] LightTypeIndexArr => null;

        private ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        //ReceivedPacket receivedPacket = new TensionReceivedPacket();
        //CasLoadCellReceivedPacket receivedPacketd;
        private TensionReceivedPacket tensionReceivedPacket;

        public override object Clone()
        {
            var serialProbe = new SerialProbe();
            serialProbe.Copy(this);

            return serialProbe;
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);

            var serialProbe = (SerialProbe)probe;

            PortName = serialProbe.PortName;
            InspectionSerialPort = serialProbe.InspectionSerialPort;
            UpperValue = serialProbe.UpperValue;
            LowerValue = serialProbe.LowerValue;
            NumSerialReading = serialProbe.NumSerialReading;

        }

        public override void OnPreInspection()
        {
        }

        public override void OnPostInspection()
        {
            if (InspectionSerialPort == null)
            {
                LogHelper.Error(string.Format("Serial Port is not assigned. {0}", FullId));
                return;
            }
        }

        public override bool IsControllable()
        {
            return true;
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Result", "", true));
            resultValues.Add(new ResultValue("Value", "", UpperValue, LowerValue));

            return resultValues;
        }

        public override ProbeResult DoInspect(InspectParam inspectParam, ProbeResultList probeResultList)
        {
            if (InspectionSerialPort == null)
            {
                LogHelper.Error(string.Format("Serial Port is not assigned. {0}", FullId));
                return null;
            }

            var serialProbeResult = new ProbeResult(this);

            var valueList = new List<float>();

            float maxValue = float.MinValue, minValue = float.MaxValue;

            for (int i = 0; i < NumSerialReading; i++)
            {
                float value = GetValue();
                if (value > 0)
                {
                    valueList.Add(value);
                    if (value > maxValue)
                    {
                        maxValue = value;
                    }

                    if (value < minValue)
                    {
                        minValue = value;
                    }
                }
            }

            float resultValue = 0;

            if (valueList.Count > 0)
            {
                if (valueList.Count >= 5)
                {
                    resultValue = (valueList.Sum() - minValue - maxValue) / (valueList.Count - 2);
                }
                else
                {
                    resultValue = valueList.Average();
                }
            }

            bool result = (resultValue >= LowerValue && resultValue <= UpperValue);

            serialProbeResult.SetResult(result);
            serialProbeResult.AddResultValue(new ResultValue("Result", "", result));
            serialProbeResult.AddResultValue(new ResultValue("Value", "", UpperValue, LowerValue, resultValue));

            return serialProbeResult;
        }

        public virtual void DataReceived(ReceivedPacket receivedPacket)
        {
            tensionReceivedPacket = (TensionReceivedPacket)receivedPacket;
            //if (ProbeType == ProbeType.Serial)
            //    this.receivedPacket = (CasLoadCellReceivedPacket)receivedPacket;
            //else
            //    this.receivedPacket = (TensionReceivedPacket)receivedPacket;
            manualResetEvent.Set();
        }

        private float GetValue()
        {
            LogHelper.Debug(LoggerType.Inspection, "GetValue");

            PacketParser packetParser = InspectionSerialPort.PacketHandler.PacketParser;
            if (packetParser == null)
            {
                LogHelper.Error("Packet Parser is null");
                return 0;
            }
            packetParser.DataReceived = new DataReceivedDelegate(DataReceived);

            manualResetEvent.Reset();

            InspectionSerialPort.SendRequest();

            float resultValue = 0;
            string resultString = "";

            if (manualResetEvent.WaitOne(5000) == true)
            {
                LogHelper.Debug(LoggerType.Inspection, "Packet Received");

                try
                {
                    resultString = tensionReceivedPacket.ReceivedData;
                }
                catch (InvalidCastException)
                {
                    LogHelper.Warn(LoggerType.Inspection, "Invalid Packet");
                    resultValue = 0;
                }
            }
            else
            {
                LogHelper.Warn(LoggerType.Inspection, "Packet loss");
            }

            packetParser.DataReceived = null;

            return resultValue;
        }


        public override ProbeResult CreateDefaultResult()
        {
            return new ProbeResult(this);
        }
    }
}
