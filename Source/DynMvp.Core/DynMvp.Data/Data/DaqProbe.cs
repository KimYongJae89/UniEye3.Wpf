using DynMvp.Base;
using DynMvp.Devices.Daq;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Data
{
    public enum DaqMeasureType
    {
        Absolute, Difference, Distance
    }

    public enum DaqFilterType
    {
        None, Average, Median
    }

    public enum DaqDataType
    {
        Voltage, Data
    }

    public class DaqProbe : Probe
    {
        public DaqChannel DaqChannel { get; set; }
        public int NumSample { get; set; } = 100;
        public float UpperValue { get; set; }
        public float LowerValue { get; set; }

        public override int[] LightTypeIndexArr => null;
        public float LocalScaleFactor { get; set; } = 0;
        public float ValueOffset { get; set; } = 0;
        public bool UseLocalScaleFactor { get; set; }
        public DaqMeasureType MeasureType { get; set; }
        public DaqDataType DataType { get; set; }
        public string Target1Name { get; set; }
        public string Target2Name { get; set; }
        public DaqFilterType FilterType { get; set; } = DaqFilterType.Average;

        public override object Clone()
        {
            var daqProbe = new DaqProbe();
            daqProbe.Copy(this);

            return daqProbe;
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);

            var daqProbe = (DaqProbe)probe;

            NumSample = daqProbe.NumSample;
            UpperValue = daqProbe.UpperValue;
            LowerValue = daqProbe.LowerValue;
            UseLocalScaleFactor = daqProbe.UseLocalScaleFactor;
            LocalScaleFactor = daqProbe.LocalScaleFactor;
            ValueOffset = daqProbe.ValueOffset;
            MeasureType = daqProbe.MeasureType;
            FilterType = daqProbe.FilterType;
        }

        public override void OnPreInspection()
        {

        }

        public override void OnPostInspection()
        {

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

        public override ProbeResult DoInspect(InspectParam inspParam, ProbeResultList probeResultList)
        {
            double value = 0;
            switch (MeasureType)
            {
                case DaqMeasureType.Absolute:
                    if (DataType == DaqDataType.Voltage)
                    {
                        value = GetVoltage();
                    }
                    else
                    {
                        value = GetData();
                    }

                    break;
                //case DaqMeasureType.Distance:
                //    value = GetDistance(probeResultList, inspParam.SensorDistance);
                //    break;
                case DaqMeasureType.Difference:
                    value = GetDifference(probeResultList);
                    break;
            }

            bool result = (value >= LowerValue) && (value <= UpperValue);

            LogHelper.Debug(LoggerType.Inspection, string.Format("DAQ Probe [{0}] Inspected. Result : {1:0.000}", FullId, value));

            var daqProbeResult = new ProbeResult(this);
            daqProbeResult.AddResultValue(new ResultValue("Result", "", result));
            daqProbeResult.AddResultValue(new ResultValue("Value", "", UpperValue, LowerValue, (float)value));

            return daqProbeResult;
        }

        private double GetDifference(ProbeResultList probeResultList)
        {
            ProbeResult probe1Result = probeResultList.GetProbeResult(Target1Name, 1);
            ProbeResult probe2Result = probeResultList.GetProbeResult(Target2Name, 1);

            double value = 0;
            if (probe1Result != null && probe2Result != null)
            {
                ResultValue resultValue1 = probe1Result.GetResultValue("Value");
                ResultValue resultValue2 = probe2Result.GetResultValue("Value");

                value = Math.Abs(Convert.ToSingle(resultValue1.Value) - Convert.ToSingle(resultValue2.Value));
            }

            return value;
        }

        private double GetDistance(ProbeResultList probeResultList, float laserDistance)
        {
            ProbeResult probe1Result = probeResultList.GetProbeResult(Target1Name, 1);
            ProbeResult probe2Result = probeResultList.GetProbeResult(Target2Name, 1);

            double value = 0;
            if (probe1Result != null && probe2Result != null)
            {
                ResultValue resultValue1 = probe1Result.GetResultValue("Value");
                ResultValue resultValue2 = probe2Result.GetResultValue("Value");

                value = laserDistance - Convert.ToSingle(resultValue1.Value) - Convert.ToSingle(resultValue2.Value);
            }

            return value;
        }

        private double GetVoltage()
        {
            if (DaqChannel == null)
            {
                return 0.0;
            }

            double[] values = DaqChannel.ReadVoltage(NumSample);

            double averageRaw = 0;
            double average = 0;
            if (values != null && values.Count() > 0)
            {
                if (FilterType == DaqFilterType.Average)
                {
                    averageRaw = values.Average();
                }
                else
                {
                    Array.Sort(values);
                    averageRaw = values[values.Length / 2];
                }

                if (DaqChannel.ChannelProperty.UseCustomScale)
                {
                    average = (averageRaw - DaqChannel.ChannelProperty.ValueOffset) / DaqChannel.ChannelProperty.ScaleFactor;
                }

                if (UseLocalScaleFactor == true)
                {
                    average = average * LocalScaleFactor + ValueOffset;
                }
            }

            return average;
        }

        private double GetData()
        {
            if (DaqChannel == null)
            {
                return 0.0;
            }

            double[] values = DaqChannel.ReadData(NumSample);

            double averageRaw = 0;
            double average = 0;
            if (values != null && values.Count() > 0)
            {
                if (FilterType == DaqFilterType.Average)
                {
                    averageRaw = values.Average();
                }
                else
                {
                    Array.Sort(values);
                    averageRaw = values[values.Length / 2];
                }

                if (DaqChannel.ChannelProperty.UseCustomScale)
                {
                    average = (averageRaw - DaqChannel.ChannelProperty.ValueOffset) / DaqChannel.ChannelProperty.ScaleFactor;
                }

                if (UseLocalScaleFactor == true)
                {
                    average = average * LocalScaleFactor + ValueOffset;
                }
            }

            return average;
        }


        public override ProbeResult CreateDefaultResult()
        {
            return new ProbeResult(this);
        }
    }
}
