using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Inspect;
using DynMvp.InspectData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;

namespace DynMvp.Data
{
    public class IoProbe : Probe
    {
        public string DigitalIoName { get; set; }
        public int PortNo { get; set; }

        public override int[] LightTypeIndexArr => null;

        public override object Clone()
        {
            var ioProbe = new IoProbe();
            ioProbe.Copy(this);

            return ioProbe;
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);

            var ioProbe = (IoProbe)probe;

            DigitalIoName = ioProbe.DigitalIoName;
            PortNo = ioProbe.PortNo;
        }

        public override void OnPreInspection()
        {

        }

        public override void OnPostInspection()
        {

        }

        public override bool IsControllable()
        {
            return false;
        }

        public override List<ResultValue> GetResultValues()
        {
            var resultValues = new List<ResultValue>();
            resultValues.Add(new ResultValue("Result", "", true));

            return resultValues;
        }

        public override ProbeResult DoInspect(InspectParam inspectParam, ProbeResultList probeResultList)
        {
            bool value = false;

            DigitalIoHandler digitalIoHandler = DeviceManager.Instance().DigitalIoHandler;

            int deviceIndex = digitalIoHandler.GetIndex(DigitalIoName);
            if (deviceIndex > -1)
            {
                value = digitalIoHandler.ReadInput(deviceIndex, 0, PortNo);

                LogHelper.Debug(LoggerType.Inspection, string.Format("IO Probe [{0}] Inspected. Result : {1}", FullId, value));
            }
            else
            {
                LogHelper.Debug(LoggerType.Inspection, string.Format("IO Probe [{0}] is failed : Can't find Digital I/O : {1}", FullId, DigitalIoName));
            }

            var ioProbeResult = new ProbeResult();
            ioProbeResult.AddResultValue(new ResultValue("Result Value", "", value));

            return ioProbeResult;
        }

        public override ProbeResult CreateDefaultResult()
        {
            return new ProbeResult(this);
        }
    }
}
