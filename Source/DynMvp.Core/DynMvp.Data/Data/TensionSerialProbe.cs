using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DynMvp.Data
{
    public enum TensionUnitType
    {
        Newton, mm
    }

    public class TensionSerialProbe : SerialProbe
    {
        private ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        private TensionReceivedPacket receivedPacket;
        public string TensionFilePath { get; set; }
        public Dictionary<float, int> TensionMap { get; set; } = new Dictionary<float, int>();
        public TensionUnitType UnitType { get; set; } = TensionUnitType.mm;

        public override object Clone()
        {
            var tensionSerialProbe = new TensionSerialProbe();
            tensionSerialProbe.Copy(this);

            return tensionSerialProbe;
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);
            TensionFilePath = ((TensionSerialProbe)probe).TensionFilePath;
            TensionMap = ((TensionSerialProbe)probe).TensionMap;
            UnitType = ((TensionSerialProbe)probe).UnitType;
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
            resultValues = base.GetResultValues();
            return resultValues;
        }

        public override ProbeResult DoInspect(InspectParam inspectParam, ProbeResultList probeResultList)
        {
            if (InspectionSerialPort == null)
            {
                LogHelper.Error(string.Format("Serial Port is not assigned. {0}", FullId));
                return null;
            }
            InitTesnionMap();

            var serialProbeResult = new ProbeResult(this);

            var valueList = new List<float>();

            float maxValue = float.MinValue, minValue = float.MaxValue;

            //for (int i = 0; i < Configuration.NumSerialReading; i++)
            for (int i = 0; i < NumSerialReading; i++)
            {
                //Thread.Sleep(500);
                float value = GetValue();
                //                if (value > 0)
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
                resultValue = valueList.Average();
            }

            float nResultValue = resultValue * (-1);
            int nNewtonValue = GetTensionDataConvertedLengthToNewton(nResultValue);

            bool result;
            if (UnitType == TensionUnitType.mm)
            {
                result = (nResultValue >= LowerValue && nResultValue <= UpperValue);

                serialProbeResult.AddResultValue(new ResultValue("ValueType", "", "mm", true));
                serialProbeResult.AddResultValue(new ResultValue("Value", "", UpperValue, LowerValue, nResultValue, result));
                serialProbeResult.BriefMessage = string.Format("{0} : Value = {1}", result ? "Good" : "NG", nResultValue);
            }
            else
            {
                result = (nNewtonValue >= LowerValue && nNewtonValue <= UpperValue);

                serialProbeResult.AddResultValue(new ResultValue("ValueType", "", "Newton", true));
                serialProbeResult.AddResultValue(new ResultValue("Value", "", UpperValue, LowerValue, nNewtonValue, result));
                serialProbeResult.BriefMessage = string.Format("{0} : Value = {1}", result ? "Good" : "NG", nNewtonValue);
            }

            serialProbeResult.SetResult(result);
            serialProbeResult.AddResultValue(new ResultValue("Result", "", result));

            return serialProbeResult;
        }

        public override void DataReceived(ReceivedPacket receivedPacket)
        {
            this.receivedPacket = (TensionReceivedPacket)receivedPacket;
            manualResetEvent.Set();
        }

        private float GetValue()
        {
            LogHelper.Debug(LoggerType.Inspection, "TensionSerialProbe - GetValue");

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


            if (manualResetEvent.WaitOne(500) == true)
            {
                LogHelper.Debug(LoggerType.Inspection, "Packet Received");

                try
                {
                    resultValue = Convert.ToSingle(receivedPacket.ResultValue);
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

        private void InitTesnionMap()
        {
            if (TensionMap.Count > 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(TensionFilePath))
            {
                LogHelper.Debug(LoggerType.Inspection, "Can't find tension file.");
                return;
            }

            var reader = new StreamReader(File.OpenRead(TensionFilePath));

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                if (!string.IsNullOrEmpty(values[0]))
                {
                    float id = Convert.ToSingle(values[0]);

                    if (!string.IsNullOrEmpty(values[1]))
                    {
                        TensionMap.Add(id, Convert.ToInt32(values[1]));
                    }
                }
            }
        }

        private int GetTensionDataConvertedLengthToNewton(float resultData)
        {
            if (TensionMap == null)
            {
                return 0;
            }

            resultData = (float)Math.Round(resultData, 2);
            TensionMap.TryGetValue(resultData, out int result);
            return result;
        }
    }
}
